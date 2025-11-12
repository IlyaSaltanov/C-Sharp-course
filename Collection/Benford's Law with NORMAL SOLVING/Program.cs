



using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection.Metadata;

class Program
{
    public static int[] GetBenfordStatistics(string text)
    {
        // Regex regex = new Regex(@"[0-9]");
        var statistics = new int[10];
        string[] lines = text.Split("\n");
        for (int i = 0; i < lines.Length; i++)
        {
            // Console.WriteLine(lines[i]);
            var line = lines[i];
            bool flag = true;
            // for (int w = 0; w < line.Length; w++)
            // {
            //     if (!char.IsDigit(line[w]))
            //     {
            //         flag = false;
            //         break;
            //     }
            // }
            if (flag == true)
            {
                var elements = line.Split(' ');
                // var number = elements[elements.Length - 1];
                for (int q = 0; q < elements.Length; q++)
                {
                    var element = elements[q];
                    bool flag2 = true;
                    for (int r = 0; r < element.Length; r++)
                    {
                        if (!char.IsDigit(element[r]))
                        {
                            flag2 = false;
                            break;
                        }
                    }
                    if (flag2 == true)
                    {
                        var digit = element[0];
                        statistics[digit - '0'] += 1;
                    }
                }
            }
        }
        return statistics;
    }

    static void PrintNumbers(int[] massive)
    {
        string result = "";
        for (int i = 0; i < massive.Length; i++)
        {
            result = result + massive[i] + ", ";
            // Console.WriteLine(result);
        }
        Console.WriteLine(result.Substring(0, result.Length - 2));
    }
    static void Main(string[] args)
    {
        // Здесь будет ваш код
        string tallestBuildings =
            """
            Burj Khalifa 830
            Petronius (oil platform) 640
            Tokyo Skytree 634
            KVLY-TV mast 629
            Canton Tower 604
            Abraj Al Bait Towers 601
            Bullwinkle (oil platform) 529
            Troll A platform 472
            Lualualei VLF transmitter 458
            Petronas Twin Towers 452
            Willis Tower 442
            Ekibastuz GRES-Two Power Station 420
            Dimona Radar Facility 400
            Kiev TV Tower 385
            Zhoushan Island Overhead Powerline Tie 370
            Gerbrandy Tower 367
            TV Tower Vinnytsia 354
            Millau Viaduct 342
            Amazon Tall Tower Observatory 325
            Lakihegy Tower 314
            Jinping-I Dam 305
            Star Tower 291
            H-One Tower 274
            Djamaa el Djazaïr 265
            Mohammed bin Rashid Al Maktoum Solar Park 262
            LR 248
            GE wind turbine at Naturstromspeicher Gaildorf 247
            Statue of Unity 240
            Noble Lloyd Noble 214
            Kalisindh Thermal Power Station 202
            Gateway Arch 192
            Main tower of Kuwait Towers 187
            Anaconda Smelter Stack 178
            Olympic Stadium 175
            San Jacinto Monument 174
            Niederaussem Power Station 172
            Jeddah Flagpole 171
            High Roller 168
            Mole Antonelliana 168
            Ulmer Münster 162
            Vehicle Assembly Building 160
            Santa Cruz del Valle de los Caídos 152
            Arecibo Telescope 150
            Kingda Ka 139
            Great Pyramid of Giza 139
            Kuala Lumpur International Airport Two Control Tower 141
            Zumanjaro: Drop of Doom 139
            Kockums Crane 138
            Jetavanaramaya 122
            Gliwice Radio Tower 118
            Gasometer Oberhausen 118
            Schapfen Mill Tower 115
            Pillar of third section of Gletscherbahn Kaprun 114
            Joseph Chamberlain Memorial Clock Tower 100
            Éole 96
            Mjøstårnet 85
            Ericsson Globe 85
            Île Vierge Lighthouse 83
            Murudeshwara Temple 76
            """;
        PrintNumbers(GetBenfordStatistics("1"));
        PrintNumbers(GetBenfordStatistics("abc"));
        PrintNumbers(GetBenfordStatistics("123 456 789"));
        PrintNumbers(GetBenfordStatistics("abc 123 def 456 gf 789 i"));
        PrintNumbers(GetBenfordStatistics(tallestBuildings));
    }
}

// Дополнительные элементы можно добавить здесь