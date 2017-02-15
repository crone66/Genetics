
using System;
using System.Collections.Generic;

namespace Genetics
{
    public abstract class Problem<T>
    {
        protected static Random rand = new Random();

        public enum CrossoverType
        {
            OnePoint,
            NPoint,
        }

        protected DNA<T>[] dna;
        protected DNA<T> best;
        protected int populationSize;
        protected double crossoverChance;
        protected double mutationChance;
        protected double elitistChance;
        protected double surviveChance;
        protected int iterations;
        protected CrossoverType crossoverType;

        public DNA<T>[] DNA
        {
            get
            {
                return dna;
            }
        }

        public DNA<T> Best
        {
            get
            {
                return best;
            }
        }

        public Problem(int populationSize, double mutationChance, double crossoverChance, double elitistChance, double surviveChance, int iterations)
        {
            this.populationSize = populationSize;
            this.mutationChance = mutationChance;
            this.crossoverChance = crossoverChance;
            this.elitistChance = elitistChance;
            this.surviveChance = surviveChance;
            this.iterations = iterations;
        }

        public Problem(int populationSize, double mutationChance, double crossoverChance, double elitistChance, double surviveChance, int iterations, DNA<T>[] dna)
        {
            this.dna = dna;
            this.populationSize = populationSize;
            this.mutationChance = mutationChance;
            this.crossoverChance = crossoverChance;
            this.elitistChance = elitistChance;
            this.surviveChance = surviveChance;
            this.iterations = iterations;
        }        

        protected virtual void Initzialize()
        {
            DNA<T>[] newDna = new DNA<T>[populationSize];
            if(dna != null)
                ChooseSurvivors(ref newDna);

            dna = newDna;

            for (int i = 0; i < dna.Length; i++)
            {
                if (dna[i] == null)
                    dna[i] = CreateGen();
            }
        }

        protected virtual void ChooseSurvivors(ref DNA<T>[] newDna)
        {
            for (int i = 0; i < Math.Ceiling(dna.Length * elitistChance) && i < newDna.Length; i++)
            {
                newDna[i] = (DNA<T>)dna[i].Clone();
            }
        }

        protected virtual void CreateDNA()
        {
            dna = new DNA<T>[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                dna[i] = CreateGen();
            }
        }

        protected virtual double[] CalculateSummedFitness()
        {
            double[] SummedFitness = new double[populationSize];
            double SumFitness = 0;
            for (int i = 0; i < populationSize; i++)
            {
                dna[i].Fitness = CalculateFitness(dna[i]);
            }
            Array.Sort(dna);

            for (int i = 0; i < populationSize; i++)
            {
                if (best == null || dna[i].Fitness < best.Fitness)
                {
                    best = dna[i];
                }
                SumFitness += dna[i].Fitness;
                SummedFitness[i] = SumFitness;
            }

            return SummedFitness;
        }

        protected virtual void ChooseParents(double[] summedFitness, ref DNA<T> parent1, ref DNA<T> parent2)
        {
            double summed = summedFitness[summedFitness.Length - 1];
            do
            {
                for (int j = 0; j < populationSize; j++)
                {
                    if (parent1 != null && parent2 != null)
                        return;

                    if (parent1 == null && parent2 != dna[j] && 1 - (dna[j].Fitness / summed) >= rand.NextDouble())
                        parent1 = dna[j];

                    if (parent2 == null && parent1 != dna[j] && 1 - (dna[j].Fitness / summed) >= rand.NextDouble())
                        parent2 = dna[j];
                }
            } while (parent1 == null || parent2 == null);
        }

        protected virtual DNA<T>[] GetElitist()
        {
            DNA<T>[] newDna = new DNA<T>[populationSize];
            for (int i = 0; i < Math.Ceiling(populationSize * elitistChance); i++)
            {
                newDna[i] = (DNA<T>)dna[i].Clone();
            }

            return newDna;
        }

        public virtual DNA<T> Do()
        {
            for (int i = 0; i < iterations; i++)
            {
                double[] SummedFitness = CalculateSummedFitness();
                DNA<T>[] NewPopulation = GetElitist();

                for (int j = (int)Math.Ceiling(populationSize * elitistChance); j < populationSize; j++)
                {
                    bool born = false;
                    if (rand.NextDouble() <= crossoverChance)
                    {
                        DNA<T> parent1 = null;
                        DNA<T> parent2 = null;
                        ChooseParents(SummedFitness, ref parent1, ref parent2);
                        DNA<T> child = Crossover(parent1, parent2);
                        child.Fitness = CalculateFitness(child);
                        if (child.Fitness != parent1.Fitness && child.Fitness != parent2.Fitness)
                        {
                            NewPopulation[j] = child;
                            born = true;
                        }
                    }

                    if(!born)
                    {
                        if (surviveChance >= rand.NextDouble())
                        {
                            Mutation(dna[j]);
                            NewPopulation[j] = dna[j];
                        }
                        else
                        {
                            NewPopulation[j] = CreateGen();
                        }
                    }
                }
                dna = NewPopulation;
            }
            return Best;
        }

        protected virtual void Mutation(DNA<T> dna)
        {
            for (int j = 0; j < dna.Data.Length; j++)
            {
                if (rand.NextDouble() <= mutationChance)
                {
                    int swapIndex = rand.Next(dna.Data.Length);
                    T swapValue = dna.Data[j];
                    dna.Data[j] = dna.Data[swapIndex];
                    dna.Data[swapIndex] = swapValue;
                }
            }
        }

        protected abstract DNA<T> CreateGen();
        
        protected abstract int CalculateFitness(DNA<T> dna);

        protected abstract DNA<T> Crossover(DNA<T> dna1, DNA<T> dna2);
    }
}
