using System.Diagnostics;

namespace Algos1
{
    public class Algos1
    {
        public static void Main()
        {
            var array = new[] { 3, 1, 5, 2, 6, 4, 15, 20, 11, 32, 12, 46, 22, 23 };
            
            var diagnosticsSort = new CompairSort(array);
            
            diagnosticsSort.Print();
        }
    }

    public class CompairSort
    {
        private int[] _array;
        public static Dictionary<string, List<double>> DictResult = new();

        public CompairSort(int[] array) => _array = array;
        
        private static void UpdateDictionary()
        {
            DictResult.Add("BubbleSort", new List<double>());
            DictResult.Add("HoareSort", new List<double>());
            DictResult.Add("CountingSort", new List<double>());
        }
        
        public void Print()
        {
            UpdateDictionary();
            TimeCounting.Start(_array);

            Console.WriteLine("Сравнение алгоритмов сортировки:");
            Console.WriteLine("Алгоритм\t    Время (10 запусков)");
            
            foreach (var e in DictResult)
            {
                Console.WriteLine($"{e.Key,-20}{e.Value.Average():F4} ms");
            }
        }
    }

    public class TimeCounting
    {
        private delegate void TimeDelegate(int[] array);
        
        private static void Calldelegate(TimeDelegate del, int[] array, string sortName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            del(array);

            stopwatch.Stop();
            
            CompairSort.DictResult[sortName].Add(stopwatch.Elapsed.TotalMilliseconds);
        }
        public static void Start(int[] array)
        {
            for (var i = 0; i < 10; i++)
            {
                Calldelegate(BubbleSort.Sort, array, "BubbleSort");
                Calldelegate(HoareSort.Sort, array, "HoareSort");
                Calldelegate(CountingSort.Sort, array, "CountingSort");
            }
        }
    }

    public class BubbleSort
    {
        public static void Sort(int[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                for (var j = 0; j < array.Length - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        var temp = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }
    }

    public class CountingSort
    {
        public static void Sort(int[] array)
        {
            var maxValue = array.Max();
            var occurrences = new int[maxValue + 1];
            var j = 0;
            
            foreach (var value in array)
                occurrences[value]++;
            
            for (int i = 0; i <= maxValue; i++)
            {
                while (occurrences[i] > 0)
                {
                    array[j] = i;
                    occurrences[i]--;
                    j++;
                }
            }
        }
    }

    public static class HoareSort
    {
        private static void QuickSort(int[] array, int start, int end)
        {
            if (end <= start)
                return;

            var pivot = array[end];
            var storeIndex = start;
            
            for (int i = start; i <= end - 1; i++)
            {
                if (array[i] <= pivot)
                {
                    array.Swap(i, storeIndex);
                    storeIndex++;
                }
            }
            
            array.Swap(storeIndex, end);
            
            if (storeIndex > start)
                QuickSort(array, start, storeIndex - 1);
            if (storeIndex < end)
                QuickSort(array, storeIndex + 1, end);
        }
        
        public static void Swap(this int[] array, int i, int j)
        {
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        
        public static void Sort(int[] array)
        {
            QuickSort(array, 0, array.Length - 1);
        }
    }
}

