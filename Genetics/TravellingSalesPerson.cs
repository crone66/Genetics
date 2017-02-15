using System;

namespace Genetics
{
    public class TravellingSalesPerson
    {
        private static Random rand = new Random();

        public int CityCount;
        public int[][] Connections;

        public TravellingSalesPerson(int cityCount)
        {
            CityCount = cityCount;
            Connections = new int[cityCount][];
            for (int i = 0; i < cityCount; i++)
            {
                Connections[i] = new int[cityCount];
                for (int j = 0; j < cityCount; j++)
                {
                    Connections[i][j] = int.MaxValue;
                }
            }

            SetupConnections();
        }

        private void SetupConnections()
        {
            int sum = 0;
            for (int i = 0; i < CityCount; i++)
            {
                for (int j = 0; j < CityCount; j++)
                {
                    int val = rand.Next(10, 1000);
                    sum += val;
                    AddConnection(i, j, val);
                }
            }
        }

        private void AddConnection(int start, int end, int distance)
        {
            Connections[start][end] = distance;
        }
    }
}
