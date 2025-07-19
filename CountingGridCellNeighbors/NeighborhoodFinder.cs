

using System;

namespace CountingGridCellNeighbors
{
   public static class NeighborhoodFinder
   {
      /// <summary>
      /// Gets the wrapping value.
      /// </summary>
      /// <param name="index"></param>
      /// <param name="length"></param>
      /// <returns></returns>
      public static int Wrap(int index, int length) => (index + length) % length;

      /// <summary>
      /// Analyzes the grid (matrix) finding positive cells and then calculating their associated neighboring cells within the specified distance "n"
      /// </summary>
      /// <param name="grid"></param>
      /// <param name="n"></param>
      /// <param name="distanceType"></param>
      /// <param name="wrap"></param>
      /// <returns>
      /// The number of cells (neighbors) within range of the positive cells
      /// </returns>
      /// <exception cref="NotSupportedException"></exception>
      public static int FindNeighbors(Cell[,] grid, int n = 3, DistanceType distanceType = DistanceType.Manhattan, bool wrap = true)
      {
         int height = grid.GetLength(0);
         int width = grid.GetLength(1);
         int count = 0;

         double distanceFunction(Cell a, Cell b) => (distanceType, wrap) switch
         {
            (DistanceType.Manhattan, false) => Distance.Manhattan(a, b),
            (DistanceType.Manhattan, true) => Distance.Manhattan(a, b, height, width),
            (DistanceType.Euclidean, false) => Distance.Euclidean(a, b),
            (DistanceType.Euclidean, true) => Distance.Euclidean(a, b, height, width),
            (DistanceType.Chebyshev, false) => Distance.Chebyshev(a, b),
            (DistanceType.Chebyshev, true) => Distance.Chebyshev(a, b, height, width),
            _ => throw new NotSupportedException()
         };

         for (int y = 0; y < height; y++)
         {
            for (int x = 0; x < width; x++)
            {
               Cell origin = grid[y, x];
               if (!origin.IsPositive) continue;

               // Include the positive cell itself
               if (!origin.IsNeighbor)
               {
                  origin.IsNeighbor = true;
                  count++;
               }

               for (int dy = -n; dy <= n; dy++)
               {
                  for (int dx = -n; dx <= n; dx++)
                  {
                     int ny = wrap ? Wrap(y + dy, height) : y + dy;
                     int nx = wrap ? Wrap(x + dx, width) : x + dx;

                     if (ny < 0 || ny >= height || nx < 0 || nx >= width) continue;

                     Cell cell = grid[ny, nx];
                     double distance = distanceFunction(origin, cell);

                     if (distance <= n && !cell.IsNeighbor)
                     {
                        cell.IsNeighbor = true;
                        count++;
                     }
                  }
               }
            }
         }
         return count;
      }
   }
}