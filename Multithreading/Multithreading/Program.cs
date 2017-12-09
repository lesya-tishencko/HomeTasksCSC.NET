using System.Threading;

namespace Multithreading
{
    class Program
    {
        static void Main(string[] args)
        {
            var philosophers = new Philosophers();
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(philosophers.Philosopher);
                thread.Start(i);
            }
        }
    }
}
