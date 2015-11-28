using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFS14DifficultyTool {
    class NFSSpikestripWeapon : NFSObjectBlob {
        public NFSSpikestripWeapon(MemoryManager memManager, string guid)
            : base(memManager, guid) {
            //foxSpikeWeap
            FieldList.Add("Classification (0 = Forward, 1 = Backward)", new NFSFieldBool(this, "100"));
        }
    }
}
