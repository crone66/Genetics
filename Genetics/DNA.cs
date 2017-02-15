using System;

namespace Genetics
{
    public class DNA<T> : ICloneable, IComparable<DNA<T>>
    {
        public delegate T[] DataDeepCopy(T[] Data);

        public double Fitness;
        public T[] Data;
        public DataDeepCopy DeepCopyCreator;
        
        public DNA(T[] data)
        {
            Data = data;
        }

        public DNA(DataDeepCopy deepCopyCreator)
        {
            DeepCopyCreator = deepCopyCreator;
        }

        public DNA(DataDeepCopy deepCopyCreator, T[] data)
        {
            Data = data;
            DeepCopyCreator = deepCopyCreator;
        }

        public int CompareTo(DNA<T> other)
        {
            if (Fitness > other.Fitness)
                return 1;
            else if (Fitness < other.Fitness)
                return -1;
            else
                return 0;
        }

        public object Clone()
        {
            DNA<T> Cloned = new DNA<T>(DeepCopyCreator, Data);
            Cloned.Data = DeepCopyCreator(Data);
            Cloned.Fitness = Fitness;
            return Cloned;
        }
    }
}
