namespace CVAS.FileSystem
{
    /// <summary>
    /// Represents any exception relating to the <see cref="FileSystem"/> namespace.
    /// </summary>
    /// <param name="message"></param>
    public class FileSystemException(string message) : Exception(message);
}
