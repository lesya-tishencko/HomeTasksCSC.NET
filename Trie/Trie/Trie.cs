namespace Trie
{
    public class Trie
    {
        public bool Add(string element)
        {
            if (element == "") return false;

            Vertex curr = root;
            bool notAdded = false;
            foreach (char ch in element.ToCharArray())
            {
                if (curr.next[ch] == null)
                {
                    notAdded = true;
                    break;
                }
                curr = curr.next[ch];
            }
            if (!notAdded && curr.terminal) return false;

            curr = root;
            foreach (char ch in element.ToCharArray())
            {
                if (curr.next[ch] == null)
                {
                    curr.next[ch] = new Vertex();
                }
                curr.postfixCount++;
                curr = curr.next[ch];
            }
            curr.postfixCount++;
            curr.terminal = true;
            return true;
        }

        public bool Contains(string element)
        {
            if (element == "") return false;

            Vertex curr = root;
            foreach (char ch in element.ToCharArray())
            {
                if (curr.next[ch] == null) return false;
                curr = curr.next[ch];
            }
            return curr.terminal;
        }

        public bool Remove(string element)
        {
            if (!Contains(element)) return false;

            Vertex curr = root;
            curr.postfixCount--;
            foreach (char ch in element.ToCharArray())
            {
                curr.next[ch].postfixCount--;
                if (curr.next[ch].postfixCount == 0)
                {
                    curr.next[ch] = null;
                    return true;
                }
                curr = curr.next[ch];
            }
            curr.terminal = false;
            return true;
        }

        public int Size() => root.postfixCount;

        public int HowManyStartsWithPrefix(string prefix)
        {
            if (prefix == "") return Size();

            Vertex curr = root;
            foreach (char ch in prefix.ToCharArray())
            {
                if (curr.next[ch] == null) return 0;
                curr = curr.next[ch];
            }
            return curr.postfixCount;
        }

        private class Vertex
        {
            public Vertex[] next = new Vertex[upperBoundTableSize];
            public int postfixCount;
            public bool terminal;
        }

        private const int upperBoundTableSize = 256;
        private Vertex root = new Vertex();
    }
}
