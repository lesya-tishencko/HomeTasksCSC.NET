using System.Threading;

namespace Multithreading
{
    public class BlockingArrayQueue<T>
    {
        private T[] array;
        private int size;
        private int head;
        private int tail;

        public BlockingArrayQueue(int capacity = 256)
        {
            array = new T[capacity];
        }

        public bool TryEnqueue(T item)
        {
            lock (array)
            {
                if (size == array.Length)
                {
                    return false;
                }
                array[tail] = item;
                tail = (tail + 1) % array.Length;
                size++;
                return true;
            }
        }

        public bool TryDequeue(out T result)
        {
            lock (array)
            {
                if (size == 0)
                {
                    result = default(T);
                    return false;
                }
                result = array[head];
                array[head] = default(T);
                head = (head + 1) % array.Length;
                size--;
                return true;
            }
        }

        public void Enqueue(T res)
        {
            while (!TryEnqueue(res))
            {
                Thread.Yield();
            }
        }

        public T Dequeue()
        {
            T res;
            while (!TryDequeue(out res))
            {
                Thread.Yield();
            }
            return res;
        }

        public void Clear()
        {
            lock(array)
            {
                if (size != 0)
                {
                    size = 0;
                    head = 0;
                    head = tail;
                }
            }
        }
    }
}
