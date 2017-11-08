using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading.Tests
{
    [TestClass]
    public class TestsBlockingArrayQueue
    {
        private BlockingArrayQueue<int> array;
        private volatile bool finished;
        private int sum;
        private Task[] tasks;

        [TestInitialize]
        public void Initialize()
        {
            array = new BlockingArrayQueue<int>(100);
            finished = false;
            tasks = new Task[10];
        }

        private void Setter()
        {
            int nxt;
            int acc = 0;
            int limit = 100;
            while ((nxt = Interlocked.Increment(ref acc)) < limit)
            {
                array.Enqueue(nxt);
            }
        }

        private void Sum()
        {
            while (true)
            {
                int cur;
                if (array.TryDequeue(out cur))
                {
                    sum += cur;
                }
                else if (finished)
                {
                    break;
                }
            }
        }

        private void Push()
        {
            int nxt;
            int acc = 0;
            while ((nxt = Interlocked.Increment(ref acc)) <= 100)
            {
                array.Enqueue(nxt);
            }
        }

        [TestMethod]
        public void TestTryEnqueue()
        {
            for(int i = 0; i < 100; ++i)
            {
                Assert.IsTrue(array.TryEnqueue(i));
            }
            Assert.IsFalse(array.TryEnqueue(1));
            array.Clear();
        }

        [TestMethod]
        public void TestTryDequeue()
        {
            for (int i = 0; i < 100; i++)
            {
                array.TryEnqueue(i);
            }
            for (int i = 0; i < 100; i++)
            {
                int result;
                Assert.IsTrue(array.TryDequeue(out result));
                Assert.AreEqual(i, result);
            }
        }

        [TestMethod]
        public void TestEnqueue()
        {
            for (int i = 0; i < 10; i++) tasks[i] = Task.Factory.StartNew(Push, TaskCreationOptions.LongRunning);
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(i + 1, array.Dequeue());
            }
            array.Clear();
        }

        [TestMethod]
        public void TestDequeue()
        {
            for (int i = 0; i < 100; i++)
            {
                array.Enqueue(i);
            }
            for (int i = 0; i < 10; i++) tasks[i] = Task.Factory.StartNew(Sum, TaskCreationOptions.LongRunning);
            new TaskFactory().ContinueWhenAll(tasks, _ => finished = true);
            int result = 0;
            for(int i = 0; i < 100; i++)
            {
                result += i;
            }
            Assert.AreEqual(result, sum);
        }

        [TestMethod]
        public void TestClear()
        {
            for (int i = 0; i < 10; i++) tasks[i] = Task.Factory.StartNew(() => array.Enqueue(100), TaskCreationOptions.LongRunning);
            Assert.AreEqual(100, array.Dequeue());
            for (int i = 0; i < 10; i++) tasks[i] = Task.Factory.StartNew(() => array.Clear(), TaskCreationOptions.LongRunning);
            int result;
            Assert.IsFalse(array.TryDequeue(out result));
        }
    }
}