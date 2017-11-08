using System;
using System.Threading;

namespace Multithreading
{
    /// <summary>
    /// Based on algorithm dinning philosophers from Tanenbaum's Modern Operating System
    /// </summary>
    public class Philosophers
    {
        private const int countOfPhilosophers = 5;
        private const int maxWaitingTime = 100;
        private enum State { Thinking, Hungry, Eating };
        private State[] states;
        private Semaphore mutex;
        private Semaphore[] semaphores;
        private Random randomizer;

        public Philosophers()
        {
            mutex = new Semaphore(1, 5);
            semaphores = new Semaphore[countOfPhilosophers];
            states = new State[countOfPhilosophers];
            for (int i = 0; i < countOfPhilosophers; i++)
            {
                semaphores[i] = new Semaphore(1, 5);
                states[i] = State.Thinking;
            }
            randomizer = new Random();
        }

        public void Philosopher(object index)
        {
            while (true)
            {
                Think((int)index);
                TakeForks((int)index);
                Eat((int)index);
                PutForks((int)index);
            }
        }

        private int leftNeibor(int index) => (index + countOfPhilosophers - 1) % countOfPhilosophers;
        private int rightNeibor(int index) => (index + 1) % countOfPhilosophers;

        private void PutForks(int index)
        {
            mutex.WaitOne();
            states[index] = State.Thinking;
            Test(leftNeibor(index));
            Test(rightNeibor(index));
            mutex.Release();
        }

        private void Eat(int index)
        {
            Console.WriteLine("Philosopher {0} is eating", index);
            Thread.Sleep(randomizer.Next(maxWaitingTime));
        }

        private void TakeForks(int index)
        {
            mutex.WaitOne();
            states[index] = State.Hungry;
            Test(index);
            mutex.Release();
            semaphores[index].WaitOne();
        }

        private void Test(int index)
        {
            if (states[index] == State.Hungry &&
                states[leftNeibor(index)] != State.Eating &&
                states[rightNeibor(index)] != State.Eating)
            {
                states[index] = State.Eating;
                semaphores[index].Release();
            }
        }

        private void Think(int index)
        {
            Console.WriteLine("Philosopher {0} is thinking", index);
            Thread.Sleep(randomizer.Next(maxWaitingTime));
        }
    }
}
