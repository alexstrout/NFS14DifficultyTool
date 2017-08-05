namespace NFS14DifficultyTool {
    public class NFSObjectAttackHeliEntityData : NFSObject {
        public NFSObjectAttackHeliEntityData(MemoryManager memManager)
            : base(memManager, "1fb3aa637aeac9449e68cae20be13624") {
            //foxAtkHeliEntData
            FieldList.Add("Projectile-EX0", new NFSFieldPointer(this, "1760"));
            FieldList.Add("Projectile-EX1", new NFSFieldPointer(this, "17A0"));
            FieldList.Add("Projectile-EX2", new NFSFieldPointer(this, "17E0"));
            FieldList.Add("Projectile-EX3", new NFSFieldPointer(this, "1820"));
        }
    }
}
