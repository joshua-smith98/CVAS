namespace CVAS.FileSystem
{
    /// <summary>
    /// Represents any validation error with the formatting or structure of a folder.
    /// </summary>
    /// <param name="message"></param>
    internal class InvalidFolderFormatException(string message) : FileSystemException(message);
}
