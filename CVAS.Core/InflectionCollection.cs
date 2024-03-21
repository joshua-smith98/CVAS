using CVAS.AudioEngineNS;
using System.Collections;

namespace CVAS.Core
{
    /// <summary>
    /// A collection of <see cref="Inflection"/>s. Used in <see cref="Phrase"/>.
    /// </summary>
    public class InflectionCollection : ICollection<Inflection>, IDisposable
    {
        public int Count => inflections.Count;

        public bool IsReadOnly => false;

        private List<Inflection> inflections = new();

        /// <summary>
        /// Gets the <see cref="IAudioClip"/> associated with the given <see cref="InflectionType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public IAudioClip this[InflectionType type]
        {
            get
            {
                var ret = inflections.Where(x => x.InflectionType == type);
                if (!ret.Any()) throw new KeyNotFoundException();
                return ret.First().AudioClip;
            }
        }

        public void Add(Inflection item)
        {
            inflections.Add(item);
        }

        /// <summary>
        /// Adds a range of items to this <see cref="InflectionCollection"/>.
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(Inflection[] items)
        {
            inflections.AddRange(items);
        }

        public void Clear()
        {
            inflections.Clear();
        }

        public bool Contains(Inflection item)
        {
            return inflections.Contains(item);
        }

        public void CopyTo(Inflection[] array, int arrayIndex)
        {
            inflections.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Inflection> GetEnumerator()
        {
            return inflections.GetEnumerator();
        }

        public bool Remove(Inflection item)
        {
            return inflections.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return inflections.GetEnumerator();
        }

        public void Dispose()
        {
            for (int i = 0; i < inflections.Count; i++)
            {
                inflections[i]?.Dispose();
                inflections[i] = null!;
            }

            inflections = null!;
            GC.SuppressFinalize(this);
        }
    }
}
