namespace Trie
{
    public interface ITrie
    {
        bool Add(string element);
        bool Contains(string element);
        bool Remove(string element);
        int Size();
        int HowManyStartsWithPrefix(string prefix);
    }
}
