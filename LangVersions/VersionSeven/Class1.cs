using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace VersionSeven
{
    public static class Example
    {
        //named tuples

        public class Point
        {
            public Point(double x, double y) => (X, Y) = (x, y);

            public double X { get; }
            public double Y { get; }

            public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);
        }

        public static (int Alpha, int Beta) Tuples() => (0, 0);

        public static void TuplesExample()
        {
            (string Alpha, string Beta) namedLetters = ("a", "b");
            (int max, int min) = Tuples();
            Console.WriteLine(max);
            Console.WriteLine(min);

            var p = new Point(3.14, 2.71);
            (double X, double Y) = p;
            var (_, y) = p; //discards
        }

        // patterns

        public static int PatternsExample(IEnumerable<object> sequence)
        {
            var sum = 0;
            foreach (var i in sequence)
            {
                switch (i)
                {
                    case 0: break;

                    case IEnumerable<int> childSequence:
                        foreach (var item in childSequence)
                        {
                            sum += (item > 0) ? item : 0;
                        }
                        break;

                    case int n when n > 0:
                        sum += n;
                        break;

                    case null: throw new NullReferenceException("Null found in sequence");
                    default: throw new InvalidOperationException("Unrecognized type");
                }
            }
            return sum;
        }

        //async main

        static async Task<int> Main() => await Task.FromResult(0);

        //local functions

        public static IEnumerable<char> AlphabetSubset3(char start, char end)
        {
            if (start < 'a' || start > 'z')
                throw new ArgumentOutOfRangeException(paramName: nameof(start), message: "start must be a letter");
            if (end < 'a' || end > 'z')
                throw new ArgumentOutOfRangeException(paramName: nameof(end), message: "end must be a letter");

            if (end <= start)
                throw new ArgumentException($"{nameof(end)} must be greater than {nameof(start)}");

            return alphabetSubsetImplementation();

            IEnumerable<char> alphabetSubsetImplementation()
            {
                for (var c = start; c < end; c++)
                    yield return c;
            }
        }

        public static Task<string> PerformLongRunningWork(string address, int index, string name)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException(message: "An address is required", paramName: nameof(address));
            if (index < 0)
                throw new ArgumentOutOfRangeException(paramName: nameof(index), message: "The index must be non-negative");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(message: "You must supply a name", paramName: nameof(name));

            return longRunningWorkImplementation();

            async Task<string> longRunningWorkImplementation()
            {
                var interimResult = await Task.FromResult(address);
                var secondResult = await Task.FromResult(address);
                return $"The results are {interimResult} and {secondResult}. Enjoy.";
            }
        }

        // expression members

        class ExpressionMembersExample
        {
            // Expression-bodied constructor
            public ExpressionMembersExample(string label) => Label = label;

            // Expression-bodied finalizer
            ~ExpressionMembersExample() => Console.Error.WriteLine("Finalized!");

            private string label;

            // Expression-bodied get / set accessors.
            public string Label
            {
                get => label;
                set => label = value ?? "Default label";
            }
        }

        // better defaults

        public static void Defaults()
        {
            Func<string, bool> whereClause = default;
        }

        // numerical syntax

        public const int ThirtyTwo = 0b0010_0000;
        public const long BillionsAndBillions = 100_000_000_000;
        public const double AvogadroConstant = 6.022_140_857_747_474e23;
        public const decimal GoldenRatio = 1.618_033_988_749_894_848_204_586_834_365_638_117_720_309_179M;

        // out parameters, out var

        public class Outs
        {
            public Outs(int i, out int j)
            {
                j = i;
            }

            public void Method(string input)
            {
                if (int.TryParse(input, out var answer))
                {
                    Console.WriteLine(answer);
                }
                else
                {
                    Console.WriteLine("Could not parse input");
                }
            }
        }

        // type constraints

        public static T EnumConstraint<T>() where T : Enum => default;

        public static T DelegateConstraint<T>() where T : Delegate => default;

        // everything async

        public static async ValueTask<int> Func()
        {
            await Task.Delay(100);
            return 5;
        }

        public static async MyTask<int> Func2()
        {
            await new MyTask<int>();
            return 5;
        }

        [AsyncMethodBuilder(typeof(MyTaskMethodBuilder<>))]
        public class MyTask<TResult>
        {
            public MyClassAwaiter<TResult> GetAwaiter() => new MyClassAwaiter<TResult>();
        }

        public struct MyTaskMethodBuilder<TResult>
        {
            public MyTask<TResult> Task { get; }
            public static MyTaskMethodBuilder<TResult> Create() => default;
            public void SetException(Exception exception) { }
            public void SetResult(TResult result) { }
            public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine { }

            public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : INotifyCompletion
                where TStateMachine : IAsyncStateMachine
            { }
            public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : ICriticalNotifyCompletion
                where TStateMachine : IAsyncStateMachine
            { }
        }

        public class MyClassAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
        {
            public bool IsCompleted { get; set; }
            public TResult GetResult() => default;
            public void OnCompleted(Action continuation) => throw new NotImplementedException();
            public void UnsafeOnCompleted(Action continuation) => throw new NotImplementedException();
        }

        //private protected
        protected internal class MyClassForOutside
        {
        }
        private protected class MyClassForInside
        {
        }
    }
}