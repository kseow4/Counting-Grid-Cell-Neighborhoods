namespace CountingGridCellNeighbors;


/// <summary>
/// 
/// </summary>
public class Cell
{
   public float Value { get; protected set; } = -1;
   public bool IsNeighbor { get; set; } = false;
   public int Y { get; protected set; }
   public int X { get; protected set; }
   public bool IsPositive => Value >= 0;
   public Cell(int y, int x, float value = -1, bool isNeighbor = false) => (X, Y, Value, IsNeighbor) = (x, y, value, isNeighbor);
   public override string ToString() => IsPositive ? "▩" : IsNeighbor ? "☒" : "☐";
}


