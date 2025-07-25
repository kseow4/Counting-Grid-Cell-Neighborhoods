using System;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using System.Drawing.Imaging;

namespace GridViewerCLI.Enumerations;

public enum FileExtension
{
   [Description(".json")] JSON,

   [Description(".csv")] CSV,

   [Description(".bmp")] BMP,

   [Description(".png")] PNG,

   [Description(".jpeg")] JPEG,

}
public static class FileExtensionHelper
{
   public static string GetExtension(this FileExtension extension) => extension.GetType().GetField(extension.ToString())?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? extension.ToString();

   // public static FileExtension FromExtension(string extension) => Enum.GetValues<FileExtension>().Cast<FileExtension>().FirstOrDefault(extension => Equals(extension.GetExtension(), (extension, StringComparison.OrdinalIgnoreCase)));

   public static FileExtension FromExtension(string extension)
   {
      if (string.IsNullOrWhiteSpace(extension))
         throw new ArgumentException("Extension cannot be null or empty.");

      string normalized = extension.Trim().ToLower();
      if (!normalized.StartsWith("."))
         normalized = "." + normalized;

      foreach (FileExtension ext in Enum.GetValues<FileExtension>())
      {
         if (string.Equals(ext.GetExtension(), normalized, StringComparison.OrdinalIgnoreCase))
            return ext;
      }

      throw new ArgumentException($"Unsupported file extension: {extension}");
   }


   // Map to System.Drawing.Imaging.ImageFormat
   public static ImageFormat? ToImageFormat(this FileExtension ext) =>
       ext switch
       {
          FileExtension.BMP => ImageFormat.Bmp,
          FileExtension.PNG => ImageFormat.Png,
          FileExtension.JPEG => ImageFormat.Jpeg,
          _ => throw new NotSupportedException($"Image format not supported for: {ext}")
         //  _ => null
       };

   public static string ToPrettyString(this FileExtension ext) => ext.GetExtension();

   public static bool TryFromExtension(string extension, out FileExtension result)
   {
      result = default;
      if (string.IsNullOrWhiteSpace(extension))
         return false;

      string normalized = extension.Trim().ToLower();
      if (!normalized.StartsWith("."))
         normalized = "." + normalized;

      foreach (FileExtension ext in Enum.GetValues<FileExtension>())
      {
         if (string.Equals(ext.GetExtension(), normalized, StringComparison.OrdinalIgnoreCase))
         {
            result = ext;
            return true;
         }
      }

      return false;
   }


   public static string GetMimeType(this FileExtension ext) => ext switch
   {
      FileExtension.JSON => "application/json",
      FileExtension.CSV => "text/csv",
      FileExtension.BMP => "image/bmp",
      FileExtension.PNG => "image/png",
      FileExtension.JPEG => "image/jpeg",
      _ => "application/octet-stream"
   };

}