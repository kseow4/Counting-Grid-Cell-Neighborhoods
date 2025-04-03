public class Cell
{
   public float Value { get; protected set; }
   public static int X { get; protected set; }
   public static int Y { get; protected set; }
   public bool IsPositive => Value >= 0; 
   public bool IsNegative => Value < 0;
   public bool Counted { get; set; } = false;

   public Cell(float value, int x, int y) => (Value, X, Y) = (value, x , y);
}


