using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Strings
{
    public class StringsLoader
    {
        private Dictionary<string, string> value;
        public Dictionary<string, string> Value { get => value; }

        public void Load(string key) {
            if (InMemoryStrings.Strings.TryGetValue(key, out var strs))
            {
                value = strs;
            }
            else {
                throw new ArgumentException($"Cannot load strings with key of '{key}'");
            }
        }
             

    }
}
