public static int MaxIndex(double[] array)
{
	if (array.Length == 0)
        return -1;
	var max = array[0]; 
	var index = 0;
	var list = new List<int>(){0};
    for (int i = 1; i < array.Length; i++)
		if (array[i] > max)
		{
			max = array[i];
			index = i;
		}
    return index;
}