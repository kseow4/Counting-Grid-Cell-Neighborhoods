

using System;
using System.Text;

namespace CountingGridCellNeighbors;

using System.Collections.Generic;
using System.Text;
using System.Text.Json;

public static class GridPrinter
{
    public static void Print(Cell[,] grid, bool useColor = true)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = grid[y, x];
                if (useColor)
                {
                    if (cell.IsPositive) Console.ForegroundColor = ConsoleColor.Red;
                    else if (cell.IsNeighbor) Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.DarkGray;
                }

                Console.Write(cell.IsPositive ? "P " : cell.IsNeighbor ? "N " : ". ");

                if (useColor) Console.ResetColor();
            }
            Console.WriteLine();
        }
    }

    public static string ToStringOutput(Cell[,] grid)
    {
        var output = new StringBuilder();
        int height = grid.GetLength(0), width = grid.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = grid[y, x];
                output.Append(cell.IsPositive ? "P " : cell.IsNeighbor ? "N " : ". ");
            }
            output.AppendLine();
        }
        return output.ToString();
    }

    public static string ToCsv(Cell[,] grid)
    {
        var sb = new StringBuilder();
        int height = grid.GetLength(0), width = grid.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            var row = new List<string>();
            for (int x = 0; x < width; x++)
            {
                var cell = grid[y, x];
                row.Add(cell.IsPositive ? "P" : cell.IsNeighbor ? "N" : ".");
            }
            sb.AppendLine(string.Join(",", row));
        }
        return sb.ToString();
    }

    public static string ToJson(Cell[,] grid)
    {
        int height = grid.GetLength(0), width = grid.GetLength(1);
        var cells = new List<object>();

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var cell = grid[y, x];
                cells.Add(new {
                    X = x,
                    Y = y,
                    IsPositive = cell.IsPositive,
                    IsNeighbor = cell.IsNeighbor
                });
            }

        return JsonSerializer.Serialize(cells, new JsonSerializerOptions { WriteIndented = true });
    }
}
