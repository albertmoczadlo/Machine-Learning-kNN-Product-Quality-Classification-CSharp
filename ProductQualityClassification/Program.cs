using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        // Training data
        var trainingData = new List<Product>
        {
            new Product(1.0, 5.0, "Good"),
            new Product(1.2, 5.5, "Good"),
            new Product(0.8, 4.8, "Good"),
            new Product(1.5, 6.0, "Defective"),
            new Product(1.6, 6.2, "Defective"),
            new Product(1.4, 5.8, "Defective")
        };

        // New product for classification
        Console.WriteLine("Enter the product weight (kg):");
        double newWeight = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture); // added CultureInfo.InvariantCulture

        Console.WriteLine("Enter the product size (cm):");
        double newSize = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture); // added CultureInfo.InvariantCulture

        var newProduct = new Product(newWeight, newSize, null);

        // Value of k
        int k = 3;

        // Classification
        string predictedClass = Classify(trainingData, newProduct, k);

        Console.WriteLine($"\nThe new product with weight {newProduct.Weight} kg and size {newProduct.Size} cm has been classified as: {predictedClass}");

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    static string Classify(List<Product> trainingData, Product newProduct, int k)
    {
        // Calculating distances
        foreach (var product in trainingData)
        {
            product.Distance = EuclideanDistance(product, newProduct);
        }

        // Sorting by distance
        var nearestNeighbors = trainingData.OrderBy(p => p.Distance).Take(k);

        // Majority voting
        var classVotes = nearestNeighbors.GroupBy(p => p.ClassLabel)
                                         .Select(group => new { ClassLabel = group.Key, Count = group.Count() })
                                         .OrderByDescending(g => g.Count);

        return classVotes.First().ClassLabel;
    }

    static double EuclideanDistance(Product p1, Product p2)
    {
        return Math.Sqrt(Math.Pow(p1.Weight - p2.Weight, 2) + Math.Pow(p1.Size - p2.Size, 2));
    }
}

class Product
{
    public double Weight { get; set; }
    public double Size { get; set; }
    public string ClassLabel { get; set; }
    public double Distance { get; set; }

    public Product(double weight, double size, string classLabel)
    {
        Weight = weight;
        Size = size;
        ClassLabel = classLabel;
    }
}

