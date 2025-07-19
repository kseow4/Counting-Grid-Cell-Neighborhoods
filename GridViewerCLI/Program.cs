using CountingGridCellNeighbors;
using System.Text.Json;
using System.Text;

namespace GridViewerCLI;

class Program
{
   static void Main(string[] args)
   {
      Console.OutputEncoding = Encoding.UTF8; // support emoji

      Console.WriteLine("=== Counting Grid Cell Neighbors CLI ===");

      Console.Write("Enter grid width: ");
      int width = int.Parse(Console.ReadLine()!);

      Console.Write("Enter grid height: ");
      int height = int.Parse(Console.ReadLine()!);

      Console.Write("Enter number of positive cells: ");
      int positives = int.Parse(Console.ReadLine()!);

      Console.Write("Enter neighborhood distance (n): ");
      int n = int.Parse(Console.ReadLine()!);

      Console.Write("Wraparound edges? (y/n): ");
      bool wrap = Console.ReadLine()!.ToLower() == "y";

      Console.WriteLine("Select distance type:");
      var distanceTypes = Enum.GetValues<DistanceType>();
      for (int i = 0; i < distanceTypes.Length; i++)
         Console.WriteLine($"[{i}] {distanceTypes[i]}");

      Console.Write("Enter selection: ");
      var distanceType = distanceTypes[int.Parse(Console.ReadLine()!)];

      // Generate grid
      var grid = GridGenerator.Generate(height, width, positives);

      // Find neighbors
      _ = NeighborhoodFinder.FindNeighbors(grid, n, distanceType, wrap);

      // Print grid
      GridPrinter.Print(grid, useColor: true);

      // Print legend
      PrintLegend();

      // Print stats
      PrintStats(grid);

      // Export options
      Console.Write("Export to CSV? (y/n): ");
      if (Console.ReadLine()!.ToLower() == "y")
      {
         Console.WriteLine("File Name? Defaults to grid.csv, overwriting previous (if appliciable).");
         string filename = Console.ReadLine()!;
         ExportToCsv(grid, filename.Length > 0 ? filename.Contains(".csv") ? filename[..^4] : filename : "grid");
      }
      Console.Write("Export to JSON? (y/n): ");
      if (Console.ReadLine()!.ToLower() == "y")
      {
         Console.WriteLine("File Name? Defaults to grid.json, overwriting previous (if appliciable).");
         string filename = Console.ReadLine()!;
         ExportToJson(grid, filename.Length > 0 ? filename.Contains(".json") ? filename[..^5] : filename : "grid");
      }

      // Graphic Generation
      Console.Write("Generate Graphic Image? (y/n): ");
      if (Console.ReadLine()!.ToLower() == "y")
      {
         int default_size = 20;
         Console.WriteLine($"Cell Size? (Defaults {default_size}): ");
         int cell_size = Convert.ToInt32(Console.ReadLine()! ?? $"{default_size}");

         Console.WriteLine("File Name? Defaults to grid.bmp, overwriting previous (if appliciable).");
         string filename = Console.ReadLine()!;
         GridImageExporter.ExportToImage(grid, filename.Length > 0 ? filename.Contains(".bmp") ? filename[..^4] : filename : "grid", cell_size > 0 ? cell_size : default_size);
      }
   }

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
      Console.WriteLine($"Positive cells: {positives} ({(100.0 * positives / total):F2}%)");
      Console.WriteLine($"Neighbor cells: {neighbors} ({(100.0 * neighbors / total):F2}%)");
   }

   static void ExportToCsv(Cell[,] grid, string filename)
   {
      var sb = new StringBuilder();
      sb.AppendLine("Row,Col,Value,IsNeighbor");

      foreach (var cell in grid)
         sb.AppendLine($"{cell.X},{cell.Y},{cell.Value},{cell.IsNeighbor}");

      File.WriteAllText(filename + ".csv", sb.ToString());
      Console.WriteLine($"✅ Exported {filename}.csv");
   }

   static void ExportToJson(Cell[,] grid, string filename)
   {
      File.WriteAllText(filename + ".json", JsonSerializer.Serialize(grid.Cast<Cell>().Select(c => new
      {
         c.X,
         c.Y,
         c.Value,
         c.IsNeighbor
      }), new JsonSerializerOptions { WriteIndented = true }));
      Console.WriteLine($"✅ Exported {filename}.json");
   }
}
