// Вставьте сюда финальное содержимое файла HeatmapTask.cs

namespace Names;

internal static class HeatmapTask
{
    public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
    {
        var days = 30;
        var months = 12;
        var heat = new double[days, months];
        
        foreach (var name in names)
        {
            var day = name.BirthDate.Day;
            if (day == 1) continue;
            
            var month = name.BirthDate.Month;
            heat[day - 2, month - 1]++;
        }

        var xTitle = new string[days];

        for (int q = 0; q < days; q++)
        {
            xTitle[q] = (q + 2).ToString();
        }

        var yTitle = new string[months];
        
        for (int q = 0; q < months; q++)
        {
            yTitle[q] = (q + 1).ToString();   
        }

        return new HeatmapData(
            "Тепловая карта рождаемости",
            heat,
            xTitle,
            yTitle);
    }
}