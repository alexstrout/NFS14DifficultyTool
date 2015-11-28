using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFS14DifficultyTool {
    class NFSGameTime : NFSObjectBlob {
        public NFSGameTime(MemoryManager memManager, string guid)
            : base(memManager, guid) {
            //foxGameTime
            FieldList.Add("VariableSimTickTimeEnable", new NFSFieldBool(this, "62"));
        }
    }
}
