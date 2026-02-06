public class Solution
{
    public static int FindMajorityElement(int[] nums)
    {

        int candidate = 0;
        int count = 0;
        
        // Первый проход: находим кандидата
        foreach (int num in nums)
        {
            if (count == 0)
            {
                candidate = num;
                count = 1;
            }
            else if (num == candidate)
            {
                count++;
            }
            else
            {
                count--;
            }
        }
        
        // Второй проход: проверяем, действительно ли кандидат встречается
        // более чем n/2 раз (опционально, если гарантировано, что такой элемент существует)
        count = 0;
        foreach (int num in nums)
        {
            if (num == candidate)
                count++;
        }
        
        // Если гарантировано, что элемент с большинством существует,
        // можно вернуть candidate без проверки
        // return candidate;
        
        // Иначе проверяем:
        if (count > nums.Length / 2)
            return candidate;
        else
            throw new ArgumentException("В массиве нет элемента, встречающегося более половины раз");
    }
    
    // Пример использования
    public static void Main()
    {
        // Пример 1
        int[] arr1 = { 3, 3, 4, 2, 3, 3, 3, 1, 3 };
        Console.WriteLine($"X = {FindMajorityElement(arr1)}"); // Вывод: 3
        
        // Пример 2
        int[] arr2 = { 2, 2, 1, 1, 1, 1, 2 };
        Console.WriteLine($"X = {FindMajorityElement(arr2)}"); // Вывод: 1
        
        // Пример 3 (все элементы одинаковые)
        int[] arr3 = { 5, 5, 5, 5, 5 };
        Console.WriteLine($"X = {FindMajorityElement(arr3)}"); // Вывод: 5
        
        // Пример 4 (четное количество элементов)
        int[] arr4 = { 1, 2, 1, 2, 1, 2, 1 };
        Console.WriteLine($"X = {FindMajorityElement(arr4)}"); // Вывод: 1

        // int[] arr5 = { 1, 2, 1, 2, 1, 2, 1, 2 };
        // Console.WriteLine($"X = {FindMajorityElement(arr5)}"); // исключение

        // int[] arr6 = { 3, 3, 4, 2, 3, 3, 3, 1, 3, 4, 4, 4, 4, 4, 4, 4};
        // Console.WriteLine($"X = {FindMajorityElement(arr6)}"); // исключение

        int[] arr7 = { 3, 3, 4, 2, 3, 3, 3, 1, 3, 4, 4, 4, 4, 4, 4, 4, 4};
        Console.WriteLine($"X = {FindMajorityElement(arr7)}"); // не будет исключения
    }
}