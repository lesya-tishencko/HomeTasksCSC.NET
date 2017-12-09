using System;
using System.Threading;

namespace Multithreading
{
    /// <summary>
    /// Based on algorithm dinning philosophers from Tanenbaum's Modern Operating System
    /// </summary>
    public class Philosophers
    {
        private const int CountOfPhilosophers = 5;
        private const int MaxWaitingTime = 100;
        private readonly Fork[] _forks;
        private readonly Random _randomizer;

        public Philosophers()
        {
            _forks = new Fork[CountOfPhilosophers];
            for (var i = 0; i < CountOfPhilosophers; i++)
            {
                _forks[i] = new Fork(i);
            }
            _randomizer = new Random();
        }

        public void Philosopher(object index)
        {
            while (true)
            {
                Think((int)index);
                while(!_forks[leftFork((int)index)].Take()) { }
                while (!_forks[rightFork((int)index)].Take()) { }

                Eat((int)index);
                _forks[leftFork((int)index)].Put();
                _forks[rightFork((int)index)].Put();
            }
        }

        private int leftFork(int index) => index;
        private int rightFork(int index) => (index + 1) % CountOfPhilosophers;

        private void Eat(int index)
        {
            Console.WriteLine("Philosopher {0} is eating", index);
            Thread.Sleep(_randomizer.Next(MaxWaitingTime));
        }

        private void Think(int index)
        {
            Console.WriteLine("Philosopher {0} is thinking", index);
            Thread.Sleep(_randomizer.Next(MaxWaitingTime));
        }
    }
}
