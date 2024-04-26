namespace CVAS.FileSystem
{
    /// <summary>
    /// Represents any validation error with the formatting or structure of a file.
    /// </summary>
    /// <param name="message"></param>
    internal class InvalidFileFormatException(string message) : FileSystemException(message);
}
