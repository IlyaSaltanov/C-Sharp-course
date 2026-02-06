public static int GetElementCount(int[] items, int itemToCount)
{
	int k = 0;
	if (items.Length > 0)
	{	
		for (int i = 0; i <= (items.Length-1); i++)
		{
			if (items[i] == itemToCount)
			{
				k = k + 1;
			}
		}
	}
	return k;
}