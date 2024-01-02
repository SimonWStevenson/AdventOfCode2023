namespace AdventDay12backup;

class Map {
    public List<SpringRow> SpringRows {get; set;} = [];
}

class SpringRow {
    public required string Symbol {get; set;}
    public List<int> DamagedStringsGroup {get; set;} = [];
    List<string> SymbolCombination {get {
        // prep empty list
        var myList = new List<string>{};

        // prep unknowns
        int totalUnknowns = Symbol.Count(x => x == '?');
        int totalCombinations = (int)Math.Pow(2, totalUnknowns);

        // calculate all possible combinations
        for (int i = 0; i < totalCombinations; i++)
        {
            char[] combination = Symbol.ToCharArray();
            for (int j = 0; j < totalUnknowns; j++)
            {
                char replacement = ((i >> j) & 1) == 0 ? '#' : '.';
                int index = Array.IndexOf(combination, '?');
                combination[index] = replacement;
            }
            myList.Add(new string(combination));
        }
        return myList;
    }}

    List<string> SymbolCombinationValid {get {
        var myValidList = new List<string>{};
        foreach (string possList in SymbolCombination){
            var splitPossList = possList.Split('.', StringSplitOptions.RemoveEmptyEntries);
            var count = splitPossList.Length;
            if(count == DamagedStringsGroup.Count){
                var itMatches = true;
                for (int i = 0; i < count; i++){
                    if(splitPossList[i].Length!=DamagedStringsGroup[i]){
                        itMatches = false;
                    }
                }
                if(itMatches){
                    myValidList.Add(possList);
                }
            }
        }
        return myValidList;
    }}
    public int NoOfValidCombinations {get {
        return SymbolCombinationValid.Count;
    }}
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day12\\", "Test.txt");
        string[] lines = File.ReadAllLines(filePath);

        Map thisMap = new();

        // input data and duplicate expansion rows
        foreach (string l in lines){
            var splitLine = l.Split(' ');
            SpringRow thisSpringRow = new(){Symbol = splitLine[0]};

            var splitLineDamaged = splitLine[1].Split(',');
            thisSpringRow.DamagedStringsGroup ??= new List<int>();
            foreach(string i in splitLineDamaged){
                thisSpringRow.DamagedStringsGroup.Add(Convert.ToInt32(i));
            }
            thisMap.SpringRows.Add(thisSpringRow);
        }

        // algorithm option 1:
        // for each line
        // calculate every combination, ie for each ?, it can be either . or #
        // filter to those which fit the pattern
        // sum


        // Part One
        Console.WriteLine("Day 12 -- Part One: " + thisMap.SpringRows.Sum(x => x.NoOfValidCombinations));

        // Part Two
        //Console.WriteLine("Day 10 -- Part Two: " + thisMap.CalculateInternalArea(lines));

        //var forprint = thisMap.GalaxyPairs.Where(x => x.Location1.GalaxyId == 7 && x.Location2.GalaxyId == 1);
        //Console.WriteLine(JsonSerializer.Serialize(forprint, new JsonSerializerOptions {WriteIndented = true}));
        //Console.WriteLine(JsonSerializer.Serialize(thisMap, new JsonSerializerOptions {WriteIndented = true}));
        //Console.WriteLine(thisMap);
        
        
    }
}

