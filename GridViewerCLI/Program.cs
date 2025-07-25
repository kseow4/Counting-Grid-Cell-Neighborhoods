using CountingGridCellNeighbors;
using System.Text.Json;
using System.Text;
using System.Xml.XPath;
using System.Globalization; 
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using GridViewerCLI.Enumerations;
using GridViewerCLI.Tools;


namespace GridViewerCLI;

class Program
{
   public static string DefaultFileName { get; set; } = "grid";
   public static bool BatchMode { get; set; }
   public static bool AutoMode { get; set; } = true;
   public static Dictionary<string, string> ArgDict { get; set; } = [];
   public static HashSet<string> ExportFormats { get; set; } = [];

   public static readonly Dictionary<string, string> ArgAliases = new Dictionary<string, string>
   {
      ["-wt"] = "--width",
      ["-ht"] = "--height",
      ["-x"] = "--width",
      ["-y"] = "--height",
      ["-p"] = "--positives",
      ["-n"] = "--n",
      ["-d"] = "--dist",
      ["-f"] = "--format",
      ["-o"] = "--out",
      ["-a"] = "--auto",
      ["-w"] = "--wrap",
      ["-i"] = "--image",
      ["-j"] = "--json",
      ["-c"] = "--csv",
      ["-g"] = "--graphic",
      ["-cs"] = "--cellsize",
      ["-np"] = "--no-print",
      ["-s"] = "--stats-only",
      ["-nc"] = "--no-color",
      ["-ne"] = "--no-export",
      ["-ns"] = "--no-stats",
      ["-npr"] = "--no-prompt",
      ["-nh"] = "--no-header",
      ["-h"] = "--help",

   };

   static void Main(string[] args)
   {
      Console.OutputEncoding = Encoding.UTF8; // support emoji

      Console.WriteLine("=== Counting Grid Cell Neighbors CLI ===");

      ArgDict = ParseArgs(args);


      Utilities.PrintHelp();

      // Help flag
      if (ArgDict.ContainsKey("--help"))
      {
         Console.WriteLine(@"
Options:           | Shorthand: | Description:
--width=10         | -w, -x     | Set grid width
--height=10        | -h, -y     | Set grid height
--positives=5      | -p         | Set # of positive cells
--n=1              | -n         | Set neighborhood distance
--wrap             | -r         | Enable wraparound edges
--dist=chebyshev   | -d         | Distance type: manhattan, euclidean, chebyshev
--format=csv,json  | -f         | Export file formats
--out=mygrid       | -o         | Base filename for exports
--auto             | -a         | Skip prompts and auto-export
--no-print         | -c         | Skip printing grid and stats (silent mode)
--stats-only       | -s         | Print only statistics (no grid)
--help             | -q         | Displays list of available arguments
");
         return;
      }


      // ExportFormats = ArgDict.TryGetValue("--format", out string? exported) ? [.. exported.ToLower().Split(",")] : ExportFormats;

      if (ArgDict.TryGetValue("--format", out string? exported))
         ExportFormats = [.. exported
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.TrimStart('.').ToLowerInvariant())];
  
      bool SuppressAllPrint = ArgDict.ContainsKey("--no-print");
      bool StatsOnly = ArgDict.ContainsKey("--stats-only");
      if (SuppressAllPrint && StatsOnly)
      {
         Console.WriteLine("⚠️ Ignoring --stats-only because --no-print was also specified.");
         StatsOnly = false;
      }

      DefaultFileName = Path.GetFileNameWithoutExtension(ArgDict.TryGetValue("--out", out string? name) && !string.IsNullOrWhiteSpace(name) ? name : DefaultFileName);

      // Batch Mode if any args passed
      BatchMode = ArgDict.Count > 0;
      AutoMode = BatchMode && ArgDict.ContainsKey("--auto"); 

      // int width = ReadInt("Enter grid width: ");
      int width = BatchMode ? GetArgInt(ArgDict, "--width", 10) : ReadInt("Enter grid width: ");

      // int height = ReadInt("Enter grid height: ");
      int height = BatchMode ? GetArgInt(ArgDict, "--height", 10) : ReadInt("Enter grid height: ");

      // int positives = ReadInt("Enter number of positive cells: ");
      int positives = BatchMode ? GetArgInt(ArgDict, "--positives", 1) : ReadInt("Enter number of positive cells: ");

      // int n = ReadInt("Enter neighborhood distance (n): ");
      int n = BatchMode ? GetArgInt(ArgDict, "--n", 3) : ReadInt("Enter neighborhood distance (n): ");

      // bool wrap = ReadYesNo("Wraparound edges? (y/n): ");
      bool wrap = BatchMode ? ArgDict.ContainsKey("--wrap") : ReadYesNo("Wraparound edges? (y/n): ");

      DistanceType distanceType = DistanceType.Manhattan;
      if (BatchMode)
      {
         string dist = ArgDict.GetValueOrDefault("--dist", "manhattan").ToLower();
         _ = Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dist), out distanceType);
         if (!Enum.TryParse(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dist), out distanceType))
         {
            Console.WriteLine($"⚠️ Unknown distance type: {dist}. Defaulting to Manhattan.");
            distanceType = DistanceType.Manhattan;
         }

      }
      else
      {
         Console.WriteLine("Select distance type:");
         DistanceType[] distanceTypes = Enum.GetValues<DistanceType>();
         for (int i = 0; i < distanceTypes.Length; i++)
            Console.WriteLine($"[{i}] {distanceTypes[i]}");

         int index = ReadInt("Enter selection");
         distanceType = (index >= 0 && index < distanceTypes.Length) ? (DistanceType)distanceTypes.GetValue(index)! : DistanceType.Manhattan;
      }

      // Generate grid
      Cell[,] grid = GridGenerator.Generate(height, width, positives);

      // Find neighbors
      _ = NeighborhoodFinder.FindNeighbors(grid, n, distanceType, wrap);

      // Print grid
      if (!SuppressAllPrint)
      {
         if (!StatsOnly)
            GridPrinter.Print(grid, useColor: !BatchMode);

         PrintStats(grid);
      }

      // Print legend
      PrintLegend();

      // CSV Export
      if (ShouldExport(FileExtension.CSV)) ExportToCsv(grid, AskFileNameSafe(FileExtension.CSV));

      // JSON Export
      if (ShouldExport(FileExtension.JSON)) ExportToJson(grid, AskFileNameSafe(FileExtension.JSON));

      // Image Export 
      // BMP Export
      if (ShouldExport(FileExtension.BMP)) ExportToImage(grid, ReadInt("Image Cell Size"), AskFileNameSafe(FileExtension.BMP));

      // PNG Export
      if (ShouldExport(FileExtension.PNG)) ExportToImage(grid, ReadInt("Image Cell Size"), AskFileNameSafe(FileExtension.PNG));

   }

   #region Local Print Functions

   static void PrintLegend()
   {
      Console.WriteLine("\nLegend:");
      Console.WriteLine("🟥 = Positive Cell");
      Console.WriteLine("🟨 = Neighbor Cell");
      Console.WriteLine("⬜ = Empty Cell");
   }

   static void PrintStats(Cell[,] grid)
   {
      int height = grid.GetLength(0);
      int width = grid.GetLength(1);
      int total = height * width;

      int positives = grid.Cast<Cell>().Count(c => c.Value > 0);
      int neighbors = grid.Cast<Cell>().Count(c => c.IsNeighbor);

      Console.WriteLine("\nGrid Stats:");
      Console.WriteLine($"Total cells: {total}");
      Console.WriteLine($"Positive cells: {positives} ({100.0 * positives / total:F2}%)");
      Console.WriteLine($"Neighbor cells: {neighbors} ({100.0 * neighbors / total:F2}%)");
   }

   #endregion

   #region Export File Functions

   static void ExportToCsv(Cell[,] grid, string filename)
   {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("Row,Col,Value,IsNeighbor");

      foreach (Cell cell in grid)
         sb.AppendLine($"{cell.X},{cell.Y},{cell.Value},{cell.IsNeighbor}");

      File.WriteAllText(filename, sb.ToString());
      Console.WriteLine($"✅ Exported {filename}");
   }

   static void ExportToJson(Cell[,] grid, string filename)
   {
      File.WriteAllText(filename, JsonSerializer.Serialize(grid.Cast<Cell>().Select(c => new
      {
         c.X,
         c.Y,
         c.Value,
         c.IsNeighbor
      }), new JsonSerializerOptions { WriteIndented = true }));
      Console.WriteLine($"✅ Exported {filename}");
   }

    public static void ExportToImage(Cell[,] grid,int cellSize, string filePath)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        using Image<Rgba32> image = new(width * cellSize, height * cellSize, Color.White);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = grid[y, x];
                Color color = cell.IsPositive ? Color.Red :
                            cell.IsNeighbor ? Color.LightBlue :
                            Color.White;

                Rectangle rectangle = new(x * cellSize, y * cellSize, cellSize, cellSize);
                image.Mutate(ctx => ctx.Fill(color, rectangle));
                image.Mutate(ctx => ctx.Draw(Color.Gray, 1, rectangle));
            }
        }

        // Infer format from extension
      //   var extension = Path.GetExtension(filePath).ToLowerInvariant();
      //   IImageEncoder encoder = extension switch
      //   {
      //       ".png" => new SixLabors.ImageSharp.Formats.Png.PngEncoder(),
      //       ".jpeg" or ".jpg" => new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder(),
      //       ".bmp" => new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder(),
      //       _ => throw new ArgumentException($"Unsupported image format: {extension}")
      //   };
      //   image.Save(filePath, encoder);

        image.Save(filePath);
        Console.WriteLine($"✅ Image exported to: {filePath}");
    }

   // static void ExportToImagse(Cell[,] grid, int cellSize, string filePath)
   // {
   //    int height = grid.GetLength(0);
   //    int width = grid.GetLength(1);
   //    using var bmp = new Bitmap(width * cellSize, height * cellSize);
   //    using var g = Graphics.FromImage(bmp);
   //    g.Clear(Color.White);

   //    for (int y = 0; y < height; y++)
   //    {
   //       for (int x = 0; x < width; x++)
   //       {
   //          var cell = grid[y, x];
   //          Brush brush = cell.IsPositive
   //              ? Brushes.Red
   //              : cell.IsNeighbor
   //                  ? Brushes.LightBlue
   //                  : Brushes.White;

   //          g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
   //          g.DrawRectangle(Pens.Gray, x * cellSize, y * cellSize, cellSize, cellSize);
   //       }
   //    }

   //    // string extension = Path.GetExtension(filePath).ToLowerInvariant();
   //    // var format = extension switch
   //    // {
   //    //    ".bmp" => ImageFormat.Bmp,
   //    //    ".png" => ImageFormat.Png,
   //    //    ".jpg" or ".jpeg" => ImageFormat.Jpeg,
   //    //    _ => throw new ArgumentException($"❌ Unsupported image format: {extension}")
   //    // };
   //    bmp.Save(filePath);
   //    Console.WriteLine($"✅ Image exported to: {filePath}");
   // }

   #endregion

   #region Local Helper Functions

   static Dictionary<string, string> ParseArgs(string[] args)
   {
      Dictionary<string, string> dict = new(StringComparer.OrdinalIgnoreCase);
      for (int i = 0; i < args.Length; i++)
      {
         string arg = args[i];

         if (arg.StartsWith("--"))
         {
               string[] parts = arg.Split("=", 2);
               string key = parts[0];
               string value = parts.Length > 1 ? parts[1] : "true";
               dict[key] = value;
         }
         else if (arg.StartsWith("-"))
         {
               if (ArgAliases.TryGetValue(arg, out string? fullKey))
                  arg = fullKey;

               if (i + 1 < args.Length && !args[i + 1].StartsWith("-"))
                  dict[arg] = args[++i]; // consume next token as value
               else
                  dict[arg] = "true";
         }
      }

      return dict;
   }


   static int GetArgInt(Dictionary<string, string> args, string key, int fallback) => args.TryGetValue(key, out string? val) && int.TryParse(val, out int result) ? result : fallback;


   static int ReadInt(string prompt, int defaultValue = 0, Func<int, bool>? validator = null, string? errorMessage = null, int? min = null, int? max = null, int maxRetries = -1)
   {
      if (AutoMode) return defaultValue;

      int attempts = 0;
      while (maxRetries < 0 || attempts < maxRetries)
      {
         Console.Write($"{prompt}{(defaultValue != 0 ? $" (default: {defaultValue})" : "")}: ");
         string input = Console.ReadLine()?.Trim() ?? "";

         if (string.IsNullOrEmpty(input))
               return defaultValue;

         if (int.TryParse(input, out int value))
         {
               if ((min.HasValue && value < min.Value) || (max.HasValue && value > max.Value))
               {
                  Console.WriteLine($"Value must be between {min} and {max}.");
               }
               else if (validator != null && !validator(value))
               {
                  Console.WriteLine(errorMessage ?? "Input did not pass custom validation.");
               }
               else
               {
                  return value;
               }
         }
         else
         {
               Console.WriteLine("Invalid number. Please try again.");
         }

         attempts++;
      }

      Console.WriteLine("Retry limit exceeded. Using default.");
      return defaultValue;
   }



   static bool ReadYesNo(string prompt)
   {
      if (AutoMode) return false;
      Console.Write($"{prompt} (y/n): ");
      return Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) == true;
   }

   static string AskFileNameSafe(FileExtension extension) => GetAvailableFilename(AskFileName(extension), extension);

   static string AskFileName(FileExtension extension)
   {
      if (AutoMode) return $"{DefaultFileName}.{extension}";
      Console.Write($"File Name (default: '{DefaultFileName}.{extension}'): ");
      return Console.ReadLine() is string fn && !string.IsNullOrWhiteSpace(fn) ? Path.GetFileNameWithoutExtension(fn) : DefaultFileName;
   }

   static string GetAvailableFilename(string baseName, FileExtension extension)
   {
      string filename = $"{baseName}.{extension}";
      int counter = 1;
      while (File.Exists(filename))
      {
         filename = $"{baseName}_{counter}.{extension}";
         counter++;
      }
      return filename;
   }

   static bool ShouldExport(FileExtension format) =>
    (BatchMode && (ExportFormats.Contains($"{format}".ToLower()) || ArgDict.ContainsKey($"--{format}".ToLower())))
    || (!BatchMode && ReadYesNo($"Export to .{format}?"));



   #endregion
}
