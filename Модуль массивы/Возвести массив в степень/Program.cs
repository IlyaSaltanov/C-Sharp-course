public static int[] GetPoweredArray(int[] arr, int power)
{
	int[] sp = new int[arr.Length];
	for (int i = 0; i <= (arr.Length - 1); i++)
	{
		sp[i] = (int)Math.Pow(arr[i], power);
	}
	return sp;
}