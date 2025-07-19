
namespace CountingGridCellNeighbors;


/// <summary>
/// 
/// </summary>
public static class CellFactory
{
   public static Cell[,] MakeCells(float[,] matrix)
   {

      int height = matrix.GetLength(0);
      int width = matrix.GetLength(1);
      Cell[,] cells = new Cell[height, width];

      for (int y = 0; y < height; y++)
      {
         for (int x = 0; x < width; x++)
         {
            cells[y, x] = new Cell(x, y, matrix[y, x]);
         }
      }
      return cells;
   }
}