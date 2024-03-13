namespace CVAS.TerminalNS
{
    /// <summary>
    /// Determines which block type is currently active in the <see cref="Terminal"/>.
    /// </summary>
    internal enum TerminalBlockStatus
    {
        NoBlockActive,
        MessageBlockActive,
        ReportBlockActive
    }
}
