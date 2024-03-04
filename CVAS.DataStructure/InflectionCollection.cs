using CVAS.AudioEngine;
using System.Collections;

namespace CVAS.DataStructure
{
    public class InflectionCollection : ICollection<Inflection>
    {
        public int Count => inflections.Count();

        public bool IsReadOnly => false;

        private List<Inflection> inflections = new List<Inflection>();

        public IAudioClip this[InflectionType type]
        {
            get
            {
                var ret = inflections.Where(x => x.InflectionType == type);
                if (ret.Count() == 0) throw new KeyNotFoundException();
                return ret.First().AudioClip;
            }
        }

        public void Add(Inflection item)
        {
            inflections.Add(item);
        }

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
    }
}
