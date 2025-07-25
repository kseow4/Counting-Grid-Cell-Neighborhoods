// using System.Drawing;
// using CountingGridCellNeighbors;

// public static class GridImageExporter
// {
//     public static void ExportToImage(Cell[,] grid, int cellSize = 20, string filePath = "grid")
//     {
//         int height = grid.GetLength(0);
//         int width = grid.GetLength(1);
//         using var bmp = new Bitmap(width * cellSize, height * cellSize);
//         using var g = Graphics.FromImage(bmp);
//         g.Clear(Color.White);

//         for (int y = 0; y < height; y++)
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 var cell = grid[y, x];
//                 Brush brush = cell.IsPositive
//                     ? Brushes.Red
//                     : cell.IsNeighbor
//                         ? Brushes.LightBlue
//                         : Brushes.White;

//                 g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
//                 g.DrawRectangle(Pens.Gray, x * cellSize, y * cellSize, cellSize, cellSize);
//             }
//         }

//         bmp.Save(filePath);
//         Console.WriteLine($"✅ Image exported to: {filePath}");
//     }
// }


using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using CountingGridCellNeighbors;

public static class GridImageExporter
{
    public static void ExportToImage(Cell[,] grid,int cellSize, string filePath)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        using Image<Rgba32> image = new(width * cellSize, height * cellSize, Color.White);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = grid[y, x];
                var color = cell.IsPositive ? Color.Red :
                            cell.IsNeighbor ? Color.LightBlue :
                            Color.White;

                Rectangle rectangle = new(x * cellSize, y * cellSize, cellSize, cellSize);
                image.Mutate(ctx => ctx.Fill(color, rectangle));
                image.Mutate(ctx => ctx.Draw(Color.Gray, 1, rectangle));
            }
        }

        // Infer format from extension
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        IImageEncoder encoder = extension switch
        {
            ".png" => new SixLabors.ImageSharp.Formats.Png.PngEncoder(),
            ".jpeg" or ".jpg" => new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder(),
            ".bmp" => new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder(),
            _ => throw new ArgumentException($"Unsupported image format: {extension}")
        };
        image.Save(filePath, encoder);

        image.Save(filePath);
        Console.WriteLine($"✅ Image exported to: {filePath}");
    }
}
