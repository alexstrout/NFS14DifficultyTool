namespace NFS14DifficultyTool {
    public class NFSObjectSpikestripWeapon : NFSObject {
        public enum VehicleWeaponClassification {
            VehicleWeaponClassification_ForwardFiring = 0,
            VehicleWeaponClassification_BackwardFiring = 1
        }

        public NFSObjectSpikestripWeapon(MemoryManager memManager)
            : base(memManager, "073c76e4864aec065409eff77d578b2c") {
            //foxSpikeWeap
            FieldList.Add("Classification", new NFSFieldInt(this, "100"));
            FieldList.Add("ConeLength-Low", new NFSFieldFloat(this, "1C0"));
            FieldList.Add("ConeLength-High", new NFSFieldFloat(this, "1C4"));
            FieldList.Add("MinimumTriggerDistance-Low", new NFSFieldFloat(this, "1D0"));
            FieldList.Add("MinimumTriggerDistance-High", new NFSFieldFloat(this, "1D4"));
            FieldList.Add("Projectile-EX0", new NFSFieldPointer(this, "7A0"));
            FieldList.Add("Projectile-EX1", new NFSFieldPointer(this, "7F8"));
            FieldList.Add("Projectile-EX2", new NFSFieldPointer(this, "850"));
            FieldList.Add("Projectile-EX3", new NFSFieldPointer(this, "8A8"));
        }
    }
}
