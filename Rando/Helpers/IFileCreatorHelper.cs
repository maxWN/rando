namespace Rando.Helpers;

public interface IFileCreatorHelper
{
    Task CreateFileAsync(string path, string fileContent, string fileType);
}