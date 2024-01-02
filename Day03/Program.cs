using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
namespace AdventDay3;

class EngineSchematic {
    public List<EnginePart> EngineParts {get; set;} = new();
    public int SumPartNumbers { get {return EngineParts.Where(e => e.HasSymbol).Sum(e => e.PartNumber);}}
    public List<Gear> Gears {get; set;} = new();
}

class EnginePart {
    public int PartNumber {get; set;}
    public bool HasSymbol {get; set;}
    public int Row {get; set;}
    public int StartColumn {get {return EndColumn - PartNumber.ToString().Length + 1;}}
    public int EndColumn {get; set;}
    //public List<Gear> AdjacentGears {get; set;} = new();
}

class Gear {
    public int GearId {get; private set;}
    public int Row {get; set;}
    public int Column {get; set;}
    static int nextId;
    public Gear() {GearId = Interlocked.Increment(ref nextId);}
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day03\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);

        var rows = lines.Length;
        var cols = lines[0].Length;
        //Console.WriteLine(rows);
        //Console.WriteLine(cols);
        EngineSchematic thisEngineSchematic = new();

        // Part One
        //int PartOne = 0;
        for (int i = 0; i < rows; i++){
            //Console.WriteLine("Row: " + (i+1));
            var tempEnginePartNumber = "";
            var HasSymbol = false;
            for (int j = 0; j < cols; j++){
                var c = lines[i][j];
                //Console.WriteLine("Column: " + (j+1) + " - " + c.ToString());
                
                if (c.ToString() == "*"){
                    Gear thisGear = new();
                    thisGear.Row = i;
                    thisGear.Column = j;
                    thisEngineSchematic.Gears.Add(thisGear);
                }
                
                if (char.IsDigit(c)){
                    //Console.WriteLine("I am here");
                    tempEnginePartNumber += c;

                    // check if surrounding cells have a symbol
                    for (int x = -1; x <= 1; x++){
                        for (int y = -1; y <= 1; y++){
                            //Console.WriteLine(lines[i][j]);
                            //Console.WriteLine("x: " + x.ToString() + ", x+i: " + (x+i).ToString() + ", y: " + y.ToString() + ", y+j: " + (y+j).ToString());
                            if (x == 0 && y == 0){}
                            else if (i + x < 0){}
                            else if (j + y < 0){}
                            else if (i + x >= rows){}
                            else if (j + y >= cols){}
                            else {
                                var l = lines[i+x][j+y];
                                if (!char.IsLetterOrDigit(l) && l.ToString() != "."){
                                    HasSymbol = true;
                                    //Console.WriteLine(l);
                                    }
                                }
                        }
                    }
                    }
                else if (tempEnginePartNumber == ""){
                    //Console.WriteLine("Now I am here");
                    }
                else {
                    //Console.WriteLine("Finally I am here");
                    EnginePart thisEnginePart = new();
                    thisEnginePart.PartNumber = Convert.ToInt32(tempEnginePartNumber);
                    thisEnginePart.HasSymbol = HasSymbol;
                    thisEnginePart.Row = i;
                    thisEnginePart.EndColumn = j - 1;
                    thisEngineSchematic.EngineParts.Add(thisEnginePart);
                    //Console.WriteLine(HasSymbol.ToString() + " - " + tempEnginePartNumber.ToString() + " - " + thisEngineSchematic.SumPartNumbers);
                    tempEnginePartNumber = "";
                    HasSymbol = false;
                    }
                if (char.IsDigit(c) && j == cols-1){
                    EnginePart thisEnginePart = new();
                    thisEnginePart.PartNumber = Convert.ToInt32(tempEnginePartNumber);
                    thisEnginePart.HasSymbol = HasSymbol;
                    thisEnginePart.Row = i;
                    thisEnginePart.EndColumn = j - 1;
                    thisEngineSchematic.EngineParts.Add(thisEnginePart);
                    //Console.WriteLine(HasSymbol.ToString() + " - " + tempEnginePartNumber.ToString() + " - " + thisEngineSchematic.SumPartNumbers);
                    tempEnginePartNumber = "";
                    HasSymbol = false;                    
                }
                
                //Console.WriteLine(tempEnginePartNumber);
                //Console.WriteLine(char.IsLetterOrDigit(c));

            }
            // prepare compartments
            //Console.WriteLine("------------------------");
        }

        // Part Two
        int PartTwo = 0;
        
        foreach (Gear g in thisEngineSchematic.Gears){
            var EnginePart1 = 0;
            var EnginePart2 = 0;
            var ToAdd = 0;
            foreach (EnginePart e in thisEngineSchematic.EngineParts){
                /*
                if (g.GearId == 178 && 66 <= g.Row && g.Row <= 68){
                    //string json = JsonSerializer.Serialize(e, new JsonSerializerOptions {WriteIndented = true});
                    //Console.WriteLine(json);
                    Console.WriteLine(g.Row);
                    Console.WriteLine(e.Row);
                    Console.WriteLine(g.Column);
                    //Console.WriteLine(i);
                    Console.WriteLine(e.PartNumber);
                }
                */
                for (int i = e.StartColumn; i <= e.EndColumn; i++){

                    if (
                        Math.Abs(g.Row - e.Row) <= 1
                        && Math.Abs(g.Column - i) <= 1
                    ){
                        if(EnginePart1 == 0){
                            EnginePart1 = e.PartNumber;
                            //Console.WriteLine(e.PartNumber.ToString() + " - " + g.GearId.ToString());
                            break;
                            }
                        else if (EnginePart2 == 0){
                            EnginePart2 = e.PartNumber;
                            //Console.WriteLine(e.PartNumber.ToString() + " - " + g.GearId.ToString());
                            break;
                            }
                        else {Console.WriteLine("apparently there can be 3 adjacent gears");}
                        //Console.WriteLine(e.PartNumber.ToString() + " - " + g.GearId.ToString());
                    }
                }
            }
            //Console.WriteLine(g.GearId.ToString() + " - " + PartTwo.ToString() + " - " + EnginePart1.ToString() + " - " + EnginePart2.ToString() + " - " + (EnginePart1*EnginePart2).ToString() );
            ToAdd = EnginePart1 * EnginePart2;
            PartTwo = PartTwo + ToAdd;
            //EnginePart1 = 0;
            //EnginePart2 = 0;
        }
        
        Console.WriteLine("Day 3 -- Part One: " + thisEngineSchematic.SumPartNumbers); // 551521 is too low
        Console.WriteLine("Day 3 -- Part Two: " + PartTwo); // 80,814,585 is too low; 86,237,783 is too low

        //string json = JsonSerializer.Serialize(thisEngineSchematic, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        
    }
}

