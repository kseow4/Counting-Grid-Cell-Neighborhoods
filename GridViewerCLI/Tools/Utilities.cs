
namespace GridViewerCLI.Tools;

public static class Utilities
{

   static readonly Dictionary<string, string[][]> HelpTable = new()
   {
      { "Grid Options", [
         ["--width=10", "-wt, -x", "Set grid width"],
         ["--height=10", "-ht, -y", "Set grid height"],
         ["--positives=5", "-p", "Set # of positive cells"],
         ["--n=1", "-n", "Set neighborhood distance"],
         ["--dist=chebyshev", "-d", "Distance type: manhattan, euclidean, chebyshev"],
         ["--wrap", "-w", "Enable wraparound edges"],
      ]},
      { "Export Options", [
         ["--format=csv,json", "-f", "Export file formats"],
         ["--out=mygrid", "-o", "Base filename for exports"],
         ["--image", "-i", "Export PNG image"],
         ["--json", "-j", "Export JSON file"],
         ["--csv", "-c", "Export CSV file"],
         ["--graphic", "-g", "Export BMP graphic"],
         ["--cellsize=10", "-cs", "Cell size for image export"],
      ]},
      { "Display Options", [
         ["--no-print", "-np", "Skip printing grid and stats"],
         ["--stats-only", "-s", "Print only statistics (no grid)"],
         ["--no-color", "-nc", "Disable colored console output"],
         ["--no-export", "-ne", "Do not export any files"],
         ["--no-stats", "-ns", "Suppress statistics output"],
         ["--no-prompt", "-npr", "Disable all prompts (non-interactive mode)"],
         ["--no-header", "-nh", "Suppress grid coordinate headers"],
      ]},
      { "Control Flags", [
         ["--auto", "-a", "Skip prompts and auto-export"],
         ["--help", "-h", "Displays help menu"],
      ]},
   };

   public static void PrintHelpBasic()
   {

   }

   public static void PrintHelp()
   {
      static void PrintSection(string header, string[][] items)
      {
         Console.ForegroundColor = ConsoleColor.Cyan;
         Console.WriteLine($"\n{header,-20} | {"Shorthand(s)",-12} | Description");
         Console.WriteLine(new string('-', 80));
         Console.ResetColor();

         Console.ForegroundColor = ConsoleColor.Green;
         foreach (string[] item in items)
         {
            Console.WriteLine($"  {item[0],-18} |  {item[1],-11} |  {item[2]}");
         }
         Console.ResetColor();
      }

      foreach (KeyValuePair<string, string[][]> section in HelpTable) PrintSection(section.Key, section.Value);
   }


   
}