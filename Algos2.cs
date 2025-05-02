using System.Diagnostics;
using System.Text;

namespace Algos2;

class StringSearchAlgorithms
{
    // Простой алгоритм поиска
    static int NaiveSearch(string text, string pattern)
    {
        int n = text.Length;
        int m = pattern.Length;
        for (int i = 0; i <= n - m; i++)
        {
            int j;
            for (j = 0; j < m; j++)
            {
                if (text[i + j] != pattern[j])
                    break;
            }
            if (j == m)
                return i;
        }
        return -1;
    }

    // Алгоритм Бойера-Мура
    static int BoyerMooreSearch(string text, string pattern)
    {
        int[] badChar = new int[256];
        BadCharHeuristic(pattern, badChar);

        int s = 0;
        int m = pattern.Length;
        int n = text.Length;

        while (s <= (n - m))
        {
            int j = m - 1;
            while (j >= 0 && pattern[j] == text[s + j])
                j--;

            if (j < 0)
            {
                return s;
            }
            else
            {
                s += Math.Max(1, j - badChar[text[s + j]]);
            }
        }
        return -1;
    }

    static void BadCharHeuristic(string str, int[] badChar)
    {
        for (int i = 0; i < badChar.Length; i++)
            badChar[i] = -1;

        for (int i = 0; i < str.Length; i++)
            badChar[str[i]] = i;
    }

    // Алгоритм Рабина-Карпа
    static int RabinKarpSearch(string text, string pattern)
    {
        const int prime = 101;
        const int d = 256;
        int m = pattern.Length;
        int n = text.Length;
        int p = 0; 
        int t = 0;
        int h = 1;

        for (int i = 0; i < m - 1; i++)
            h = (h * d) % prime;

        for (int i = 0; i < m; i++)
        {
            p = (d * p + pattern[i]) % prime;
            t = (d * t + text[i]) % prime;
        }

        for (int i = 0; i <= n - m; i++)
        {
            if (p == t)
            {
                int j;
                for (j = 0; j < m; j++)
                {
                    if (text[i + j] != pattern[j])
                        break;
                }
                if (j == m)
                    return i;
            }

            if (i < n - m)
            {
                t = (d * (t - text[i] * h) + text[i + m]) % prime;
                if (t < 0)
                    t = t + prime;
            }
        }
        return -1;
    }

    static void TestStringSearchAlgorithms()
    {
        StringBuilder sb = new StringBuilder();
        Random rand = new Random();
        for (int i = 0; i < 1000000; i++)
        {
            sb.Append((char)rand.Next('a', 'z'));
        }
        string text = sb.ToString();
        string pattern = "testpattern";
        
        for (int i = 0; i < 10; i++)
        {
            int pos = rand.Next(0, text.Length - pattern.Length);
            text = text.Insert(pos, pattern);
        }

        Console.WriteLine("\nСравнение алгоритмов поиска подстроки:");
        Console.WriteLine("Алгоритм\t\tВремя (10 запусков)");

        double naiveTime = 0, bmTime = 0, rkTime = 0;

        for (int i = 0; i < 10; i++)
        {
            var watch1 = Stopwatch.StartNew();
            NaiveSearch(text, pattern);
            watch1.Stop();
            naiveTime += watch1.Elapsed.TotalMilliseconds;

            var watch2 = Stopwatch.StartNew();
            BoyerMooreSearch(text, pattern);
            watch2.Stop();
            bmTime += watch2.Elapsed.TotalMilliseconds;

            var watch3 = Stopwatch.StartNew();
            RabinKarpSearch(text, pattern);
            watch3.Stop();
            rkTime += watch3.Elapsed.TotalMilliseconds;
        }

        Console.WriteLine($"Наивный\t\t\t{naiveTime / 10:F4} ms");
        Console.WriteLine($"Бойера-Мура\t\t{bmTime / 10:F4} ms");
        Console.WriteLine($"Рабина-Карпа\t\t{rkTime / 10:F4} ms");
    }

    static void Main(string[] args)
    {
        TestStringSearchAlgorithms();
    }
}
