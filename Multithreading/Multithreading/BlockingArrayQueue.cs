using System.Threading;

namespace Multithreading
{
    public class BlockingArrayQueue<T>
    {
        private T[] _array;
        private int _size;
        private int _head;
        private int _tail;
        private readonly SpinWait spinner;

        public BlockingArrayQueue(int capacity = 256)
        {
            _array = new T[capacity];
        }

        public bool TryEnqueue(T item)
        {
            lock (_array)
            {
                if (_size == _array.Length)
                {
                    return false;
                }
                _array[_tail] = item;
                _tail = (_tail + 1) % _array.Length;
                _size++;
                return true;
            }
        }

        public bool TryDequeue(out T result)
        {
            lock (_array)
            {
                if (_size == 0)
                {
                    result = default(T);
                    return false;
                }
                result = _array[_head];
                _array[_head] = default(T);
                _head = (_head + 1) % _array.Length;
                _size--;
                return true;
            }
        }

        public void Enqueue(T res)
        {
            while (!TryEnqueue(res))
            {
                spinner.SpinOnce();
            }
        }

        public T Dequeue()
        {
            T res;
            while (!TryDequeue(out res))
            {
                spinner.SpinOnce();
            }
            return res;
        }

        public void Clear()
        {
            lock(_array)
            {
                if (_size != 0)
                {
                    _array = new T[0];
                    _size = 0;
                    _head = 0;
                    _head = _tail;
                }
            }
        }
    }
}
