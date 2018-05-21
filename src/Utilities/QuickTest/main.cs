﻿using static goutil.BuiltInFunctions;
using goutil;
using System;
using System.Numerics;
using MyFloat = System.Double;

public static unsafe partial class main_package
{
    private static void Main()
    {
        Main1();
        Main2();
        Main3();
        Main4();
        Main5();
        Main6();

        Console.ReadLine();
    }

    public interface Abser
    {
        double Abs();
    }

    public struct MyError
    {
        public DateTime When;
        public string What;
    }

    private struct buffer
    {

    }

    // flags placed in a separate struct for easy clearing.
    private struct fmtFlags
    {
        public bool widPresent;
        public bool precPresent;
        public bool minus;
        public bool plus;
        public bool sharp;
        public bool space;
        public bool zero;

        // For the formats %+v %#v, we set the plusV/sharpV flags
        // and clear the plus/sharp flags since %+v and %#v are in effect
        // different, flagless formats set at the top level.
        public bool plusV;
        public bool sharpV;
    }

    // A fmt is the raw formatter used by Printf etc.
    // It prints into a buffer that must be set up separately.
    private struct fmt
    {
        public buffer* buf;

        public fmtFlags fmtFlags;

        public int wid;  // width
        public int prec; // precision

        // intbuf is large enough to store %b of an int64 with a sign and
        // avoids padding at the end of the struct on 32 bit architectures.
        public fixed byte intbuf[68];
    }

    private static void clearFlags(ref this fmt _this) => func(ref _this, (ref fmt f, Defer defer, Panic panic, Recover recover) =>
    {
        f.fmtFlags = new fmtFlags();
    });

    private static string Error(this MyError myError)
    {
        fmt f = new fmt();

        val(ref *f.buf);

        f.clearFlags();

        return $"at {myError.When}, {myError.What}";
    }

    private static void val(ref buffer buf)
    {

    }

    private static void Main1()
    {
        Complex vi1 = new Complex(0, 0);

        // 12 * 1.5 + 1i * iota / 3 + 2i
        for (int iota = 0; iota < 2; iota++)
        {
            Complex result = 12 * 1.5 + i(1) * iota / 3 + i(2);
            Console.WriteLine(result);
        }

        //Complex t1 = new Complex(12 * 1.5, 1 * 1 / 3 + 2);
        Abser ab;
        MyFloat f = (MyFloat)(-Math.Sqrt(2));
        Vertex v = new Vertex { X = 3, Y = 4 };

        ab = Abser_cast(f);
        ab = (Abser<MyFloat>)f;
        ab = Abser_cast(v);

        Console.WriteLine(ab.Abs());
    }

    public static double Abs(this MyFloat f)
    {
        if (f < 0)
            return -f;

        return f;
    }

    public struct Vertex
    {
        public double X;
        public double Y;
    }

    public static double Abs(this Vertex v)
    {
        return Math.Sqrt(v.X * v.X + v.Y * v.Y);
    }

    private static void Main2()
    {
        Slice<int> s = new Slice<int>(new[] { 2, 3, 5, 7, 11, 13 });
        printSlice(s);

        // Slice the slice to give it zero length.
        s = s.Slice(high: 0);
        printSlice(s);

        // Extend its length.
        s = s.Slice(high: 4);
        printSlice(s);

        // Drop its first two values.
        s = s.Slice(2);
        printSlice(s);
    }

    private static void printSlice(Slice<int> s)
    {
        Console.WriteLine("len={0} cap={1} {2}", len(s), cap(s), s);
    }

    private static void Main3()
    {
        Slice<int> s = new Slice<int>();
        Console.WriteLine("{0} {1} {2}", s, len(s), cap(s));

        if (s == nil)
            Console.WriteLine("nil!");
    }

    private static void Main4()
    {
        Slice<Slice<string>> board = new Slice<Slice<string>>(new[]
        {
            new Slice<string>(new[] {"_", "_", "_"}),
            new Slice<string>(new[] {"_", "_", "_"}),
            new Slice<string>(new[] {"_", "_", "_"}),
        });

        // The players take turns.
        board[0][0] = "X";
        board[2][2] = "O";
        board[1][2] = "X";
        board[1][0] = "O";
        board[0][2] = "X";

        for (var i = 0; i < len(board); i++)
        {
            Console.WriteLine("{0}", string.Join(" ", board[i]));
        }
    }

    private static void Main5()
    {
        //Slice<int> s = make(Slice<int>.Nil, 0, 5);
        //printSlice(s);

        //for (int j = 0; j < 33; j++)
        //{
        //    s = append(s, j);
        //    printSlice(s);
        //}

        Slice<int> s = new Slice<int>();
        printSlice(s);

        // append works on nil slices.
        s = append(s, 0);
        printSlice(s);

        // The slice grows as needed.
        s = append(s, 1);
        printSlice(s);

        // We can add more than one element at a time.
        s = append(s, 2, 3, 4);
        printSlice(s);
    }

    private static void Main6()
    {
        f();
        Console.WriteLine("Returned normally from f.");

        //f2();
        //Console.WriteLine("Returned normally from f.");
    }

    private static void f() => func((defer, panic, recover) =>
    {
        defer(handleError);

        Console.WriteLine("Calling g.");
        g(0);
        Console.WriteLine("Returned normally from g.");
    });

    private static void g(int i) => func((defer, panic, recover) =>
    {
        if (i > 3)
        {
            Console.WriteLine("Panicking!");
            panic($"{i}");
        }

        defer(() => Console.WriteLine($"Defer in g {i}"));
        Console.WriteLine($"Printing in g {i}");
        g(i + 1);
    });

    private static void handleError() => func((defer, panic, recover) =>
    {
        var r = recover();

        if (r != nil)
            Console.WriteLine($"Recovered in f {r}");
    });

    //private static void f2() => func((defer, panic, recover) =>
    //{
    //    defer(() =>
    //    {
    //        {
    //            var r = recover();

    //            if (r != nil)
    //                Console.WriteLine($"Recovered in f2 {r}");
    //        }
    //    });

    //    Console.WriteLine("Calling g.");
    //    (string name, int age) = g2(0);
    //    Console.WriteLine($"Returned normally from g2: {name} - {age}.");
    //});

    //private static (string message, int err) g2(int i) => func((defer, panic, recover) =>
    //{
    //    string message = "Test";
    //    int err = 12;

    //    if (i > 3)
    //    {
    //        Console.WriteLine("Panicking!");
    //        panic($"{i}");
    //    }

    //    defer(() => Console.WriteLine($"Defer in g2 {i}"));
    //    Console.WriteLine($"Printing in g2 {i}");
    //    g2(i + 1);

    //    return (message, err);
    //});
}