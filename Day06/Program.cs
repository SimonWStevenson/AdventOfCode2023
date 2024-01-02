using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Dynamic;
namespace AdventDay6;

class Competition {
    public List<Race> Races {get; set;} = new();
    public List<Race> RacesPart2 {get; set;} = new();
    public long CombinationWaysToWin {get {
        return Races.Aggregate(1, (a, b) => a * b.WaysToWin);
        //return Races.Aggregate((result.WaysToWin, next.WaysToWin) => result.WaysToWin * next.WaysToWin);
    }}
}

class Race {
    public long RecordTime {get; set;}
    public long Distance {get; set;}
    List<RaceStrategy> RaceStrategies {get; set;} = new();
    public void GenerateStrategies(){
        RaceStrategies = new List<RaceStrategy>();
        for (int i = 1; i < RecordTime; i++){
            RaceStrategy thisRaceStrategy = new();
            thisRaceStrategy.TimeAccelerating = i;
            thisRaceStrategy.TimeTravelling = Distance/thisRaceStrategy.SpeedReached;
            thisRaceStrategy.IsWin = thisRaceStrategy.TotalTime < RecordTime;
            RaceStrategies.Add(thisRaceStrategy);
        }
    }
    public long FirstWin {get {
        for (long i = 1; i < RecordTime; i++){
            RaceStrategy thisRaceStrategy = new();
            thisRaceStrategy.TimeAccelerating = i;
            thisRaceStrategy.TimeTravelling = Distance/thisRaceStrategy.SpeedReached;
            thisRaceStrategy.IsWin = thisRaceStrategy.TotalTime < RecordTime;
            if(thisRaceStrategy.IsWin == true){return i;}
            //else {return null;}
            //RaceStrategies.Add(thisRaceStrategy);
        }        
        return 10;
    }}
    public long LastWin {get {
        for (long i = RecordTime; i > 1; i--){
            RaceStrategy thisRaceStrategy = new();
            thisRaceStrategy.TimeAccelerating = i;
            thisRaceStrategy.TimeTravelling = Distance/thisRaceStrategy.SpeedReached;
            thisRaceStrategy.IsWin = thisRaceStrategy.TotalTime < RecordTime;
            if(thisRaceStrategy.IsWin == true){return i;}
            //else {return null;}
            //RaceStrategies.Add(thisRaceStrategy);
        }        
        return 10;
    }}
    public int WaysToWin {get {
        return RaceStrategies.Count(s => s.IsWin);
    }}
    public long WaysToWinPart2 {get {
        return LastWin - FirstWin + 1;
    }}
}

class RaceStrategy {
    public long TimeAccelerating {get; set;}
    public long SpeedReached {get {return TimeAccelerating;}}
    public long TimeTravelling {get; set;}
    public long TotalTime {get {return TimeAccelerating + TimeTravelling;}}
    public bool IsWin {get; set;}
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day06\\", "Input.txt");

        // setup
        // TODO: why is it so slow
        string[] lines = File.ReadAllLines(filePath);
        string[]? lineRaceSplit = null;
        string[]? times = null;
        string[]? distances = null;

        Competition thisCompetition = new();
        foreach (string l in lines){
            //Console.WriteLine(l);
            var lineSplit = l.Split(":");
            var lineType = lineSplit[0].Trim();
            lineRaceSplit = lineSplit[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (lineType == "Time"){
                times = lineRaceSplit;
            }
            else if (lineType == "Distance"){
                distances = lineRaceSplit;
            }
            else {Console.WriteLine("Something went wrong");}
        }
        // setup part 1 races
        if (lineRaceSplit != null && times != null && distances != null){
            for (int i = 0; i <= lineRaceSplit.Length-1; i++){
                Race thisRace = new();
                thisRace.RecordTime = Convert.ToInt32(times[i].Trim());
                thisRace.Distance = Convert.ToInt32(distances[i].Trim());
                thisRace.GenerateStrategies();
                thisCompetition.Races.Add(thisRace);
            }
        }
        else {Console.WriteLine("Something went wrong");}

        // setup part 2 race
        var time = Convert.ToInt64(lines[0].Split(":")[1].Replace(" ", ""));
        var distance = Convert.ToInt64(lines[1].Split(":")[1].Replace(" ", ""));
        Race thisRace2 = new();
        thisRace2.RecordTime = time;
        thisRace2.Distance = distance;
        thisCompetition.RacesPart2.Add(thisRace2);


        // Part One
        Console.WriteLine("Day 6 -- Part One: " + thisCompetition.CombinationWaysToWin);

        // Part Two
        Console.WriteLine("Day 6 -- Part Two: " + thisCompetition.RacesPart2[0].WaysToWinPart2);

        //string json = JsonSerializer.Serialize(thisCompetition.RacesPart2, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        
    }
}

