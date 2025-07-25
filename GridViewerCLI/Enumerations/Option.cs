
using System.ComponentModel;
using System.Reflection;


namespace GridViewerCLI.Enumerations;

public enum Option
{
   [Description("Set grid width")] Width,
   [Description("Export formats: csv, json, png, bmp")] Format,
   // ...
}
