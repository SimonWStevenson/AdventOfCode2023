using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Linq;
namespace AdventDay12;

class Map {
    public List<SpringRow> SpringRows {get; set;} = [];
}

class SpringRow {
    private static readonly Dictionary<string, List<int>> matchCache = [];
    public required string Conditions {get; set;}
    public List<int> DamagedStringsGroup {get; set;} = [];
    private bool IsGroupValid(string TestConditions) {
        var upToQuestionMark = TestConditions.Split("?")[0];
        List<int> lengthsOfEachGroup;
        
        // Cache approach: https://pastebin.com/djb8RJ85

        // get it from the cache if possible
        if(matchCache.TryGetValue(upToQuestionMark, out List<int>? value)) {lengthsOfEachGroup = value;}

        // otherwise calculate and save to cache
        var splitCondition = upToQuestionMark.Split(".", StringSplitOptions.RemoveEmptyEntries);
        lengthsOfEachGroup = splitCondition.Select(s => s.Length).ToList();
        matchCache[upToQuestionMark] = lengthsOfEachGroup;

        int NoOfConditionGroups = lengthsOfEachGroup.Count;
        int NoOfExpectedGroups = DamagedStringsGroup.Count;

        if (NoOfConditionGroups > NoOfExpectedGroups){
            //Console.WriteLine("Error 1");
            return false;
            }
        else {
            //Console.WriteLine("TestConditions.Length: " + TestConditions.Length);
            if (TestConditions.Length != Conditions.Length){
                for (int i = 0; i < NoOfConditionGroups - 1; i++){
                    if (lengthsOfEachGroup[i] != DamagedStringsGroup[i]){
                        //Console.WriteLine("Error 2");
                        return false;
                        }
                }
            }
            else if (NoOfConditionGroups < NoOfExpectedGroups){return false;}
            else {
                for (int i = 0; i < NoOfConditionGroups; i++){
                    if (lengthsOfEachGroup[i] != DamagedStringsGroup[i]){
                        //Console.WriteLine("Error 3");
                        return false;
                        }
                }
            }
            return true;
        }
    }
    public int NoOfValidCombinations {get {
        Dictionary<int, string> cache = [];
        int ConditionsLength = Conditions.Length;

        // put in the first input character
        if (Conditions[0]=='?'){
            var toAssessA = "#";
            var resultA = IsGroupValid(toAssessA);
            if(resultA){cache.Add(cache.Count, toAssessA);}
            var toAssessB = ".";
            var resultB = IsGroupValid(toAssessB);
            if(resultB){cache.Add(cache.Count, toAssessB);}
        }
        else {
            var toAssess = Conditions[0].ToString();
            var result = IsGroupValid(toAssess);
            if(result){cache.Add(cache.Count, toAssess);}
        }
        //Console.WriteLine("Initial setup:");
        //Console.WriteLine(JsonSerializer.Serialize(cache, new JsonSerializerOptions {WriteIndented = true}));
        //Console.WriteLine("------------------------------------------");

        // check the remaining input characters
        for (int i = 1; i < ConditionsLength; i++){
            var thisCache = cache;
            //Console.WriteLine("i: " + i);
            //Console.WriteLine("Conditions[i]: " + Conditions[i]);
            //Console.WriteLine("Cache at start:");
            //Console.WriteLine(JsonSerializer.Serialize(thisCache, new JsonSerializerOptions {WriteIndented = true}));
            cache = [];
            foreach (var kvp in thisCache){
                //Console.WriteLine("Existing cache position: " + kvp.Key);
                if (Conditions[i]=='?'){
                    var toAssessA = kvp.Value + "#";
                    var resultA = IsGroupValid(toAssessA);
                    //Console.WriteLine("toAssessA: " + toAssessA + ", resultA: " + resultA);
                    if(resultA){cache.Add(cache.Count, toAssessA);}

                    var toAssessB = kvp.Value + ".";
                    var resultB = IsGroupValid(toAssessB);
                    //Console.WriteLine("resultB: " + resultB);
                    if(resultB){cache.Add(cache.Count, toAssessB);}
                }
                else {
                    var toAssess = kvp.Value + Conditions[i].ToString();
                    var result = IsGroupValid(toAssess);
                    //Console.WriteLine("toAssessA: " + toAssess + ", result: " + result);
                    if(result){cache.Add(cache.Count, toAssess);}
                }
            }
            //Console.WriteLine(JsonSerializer.Serialize(cache, new JsonSerializerOptions {WriteIndented = true}));
            //Console.WriteLine("------------------------------------------");
        }
        return cache.Count;
    }}
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day12\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);

        Map thisMap = new();

        // input data and duplicate expansion rows
        foreach (string l in lines){
            var splitLine = l.Split(' ');
            SpringRow thisSpringRow = new(){Conditions = splitLine[0]};

            var splitLineDamaged = splitLine[1].Split(',');
            thisSpringRow.DamagedStringsGroup ??= new List<int>();
            foreach(string i in splitLineDamaged){
                thisSpringRow.DamagedStringsGroup.Add(Convert.ToInt32(i));
            }
            thisMap.SpringRows.Add(thisSpringRow);
        }

        //var myInput = "#.#.###.#?#";
        //thisMap.SpringRows[0].Conditions;
        //var myResult = thisMap.SpringRows[0].IsGroupValid(myInput);
        //Console.WriteLine(myResult);

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
        //Console.WriteLine(JsonSerializer.Serialize(thisMap.SpringRows[5], new JsonSerializerOptions {WriteIndented = true}));
        //Console.WriteLine(thisMap);
        
        
    }
}

