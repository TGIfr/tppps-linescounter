using System;
using System.IO;

namespace LinesCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            string defaultDir = @"D:\projects\repos\sharpgl";
            var cloc = new CLOC();
            var result = cloc.CountNumberOfLinesInCSFilesOfDirectory(defaultDir);
            Console.WriteLine($"Total lines of code: {result.CodeLines}");
            Console.WriteLine($"Physical lines of code: {result.PhysicalLines}");
            Console.WriteLine($"Logical lines of code: {result.LogicalLines}");
            Console.WriteLine($"Comment lines of code: {result.CommentLines}");
            Console.WriteLine($"Commenting level: {(double)result.CommentLines/result.CodeLines}");
            Console.WriteLine($"Empty lines of code: {result.EmptyLines}");
            //Console.WriteLine($"Curly braces and # lines of code: {result.CurlyBracesAndSharpLines}");

        }

        
    }
}
