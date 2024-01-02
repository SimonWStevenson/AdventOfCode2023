using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Dynamic;
namespace AdventDay4;

class ScratchCard {
    public List<Card> Cards {get; set;} = new();
    public int TotalPoints {get {
        return Cards.Sum(c => c.Points);
    }}
    public int TotalCount {get {
        return Cards.Sum(c => c.Count);
    }}
}

class Card {
    public int CardNo {get; set;}
    public List<int> WinningNumbers {get; set;} = new();
    public List<int> MyNumbers {get; set;} = new();
    List<int> MyWinningNumbers {get {
        return WinningNumbers.Intersect(MyNumbers).ToList();
    }}
    public int NoOfMyWinningNumbers {get {
        return MyWinningNumbers.Count;
    }}
    public int Points {get {
        var myPoints = 0;
        if (NoOfMyWinningNumbers > 0){myPoints = (int)Math.Pow(2, NoOfMyWinningNumbers-1);}
        return myPoints;
    }}
    public int Count {get; set;} = 1;
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day04\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);
        ScratchCard thisScratchCard = new();

        foreach (string line in lines){
            List<string> lineToSplit = line.Split(":").ToList();

            // get the card number
            var thisCardLabel = lineToSplit[0];
            List<string> thisCardLabelToSplit = thisCardLabel.Split(" ").ToList();
            var thisCardNo = Convert.ToInt32(thisCardLabelToSplit.Last().Trim());
            Card thisCard = new();
            thisCard.CardNo = thisCardNo;

            // get the card numbers
            List<string> cardNumberList = lineToSplit[1].Split("|").ToList();

            // get the winning card numbers
            var winningCardNumbers = cardNumberList[0].Trim();
            List<string> winningCardNumberList = winningCardNumbers.Split(" ").ToList();
            foreach (string c in winningCardNumberList){
                if (c.Trim() == ""){}
                else {thisCard.WinningNumbers.Add(Convert.ToInt32(c.Trim()));}
            }

            // get my card numbers
            var myCardNumbers = cardNumberList[1].Trim();
            List<string> myCardNumberList = myCardNumbers.Split(" ").ToList();
            foreach (string c in myCardNumberList){
                if (c.Trim() == ""){}
                else {thisCard.MyNumbers.Add(Convert.ToInt32(c.Trim()));}
            }

            thisScratchCard.Cards.Add(thisCard);
            //Console.WriteLine(thisScratchCard.TotalPoints);
        }

        // Part One
        Console.WriteLine("Day 4 -- Part One: " + thisScratchCard.TotalPoints);

        // Part Two
        foreach (Card c in thisScratchCard.Cards){
            if (c.NoOfMyWinningNumbers > 0){
                for (
                    int i = 1; 
                    i <= c.NoOfMyWinningNumbers && (i + c.CardNo <= thisScratchCard.Cards.Count); 
                    i++
                ){
                    var targetCard = thisScratchCard.Cards.FirstOrDefault(card => card.CardNo == c.CardNo + i);
                    if (targetCard != null){targetCard.Count += c.Count;}
                }
            }
        }

        Console.WriteLine("Day 4 -- Part Two: " + thisScratchCard.TotalCount);

        //string json = JsonSerializer.Serialize(thisScratchCard, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        
    }
}

