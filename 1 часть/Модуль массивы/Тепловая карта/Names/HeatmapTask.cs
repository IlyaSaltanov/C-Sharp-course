namespace Names;

internal static class HeatmapTask
{
    public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
    {
        var heat = FillHeatmapData(names);
        var xTitle = GenerateDayTitle();
        var yTitle = GenerateMonthTitle();
        
        return new HeatmapData(
            "Тепловая карта рождаемости",
            heat,
            xTitle,
            yTitle);
    }

    private static double[,] FillHeatmapData(NameData[] names)
    {
        const int days = 30;
        const int months = 12;
        var heat = new double[days, months];
        
        foreach (var name in names)
        {
            var day = name.BirthDate.Day;
            if (day == 1) continue;
            
            var month = name.BirthDate.Month;
            heat[day - 2, month - 1]++;
        }
        
        return heat;
    }

    private static string[] GenerateDayTitle()
    {
        var days = 30;
        var labels = new string[days];
        
        for (int q = 0; q < days; q++)
        {
            labels[q] = (q + 2).ToString();
        }
        
        return labels;
    }

    private static string[] GenerateMonthTitle()
    {
        var months = 12;
        var labels = new string[months];
        
        for (int q = 0; q < months; q++)
        {
            labels[q] = (q + 1).ToString();
        }
        
        return labels;
    }
}