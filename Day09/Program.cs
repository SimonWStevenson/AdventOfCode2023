using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Text;
namespace AdventDay9;

class Map {
    public List<History> Histories {get; set;} = new();
}
class History {
    public List<Sequence> Sequences {get; set;} = new();
    public void GetNextSequence(){
        var LastSequence = Sequences[^1];
        var LastSequenceNumbers = LastSequence.Numbers;
        var LastSequenceNumbersLength = LastSequence.Numbers.Count;
        Sequence thisSequence = new();
        for (int i = 1; i < LastSequenceNumbersLength; i++){
            var n = LastSequenceNumbers[i] - LastSequenceNumbers[i-1];
            thisSequence.Numbers.Add(n);
        }
        Sequences.Add(thisSequence);
        if (
            !thisSequence.Numbers.All(n => n == 0)
        ){
            GetNextSequence();
        }
        else {
            ExtendSequence();
            PrependSequence();
        }
    }
    public void ExtendSequence(){
        var MaxSequence = Sequences.Count - 1;
        Sequences[MaxSequence].Numbers.Add(0);
        for (int i = MaxSequence - 1; i >= 0; i--){
            var ThisSequenceLastNo = Sequences[i].Numbers[^1];
            var UnderneathSequenceLastNo = Sequences[i+1].Numbers[^1];
            var ThisSequenceNewLastNo = ThisSequenceLastNo + UnderneathSequenceLastNo;
            Sequences[i].Numbers.Add(ThisSequenceNewLastNo);
        }
    }
    public void PrependSequence(){
        var MaxSequence = Sequences.Count - 1;
        Sequences[MaxSequence].Numbers.Insert(0, 0);
        for (int i = MaxSequence - 1; i >= 0; i--){
            var ThisSequenceFirstNo = Sequences[i].Numbers[0];
            var UnderneathSequenceFirstNo = Sequences[i+1].Numbers[0];
            var ThisSequenceNewFirstNo = ThisSequenceFirstNo - UnderneathSequenceFirstNo;
            Sequences[i].Numbers.Insert(0, ThisSequenceNewFirstNo);
        }
    }
}
class Sequence {
    public List<int> Numbers {get; set;} = new();
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day09\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);
        Map thisMap = new();
        foreach (string l in lines){
            History thisHistory = new();
            var splitLine = l.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Sequence thisSequence = new();
            foreach (string i in splitLine){
                thisSequence.Numbers.Add(Convert.ToInt32(i));
            }
            thisHistory.Sequences.Add(thisSequence);
            thisMap.Histories.Add(thisHistory);
            thisHistory.GetNextSequence();
        }

        // Part One
        Console.WriteLine("Day 9 -- Part One: " + thisMap.Histories.Sum(h => h.Sequences[0].Numbers[^1]));

        // Part Two
        Console.WriteLine("Day 9 -- Part One: " + thisMap.Histories.Sum(h => h.Sequences[0].Numbers[0]));

        //string json = JsonSerializer.Serialize(thisMap, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        
    }
}

