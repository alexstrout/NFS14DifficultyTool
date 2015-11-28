namespace NFS14DifficultyTool {
    public class NFSSpikestripWeapon : NFSObjectBlob {
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
