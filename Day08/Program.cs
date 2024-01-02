using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Text;
namespace AdventDay8;

class Map {
    public List<Move> Moves {get; set;} = new();
    public List<Node> Nodes {get; set;} = new();
    public void GetPath(){
        int counter = 0;
        int nextMove = counter % Moves.Count;
        var from = "AAA";
        var to = "ZZZ";
        var current = from;
        var startNode = Nodes.First(x => x.Start == current);
        while (current != to){
            var dir = Moves[nextMove].Direction;
            if(dir == 'R'){
                current = startNode.Right;
            }
            else if (dir == 'L'){
                current = startNode.Left;
            }
            else {
                Console.WriteLine("Something went wrong");
            }
            startNode = Nodes.First(x => x.Start == current);
            counter++;
            nextMove = counter % Moves.Count;
        }
        Steps = counter;
    }
    public int Steps {get; set;}
    
}

class Move {
    public char Direction {get; set;}
}

class Node {
    public string? Start {get; set;}
    public string? Left {get; set;}
    public string? Right {get; set;}
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day08\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);
        Map thisMap = new();

        foreach (char x in lines[0]){
            Move thisMove = new();
            thisMove.Direction = x;
            thisMap.Moves.Add(thisMove);
        }
        for (int i = 2; i < lines.Length; i++){
            var splitLine = lines[i].Split("=", StringSplitOptions.TrimEntries);
            var splitLineTo = splitLine[1].ToString().Remove(0, 1).Remove(splitLine[1].Length - 2, 1).Split(",", StringSplitOptions.TrimEntries);
            Node thisNode = new();
            thisNode.Start = splitLine[0];
            thisNode.Left = splitLineTo[0];
            thisNode.Right = splitLineTo[1];
            thisMap.Nodes.Add(thisNode);

            //Console.WriteLine(lines[i]);
        }
        thisMap.GetPath();
        // Part One
        Console.WriteLine("Day 8 -- Part One: " + thisMap.Steps);

        // Part Two
        //Console.WriteLine("Day 7 -- Part Two: " + thisCompetition.RacesPart2[0].WaysToWinPart2);

        //string json = JsonSerializer.Serialize(thisMap, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        
    }
}

