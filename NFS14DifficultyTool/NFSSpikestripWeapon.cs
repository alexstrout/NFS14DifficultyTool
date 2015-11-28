using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFS14DifficultyTool {
    class NFSSpikestripWeapon : NFSObjectBlob {
        public enum VehicleWeaponClassification {
            VehicleWeaponClassification_ForwardFiring = 0,
            VehicleWeaponClassification_BackwardFiring = 1
        }

        public NFSSpikestripWeapon(MemoryManager memManager, string guid)
            : base(memManager, guid) {
            //foxSpikeWeap
            FieldList.Add("Classification", new NFSFieldInt(this, "100"));
        }
    }
}
