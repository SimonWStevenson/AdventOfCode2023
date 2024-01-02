using System.Diagnostics;
namespace Test;
class Program
{    
    public static void MyMain(){
        //var NoOfLoops = 11851;
        var NoOfLoops = 10;
        var stopwatch1 = Stopwatch.StartNew();
        long result1 = FibLoop(NoOfLoops);
        stopwatch1.Stop();
        Console.WriteLine("Fibonacci sequence: " + NoOfLoops + ", loop result: " + result1 + ", elapsed time: " + stopwatch1.ElapsedMilliseconds + " milliseconds");

        var stopwatch2 = Stopwatch.StartNew();
        long result2 = FibRecCache(NoOfLoops);
        stopwatch2.Stop();
        Console.WriteLine("Fibonacci sequence: " + NoOfLoops + ", recursion with cache result: " + result2 + ", elapsed time: " + stopwatch2.ElapsedMilliseconds + " milliseconds");

        /*
        // doesn't work from about 40 loops or more
        var stopwatch3 = Stopwatch.StartNew();
        long result3 = FibRec(NoOfLoops);
        stopwatch3.Stop();
        Console.WriteLine("Fibonacci sequence: " + NoOfLoops + ", recursion result: " + result3 + ", elapsed time: " + stopwatch3.ElapsedMilliseconds + " milliseconds");
        */
    }
    public static long FibLoop(int x){
        long a = 0;
        long b = 1;
        long result = 1;
        for (int i = 2; i <= x; i++){
            result = a + b;
            a = b;
            b = result;
        }
        return result;
    }

    static readonly Dictionary<long, long> cache = [];
    public static long FibRecCache(long x){
        if (x <= 2){
            return 1;
            }
        if (cache.TryGetValue(x, out long value))
        {
            return value;
        }
        long result = FibRecCache(x - 1) + FibRecCache(x - 2);
        cache[x] = result;
        Console.WriteLine(result);
        return result;
    }

    public static long FibRec(long x){
        if (x <= 2){
            return 1;
            }
        return FibRec(x - 1) + FibRec(x - 2);
    }
}