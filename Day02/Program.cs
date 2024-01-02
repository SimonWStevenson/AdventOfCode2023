using System.Diagnostics.Contracts;
using System.Text.Json;
namespace AdventDay2;

class Game {
    public string GameNo {get; set;} = default!;
    public int GameID {get {
        return Convert.ToInt32(GameNo.Split(" ")[1]);
    }}
    public List<Round> Rounds {get; set;} = new();
    public bool GameIsPossible {get {
        return Rounds.All(round => round.RoundIsPossible);
    }}
    int MaxBlue {get {return Rounds.Max(round => round.Blue);}}
    int MaxGreen {get {return Rounds.Max(round => round.Green);}}
    int MaxRed {get {return Rounds.Max(round => round.Red);}}
    public int GamePower {get {
        return MaxBlue*MaxGreen*MaxRed;
    }}
}
class Round {
    public int Blue {get; set;}
    public int Green {get; set;}
    public int Red {get; set;}
    public bool RoundIsPossible {get {
        if(Blue > 14){return false;}
        else if (Green > 13){return false;}
        else if (Red > 12){return false;}
        else {return true;}
        }}
    public int RoundPower {get {
        return Blue * Green * Red;
    }}
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day02\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);

        // Part One
        int PartOne = 0;
        int PartTwo = 0;
        foreach (string line in lines){
            //Console.WriteLine(line);

            Game thisGame = new();
            List<string> lineToSplit = line.Split(":").ToList();
            thisGame.GameNo = lineToSplit[0];
            List<string> rounds = lineToSplit[1].Split(";").ToList();
            foreach (string round in rounds){
                //Console.WriteLine(round);
                Round thisRound = new();
                List<string> cubes = round.Split(",").ToList();
                foreach (string cube in cubes){
                    //Console.WriteLine(cube);
                    var thisCube = cube.Trim();
                    List<string> thisCubeSplit = thisCube.Split(" ").ToList();
                    if (thisCubeSplit[1] == "red"){thisRound.Red = Convert.ToInt32(thisCubeSplit[0]);}
                    else if (thisCubeSplit[1] == "blue"){thisRound.Blue = Convert.ToInt32(thisCubeSplit[0]);}
                    else if (thisCubeSplit[1] == "green"){thisRound.Green = Convert.ToInt32(thisCubeSplit[0]);}

                }
                thisGame.Rounds.Add(thisRound);
            }
            if (thisGame.GameIsPossible){PartOne += thisGame.GameID;}
            PartTwo += thisGame.GamePower;
            //string json = JsonSerializer.Serialize(thisGame, new JsonSerializerOptions {WriteIndented = true});
            //Console.WriteLine(json);

            //Console.WriteLine("-------------------------------");

        }
        Console.WriteLine("Day 2 -- Part One: " + PartOne);
        Console.WriteLine("Day 2 -- Part Two: " + PartTwo);
        
    }
}

