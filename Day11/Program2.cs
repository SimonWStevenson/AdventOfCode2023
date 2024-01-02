using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Text;
using System.Linq.Expressions;
using System.Data;
namespace AdventDay11b;

class Map {
    public List<List<Location>> Locations {get; set;} = new();
    public List<Location>? Galaxies {get; set;}
    readonly int ExpansionFactor = 1000000 - 1;
    public void IdentifyEmpty(){
        // Is row empty?
        for (int rowIndex = 0; rowIndex < Locations.Count; rowIndex++){
            var IsEmpty = true;
            for (int colIndex = 0; colIndex < Locations[rowIndex].Count; colIndex++){
                if(Locations[rowIndex][colIndex].Symbol == '#'){
                    IsEmpty = false;
                }
            }
            if (IsEmpty == true){
                for (int colIndex = 0; colIndex < Locations[rowIndex].Count; colIndex++){
                    Locations[rowIndex][colIndex].IsRowExpanded = true;
                }
            }
        }

        // Is column empty?
        for (int colIndex = 0; colIndex < Locations[0].Count; colIndex++){
            var IsEmpty = true;
            for (int rowIndex = 0; rowIndex < Locations.Count; rowIndex++){
                if(Locations[rowIndex][colIndex].Symbol == '#'){
                    IsEmpty = false;
                }
            }
            if (IsEmpty == true){
                for (int rowIndex = 0; rowIndex < Locations.Count; rowIndex++){
                    Locations[rowIndex][colIndex].IsColExpanded = true;
                }
            }
        }
    }

    public void CalculateExpansionLevels(){
        var RowsExpanded = 0;
        for (int rowIndex = 0; rowIndex < Locations.Count; rowIndex++){
            if(Locations[rowIndex][0].IsRowExpanded){
                RowsExpanded += 1;
            }
            for (int colIndex = 0; colIndex < Locations[0].Count; colIndex++){
                Locations[rowIndex][colIndex].RowExpansionLevel = RowsExpanded * ExpansionFactor;
            }
        }

        var ColsExpanded = 0;
        for (int colIndex = 0; colIndex < Locations[0].Count; colIndex++){
            if(Locations[0][colIndex].IsColExpanded){
                ColsExpanded += 1;
            }
            for (int rowIndex = 0; rowIndex < Locations.Count; rowIndex++){
                Locations[rowIndex][colIndex].ColExpansionLevel = ColsExpanded * ExpansionFactor;
            }
        }
    }

    public void CalculateIds(){
        for (int colIndex = 0; colIndex < Locations[0].Count; colIndex++){
            for (int rowIndex = 0; rowIndex < Locations.Count; rowIndex++){
                //Console.WriteLine(Locations[rowIndex][colIndex].RowExpansionLevel);
                Locations[rowIndex][colIndex].RowId = rowIndex + Locations[rowIndex][colIndex].RowExpansionLevel;
                Locations[rowIndex][colIndex].ColId = colIndex + Locations[rowIndex][colIndex].ColExpansionLevel;
            }
        }
    }

    public void CalculateGalaxies(){
        List<Location> thisGalaxyList = new();
        int counter = 1;
        for (int rowIndex = 0; rowIndex < Locations.Count; rowIndex++){
            for (int colIndex = 0; colIndex < Locations[rowIndex].Count; colIndex++){
                Location cell = new();
                cell = Locations[rowIndex][colIndex];
                //cell.RowId = rowIndex;
                //cell.ColId = colIndex;
                if (cell.Symbol == '#'){
                    cell.GalaxyId = counter;
                    thisGalaxyList.Add(cell);
                    counter++;
                }
            }
        }
        Galaxies = thisGalaxyList;
    }
    public List<GalaxyPair> GalaxyPairs {get; set;} = new();
    public void CalculateGalaxyPairs(){
        List<GalaxyPair> thisPairs = new();
        Debug.Assert(Galaxies is not null);
        foreach (Location g1 in Galaxies){
            foreach (Location g2 in Galaxies){
                if(g1.GalaxyId > g2.GalaxyId){
                    GalaxyPair thisPair = new()
                    {
                        Location1 = g1,
                        Location2 = g2
                    };
                    thisPairs.Add(thisPair);
                }
            }
        }
        GalaxyPairs = thisPairs;
    }}

class GalaxyPair {
    public Location? Location1 {get; set;}
    public Location? Location2 {get; set;}
    public long Distance {get; set;} //{get {
    public void CalculateDistance(){
        Debug.Assert(Location1 is not null && Location2 is not null);
        int DistCol = Math.Abs(Location1.ColId - Location2.ColId);
        int DistRow = Math.Abs(Location1.RowId - Location2.RowId);
        Distance = DistCol + DistRow;
    }}

class Location {
    public char Symbol {get; set;}
    string LocationType {get {
        if (Symbol == '.'){return "Empty space";}
        else if (Symbol == '#'){return "Galaxy";}
        else {return "UNKNOWN";}
    }}
    public int GalaxyId {get; set;}
    public int RowId {get; set;}
    public int ColId {get; set;}
    public bool IsRowExpanded {get; set;} = false;
    public bool IsColExpanded {get; set;} = false;
    public int RowExpansionLevel {get; set;}
    public int ColExpansionLevel {get; set;}
}

class MyFunctions {
    public static bool IsAllSameCharacter(string input){
        if (string.IsNullOrEmpty(input)){return false;}
        char firstChar = input[0];
        for (int i = 1; i < input.Length; i++){
            if (input[i] != firstChar){return false;}
        }
        return true;
    }
}


class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day11\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);

        Map thisMap = new();

        // input data and duplicate expansion rows
        for (int i = 0; i < lines.Length; i++){
            List<Location> thisLine = new(); //thisMap.Locations 
            for (int j = 0; j < lines[i].Length; j++){
                Location thisLocation = new()
                {
                    Symbol = lines[i][j],
                };
                thisLine.Add(thisLocation);
            }
            thisMap.Locations.Add(thisLine);
        }

        // Imperative code is bad
        thisMap.IdentifyEmpty();
        thisMap.CalculateExpansionLevels();
        thisMap.CalculateIds();
        thisMap.CalculateGalaxies();
        //Console.WriteLine(JsonSerializer.Serialize(thisMap.Locations, new JsonSerializerOptions {WriteIndented = true}));
        thisMap.CalculateGalaxyPairs();
        foreach (GalaxyPair g in thisMap.GalaxyPairs){g.CalculateDistance();}
        
        // print resultant map
        /*
        Console.WriteLine("----------------------------");
        foreach (List<Location> ll in thisMap.Locations){
            foreach (Location l in ll){
                //Console.Write(l.Symbol);
                if(l.Symbol=='#'){Console.Write(l.GalaxyId);}
                else {Console.Write(l.Symbol);}
            }
            Console.Write("\n");
        }
        */


        // Part One
        Console.WriteLine("Day 11 -- Part Two: " + thisMap.GalaxyPairs.Sum(x => x.Distance));

        // Part Two
        //Console.WriteLine("Day 10 -- Part Two: " + thisMap.CalculateInternalArea(lines));

        //var forprint = thisMap.GalaxyPairs.Where(x => x.Location1.GalaxyId == 7 && x.Location2.GalaxyId == 1);
        //Console.WriteLine(JsonSerializer.Serialize(forprint, new JsonSerializerOptions {WriteIndented = true}));
        
        
    }
}

