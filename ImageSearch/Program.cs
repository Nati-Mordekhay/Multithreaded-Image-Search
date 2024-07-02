using System;
using System.Drawing;
using System.Text.RegularExpressions;

class ImageSerach
{
    static void Main(string[] args)
    {
        try
        {
            // Validating input arguments
            inputValidation(args);

            // Call the imageSearch method
            imageSearchMethod(args);

        }
        catch (ArgumentException e)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }

    private static void imageSearchMethod(string[] args)
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

    /*
     * findMatchesExact: functions iterates the image and finds occurences of the smaller image inside of the bigger, than adds their top-left point to the list of 
     * it was given as an input, using exact match
     */
    private static void findMatchesExact(Color[][] image1, Color[][] image2, List<Point> matchPoints, int startRow, int endRow, int widthImage1, int widthImage2, int heightImage2)
    {
        //TODO : FIND OUT OF BOUND!
        //TODO : FINDS ONLY ONE OCCURENCE || fixed netsted match
        //Console.WriteLine("start: " + startRow + " end: " + endRow);

        // Loop for rows until end height
        for (int y = startRow; y < endRow; y++)
        {
            // Determine width 
            // TODO : delete (?)
            int maximumWidth = widthImage1 - widthImage2;

            // Iterate start for width
            for (int x = 0; x <= maximumWidth; x++)
            {
                bool boolMatch = true;

                // iterate start for width smaller image, for comparing later
                for (int i = 0; i < widthImage2; i++)
                {
                    for (int j = 0; j < heightImage2; j++)
                    {

                        // If the pixels are not matching than don't match, break
                        if (image1[x + i][y + j] != image2[i][j])
                        {
                            boolMatch = false;
                            break;
                        }
                    }

                    // If the pixels didn't match break the loop again
                    if (!boolMatch)
                    {
                        break;
                    }

                }

                // If did match, add point
                if (boolMatch)
                {
                    matchPoints.Add(new Point(x, y));
                    //Console.WriteLine("added point check");
                }
            }
        }
    }

    /*
     * findMatchesEuclidian: functions iterates the image and finds occurences of the smaller image inside of the bigger, than adds their top-left point to the list of 
     * it was given as an input, using euclidian match
     */
    private static void findMatchesEuclidian(Color[][] image1, Color[][] image2, List<Point> matchPoints, int startRow, int endRow, int widthImage1, int widthImage2, int heightImage2)
    {
        //Console.WriteLine("start: " + startRow + " end: " + endRow);
        const double threshold = 0;
        // Loop for rows until end height
        for (int y = startRow; y < endRow; y++)
        {
            // Determine width 
            int maximumWidth = widthImage1 - widthImage2;

            // Iterate start for width
            for (int x = 0; x <= maximumWidth; x++)
            {
                // define function's nested variables
                double totalDistance = 0; // to be compared to the euclidian distance
                bool boolMatch = true; // check if nested image matches

                // iterate start for width smaller image, for comparing later
                for (int i = 0; i < widthImage2; i++)
                {
                    for (int j = 0; j < heightImage2; j++)
                    {
                        // calculate color difference
                        double rDiff = (double)(image1[x + i][y + j].R - image2[i][j].R);
                        double gDiff = (double)(image1[x + i][y + j].G - image2[i][j].G);
                        double bDiff = (double)(image1[x + i][y + j].B - image2[i][j].B);

                        // Add squared difference to total distance
                        totalDistance += rDiff * rDiff + gDiff * gDiff + bDiff * bDiff;

                        // If the pixels didn't match break the loop again
                        if (Math.Sqrt(totalDistance) > threshold)
                        {
                            boolMatch = false;
                            break;
                        }
                    }
                    // If the pixels didn't match break the loop again
                    if (!boolMatch)
                    {
                        break;
                    }

                }

                // If did match, add point
                if (boolMatch)
                {
                    matchPoints.Add(new Point(x, y));
                }
            }
        }
    }

    /*
     * getColorArray: converts the image into a two dimensional array of pixels
     */
    private static Color[][] getColorArray(string imagePath)
    {
        // Load image and get dimensions
        Bitmap bitmap = new Bitmap(imagePath);
        int imageWidth = bitmap.Width;
        int imageHeight = bitmap.Height;

        // Create a Color array to be filled with the image's pixels
        Color[][] pixels = new Color[imageWidth][];

        // Fill pixels for Color array from Bitmap
        for (int i = 0; i < imageWidth; i++)
        {
            pixels[i] = new Color[imageHeight];
            for (int j = 0; j < imageHeight; j++)
            {
                pixels[i][j] = bitmap.GetPixel(i, j);
            }
        }

        // Return statement
        return pixels;
    }

    /*
     * inputValidation: The function validates inputed arguments
     */
    private static void inputValidation(string[] args)
    {
        // Check if inputed args list is excatcly the length of 4
        if (args.Length != 4)
        {
            throw new ArgumentException("Invalid arguments: please enter 4 input argumnets.");
        }

        // Check image 1 for correct format
        string image1 = args[0];
        if (!image1.ToLower().EndsWith(".jpg") && !image1.ToLower().EndsWith(".gif") && !image1.ToLower().EndsWith(".png"))
        {
            throw new ArgumentException("Image 1: invalid format. Image should be .jpg or .gif or .png");
        }

        // Check image 2 for correct format (handle uppercase extension)
        string image2 = args[1];
        if (!image2.ToLower().EndsWith(".jpg") && !image2.ToLower().EndsWith(".gif") && !image2.ToLower().EndsWith(".png"))
        {
            throw new ArgumentException("Image 2: invalid format. Image should be .jpg or .gif or .png");
        }

        // Check the inputed number of threads is an int
        int nThreads;
        if (!(int.TryParse(args[2], out nThreads)))
        {
            throw new ArgumentException("Number of threads: Invalid format. Please enter a valid integer. youre input: " + args[2]);
        }

        // Check minimum number of threads
        if (!(nThreads > 0))
        {
            throw new ArgumentException("Number of threads: Should be 1 or greater. your input: " + nThreads);
        }

        // Check inputed calculation algorithm
        string inputAlgorithm = args[3];
        if (inputAlgorithm != "exact" && inputAlgorithm != "euclidian")
        {
            throw new ArgumentException("Input algorithm: the inputed algorithm does not match 'exact' to 'euclidian' (case sensetive): " + inputAlgorithm);
        }
    }

}