public static int BinSearchLeftBorder(long[] array, long value, int left, int right)
{
    if (right - left == 1) return left;
    var m = (left + right) / 2;
    if (array[m] < value)
        return BinSearchLeftBorder(array, value, m, right);
    return BinSearchLeftBorder(array, value, left, m);
}
