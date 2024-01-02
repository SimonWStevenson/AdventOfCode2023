using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
//using AdventDay2;
//using AdventDay5;
using System.Runtime.CompilerServices;
namespace AdventDay5b;

class Garden {
    public List<SeedRange> SeedRanges {get; set;} = new();
    public List<Map> Maps {get; set;} = new();
    public long LowestLocationNo {get {
        return SeedRanges.Where(s => s.Type == "location").Min(s => s.StartNo);
    }}
    }

class SeedRange {
    public long StartNo {get; set;}
    //public long Length {get; set;}
    public long EndNo {get; set;}
    public string Type {get; set;} = "seed";
    private Garden garden;
    public SeedRange(Garden garden){
        this.garden = garden;
    }}

class Map {
    public string MapType {get; set;} = "";
    public string MapSource {get {
        return MapType.Split("-")[0];
    }}
    public string MapDestination {get {
        return MapType.Split("-")[2];
    }}
    public long SourceRangeStart {get; set;}
    public long SourceRangeEnd {get {
        return SourceRangeStart + RangeLength - 1;
    }}
    public long DestinationRangeStart {get; set;}
    public long Diff {get {
        return DestinationRangeStart - SourceRangeStart;
    }}
    public long RangeLength {get; set;}
}

class Program
{
    public static void MyMain()
    {
        //var stopwatch1 = Stopwatch.StartNew();
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day05\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);
        Garden thisGarden = new();

        // seeds
        List<string> mySeedsList = lines[0].Split(":").ToList();
        List<string> mySeeds = mySeedsList[1].Trim().Split(" ").ToList();
        int NoOfRows = mySeeds.Count/2-1;
        for (int i = 0; i <= NoOfRows; i++){
            SeedRange thisSeed = new(thisGarden);
            thisSeed.StartNo = Convert.ToInt64(mySeeds[i * 2]);
            var length = Convert.ToInt64(mySeeds[i * 2 + 1]);
            thisSeed.EndNo = thisSeed.StartNo + length - 1;
            thisGarden.SeedRanges.Add(thisSeed);
        }

        // maps
        var currentMap = "";
        foreach (string line in lines){
            if (line == ""){}
            else if (line.Contains("seeds:")){}
            else {
                if (line.Contains("map:")){
                    currentMap = line.Split(" ")[0].Trim();
                }
                else {
                    Map thisMap = new();
                    thisMap.MapType = currentMap;
                    List<string> thisList = line.Split(" ").ToList();
                    thisMap.DestinationRangeStart = Convert.ToInt64(thisList[0].Trim());
                    thisMap.SourceRangeStart = Convert.ToInt64(thisList[1].Trim());
                    thisMap.RangeLength = Convert.ToInt64(thisList[2].Trim());
                    thisGarden.Maps.Add(thisMap);
                }
            }
        }

        // part 2 seed function
        List<SeedRange> MapValues(List<SeedRange> inputRanges, List<Map> maps, string inputType, string outputType){
            List<Map> filteredMaps = thisGarden.Maps.Where(i => i.MapSource == inputType).ToList();
            List<SeedRange> inputQueue = new(inputRanges);
            List<SeedRange> outputQueue = new();
            while (inputQueue.Count > 0){
                var thisItem = inputQueue.First();
                var hasThisItemMatched = false;

                foreach (Map m in filteredMaps){
                    var start = Math.Max(thisItem.StartNo, m.SourceRangeStart);
                    var end = Math.Min(thisItem.EndNo, m.SourceRangeEnd);

                    // if we do match
                    if (start <= end){
                        // add the matching bit to the output
                        hasThisItemMatched = true;
                        SeedRange matchedItem = new(thisGarden)
                        {
                            StartNo = start + m.Diff,
                            EndNo = end + m.Diff,
                            Type = m.MapDestination
                        };
                        outputQueue.Add(matchedItem);

                        // add any pre-matched bits to the end of the input
                        if(start > thisItem.StartNo){
                            SeedRange prematchedPart = new(thisGarden)
                            {
                                StartNo = thisItem.StartNo,
                                EndNo = start - 1,
                                Type = inputType
                            };
                            inputQueue.Add(prematchedPart);
                        }

                        // add any post-matched bits to the end of the input
                        if(end < thisItem.EndNo){
                            SeedRange postmatchedPart = new(thisGarden)
                            {
                                StartNo = end + 1,
                                EndNo = thisItem.EndNo,
                                Type = inputType
                            };
                            inputQueue.Add(postmatchedPart);
                        }
                        // stop looping the maps, we already found one
                        break;                 
                    }      
                }
                // if we didn't match at all, then push forwards as-is
                if(hasThisItemMatched == false) {
                    SeedRange unmatchedItem = new(thisGarden)
                    {
                        StartNo = thisItem.StartNo,
                        EndNo = thisItem.EndNo,
                        Type = outputType,
                    };
                    outputQueue.Add(unmatchedItem);                        
                }
                inputQueue.RemoveAt(0);
            }
            return outputQueue;
        }

        void CalculateMapping(string inputType, string outputType){
            var seeds = thisGarden.SeedRanges.Where(i => i.Type == inputType).ToList();
            var seedToSoilMaps = thisGarden.Maps.Where(i => i.MapSource == inputType).ToList();
            var output = MapValues(seeds, seedToSoilMaps, inputType, outputType);
            foreach (var o in output){
                thisGarden.SeedRanges.Add(o);
            }
        }

        // convert seed to soil
        CalculateMapping("seed", "soil");
        CalculateMapping("soil", "fertilizer");
        CalculateMapping("fertilizer", "water");
        CalculateMapping("water", "light");
        CalculateMapping("light", "temperature");
        CalculateMapping("temperature", "humidity");
        CalculateMapping("humidity", "location");
        Console.WriteLine("Day 5 -- Part Two: " + thisGarden.LowestLocationNo);

        //string json2 = JsonSerializer.Serialize(thisGarden.SeedRanges, new JsonSerializerOptions {WriteIndented = true});
        //string json2 = JsonSerializer.Serialize(thisGarden.Maps, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json2);
        
    }
}

