using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinesCounter
{
    class CLOC
    {

        public CodeLinesCounter CountNumberOfLinesInCSFilesOfDirectory(string dirPath)
        {
            FileInfo[] csFiles = new DirectoryInfo(dirPath.Trim())
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
            }

            return tCounter;
        }

        private CodeLinesCounter CountNumberOfLine(Object tc)
        {
            CodeLinesCounter counter = new CodeLinesCounter();
            FileInfo fo = (FileInfo)tc;
            int inComment = 0;
            using (StreamReader sr = fo.OpenText())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    counter.CodeLines++;
                    counter.PhysicalLines++;
                    var trimmed = line.Trim();

                    if (string.IsNullOrEmpty(trimmed))
                        counter.EmptyLines++;

                    if (IsRealCode(trimmed, ref inComment))
                        counter.LogicalLines++;
                    else if (inComment > 0 || trimmed.StartsWith("//")  || trimmed.Contains("//"))
                        counter.CommentLines++;
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
                && (trimmed.StartsWith("if")
                    || trimmed.StartsWith("else if")
                    || trimmed.StartsWith("using (")
                    || trimmed.StartsWith("else  if")
                    || trimmed.Contains(";")
                    || trimmed.StartsWith("public") //method signature
                    || trimmed.StartsWith("private") //method signature
                    || trimmed.StartsWith("protected") //method signature
                );
        }
    }
}
