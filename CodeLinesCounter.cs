using System;
using System.Collections.Generic;
using System.Text;

namespace LinesCounter
{
    class CodeLinesCounter
    {
        public int CodeLines { get; set; }
        public int EmptyLines { get; set; }
        public int PhysicalLines { get; set; }
        public int LogicalLines { get; set; }
        public int CommentLines { get; set; }

    }
}
