using System.Threading;

namespace Multithreading
{
    public class Fork
    {
        public int Id { get; }
        private readonly Mutex _mutex;

        public Fork(int id)
        {
            Id = id;
            _mutex = new Mutex(false);
        }
        
        public bool Take()
        {
            _mutex.WaitOne();
            return true;
        }

        public void Put()
        {
            _mutex.ReleaseMutex();
        }
    }
}
