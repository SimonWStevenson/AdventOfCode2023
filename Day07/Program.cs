using System.Text.Json;
using System.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Text;
namespace AdventDay7;

class Competition {
    public List<Hand> Hands {get; set;} = new();
    public void SortByTypeRank(){
        Hands = Hands.OrderByDescending(x => x.Rank).ToList();
        for (int i = 1; i <= Hands.Count; i++){
            Hands[i-1].RankPosition = Hands.Count - i + 1;
        }
    }
    public int TotalWinnings {get {
        return Hands.Sum(h => h.Winnings);
    }}
}
class GameRules {
    public Dictionary<char, char> CardMap {get {return new Dictionary<char, char> {
        {'A', 'Z'}
        , {'K', 'Y'}
        , {'Q', 'X'}
        , {'J', 'W'}
        , {'T', 'V'}
        , {'9', 'U'}
        , {'8', 'T'}
        , {'7', 'S'}
        , {'6', 'R'}
        , {'5', 'Q'}
        , {'4', 'P'}
        , {'3', 'O'}
        , {'2', 'N'}
    };}}
}

class Hand {
    public required string Cards {get; set;}
    string? CardsRank {get; set;}
    public int Bid {get; set;}
    char? PairOne {get; set;}
    char? PairTwo {get; set;}
    char? Three {get; set;}
    char? Four {get; set;}
    char? Five {get; set;}
    bool FullHouse {get {
        if(PairOne is not null && Three is not null && PairOne != Three){return true;}
        else {return false;}
    }}
    string TypeRank {get {
        if(Five is not null){return "9";}
        else if (Four is not null){return "8";}
        else if (FullHouse){return "7";}
        else if (Three is not null){return "6";}
        else if (PairOne is not null && PairTwo is not null){return "5";}
        else if (PairOne is not null){return "4";}
        else {return "3";}
    }}
    public string Rank {get {
        return TypeRank + CardsRank;
    }}
    public int RankPosition {get; set;}
    public int Winnings {get {return RankPosition * Bid;}}
    
    public void AssessHand(GameRules gameRules){
        for (int i = 0; i < Cards.Length; i++){
            // Calculate sets for part 1
            var count = Cards.Count(c => c == Cards[i]);
            if (count == 5){Five = Cards[i];}
            else if (count == 4){Four = Cards[i];}
            else if (count == 3){Three = Cards[i];}
            else if (count == 2){
                if (PairOne is null){PairOne = Cards[i];}
                else if (PairOne == Cards[i]){}
                else {PairTwo = Cards[i];}
            }

            // Replace card characters with sortable alphanumeric
            var stringBuilder = new StringBuilder();
            foreach (var character in Cards){
                if(gameRules.CardMap.TryGetValue(character, out var value)){
                    stringBuilder.Append(value);
                }
            }
            CardsRank = stringBuilder.ToString();
        }
    }
}

class Program
{
    public static void MyMain()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "Day07\\", "Input.txt");
        string[] lines = File.ReadAllLines(filePath);

        Competition thisCompetition = new();
        GameRules thisGameRules = new();
        foreach (string l in lines){
            var splitLine = l.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Hand thisHand = new()
            {
                Cards = splitLine[0],
                Bid = Convert.ToInt32(splitLine[1])
            };
            thisHand.AssessHand(thisGameRules);
            //thisHand.CardsRank(thisGameRules);
            thisCompetition.Hands.Add(thisHand);
        }
        thisCompetition.SortByTypeRank();

        // Part One
        Console.WriteLine("Day 7 -- Part One: " + thisCompetition.TotalWinnings);

        // Part Two
        //Console.WriteLine("Day 7 -- Part Two: " + thisCompetition.RacesPart2[0].WaysToWinPart2);

        //string json = JsonSerializer.Serialize(thisCompetition, new JsonSerializerOptions {WriteIndented = true});
        //Console.WriteLine(json);
        
    }
}

