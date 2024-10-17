# Klasyfikacja Jakości Produktów za pomocą k-NN w C# (.NET)

## Wprowadzenie
Celem tego projektu jest zademonstrowanie, jak zaimplementować algorytm k najbliższych sąsiadów (k-NN) w języku C# z wykorzystaniem technologii .NET. Skupimy się na klasyfikacji jakości produktów na podstawie ich cech, takich jak waga i rozmiar. Naszym zadaniem będzie przewidzenie, czy nowy produkt jest "Dobry" czy "Wadliwy".

Repozytorium GitHub: [Machine-Learning-kNN-Product-Quality-Classification-CSharp](https://github.com/albertmoczadlo/Machine-Learning-kNN-Product-Quality-Classification-CSharp)

## Spis Treści
1. [Wprowadzenie do Klasyfikacji i Uczenia Nadzorowanego](#wprowadzenie-do-klasyfikacji-i-uczenia-nadzorowanego)
2. [Algorytm k-NN - Wyjaśnienie Matematyczne](#algorytm-k-nn---wyjaśnienie-matematyczne)
3. [Dane Treningowe](#dane-treningowe)
4. [Obliczenia Krok po Kroku](#obliczenia-krok-po-kroku)
5. [Implementacja w C#](#implementacja-w-c)
6. [Uruchomienie Aplikacji](#uruchomienie-aplikacji)
7. [Podsumowanie](#podsumowanie)
8. [Materiały Dodatkowe](#materiały-dodatkowe)
9. [Licencja](#licencja)
10. [Autor](#autor)
11. [Zachęcam do Współpracy](#zachęcam-do-współpracy)
12. [Pliki w Repozytorium](#pliki-w-repozytorium)
13. [Kontakt](#kontakt)

---

## 1. Wprowadzenie do Klasyfikacji i Uczenia Nadzorowanego
Klasyfikacja to proces przypisywania obiektów do jednej z kilku predefiniowanych klas na podstawie ich cech. W uczeniu nadzorowanym model uczy się na danych, które zawierają zarówno cechy wejściowe, jak i oczekiwane wyjścia (etykiety klas). Model stara się nauczyć zależności między cechami a klasami, aby móc przewidywać klasy dla nowych danych.

W naszym przypadku chcemy zaklasyfikować produkty jako "Dobre" lub "Wadliwe" na podstawie ich cech produkcyjnych.

## 2. Algorytm k-NN - Wyjaśnienie Matematyczne

### Opis Algorytmu
Algorytm k najbliższych sąsiadów (k-NN) to metoda klasyfikacji, która:
- Oblicza odległości między nowym punktem a wszystkimi punktami w zbiorze treningowym.
- Wybiera k najbliższych sąsiadów (punktów o najmniejszej odległości).
- Przypisuje nowemu punktowi klasę, która najczęściej występuje wśród tych k sąsiadów (głosowanie większościowe).

### Metryka Odległości
Używamy odległości euklidesowej:

$$
d(p, q) = \sqrt{(p_1 - q_1)^2 + (p_2 - q_2)^2}
$$

Gdzie:
- \( p \) i \( q \) to punkty w przestrzeni dwuwymiarowej (waga, rozmiar).
- \( p_1, q_1 \) to wagi produktów.
- \( p_2, q_2 \) to rozmiary produktów.

## 3. Dane Treningowe
Załóżmy, że mamy następujące dane:

| Nr | Waga (kg) | Rozmiar (cm) | Klasa   |
|----|-----------|--------------|---------|
| 1  | 1.0       | 5.0          | Dobry   |
| 2  | 1.2       | 5.5          | Dobry   |
| 3  | 0.8       | 4.8          | Dobry   |
| 4  | 1.5       | 6.0          | Wadliwy |
| 5  | 1.6       | 6.2          | Wadliwy |
| 6  | 1.4       | 5.8          | Wadliwy |

## 4. Obliczenia Krok po Kroku

### Krok 1: Obliczenie Odległości
Chcemy zaklasyfikować nowy produkt o wadze 1.1 kg i rozmiarze 5.4 cm. Obliczamy odległości euklidesowe między nowym produktem a każdym produktem w zbiorze treningowym:

#### Odległości:

     Do punktu 1:  
        d = √((1.0 - 1.1)² + (5.0 - 5.4)²) ≈ 0.412
    
     Do punktu 2:  
        d = √((1.2 - 1.1)² + (5.5 - 5.4)²) ≈ 0.141
    
     Do punktu 3:  
        d = √((0.8 - 1.1)² + (4.8 - 5.4)²) ≈ 0.671
    
     Do punktu 4:  
        d = √((1.5 - 1.1)² + (6.0 - 5.4)²) ≈ 0.721
    
     Do punktu 5:  
        d = √((1.6 - 1.1)² + (6.2 - 5.4)²) ≈ 0.943
    
     Do punktu 6:  
        d = √((1.4 - 1.1)² + (5.8 - 5.4)²) = 0.5


  

### Krok 2: Wybór k Najbliższych Sąsiadów
Wybieramy wartość \( k = 3 \) i wybieramy 3 najbliższe produkty:
- Punkt 2: odległość ≈ 0.141 (Klasa: Dobry)
- Punkt 1: odległość ≈ 0.412 (Klasa: Dobry)
- Punkt 6: odległość = 0.5 (Klasa: Wadliwy)

### Krok 3: Głosowanie Większościowe
- Klasa "Dobry": 2 głosy
- Klasa "Wadliwy": 1 głos

Nowy produkt zostaje zaklasyfikowany jako **"Dobry"**.

---

## 5. Implementacja w C#

### Przygotowanie Projektu
1. Otwórz Visual Studio.
2. Wybierz **Plik** > **Nowy** > **Projekt**.
3. Wybierz **Aplikacja konsolowa (.NET Core)** lub **Aplikacja konsolowa (.NET Framework)**.
4. Nazwij projekt, np. `Machine-Learning-kNN-Product-Quality-Classification-CSharp`.

### Struktura Plików:
- `Program.cs` – główny plik z kodem źródłowym.

### Kod Źródłowy
Poniżej przedstawiamy pełny kod źródłowy aplikacji.

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
            double newWeight = double.Parse(Console.ReadLine());

            Console.WriteLine("Podaj rozmiar produktu (cm):");
            double newSize = double.Parse(Console.ReadLine());

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
```
## Wyjaśnienie Kodu

- **Klasa `Product`:**
  - Reprezentuje pojedynczy produkt z cechami:
    - `Weight` (waga)
    - `Size` (rozmiar)
    - `ClassLabel` (etykieta klasy: "Dobry" lub "Wadliwy")
    - `Distance` (odległość od nowego produktu)

- **Metoda `EuclideanDistance`:**
  - Oblicza odległość euklidesową między dwoma produktami na podstawie ich wagi i rozmiaru.

- **Metoda `Classify`:**
  - Oblicza odległości między nowym produktem a wszystkimi produktami w zbiorze treningowym.
  - Sortuje produkty według odległości i wybiera `k` najbliższych sąsiadów.
  - Przeprowadza głosowanie większościowe na podstawie klas sąsiadów i zwraca przewidywaną klasę.

- **Metoda `Main`:**
  - Pobiera dane nowego produktu od użytkownika.
  - Definiuje dane treningowe.
  - Wywołuje metodę `Classify` i wyświetla wynik klasyfikacji.

---

## 6. Uruchomienie Aplikacji

### Kompilacja:
1. Upewnij się, że wszystkie pliki są zapisane.
2. Naciśnij `Ctrl + Shift + B` lub wybierz **Kompiluj > Kompiluj rozwiązanie**.

### Uruchomienie:
1. Naciśnij `F5` lub kliknij **Start**.
2. W konsoli zostaniesz poproszony o podanie wagi i rozmiaru nowego produktu.

#### Przykładowe Wejście:


