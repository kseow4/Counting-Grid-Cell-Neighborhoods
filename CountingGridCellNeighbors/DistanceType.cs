
using System.ComponentModel;

namespace CountingGridCellNeighbors
{


   public enum DistanceType
   {
      [Description("Manhattan Distance")]
      Manhattan,
      [Description("Euclidean Distance")]
      Euclidean,
      [Description("Chebyshev Distance")]
      Chebyshev,
   }
}