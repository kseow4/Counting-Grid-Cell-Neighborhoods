
using System;

namespace CountingGridCellNeighbors;


/// <summary>
/// 
/// </summary>
public static class Distance
{
   /// <summary>
   /// Calculates the Wrapping Manhattan Distance Value.
   /// </summary>
   /// <param name="y1"></param>
   /// <param name="x1"></param>
   /// <param name="y2"></param>
   /// <param name="x2"></param>
   /// <param name="height"></param>
   /// <param name="width"></param>
   /// <returns>
   /// Integer value representing the Manhattan Distance.
   /// </returns>
   public static int Manhattan(int y1, int x1, int y2, int x2, int height, int width) => Math.Min(Math.Abs(y1 - y2), height - Math.Abs(y1 - y2)) + Math.Min(Math.Abs(x1 - x2), width - Math.Abs(x1 - x2));

   /// <summary>
   /// Calculates the Wrapping Manhattan Distance Value.
   /// </summary>
   /// <param name="a"></param>
   /// <param name="b"></param>
   /// <param name="height"></param>
   /// <param name="width"></param>
   /// <returns>
   /// Integer value representing the Manhattan Distance.
   /// </returns>
   public static int Manhattan(Cell a, Cell b, int height, int width) => Manhattan(a.Y, a.X, b.Y, b.X, height, width);

   /// <summary>
   /// Calculates the Non-Wrapping Manhattan Distance Value.
   /// </summary>
   /// <param name="y1"></param>
   /// <param name="x1"></param>
   /// <param name="y2"></param>
   /// <param name="x2"></param>
   /// <returns>
   /// Integer value representing the Manhattan Distance.
   /// </returns>
   public static int Manhattan(int y1, int x1, int y2, int x2) => Math.Abs(y1 - y2) + Math.Abs(x1 - x2);

   /// <summary>
   /// Calculates the Non-Wrapping Manhattan Distance Value.
   /// </summary>
   /// <param name="a"></param>
   /// <param name="b"></param>
   /// <returns>
   /// Integer value representing the Manhattan Distance.
   /// </returns>
   public static int Manhattan(Cell a, Cell b) => Manhattan(a.Y, a.X, b.Y, b.X);

   /// <summary>
   /// Calculates the Wrapping Euclidean Distance Value.
   /// </summary>
   /// <param name="y1"></param>
   /// <param name="x1"></param>
   /// <param name="y2"></param>
   /// <param name="x2"></param>
   /// <param name="height"></param>
   /// <param name="width"></param>
   /// <returns>
   /// Double value representing the Euclidean Distance.
   /// </returns>
   public static double Euclidean(int y1, int x1, int y2, int x2, int height, int width) => Math.Sqrt(
      Math.Pow(Math.Min(Math.Abs(y1 - y2), height - Math.Abs(y1 - y2)), 2) +
      Math.Pow(Math.Min(Math.Abs(x1 - x2), width - Math.Abs(x1 - x2)), 2));

   /// <summary>
   /// Calculates the Wrapping Euclidean Distance Value.
   /// </summary>
   /// <param name="a"></param>
   /// <param name="b"></param>
   /// <param name="height"></param>
   /// <param name="width"></param>
   /// <returns>
   /// Double value representing the Euclidean Distance.
   /// </returns>
   public static double Euclidean(Cell a, Cell b, int height, int width) => Euclidean(a.Y, a.X, b.Y, b.X, height, width);

   /// <summary>
   /// Calculates the Non-Wrapping Euclidean Distance Value.
   /// </summary>
   /// <param name="y1"></param>
   /// <param name="x1"></param>
   /// <param name="y2"></param>
   /// <param name="x2"></param>
   /// <returns>
   /// Double value representing the Euclidean Distance.
   /// </returns>
   public static double Euclidean(int y1, int x1, int y2, int x2) => Math.Sqrt(
      Math.Pow(y1 - y2, 2) + Math.Pow(x1 - x2, 2));

   /// <summary>
   /// Calculates the Non-Wrapping Euclidean Distance Value.
   /// </summary>
   /// <param name="a"></param>
   /// <param name="b"></param>
   /// <returns>
   /// Double value representing the Euclidean Distance.
   /// </returns>
   public static double Euclidean(Cell a, Cell b) => Euclidean(a.Y, a.X, b.Y, b.X);

   /// <summary>
   /// 
   /// </summary>
   /// <param name="y1"></param>
   /// <param name="x1"></param>
   /// <param name="y2"></param>
   /// <param name="x2"></param>
   /// <param name="height"></param>
   /// <param name="width"></param>
   /// <returns></returns>
   public static int Chebyshev(int y1, int x1, int y2, int x2, int height, int width) => Math.Max(
      Math.Min(Math.Abs(y1 - y2), height - Math.Abs(y1 - y2)),
      Math.Min(Math.Abs(x1 - x2), width - Math.Abs(x1 - x2)));

   /// <summary>
   /// 
   /// </summary>
   /// <param name="a"></param>
   /// <param name="b"></param>
   /// <param name="height"></param>
   /// <param name="width"></param>
   /// <returns></returns>
   public static double Chebyshev(Cell a, Cell b, int height, int width) => Chebyshev(a.Y, a.X, b.Y, b.X, height, width);

   /// <summary>
   /// 
   /// </summary>
   /// <param name="y1"></param>
   /// <param name="x1"></param>
   /// <param name="y2"></param>
   /// <param name="x2"></param>
   /// <returns></returns>
   public static int Chebyshev(int y1, int x1, int y2, int x2) => Math.Max(
      Math.Abs(y1 - y2), Math.Abs(x1 - x2));

   /// <summary>
   /// 
   /// </summary>
   /// <param name="a"></param>
   /// <param name="b"></param>
   /// <returns></returns>
   public static double Chebyshev(Cell a, Cell b) => Chebyshev(a.Y, a.X, b.Y, b.X);


}

