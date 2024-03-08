namespace CVAS.FileSystem
{
    /// <summary>
    /// Thrown when a certain type of transmutation is not allowed by a derivative of <see cref="IFile{T}"/> or <see cref="IFolder{T}"/>.
    /// </summary>
    public class TransmutationNotAllowedException : Exception
    {

    }
}
