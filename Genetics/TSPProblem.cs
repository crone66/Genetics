using System.Collections.Generic;

namespace Genetics
{
    public class TSPProblem : Problem<int>
    {
        private TravellingSalesPerson tsp;

        public TSPProblem(TravellingSalesPerson tsp, int populationSize, double mutationChance, double crossoverChance, double elitistChance, double surviveChance, int iterations)
            :base(populationSize, mutationChance, crossoverChance, elitistChance, surviveChance, iterations)
        {
            this.tsp = tsp;
            Initzialize();
        }

        public TSPProblem(DNA<int>[] dna, TravellingSalesPerson tsp, int populationSize, double mutationChance, double crossoverChance, double elitistChance, double surviveChance, int iterations)
            :base(populationSize, mutationChance, crossoverChance, elitistChance, surviveChance, iterations, dna)
        {
            this.tsp = tsp;
            Initzialize();
        }

        protected override DNA<int> CreateGen()
        {
            DNA<int> dna = new DNA<int>(DeepCopy, new int[tsp.CityCount]);

            List<int> Cities = new List<int>();
            for (int j = 0; j < tsp.CityCount; j++)
            {
                Cities.Add(j);
            }

            int Counter = 0;
            while (Cities.Count > 0)
            {
                int Index = rand.Next(Cities.Count);
                dna.Data[Counter++] = Cities[Index];
                Cities.RemoveAt(Index);
            }

            return dna;
        }

        protected override int CalculateFitness(DNA<int> dna)
        {
            int Costs = 0;
            for (int i = 0; i < dna.Data.Length; i++)
            {
                if (i == dna.Data.Length - 1)
                {
                    Costs += tsp.Connections[dna.Data[i]][dna.Data[0]];
                }
                else
                {
                    Costs += tsp.Connections[dna.Data[i]][dna.Data[i + 1]];
                }
            }
            return Costs;
        }

        protected override DNA<int> Crossover(DNA<int> parent1, DNA<int> parent2)
        {
            DNA<int> child = (DNA<int>)parent1.Clone();
            int crossOverIndex = rand.Next(tsp.CityCount - rand.Next(tsp.CityCount / 4, tsp.CityCount / 2));
            List<int> visitedCities = new List<int>();
            for (int j = 0; j < crossOverIndex; j++)
            {
                visitedCities.Add(parent1.Data[j]);
            }

            for (int j = 0; j < tsp.CityCount; j++)
            {
                if (!visitedCities.Contains(parent2.Data[j]))
                {
                    child.Data[crossOverIndex++] = parent2.Data[j];
                }
            }
            return child;
        }

        private int[] DeepCopy(int[] data)
        {
            int[] arr = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                arr[i] = data[i];
            }
            return arr;
        }
    }
}
