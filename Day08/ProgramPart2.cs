namespace AdventDay8b;

class Map {
    public List<Move> Moves {get; set;} = new();
    public List<Node> Nodes {get; set;} = new();
    public List<Node> StartNodes {get {
        return Nodes.Where(x => x.Start.EndsWith("A")).ToList();
    }}
    public List<int>? EndNodes {get; set;}
    public void GetPath(){
        int NoOfStartNodes = StartNodes.Count;
        List<int> StepsList = new(NoOfStartNodes);
        for (int i = 0; i < NoOfStartNodes; i++){
            bool IsMatching = false;
            int counter = 0;
            int nextMove = counter % Moves.Count;
            var currentNode = StartNodes[i];
            var currentStep = currentNode.Start;
            while (IsMatching == false){
                var dir = Moves[nextMove].Direction;
                if(dir == 'R'){currentStep = currentNode.Right;}
                else if (dir == 'L'){currentStep = currentNode.Left;}
                else {Console.WriteLine("Something went wrong");}
                currentNode = Nodes.First(x => x.Start == currentStep);
                counter++;
                if (currentNode.Start.EndsWith("Z")){
                    StepsList.Add(counter);
                    IsMatching = true;
                }
		        else {
                    nextMove = counter % Moves.Count;
                }
            }
        }
        EndNodes = StepsList;
    }
    public long Steps {get; set;}
}

class Move {
    public char Direction {get; set;}
}

class Node {
    public required string Start {get; set;}
    public required string Left {get; set;}
    public required string Right {get; set;}
}

class Program
{
    // mindlessly copied from here: https://www.w3resource.com/csharp-exercises/math/csharp-math-exercise-20.php
    public static long gcd(long n1, long n2){
        if(n2 == 0){return n1;}
        else {return gcd(n2, n1 % n2);}
    }
    public static long Test(long[] numbers){
        return numbers.Aggregate((S, val) => S * val / gcd(S, val));
    }
    
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day08\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);
        Map thisMap = new();

        foreach (char x in lines[0]){
            Move thisMove = new()
            {
                Direction = x
            };
            thisMap.Moves.Add(thisMove);
        }
        for (int i = 2; i < lines.Length; i++){
            var splitLine = lines[i].Split("=", StringSplitOptions.TrimEntries);
            var splitLineTo = splitLine[1].ToString().Remove(0, 1).Remove(splitLine[1].Length - 2, 1).Split(",", StringSplitOptions.TrimEntries);
            Node thisNode = new()
            {
                Start = splitLine[0],
                Left = splitLineTo[0],
                Right = splitLineTo[1]
            };
            thisMap.Nodes.Add(thisNode);

        }
        thisMap.GetPath();
        if(thisMap.EndNodes is not null){
            thisMap.Steps = Test(thisMap.EndNodes.Select(x => (long)x).ToArray());
        }

        // Part Two
        Console.WriteLine("Day 8 -- Part Two: " + thisMap.Steps);
        // 4,621,697,209,455,145,957 is too high
        //         8,811,050,362,409 was correct!


        //string json = JsonSerializer.Serialize(thisMap.EndNodes, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        
    }
}

