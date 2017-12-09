using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading.Tests
{
    [TestClass]
    public class TestsBlockingArrayQueue
    {
        private BlockingArrayQueue<int> _array;
        private volatile bool _finished;
        private int _sum;
        private Task[] _tasks;

        [TestInitialize]
        public void Initialize()
        {
            _array = new BlockingArrayQueue<int>(100);
            _finished = false;
            _tasks = new Task[10];
        }

        private void Sum()
        {
            while (true)
            {
                int cur;
                if (_array.TryDequeue(out cur))
                {
                    _sum += cur;
                }
                else if (_finished)
                {
                    break;
                }
            }
        }

        private void Push()
        {
            int nxt;
            var acc = 0;
            while ((nxt = Interlocked.Increment(ref acc)) <= 100)
            {
                _array.Enqueue(nxt);
            }
        }

        [TestMethod]
        public void TestTryEnqueue()
        {
            for(var i = 0; i < 100; ++i)
            {
                Assert.IsTrue(_array.TryEnqueue(i));
            }
            Assert.IsFalse(_array.TryEnqueue(1));
            _array.Clear();
        }

        [TestMethod]
        public void TestTryDequeue()
        {
            for (var i = 0; i < 100; i++)
            {
                _array.TryEnqueue(i);
            }
            for (var i = 0; i < 100; i++)
            {
                int result;
                Assert.IsTrue(_array.TryDequeue(out result));
                Assert.AreEqual(i, result);
            }
        }

        [TestMethod]
        public void TestEnqueue()
        {
            for (var i = 0; i < 10; i++) _tasks[i] = Task.Factory.StartNew(Push);
            for (var i = 0; i < 100; i++)
            {
                Assert.AreEqual(i + 1, _array.Dequeue());
            }
            _array.Clear();
        }

        [TestMethod]
        public void TestDequeue()
        {
            for (var i = 0; i < 100; i++)
            {
                _array.Enqueue(i);
            }
            for (var i = 0; i < 10; i++) _tasks[i] = Task.Factory.StartNew(Sum);
            var result = 0;
            for(var i = 0; i < 100; i++)
            {
                result += i;
            }
            new TaskFactory().ContinueWhenAll(_tasks, _ => { _finished = true; Assert.AreEqual(result, _sum); });
        }

        [TestMethod]
        public void TestClear()
        {
            for (var i = 0; i < 10; i++) _tasks[i] = Task.Run(() => _array.Enqueue(100));
            Assert.AreEqual(100, _array.Dequeue());
            for (var i = 0; i < 2; i++) _tasks[i] = Task.Run(() => _array.Clear());
            new TaskFactory().ContinueWhenAll(_tasks, _ => Assert.IsFalse(_array.TryDequeue(out _)));
        }
    }
}