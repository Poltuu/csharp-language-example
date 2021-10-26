using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace VersionEight
{
    //readonly

    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public readonly double Distance => Math.Sqrt(X * X + Y * Y);

        public readonly void Translate(int xOffset, int yOffset)
        {
            X += xOffset;
            Y += yOffset;
        }

        public void Deconstruct(out double x, out double y) =>            (x, y) = (X, Y);

        public readonly override string ToString() =>
            $"({X}, {Y}) is {Distance} from the origin";
    }

    public static class Examples
    {
        //default interface members
        interface ILogger
        {
            void Log(int level, string content);
            void LogCritical(string content) => Log(0, content);
        }

        //patterns

        public class RGBColor
        {
            public RGBColor(int a, int b, int c) { }
        }

        public enum Rainbow
        {
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            Indigo,
            Violet
        }

        //enum
        public static RGBColor FromRainbow(Rainbow colorBand) => colorBand switch
        {
            Rainbow.Red => new RGBColor(0xFF, 0x00, 0x00),
            Rainbow.Orange => new RGBColor(0xFF, 0x7F, 0x00),
            Rainbow.Yellow => new RGBColor(0xFF, 0xFF, 0x00),
            Rainbow.Green => new RGBColor(0x00, 0xFF, 0x00),
            Rainbow.Blue => new RGBColor(0x00, 0x00, 0xFF),
            Rainbow.Indigo => new RGBColor(0x4B, 0x00, 0x82),
            Rainbow.Violet => new RGBColor(0x94, 0x00, 0xD3),
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand)),
        };

        //property
        public class Address { public string State { get; } }
        public static decimal ComputeSalesTax(Address location, decimal salePrice) => location switch
        {
            { State: "WA" } => salePrice * 0.06M,
            { State: "MN" } => salePrice * 0.075M,
            { State: "MI" } => salePrice * 0.05M,
            _ => 0M
        };

        //tuple
        public static string RockPaperScissors(string first, string second) => (first, second) switch
        {
            ("rock", "paper") => "rock is covered by paper. Paper wins.",
            ("rock", "scissors") => "rock breaks scissors. Rock wins.",
            ("paper", "rock") => "paper covers rock. Paper wins.",
            ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
            ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
            ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
            (_, _) => "tie"
        };

        //positional
        public enum Quadrant
        {
            Unknown,
            Origin,
            One,
            Two,
            Three,
            Four,
            OnBorder
        }

        static Quadrant GetQuadrant(Point point) => point switch
        {
            (0, 0) => Quadrant.Origin,
            var (x, y) when x > 0 && y > 0 => Quadrant.One,
            var (x, y) when x < 0 && y > 0 => Quadrant.Two,
            var (x, y) when x < 0 && y < 0 => Quadrant.Three,
            var (x, y) when x > 0 && y < 0 => Quadrant.Four,
            var (_, _) => Quadrant.OnBorder,
            _ => Quadrant.Unknown //on purpose
        };

        //using
        static int WriteLinesToFile(IEnumerable<string> lines)
        {
            using var file = new System.IO.StreamWriter("WriteLines2.txt");
            int skippedLines = 0;
            foreach (string line in lines)
            {
                if (!line.Contains("Second"))
                {
                    file.WriteLine(line);
                }
                else
                {
                    skippedLines++;
                }
            }
            // Notice how skippedLines is in scope here.
            return skippedLines;
            // file is disposed here
        }

        //static local functions
        class SubExample
        {
            int M()
            {
                int y;
                LocalFunction();
                return y;

                static void LocalFunction() => y = 0;
            }

            int M2()
            {
                int y = 5;
                int x = 7;
                return Add(x, y);

                static int Add(int left, int right) => left + right;
            }
        }

        //disposable ref struct
        public ref struct DisposableStruct //actually disposable by design
        {
            public void Dispose() => throw new NotImplementedException();
        }

        //nullable reference types
        static readonly string? NullableString = null;

        public static bool TrySomething(string input, [NotNullWhen(true)] out int? answer)
        {
            if (input == "test")
            {
                answer = 42;
                return true;
            }
            answer = null;
            return false;
        }

        //async streams
        public static async System.Collections.Generic.IAsyncEnumerable<int> GenerateSequence()
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }

        static async Task UseStream()
        {
            await foreach (var number in GenerateSequence())
            {
                Console.WriteLine(number);
            }
        }

        //Async disposable
        class AsyncDisposable : IAsyncDisposable
        {
            public ValueTask DisposeAsync() => throw new NotImplementedException();
        }

        static async Task UseAsyncDisposable()
        {
            await using var lol = new AsyncDisposable();
        }

        //indices, ranges

        static readonly string[] words = new string[]
        {
                        // index from start    index from end
            "The",      // 0                   ^9
            "quick",    // 1                   ^8
            "brown",    // 2                   ^7
            "fox",      // 3                   ^6
            "jumped",   // 4                   ^5
            "over",     // 5                   ^4
            "the",      // 6                   ^3
            "lazy",     // 7                   ^2
            "dog"       // 8                   ^1
        };              // 9 (or words.Length) ^0

        static void UseWords()
        {
            Console.WriteLine($"The last word is {words[^1]}"); //dog
            var quickBrownFox = words[1..4];
            var lazyDog = words[^2..^0];
            var allWords = words[..]; // contains "The" through "dog".
            var firstPhrase = words[..4]; // contains "The" through "fox"
            var lastPhrase = words[6..]; // contains "the", "lazy" and "dog"
            Range phrase = 1..4;
        }

        //null coalescing assignments
        static void coalescingExample()
        {
            List<int>? numbers = null;
            int? i = null;

            numbers ??= new List<int>();
            numbers.Add(i ??= 17);
            numbers.Add(i ??= 20);

            Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
            Console.WriteLine(i);  // output: 17
        }

        //no order between $ and @
        static string NoOrder = $@"Lol{23}";
        static string NoOrder2 = @$"Lol{24}";
    }
}