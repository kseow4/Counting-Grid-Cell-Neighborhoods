using System;
using CountingGridCellNeighbors;

class Program
{
   static void Main(string[] args)
   {
      Console.WriteLine("=== Counting Grid Neighborhood CLI ===");

      int width = PromptInt("Grid Width", 11);
      int height = PromptInt("Grid Height", 11);
      int numPositives = PromptInt("Number of Positive Cells", 5);
      int n = PromptInt("Neighborhood Radius (n)", 1);
      bool wrap = PromptBool("Enable Wrap-Around? (y/n)", false);
      DistanceType distanceType = PromptEnum<DistanceType>("Distance Type");

      var grid = GridGenerator.Generate(height, width, numPositives);
      int count = NeighborhoodFinder.FindNeighbors(grid, n, distanceType, wrap);
      GridPrinter.Print(grid);

      Console.WriteLine($"\nDetected Neighbor Count: {count}\n");

      if (PromptBool("Export grid to file? (y/n)", false))
      {
         Console.WriteLine("Choose export format:");
         Console.WriteLine(" [1] TXT  (P N .)");
         Console.WriteLine(" [2] CSV  (P,N,.)");
         Console.WriteLine(" [3] JSON (List of cell objects)");

         string format = PromptString("Format", "1");

         string content = "";
         string extension = "";
         switch (format.Trim())
         {
            case "1":
               content = GridPrinter.ToStringOutput(grid);
               extension = "txt";
               break;
            case "2":
               content = GridPrinter.ToCsv(grid);
               extension = "csv";
               break;
            case "3":
               content = GridPrinter.ToJson(grid);
               extension = "json";
               break;
            default:
               Console.WriteLine("❌ Invalid format. Skipping export.");
               return;
         }

         string filename = PromptString($"Filename (no extension)", $"grid_output");
         filename = $"{filename}.{extension}";

         try
         {
            File.WriteAllText(filename, content);
            Console.WriteLine($"✅ Grid exported to: {filename}");

            if (PromptBool("Preview file contents?", true))
            {
               Console.WriteLine("\n--- Begin File Preview ---");
               Console.WriteLine(content);
               Console.WriteLine("--- End Preview ---\n");
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"❌ Failed to write file: {ex.Message}");
         }
      }



   }

   static int PromptInt(string prompt, int defaultVal)
   {
      Console.Write($"{prompt} [{defaultVal}]: ");
      string input = Console.ReadLine();
      return int.TryParse(input, out var val) ? val : defaultVal;
   }

   static bool PromptBool(string prompt, bool defaultVal)
   {
      Console.Write($"{prompt} [{(defaultVal ? "y" : "n")}]: ");
      string input = Console.ReadLine()?.ToLower();
      return input switch
      {
         "y" => true,
         "n" => false,
         _ => defaultVal
      };
   }

   static string PromptString(string prompt, string defaultVal)
   {
      Console.Write($"{prompt} [{defaultVal}]: ");
      string input = Console.ReadLine() ?? "";
      return string.IsNullOrWhiteSpace(input) ? defaultVal : input;
   }

   static T PromptEnum<T>(string prompt) where T : struct, Enum
   {
      Console.WriteLine(prompt);
      var values = Enum.GetValues<T>();
      for (int i = 0; i < values.Length; i++)
         Console.WriteLine($"  [{i}] {values[i]}");

      Console.Write("Choose option: ");
      string input = Console.ReadLine();
      return int.TryParse(input, out var index) && index >= 0 && index < values.Length
         ? values[index]
         : values[0];
   }
}
