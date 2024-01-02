using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Linq;
using System.Text;
namespace AdventDay12b;

class Map {
    public List<SpringRow> SpringRows {get; set;} = new List<SpringRow>();
}

class SpringRow {
    private static readonly Dictionary<string, List<int>> matchCache = [];
    /*
    public static void AddToMatchCache(string key, List<int> value){
        matchCache[key] = value;
    }
    public static List<int>? GetFromMatchCache(string key){
        if(matchCache.TryGetValue(key, out List<int>? value)) { return value; }
        else {return null;}
    }
    */
    public required string Conditions {get; set;}
    public List<int> DamagedStringsGroup {get; set;} = [];
    
    private bool IsGroupValid(string TestConditions) {
        var upToQuestionMark = TestConditions.Split("?")[0];
        List<int> lengthsOfEachGroup;

        // get it from the cache if possible
        if(matchCache.TryGetValue(upToQuestionMark, out List<int>? value)) {lengthsOfEachGroup = value;}
        //if(GetFromMatchCache(upToQuestionMark) is not null){
        //    lengthsOfEachGroup = GetFromMatchCache(upToQuestionMark);
        //}
        else {
            // otherwise calculate and save to cache
            var splitCondition = upToQuestionMark.Split(".", StringSplitOptions.RemoveEmptyEntries);
            lengthsOfEachGroup = splitCondition.Select(s => s.Length).ToList();
            matchCache[upToQuestionMark] = lengthsOfEachGroup;            
            //AddToMatchCache(upToQuestionMark, lengthsOfEachGroup);
        }


        int NoOfConditionGroups = lengthsOfEachGroup.Count;
        int NoOfExpectedGroups = DamagedStringsGroup.Count;

        if (NoOfConditionGroups > NoOfExpectedGroups){
            return false;
            }
        else {
            if (TestConditions.Length != Conditions.Length){
                for (int i = 0; i < NoOfConditionGroups - 1; i++){
                    if (lengthsOfEachGroup[i] != DamagedStringsGroup[i]){return false;}
                }
            }
            else if (NoOfConditionGroups < NoOfExpectedGroups){return false;}
            else {
                for (int i = 0; i < NoOfConditionGroups; i++){
                    if (lengthsOfEachGroup[i] != DamagedStringsGroup[i]){return false;}
                }
            }
            return true;
        }
    }

    public int NoOfValidCombinations {get {
        //Dictionary<int, string> cache = [];
        List<string> cache = [];
        int ConditionsLength = Conditions.Length;

        // when adding the next character
        // if it's not a question mark
        // then can we add all the non-question mark characters?
        // instead of adding them one-by-one and calculating everything as per current

        // put in the first input character
        if (Conditions[0]=='?'){
            var toAssessA = "#";
            var resultA = IsGroupValid(toAssessA);
            if(resultA){
                //cache.Add(cache.Count, toAssessA);
                cache.Add(toAssessA);
                }
            var toAssessB = ".";
            var resultB = IsGroupValid(toAssessB);
            if(resultB){
                //cache.Add(cache.Count, toAssessB);
                cache.Add(toAssessB);
            }
        }
        else {
            var toAssess = Conditions[0].ToString();
            var result = IsGroupValid(toAssess);
            if(result){
                //cache.Add(cache.Count, toAssess);
                cache.Add(toAssess);
                }
        }

        // check the remaining input characters
        for (int i = 1; i < ConditionsLength; i++){
            var thisCache = cache;
            cache = [];
            foreach (var kvp in thisCache){
                if (Conditions[i]=='?'){
                    //var toAssessA = kvp.Value + "#";
                    var toAssessA = kvp + "#";
                    var resultA = IsGroupValid(toAssessA);
                    if(resultA){
                        //cache.Add(cache.Count, toAssessA);
                        cache.Add(toAssessA);
                    }
                    //var toAssessB = kvp.Value + ".";
                    var toAssessB = kvp + ".";
                    var resultB = IsGroupValid(toAssessB);
                    if(resultB){
                        //cache.Add(cache.Count, toAssessB);
                        cache.Add(toAssessB);
                    }
                }
                else {
                    //var toAssess = kvp.Value + Conditions[i].ToString();
                    var toAssess = kvp + Conditions[i].ToString();
                    var result = IsGroupValid(toAssess);
                    if(result){
                        //cache.Add(cache.Count, toAssess);
                        cache.Add(toAssess);
                    }
                }
            }
            Console.WriteLine("After checking position number: " + i + ", the cache has length: " + cache.Count);
        }
        return cache.Count;
    }}
    
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day12\\", "Test2.txt");
        string[] lines = File.ReadAllLines(filePath);

        Map thisMap = new();

        // input data and duplicate expansion rows
        foreach (string l in lines){
            var splitLine = l.Split(' ');
            SpringRow thisSpringRow = new(){
                Conditions = splitLine[0] 
                    + "?" + splitLine[0]
                    + "?" + splitLine[0]
                    + "?" + splitLine[0]
                    + "?" + splitLine[0]
                };

            var splitLineDamaged = splitLine[1].Split(',');
            thisSpringRow.DamagedStringsGroup ??= [];

            // just run this 5 times, haha
            foreach(string i in splitLineDamaged){thisSpringRow.DamagedStringsGroup.Add(Convert.ToInt32(i));}
            foreach(string i in splitLineDamaged){thisSpringRow.DamagedStringsGroup.Add(Convert.ToInt32(i));}
            foreach(string i in splitLineDamaged){thisSpringRow.DamagedStringsGroup.Add(Convert.ToInt32(i));}
            foreach(string i in splitLineDamaged){thisSpringRow.DamagedStringsGroup.Add(Convert.ToInt32(i));}
            foreach(string i in splitLineDamaged){thisSpringRow.DamagedStringsGroup.Add(Convert.ToInt32(i));}
            thisMap.SpringRows.Add(thisSpringRow);
        }

        // Part One
        Console.WriteLine("Day 12 -- Part Two: " + thisMap.SpringRows.Sum(x => x.NoOfValidCombinations));
       
    }
}

