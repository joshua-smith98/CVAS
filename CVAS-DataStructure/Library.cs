namespace CVAS.DataStructure
{
    public class Library
    {
        private List<Phrase> _phrases = new List<Phrase>();
        public Phrase[] phrases => _phrases.ToArray(); //public get only

        public Library(Phrase[] phrases)
        {
            _phrases.AddRange(phrases);
        }

        /// <summary>
        /// Finds the <see cref="Phrase"/> in this library that contains <paramref name="str"/>.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The specified <see cref="Phrase"/> or null if unsuccessful.</returns>
        public Phrase? FindPhrase(string str)
        {
            foreach(Phrase phr in _phrases)
            {
                if (phr.str == str) return phr;
            }

            return null;
        }
    }
}
