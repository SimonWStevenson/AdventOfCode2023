using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ExceptionServices;
namespace AdventDay1;
class Program

{
    static string GetNumber(string thisLine, char lastChar){
            if (char.IsNumber(lastChar)){return lastChar.ToString();}
            else {
                var test = thisLine;
                if (test.Contains("one")){return "1";}
                else if (test.Contains("two")){return "2";}
                else if (test.Contains("three")){return "3";}
                else if (test.Contains("four")){return "4";}
                else if (test.Contains("five")){return "5";}
                else if (test.Contains("six")){return "6";}
                else if (test.Contains("seven")){return "7";}
                else if (test.Contains("eight")){return "8";}
                else if (test.Contains("nine")){return "9";}
            }
        return "0";
    }

    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day01\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);

        // Part One
        int PartOne = 0;
        foreach (string line in lines){
            char[] chars = line.ToCharArray();
            var first = Array.FindIndex(chars, x => char.IsNumber(x));
            var firstChar = chars[first].ToString();
            Array.Reverse(chars);
            var last = Array.FindIndex(chars, x => char.IsNumber(x));
            var lastChar = chars[last].ToString();
            var calibrationValue = Int32.Parse(firstChar + lastChar);
            PartOne += calibrationValue;
        }

        // Part Two
        int PartTwo = 0;
        foreach (string line in lines){
            int len = line.Length;
            string first = "0";
            string last = "0";
            for (int i = 1; i < len+1 && first == "0"; i++){
                string startLine = line.Substring(0, i);
                first = GetNumber(startLine, startLine[i-1]);
            }
            for (int i = 1; i < len+1 && last == "0"; i++){
                string endLine = line.Substring(len - i);
                last = GetNumber(endLine, endLine[0]);
            }
            
            var calibrationValue = Int32.Parse(first + last);
            PartTwo += calibrationValue;

        //string json = JsonSerializer.Serialize(lines, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        }
    Console.WriteLine("Day 1 -- Part One: " + PartOne);
    Console.WriteLine("Day 1 -- Part Two: " + PartTwo); 
    }
}