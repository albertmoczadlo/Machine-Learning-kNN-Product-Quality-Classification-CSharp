# Klasyfikacja Jakości Produktów za pomocą k-NN w C# (.NET)

## Wprowadzenie
Celem tego projektu jest zademonstrowanie, jak zaimplementować algorytm k najbliższych sąsiadów (k-NN) w języku C# z wykorzystaniem technologii .NET. Skupie się na klasyfikacji jakości produktów na podstawie ich cech, takich jak waga i rozmiar. Naszym zadaniem będzie przewidzenie, czy nowy produkt jest "Dobry" czy "Wadliwy".

Repozytorium GitHub: [Machine-Learning-kNN-Product-Quality-Classification-CSharp](https://github.com/albertmoczadlo/Machine-Learning-kNN-Product-Quality-Classification-CSharp)

## Spis Treści
- [Wprowadzenie do Klasyfikacji i Uczenia Nadzorowanego](#wprowadzenie-do-klasyfikacji-i-uczenia-nadzorowanego)
- [Algorytm k-NN - Wyjaśnienie Matematyczne](#algorytm-k-nn---wyjaśnienie-matematyczne)
- [Dane Treningowe](#dane-treningowe)
- [Obliczenia Krok po Kroku](#obliczenia-krok-po-kroku)
- [Implementacja w C#](#implementacja-w-c)
  - [Przygotowanie Projektu](#przygotowanie-projektu)
  - [Kod Źródłowy](#kod-żródłowy)
  - [Wyjaśnienie Kodu](#wyjaśnienie-kodu)
- [Uruchomienie Aplikacji](#uruchomienie-aplikacji)
- [Podsumowanie](#podsumowanie)
- [Materiały Dodatkowe](#materiały-dodatkowe)
- [Licencja](#licencja)
- [Autor](#autor)
- [Zachęcam do Współpracy](#zachęcam-do-współpracy)
- [Pliki w Repozytorium](#pliki-w-repozytorium)
- [Kontakt](#kontakt)

## Wprowadzenie do Klasyfikacji i Uczenia Nadzorowanego
Algorytmy klasyfikacji są jedną z podstawowych technik uczenia maszynowego, gdzie celem jest przypisanie obiektu do jednej z predefiniowanych kategorii na podstawie jego cech. Uczenie nadzorowane polega na trenowaniu modelu na wcześniej oznaczonych danych, gdzie znamy zarówno cechy, jak i etykiety klas produktów.

## Algorytm k-NN - Wyjaśnienie Matematyczne
Algorytm k-NN (k Najbliższych Sąsiadów) klasyfikuje nowy obiekt na podstawie klasy jego najbliższych sąsiadów w przestrzeni cech. Obiekt jest przypisany do tej klasy, która występuje najczęściej wśród k najbliższych punktów. 

Matematycznie:
- Dla każdego nowego punktu obliczamy odległość euklidesową od wszystkich innych punktów w zbiorze treningowym.
- Sortujemy te odległości rosnąco i wybieramy k najbliższych sąsiadów.
- Punkt jest klasyfikowany na podstawie większościowej klasy wśród tych sąsiadów.

Wzór na odległość euklidesową:
$$
d(p, q) = \sqrt{\sum_{i=1}^{n}(p_i - q_i)^2}
$$

## Dane Treningowe
W naszym projekcie klasyfikujemy jakość produktów na podstawie dwóch cech: **wagi** i **rozmiaru**. Oto przykładowy zestaw danych:

| Waga (g) | Rozmiar (cm) | Jakość  |
|----------|---------------|---------|
| 150      | 20            | Dobry   |
| 180      | 25            | Dobry   |
| 120      | 15            | Wadliwy |
| 140      | 18            | Wadliwy |
| 170      | 22            | Dobry   |

## Obliczenia Krok po Kroku
### Przykład klasyfikacji nowego produktu:
1. **Nowy produkt**: Waga = 160g, Rozmiar = 23cm.
2. **Obliczanie odległości** do wszystkich punktów w zbiorze treningowym.
3. **Wybieranie 3 najbliższych sąsiadów (k = 3)**.
4. **Przypisywanie klasy** na podstawie większościowego głosu sąsiadów.

## Implementacja w C#

### Przygotowanie Projektu
1. Otwórz Visual Studio i utwórz nowy projekt konsolowy.
2. Dodaj referencje do niezbędnych bibliotek, np. `System.Linq`.

### Kod Źródłowy
Oto przykładowa implementacja algorytmu k-NN w C#:

```csharp
using System;
using System.Linq;

namespace ProductQualityClassification
{
    class Program
    {
        // Dane treningowe: waga, rozmiar, jakość
        static (double, double, string)[] products = {
            (150, 20, "Dobry"),
            (180, 25, "Dobry"),
            (120, 15, "Wadliwy"),
            (140, 18, "Wadliwy"),
            (170, 22, "Dobry")
        };

        static void Main(string[] args)
        {
            // Nowy produkt do klasyfikacji
            var newProduct = (waga: 160.0, rozmiar: 23.0);

            // Klasyfikacja za pomocą k-NN
            var k = 3; // Liczba sąsiadów
            var result = ClassifyProduct(newProduct, k);
            Console.WriteLine($"Nowy produkt jest sklasyfikowany jako: {result}");
        }

        static string ClassifyProduct((double waga, double rozmiar) product, int k)
        {
            // Obliczanie odległości euklidesowej do wszystkich produktów
            var distances = products.Select(p =>
                (distance: Math.Sqrt(Math.Pow(p.Item1 - product.waga, 2) + Math.Pow(p.Item2 - product.rozmiar, 2)),
                 quality: p.Item3))
                .OrderBy(p => p.distance)
                .Take(k);

            // Zwracanie najczęściej występującej klasy
            return distances.GroupBy(p => p.quality)
                            .OrderByDescending(g => g.Count())
                            .First().Key;
        }
    }
}
```

### Wyjaśnienie Kodu
- **products** – Zbiór danych treningowych, gdzie każdy produkt ma przypisaną wagę, rozmiar oraz jakość.
- **ClassifyProduct** – Funkcja oblicza odległość euklidesową między nowym produktem a każdym produktem w zbiorze treningowym, a następnie wybiera `k` najbliższych sąsiadów.
- **GroupBy** i **OrderByDescending** – Metody te służą do zliczania najczęściej występującej klasy wśród sąsiadów.

## Uruchomienie Aplikacji
1. Skompiluj projekt w Visual Studio.
2. Uruchom aplikację, a następnie wpisz cechy nowego produktu (waga i rozmiar), aby sprawdzić, jaką jakość przewidzi algorytm k-NN.

## Podsumowanie
W projekcie zademonstrowaliśmy, jak w prosty sposób zaimplementować algorytm k-NN w C# z wykorzystaniem technologii .NET. Przykład klasyfikacji jakości produktów może być rozbudowany o dodatkowe cechy i ulepszony poprzez optymalizację wartości `k`.

## Materiały Dodatkowe
- [Dokumentacja .NET](https://docs.microsoft.com/dotnet/)
- [Podstawy programowania w C#](https://learn.microsoft.com/dotnet/csharp/)
- [Wprowadzenie do uczenia maszynowego](https://learn.microsoft.com/dotnet/machine-learning/)

## Licencja
Ten projekt jest licencjonowany na podstawie licencji MIT.

## Autor
Albert Moczadło

## Zachęcam do Współpracy
Jeśli masz pomysły na ulepszenie tego projektu lub znalazłeś błąd, nie wahaj się otworzyć **Issue** lub przesłać **Pull Request**.

## Pliki w Repozytorium
- **Machine-Learning-kNN-Product-Quality-Classification-CSharp/Program.cs** – implementacja algorytmu k-NN.
- **README.md** – dokumentacja projektu.

## Kontakt
Jeśli masz pytania lub potrzebujesz pomocy, możesz się ze mną skontaktować przez GitHub lub e-mail: [albertmoczadlo@gmail.com](mailto:albertmoczadlo@gmail.com)
