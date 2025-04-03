


using System.Security.Cryptography.X509Certificates;

public class Matrix 
{

   public static int Height { get; private set; } = 1;
   public static int Width { get; private set; } = 1;
   public static Cell[,] Grid { get; private set; }= new Cell[Height,Width];

   public Matrix(int height, int width) {
      Height = height; Width = width; 
      

   }

   override public string ToString() {
      StringWriter writer= new StringWriter();
      string output = " " + new string('﹎', Width + 1);
      for (int i = 0; i < Height; i++) {
         output = "\n|";
         for (int j = 0; j < Width; j++) {
            output += $" {Grid[i, j].IsPositive}";
         } 
         output += " |";
      }
      return output;
   }
}