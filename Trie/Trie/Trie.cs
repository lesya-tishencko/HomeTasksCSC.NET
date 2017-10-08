using System.Collections.Generic;

namespace Trie
{
    public class Trie : ITrie
    {
        public bool Add(string element)
        {
            if (element == "")
            {
                return false;
            }

            var isAdded = Contains(element);
            if (!isAdded)
            {
                var curr = _root;
                foreach (char ch in element)
                {
                    if (!curr.next.ContainsKey(ch))
                    {
                        curr.next.Add(ch, new Vertex());
                    }
                    ++curr.postfixCount;
                    curr = curr.next[ch];
                }
                ++curr.postfixCount;
                curr.isTerminal = true;
            }
            return !isAdded;
        }

        public bool Contains(string element)
        {
            if (element == "")
            {
                return false;
            }

            var curr = _root;
            foreach (char ch in element)
            {
                if (!curr.next.ContainsKey(ch))
                {
                    return false;
                }
                curr = curr.next[ch];
            }
            return curr.isTerminal;
        }

        public bool Remove(string element)
        {
            if (!Contains(element))
            {
                return false;
            }

            var curr = _root;
            --curr.postfixCount;
            foreach (char ch in element)
            {
                --curr.next[ch].postfixCount;
                if (curr.next[ch].postfixCount == 0)
                {
                    curr.next.Remove(ch);
                    return true;
                }
                curr = curr.next[ch];
            }
            curr.isTerminal = false;
            return true;
        }

        public int Size() => _root.postfixCount;

        public int HowManyStartsWithPrefix(string prefix)
        {
            if (prefix == "")
            {
                return Size();
            }

            var curr = _root;
            foreach (char ch in prefix)
            {
                if (!curr.next.ContainsKey(ch))
                {
                    return 0;
                }
                curr = curr.next[ch];
            }
            return curr.postfixCount;
        }

        private class Vertex
        {
            public Dictionary<char, Vertex> next = new Dictionary<char, Vertex>();
            public int postfixCount;
            public bool isTerminal;
        }
        
        private Vertex _root = new Vertex();
    }
}
