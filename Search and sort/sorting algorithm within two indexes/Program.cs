public static void BubbleSortRange(int[] array, int left, int right)
{
    for (int i = right; i > 0; i--)
        for (int j = left; j <= i-1; j++)
            if (array[j] > array[j + 1])
            {
                var t = array[j + 1];
                array[j + 1] = array[j];
                array[j] = t;
            }
}
