# Product Quality Classification using k-NN in C# (.NET)

## Introduction
The goal of this project is to demonstrate how to implement the k-nearest neighbors (k-NN) algorithm in C# using .NET technology. We will focus on classifying product quality based on their features, such as weight and size. Our task will be to predict whether a new product is "Good" or "Defective."

GitHub Repository: [Machine-Learning-kNN-Product-Quality-Classification-CSharp](https://github.com/albertmoczadlo/Machine-Learning-kNN-Product-Quality-Classification-CSharp)

## Table of Contents
1. [Introduction to Classification and Supervised Learning](#introduction-to-classification-and-supervised-learning)
2. [k-NN Algorithm - Mathematical Explanation](#k-nn-algorithm---mathematical-explanation)
3. [Training Data](#training-data)
4. [Step-by-Step Calculations](#step-by-step-calculations)
5. [Implementation in C#](#implementation-in-c)
6. [Running the Application](#running-the-application)
7. [Summary](#summary)
8. [Additional Materials](#additional-materials)
9. [License](#license)
10. [Author](#author)
11. [Encouragement for Collaboration](#encouragement-for-collaboration)
12. [Repository Files](#repository-files)
13. [Contact](#contact)

---

## 1. Introduction to Classification and Supervised Learning
Classification is the process of assigning objects to one of several predefined classes based on their features. In supervised learning, the model is trained on data that includes both input features and expected outputs (class labels). The model aims to learn the relationship between the features and the classes to be able to predict the classes for new data.

In our case, we want to classify products as "Good" or "Defective" based on their production characteristics.

## 2. k-NN Algorithm - Mathematical Explanation

### Algorithm Description
The k-nearest neighbors (k-NN) algorithm is a classification method that:
- Calculates the distances between a new point and all points in the training set.
- Selects the k closest neighbors (points with the smallest distance).
- Assigns the new point to the class that appears most frequently among those k neighbors (majority voting).

### Distance Metric
We use the Euclidean distance:

$$
d(p, q) = \sqrt{(p_1 - q_1)^2 + (p_2 - q_2)^2}
$$

Where:
- \( p \) and \( q \) are points in a two-dimensional space (weight, size).
- \( p_1, q_1 \) are the product weights.
- \( p_2, q_2 \) are the product sizes.

## 3. Training Data
Let’s assume we have the following data:

| No. | Weight (kg) | Size (cm) | Class     |
|-----|-------------|-----------|-----------|
| 1   | 1.0         | 5.0       | Good      |
| 2   | 1.2         | 5.5       | Good      |
| 3   | 0.8         | 4.8       | Good      |
| 4   | 1.5         | 6.0       | Defective |
| 5   | 1.6         | 6.2       | Defective |
| 6   | 1.4         | 5.8       | Defective |

## 4. Step-by-Step Calculations

### Step 1: Calculate the Distances
We want to classify a new product weighing 1.1 kg and measuring 5.4 cm. We calculate the Euclidean distances between the new product and each product in the training set:

#### Distances:

- To point 1:  
  \( d = \sqrt{(1.0 - 1.1)^2 + (5.0 - 5.4)^2} \approx 0.412 \)
  
- To point 2:  
  \( d = \sqrt{(1.2 - 1.1)^2 + (5.5 - 5.4)^2} \approx 0.141 \)
  
- To point 3:  
  \( d = \sqrt{(0.8 - 1.1)^2 + (4.8 - 5.4)^2} \approx 0.671 \)
  
- To point 4:  
  \( d = \sqrt{(1.5 - 1.1)^2 + (6.0 - 5.4)^2} \approx 0.721 \)
  
- To point 5:  
  \( d = \sqrt{(1.6 - 1.1)^2 + (6.2 - 5.4)^2} \approx 0.943 \)
  
- To point 6:  
  \( d = \sqrt{(1.4 - 1.1)^2 + (5.8 - 5.4)^2} = 0.5 \)

### Step 2: Select the k Nearest Neighbors
We choose \( k = 3 \) and select the 3 closest products:
- Point 2: distance ≈ 0.141 (Class: Good)
- Point 1: distance ≈ 0.412 (Class: Good)
- Point 6: distance = 0.5 (Class: Defective)

### Step 3: Majority Voting
- Class "Good": 2 votes
- Class "Defective": 1 vote

The new product is classified as **"Good"**.

---

## 5. Implementation in C#

### Project Setup
1. Open Visual Studio.
2. Select **File** > **New** > **Project**.
3. Choose **Console Application (.NET Core)** or **Console Application (.NET Framework)**.
4. Name the project, e.g., `Machine-Learning-kNN-Product-Quality-Classification-CSharp`.

### File Structure:
- `Program.cs` – main source code file.

### Source Code
Below is the complete source code of the application.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace MachineLearningkNNProductQualityClassificationCSharp
{
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
            double newWeight = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            Console.WriteLine("Enter the product size (cm):");
            double newSize = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

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
}


