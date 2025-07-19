
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CountingGridCellNeighbors
{
   public static class GridGenerator
   {
      private static readonly Random random = new Random();

      public static Cell[,] Generate(int height, int width, int numberOfPositives)
      {
         Cell[,] cells = new Cell[height, width];

         for (int y = 0; y < height; y++)
         {
            for (int x = 0; x < width; x++)
            {
               cells[y, x] = new Cell(y, x);
            }
         }

         HashSet<(int, int)> used = new HashSet<(int, int)>();
         for (int i = 0; i < numberOfPositives; i++)
         {
            int x, y;
            do
            {
               y = random.Next(height);
               x = random.Next(width);
            }
            while (!used.Add((y, x)));

            cells[y, x] = new Cell(y, x, Math.Abs(random.Next()));
         }

         return cells;
      }

      public static Cell[,] Generate(int height, int width, List<(int y, int x)> positivePositions)
      {
         Cell[,] cells = new Cell[height, width];

         for (int y = 0; y < height; y++)
         {
            for (int x = 0; x < width; x++)
            {
               cells[y, x] = new Cell(x, y);
            }
         }

         foreach (var (y, x) in positivePositions)
         {
            if (y >= height || x >= width || y < 0 || x < 0)
                   throw new ArgumentOutOfRangeException($"Cell ({y}, {x}) is out of bounds.");

               cells[y, x] = new Cell(x, y, Math.Abs(random.Next())); // Mark as positive
         }

         return cells;
      }
   }
}