// Вставьте сюда финальное содержимое файла HistogramTask.cs

namespace Names;

internal static class HistogramTask
{
    public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
    {
        var days = new string[31];
		for (var y = 0; y < days.Length; y++)
			days[y] = (y + 1).ToString();
		var birthsCounts = new double[31];
		foreach (var nameTwo in names)
            if (nameTwo.Name == name && nameTwo.BirthDate.Day != 1)
            {
                birthsCounts[nameTwo.BirthDate.Day-1]++;
            }
        return new HistogramData(
            $"Рождаемость людей с именем '{name}'", days, birthsCounts);
    }
}