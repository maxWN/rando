
using System.Text;
using Rando.Common;

namespace Rando.Helpers;

public class FileCreatorHelper : IFileCreatorHelper
{
    /// <summary>
    /// Creates a new file that contains Random API data
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileContent"></param>
    /// <param name="fileType"></param>
    public void CreateFile(string path, string fileContent, string fileType)
    {
        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(fileContent) || string.IsNullOrWhiteSpace(fileType)) {
            Console.WriteLine(AppConstants.USER_DIRECTIONS);
            return;
        }

        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException();
        }

        try
        {
            // Create the file, or overwrite if the file exists.
            using (FileStream fs = File.Create($"{path}{fileType}"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(fileContent);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"{ex.Message} - Please run rando as an admin to create your file.");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
}
