using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch
{
    internal class imageSearcher
    {
        public void imageSearchMethod(string[] args)
        {
            // input variables & initializing variables
            string image1Path = args[0];
            string image2Path = args[1];
            int nThreads = int.Parse(args[2]);
            string inputAlgorithm = args[3];
            Thread[] threadsList = new Thread[nThreads];

            // Convert images to two dimensional array of pixel
            Color[][] image1 = getColorArray(image1Path);
            Color[][] image2 = getColorArray(image2Path);
            int widthImage1 = image1.Length;
            int heightImage1 = image1[0].Length;
            int widthImage2 = image2.Length;
            int heightImage2 = image2[0].Length;

            // Allocating needed variables for each thread
            List<Point> matchPoints = new List<Point>();
            int tWidth = (widthImage1 - widthImage2 + 1);
            int tHeight = (heightImage1 - heightImage2 + 1);

            // Create threads
            for (int i = 0; i < nThreads; i++)
            {
                int startRow = i * (tHeight / nThreads);
                int endRow = (i == nThreads - 1) ? tHeight : ((i + 1) * (tHeight / nThreads));
                // If the choosen algorithm is exact match
                if (inputAlgorithm == "exact")
                {
                    threadsList[i] = new Thread(() => findMatchesExact(image1, image2, matchPoints, startRow, endRow, widthImage1, widthImage2, heightImage2));
                }
                // else if choosen algorithm is euclidian match
                else
                {
                    threadsList[i] = new Thread(() => findMatchesEuclidian(image1, image2, matchPoints, startRow, endRow, widthImage1, widthImage2, heightImage2));
                }

                // Start each thread in the thread list
                threadsList[i].Start();
            }

            // Wait for all threads to finish
            foreach (Thread thread in threadsList)
            {
                thread.Join();
            }

            // Print all results
            foreach (Point match in matchPoints)
            {
                Console.WriteLine($"{match.X},{match.Y}");
            }
        }
    }
}
