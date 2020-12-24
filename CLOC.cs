using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LinesCounter
{
    class CLOC
    {

        public CodeLinesCounter CountNumberOfLinesInCSFilesOfDirectory(string dirPath)
        {
            var csFiles = new DirectoryInfo(dirPath.Trim())
                .GetFiles("*.cs", SearchOption.AllDirectories);

            var tCounter = new CodeLinesCounter();
            foreach (var fileInfo in csFiles)
            {
                var counter = CountNumberOfLine(fileInfo);
                tCounter.EmptyLines += counter.EmptyLines;
                tCounter.CodeLines += counter.CodeLines;
                tCounter.CommentLines += counter.CommentLines;
                tCounter.PhysicalLines += counter.PhysicalLines;
                tCounter.LogicalLines += counter.LogicalLines;
                //tCounter.CurlyBracesAndSharpLines += counter.CurlyBracesAndSharpLines;
            }

            return tCounter;
        }

        private CodeLinesCounter CountNumberOfLine(FileInfo tc)
        {
            var counter = new CodeLinesCounter();
            var fo = tc;
            int inComment = 0;
            using (var sr = fo.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    counter.CodeLines++;
                    counter.PhysicalLines++;
                    var trimmed = line.Trim();

                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        counter.EmptyLines++;
                        
                    }
                        

                    //if (trimmed.Contains("{") || trimmed.Contains("}") || trimmed.Contains("#"))
                    //    counter.CurlyBracesAndSharpLines++;

                    if (IsRealCode(trimmed, ref inComment))
                    {
                        counter.LogicalLines++;
                        counter.LogicalLines += CountFuncCalls(trimmed);

                    }
                        
                    else if (inComment > 0 || trimmed.StartsWith("//") || trimmed.Contains("//"))
                    {
                        counter.CommentLines++;
                        
                    }
                }
            }

            if (counter.PhysicalLines < counter.EmptyLines * 4)
                counter.PhysicalLines -= counter.EmptyLines * 4 - counter.PhysicalLines;
            return counter;
        }

        private bool IsRealCode(string trimmed, ref int inComment)
        {
            if (trimmed.StartsWith("/*") && trimmed.EndsWith("*/"))
                return false;
            else if (trimmed.StartsWith("/*"))
            {
                inComment++;
                return false;
            } else if (trimmed.EndsWith("*/"))
            {
                inComment--;
                return false;
            }

            return
                inComment == 0
                && !trimmed.StartsWith("//")
                && (trimmed.Contains("if")
                    || trimmed.Contains("else if")
                    || trimmed.Contains("using (")
                    || trimmed.Contains("for")
                    || trimmed.Contains("using")
                    || trimmed.Contains("catch")
                    || trimmed.Contains("while")
                    || trimmed.Contains("do")
                    || trimmed.Contains("void")
                    || trimmed.Contains("else")
                    || trimmed.Contains("enum")
                    || trimmed.Contains("goto")
                    || trimmed.Contains("try")
                    || trimmed.Contains("namespace")
                    || trimmed.Contains("System")
                    || trimmed.Contains("Dll")
                    || trimmed.Contains(";")
                    || trimmed.Contains("switch")
                    || trimmed.Contains("case")
                    || trimmed.Contains("default")
                    || trimmed.Contains("get")
                    || trimmed.Contains("set")
                    || trimmed.Contains("static")
                    || trimmed.Contains("{")
                    || trimmed.Contains("}")
                    || trimmed.Contains("#")
                    || trimmed.StartsWith("[")
                    || trimmed.Contains("=")
                    || new Regex(@"[[a-zA-Z0-9_.-]\.[[a-zA-Z0-9_.-]").Matches(trimmed).Count > 0
                                        || trimmed.Contains("public") //method signature
                                        || trimmed.Contains("private") //method signature
                                        || trimmed.Contains("protected") //method signature
                );
        }

        private int CountFuncCalls(string trimmed)
        {
            if (trimmed.Contains("("))
                return trimmed.Split("(").Length - 2;
            return 0;
        }
    }
}
