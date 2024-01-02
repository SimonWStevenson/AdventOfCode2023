using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using AdventDay2;
namespace AdventDay5;

class Garden {
    public List<Seed> Seeds {get; set;} = new();
    //public List<Seed> SeedsLong {get; set;} = new();
    public List<Map> Maps {get; set;} = new();
    public long LowestLocationNo {get {
        return Seeds.Min(s => s.LocationNo);
    }}
    //public long LowestLocationNoLong {get {
    //    return SeedsLong.Min(s => s.LocationNo);
    //}}
}

class Seed {
    public long SeedNo {get; set;}
    public long SoilNo {get; set;}
    public long FertilizerNo {get; set;}
    public long WaterNo {get; set;}
    public long LightNo {get; set;}
    public long TemperatureNo {get; set;}
    public long HumidityNo {get; set;}
    public long LocationNo {get; set;}
    private Garden garden;
    public Seed(Garden garden){
        this.garden = garden;
    }
    public void CalculateAll(){
        this.SoilNo = CalculateValue("seed", this.SeedNo);
        this.FertilizerNo = CalculateValue("soil", this.SoilNo);
        this.WaterNo = CalculateValue("fertilizer", this.FertilizerNo);
        this.LightNo = CalculateValue("water", this.WaterNo);
        this.TemperatureNo = CalculateValue("light", this.LightNo);
        this.HumidityNo = CalculateValue("temperature", this.TemperatureNo);
        this.LocationNo = CalculateValue("humidity", this.HumidityNo);
    }
    public long CalculateValue(string mySource, long myInput){
        foreach (Map m in this.garden.Maps.Where(m => m.MapSource == mySource)){
            if (myInput >= m.SourceRangeStart && myInput <= m.SourceRangeEnd){
                return myInput + m.Diff;
            }
        }
        return myInput;
    }
}

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

        // seeds - part one
        List<string> mySeedsList = lines[0].Split(":").ToList();
        List<string> mySeeds = mySeedsList[1].Trim().Split(" ").ToList();
        foreach (string s in mySeeds){
            Seed thisSeed = new(thisGarden);
            thisSeed.SeedNo = Convert.ToInt64(s);
            thisGarden.Seeds.Add(thisSeed);
        }
        //Console.WriteLine($"Created seeds - part one -- Elapsed time: {stopwatch1.ElapsedMilliseconds} milliseconds");

        // seeds - part two
        // rewrite with this: https://www.reddit.com/r/adventofcode/comments/18b560a/comment/kc4dbhq
        /*
        List<string> mySeedsListLong = lines[0].Split(":").ToList();
        List<string> mySeedsLong = mySeedsList[1].Trim().Split(" ").ToList();
        var NoOfInitialSeeds = mySeedsLong.Count/2;
        for (int i = 0; i < NoOfInitialSeeds; i++){
            var firstpos = i*2;
            var secondpos = firstpos + 1;
            var InitialSeed = Convert.ToInt64(mySeedsLong[firstpos]);
            var NoOfSeeds = Convert.ToInt64(mySeedsLong[secondpos]);
            for (long j = 0; j < NoOfSeeds; j++){
                var SeedToCreate = InitialSeed + j;
                if(!thisGarden.SeedsLong.Any(s => s.SeedNo == SeedToCreate)){
                    Seed thisSeed = new(thisGarden)
                    {
                        SeedNo = SeedToCreate
                    };
                    thisGarden.SeedsLong.Add(thisSeed);
                    
                    //string json = JsonSerializer.Serialize(thisGarden.SeedsLong, new JsonSerializerOptions {WriteIndented = true});
                    //Console.WriteLine(json);
                }
                //if (j % 1000000 == 0){Console.WriteLine("Processed " + j + " rows out of " + NoOfSeeds + " - Elapsed time: " + stopwatch1.ElapsedMilliseconds/1000 + " seconds");}
            }
        }
        */
        //Console.WriteLine($"Created seeds - part two -- Elapsed time: {stopwatch1.ElapsedMilliseconds/1000} seconds");


        // maps
        var currentMap = "";
        foreach (string line in lines){
            if (line == ""){}
            else if (line.Contains("seeds:")){}
            else {
                if (line.Contains("map:")){
                    currentMap = line.Split(" ")[0].Trim();
                    //Console.WriteLine(currentMap);
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

                //Console.WriteLine(line);

            }
            //Console.WriteLine(line);
        }
        //Console.WriteLine($"Created maps -- Elapsed time: {stopwatch1.ElapsedMilliseconds/1000} seconds");

        // Map seeds to soils
        int count = 1;
        foreach (Seed s in thisGarden.Seeds){
            //Console.WriteLine("Processed " + count + " rows out of " + thisGarden.Seeds.Count + " - Elapsed time: " + stopwatch1.ElapsedMilliseconds/1000 + " seconds");
            s.CalculateAll();
            //if (count % 1000000 == 0){
            //}
            count++;
        }
        //Console.WriteLine($"Calculated locations - part one -- Elapsed time: {stopwatch1.ElapsedMilliseconds/1000} seconds");
        //foreach (Seed s in thisGarden.SeedsLong){
        //    s.CalculateAll();
        //}
        //Console.WriteLine($"Calculated locations - part two -- Elapsed time: {stopwatch1.ElapsedMilliseconds/1000} seconds");
        // Part One
        Console.WriteLine("Day 5 -- Part One: " + thisGarden.LowestLocationNo);

        // Part Two
        //Console.WriteLine("Day 5 -- Part Two: " + thisGarden.LowestLocationNoLong);

        //string json = JsonSerializer.Serialize(thisGarden.SeedsLong, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        
    }
}

