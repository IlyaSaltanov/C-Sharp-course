public static bool ContainsAtIndex(int[] sp, int[] subSp, int count)
{
	if (subSp.Length == 0)
	{
		return true;
	}
   // Сравниваем элементы посимвольно
    for (int k = 0; k < subSp.Length; k++)
    {
        if (sp[count + k] != subSp[k])
            return false;
    }
    
    return true;
}