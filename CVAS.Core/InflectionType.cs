namespace CVAS.Core
{
    /// <summary>
    /// Represents the inflection type of a spoken word, i.e. whether it is spoken in the middle of or at the end of a sentence.
    /// </summary>
    public enum InflectionType
    {
        Null, // Used in a SpokenPhrase when its previous Phrase had no inflections - only occurs when a Phrase can't be found
        Middle,
        End
    }
}
