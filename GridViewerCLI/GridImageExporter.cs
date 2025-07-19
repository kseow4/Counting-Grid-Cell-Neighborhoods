using System.Drawing;
using CountingGridCellNeighbors;

public static class GridImageExporter
{
    public static void ExportToImage(Cell[,] grid, string filePath, int cellSize = 20)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);
        using var bmp = new Bitmap(width * cellSize, height * cellSize);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.White);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = grid[y, x];
                Brush brush = cell.IsPositive
                    ? Brushes.Red
                    : cell.IsNeighbor
                        ? Brushes.LightBlue
                        : Brushes.White;

                g.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                g.DrawRectangle(Pens.Gray, x * cellSize, y * cellSize, cellSize, cellSize);
            }
        }

        bmp.Save(filePath);
        Console.WriteLine($"✅ Image exported to: {filePath}");
    }
}
