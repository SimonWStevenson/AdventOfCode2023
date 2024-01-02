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
namespace AdventDay10;

class Map {
    public Position? Start {get; set;}
    public List<Position> PositionsA {get; set;} = new();
    public List<Position> PositionsB {get; set;} = new();
    public List<Position> PositionsCombined {get {return PositionsA.Concat(PositionsB).DistinctBy(p => new {p.Row, p.Col}).ToList();}}
    public void GetFirstPositions(){
        // I hardcoded these, I am a bad person
        //Position thisPositionA = new(){Symbol = 'J', Row = 2, Col = 1}; // Test.txt
        //Position thisPositionB = new(){Symbol = '|', Row = 3, Col = 0}; // Test.txt
        if(Start is not null){
            Position thisPositionA = new(){Symbol = '|', Row = Start.Row - 1, Col = Start.Col}; // Input.txt
            Position thisPositionB = new(){Symbol = '|', Row = Start.Row + 1, Col = Start.Col}; // Input.txt
            //Position thisPositionA = new(){Symbol = '|', Row = Start.Row + 1, Col = Start.Col}; // Test2.txt
            //Position thisPositionB = new(){Symbol = '-', Row = Start.Row, Col = Start.Col + 1}; // Test2.txt
            //Position thisPositionA = new(){Symbol = '7', Row = Start.Row, Col = Start.Col + 1}; // Test3.txt
            //Position thisPositionB = new(){Symbol = 'J', Row = Start.Row + 1, Col = Start.Col}; // Test3.txt
            PositionsA.Add(thisPositionA);
            PositionsB.Add(thisPositionB);
        }
    }
    public void GetNextPosition(string[] lines, List<Position> MyPositions){
        Position ThisPosition = MyPositions[^1];
        Position LastPosition = (MyPositions.Count == 1 && Start is not null) ? Start : MyPositions[^2];
        int[] p0 = new int[]{ThisPosition.Row + ThisPosition.AvailableMoves[0][0], ThisPosition.Col + ThisPosition.AvailableMoves[0][1]};
        int[] p1 = new int[]{ThisPosition.Row + ThisPosition.AvailableMoves[1][0], ThisPosition.Col + ThisPosition.AvailableMoves[1][1]};
        int[] lp = new int[]{LastPosition.Row, LastPosition.Col};
        //Console.WriteLine("-----------This position--------------");
        //Console.WriteLine(JsonSerializer.Serialize(ThisPosition, new JsonSerializerOptions {WriteIndented = true}));
        //Console.WriteLine("-----------Last position--------------");
        //Console.WriteLine(JsonSerializer.Serialize(LastPosition, new JsonSerializerOptions {WriteIndented = true}));
        //Console.WriteLine("-------------------------");
        if(p0.SequenceEqual(lp)){
            int NewRow = p1[0];
            int NewCol = p1[1];
            Position NextPosition = new(){Row = NewRow, Col = NewCol, Symbol = lines[NewRow][NewCol]};
            MyPositions.Add(NextPosition);
        }
        else if (p1.SequenceEqual(lp)){
            int NewRow = p0[0];
            int NewCol = p0[1];
            Position NextPosition = new(){Row = NewRow, Col = NewCol, Symbol = lines[NewRow][NewCol]};
            MyPositions.Add(NextPosition);
        }
        else {
            //Console.WriteLine("Something went wrong!");
        }
    }
    //public List<int[]> Inside {get; set;} = new();
    public List<Position> Inside {get; set;} = new();
    public int CalculateInternalArea(string[] lines){
        int result = 0;
        for (int row = 1; row < lines.Length-1; row++){
            var isInside = false;
            var isItAWall = "";
            for (int col = 0; col < lines[row].Length; col++){
                var cell = lines[row][col];
                int onTop = PositionsCombined.Where(x => x.Row == row && x.Col == col).Count();
                Debug.Assert(Start is not null);
                bool isStart = (Start.Row == row && Start.Col == col);
                if (onTop == 1 || isStart){
                    //if (cell == 'S'){cell = 'F';} // Test3.txt hard-coded
                    if (cell == 'S'){cell = '|';} // Input.txt hard-coded
                    if (cell == '|'){
                        isInside = !isInside;
                        isItAWall = "";
                    }
                    else if (cell == '-'){}
                    else if (cell == '.'){}
                    else {
                        isItAWall += cell;
                        if (isItAWall == "FJ" || isItAWall == "L7"){
                            isInside = !isInside;
                            isItAWall = "";
                        }
                        else if (isItAWall == "F7" || isItAWall == "LJ"){
                            isItAWall = "";
                        }
                        else {
                            //Console.WriteLine("Something went wrong.  isItAWall: " + isItAWall + ", row: " + row + ", col: " + col + ", cell: " + cell);
                            }
                    }
                }
                else if (onTop == 0 && !isStart){
                    if (isInside == true){
                        //Console.WriteLine("row: " + row + ", col: " + col);
                        Position thisInsidePosition = new(){
                            Symbol = '+',
                            Row = row,
                            Col = col,
                        };
                        Inside.Add(thisInsidePosition);
                        result ++;
                        }
                }
                else {
                    Console.WriteLine("Something went wrong.  isItAWall: " + isItAWall + ", row: " + row + ", col: " + col + ", cell: " + cell);
                }
            }
        }
        return result;
    }
}

class Position {
    public required char Symbol {get; set;}
    public int Row {get; set;}
    public int Col {get; set;}
    public int[][] AvailableMoves {get {
        if (Symbol == 'F'){return new int[][] {new int[]{0, 1}, new int[]{1, 0}};} // {"Right", "Down"};}
        else if (Symbol == '7'){return new int[][] {new int[]{0, -1}, new int[]{1, 0}};} // {"Left", "Down"};}
        else if (Symbol == 'L'){return new int[][] {new int[]{0, 1}, new int[]{-1, 0}};} // {"Right", "Up"};}
        else if (Symbol == 'J'){return new int[][] {new int[]{0, -1}, new int[]{-1, 0}};} // {"Left", "Up"};}
        else if (Symbol == '|'){return new int[][] {new int[]{-1, 0}, new int[]{1, 0}};} // {"Up", "Down"};}
        else if (Symbol == '-'){return new int[][] {new int[]{0, -1}, new int[]{0, 1}};} // {"Left", "Right"};}
        //else if (Symbol == '.'){return new int[][] {new int[]{0, 0}, new int[]{0, 0}};} // Not in use
        //else if (Symbol == 'S'){return new int[][] {new int[]{0, 0}, new int[]{0, 0}};} // Test3.txt
        else {return Array.Empty<int[]>(); }
    }}
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day10\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);
        Map thisMap = new();
        for (int i = 0; i < lines.Length - 1; i++){
            for (int j = 0; j < lines[i].Length - 1; j++){
                if (lines[i][j] == 'S'){
                    thisMap.Start = new(){Symbol = 'S', Row = i, Col = j};
                    }
            }
        }
        thisMap.GetFirstPositions();
        thisMap.GetNextPosition(lines, thisMap.PositionsA);
        thisMap.GetNextPosition(lines, thisMap.PositionsB);
        //while (!(thisMap.PositionsA[^1].Row == thisMap.PositionsB[^1].Row && thisMap.PositionsA[^1].Col == thisMap.PositionsB[^1].Col)){
        for (int i = 1; true; i++){
            thisMap.GetNextPosition(lines, thisMap.PositionsA);
            thisMap.GetNextPosition(lines, thisMap.PositionsB);
            if (
                thisMap.PositionsA[^1].Row == thisMap.PositionsB[^1].Row 
                && thisMap.PositionsA[^1].Col == thisMap.PositionsB[^1].Col
            ){break;}
        }

        // Part One
        Console.WriteLine("Day 10 -- Part One: " + thisMap.PositionsA.Count);

        // Part Two
        Console.WriteLine("Day 10 -- Part Two: " + thisMap.CalculateInternalArea(lines));
        // wrong answers:
        // 2451
        // 901 
        // 322
        // 32
        // 35

        /*
        for (int i = 0; i < lines.Length - 1; i++){
            Console.Write("\n");
            for (int j = 0; j < lines[i].Length - 1; j++){
                var pipe = thisMap.PositionsCombined.Where(x => x.Row == i && x.Col == j);
                if(pipe.Count() == 1){
                    Console.Write(pipe.FirstOrDefault().Symbol);
                }
                else if (thisMap.Inside.Where(x => x.Row == i && x.Col == j).Count() == 1){
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write('.');
                    Console.ResetColor();
                }
                else {Console.Write(' ');}
            }
        }
        */


        //Console.WriteLine(JsonSerializer.Serialize(thisMap.Inside, new JsonSerializerOptions {WriteIndented = true}));
        //Console.WriteLine(JsonSerializer.Serialize(thisMap.PositionsCombined.Where(x => x.Symbol == 'S'), new JsonSerializerOptions {WriteIndented = true}));
        
        
    }
}

