namespace CVAS.FileSystem
{
    /// <summary>
    /// Represents any error caused by an invalid or out-of-date file version.
    /// </summary>
    /// <param name="message"></param>
    internal class InvalidFileVersionException(string message) : Exception(message);
}
