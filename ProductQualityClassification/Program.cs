using System.Globalization;

namespace MachineLearningkNNProductQualityClassificationCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Dane treningowe
            var trainingData = new List<Product>
            {
                new Product(1.0, 5.0, "Dobry"),
                new Product(1.2, 5.5, "Dobry"),
                new Product(0.8, 4.8, "Dobry"),
                new Product(1.5, 6.0, "Wadliwy"),
                new Product(1.6, 6.2, "Wadliwy"),
                new Product(1.4, 5.8, "Wadliwy")
            };

            // Nowy produkt do klasyfikacji
            Console.WriteLine("Podaj wagę produktu (kg):");
            double newWeight = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            Console.WriteLine("Podaj rozmiar produktu (cm):");
            double newSize = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            var newProduct = new Product(newWeight, newSize, null);

            // Wartość k
            int k = 3;

            // Klasyfikacja
            string predictedClass = Classify(trainingData, newProduct, k);

            Console.WriteLine($"\nNowy produkt o wadze {newProduct.Weight} kg i rozmiarze {newProduct.Size} cm został zaklasyfikowany jako: {predictedClass}");

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby zakończyć.");
            Console.ReadKey();
        }

        static string Classify(List<Product> trainingData, Product newProduct, int k)
        {
            // Obliczanie odległości
            foreach (var product in trainingData)
            {
                product.Distance = EuclideanDistance(product, newProduct);
            }

            // Sortowanie według odległości
            var nearestNeighbors = trainingData.OrderBy(p => p.Distance).Take(k);

            // Głosowanie większościowe
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
}
