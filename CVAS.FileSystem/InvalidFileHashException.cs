namespace CVAS.FileSystem
{
    /// <summary>
    /// Represents any error caused by an invalid hash in a file. Usually used to check if some data has changed.
    /// </summary>
    /// <param name="message"></param>
    internal class InvalidFileHashException(string message) : FileSystemException(message);
}
