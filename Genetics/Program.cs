
using System;
using System.Diagnostics;

namespace Genetics
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            TravellingSalesPerson tsp = new TravellingSalesPerson(20);

            TSPProblem tspDna = new TSPProblem(tsp, 100, 0.1, 0.8, 0.1, 1, 1000);
            
            DNA<int> best = tspDna.Do();
            Console.WriteLine("Best: " + best.Fitness);
            DNA<int>[] dna = tspDna.DNA;

            int upgrades = 0;
            for (int i = 0; i < 1000; i++)
            {
                TSPProblem tspDna2 = new TSPProblem(dna, tsp, 100, 0.1, 0.8, 0.1, 1, 1000);

                DNA<int> best2 = tspDna2.Do();
                Console.WriteLine("["+i.ToString() + "] Best: " + best2.Fitness);   

                if(best2.Fitness < best.Fitness)
                {
                    best = best2;
                    upgrades++;
                }           
                dna = tspDna2.DNA;

            }
            watch.Stop();
            Console.WriteLine("Duration: " + watch.Elapsed.TotalMilliseconds);
            Console.WriteLine("Upgrades: " + upgrades.ToString());
            Console.ReadKey();
        }
    }
}