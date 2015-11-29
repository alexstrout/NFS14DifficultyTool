namespace NFS14DifficultyTool {
    //Note: This is actually just "Helicopter" in the game, but renamed to HeliSpikestripWeapon for clarity
    public class NFSHeliSpikestripWeapon : NFSObjectBlob {
        public NFSHeliSpikestripWeapon(MemoryManager memManager, string guid)
            : base(memManager, guid) {
            //foxHeliSpikeWeap
            FieldList.Add("Projectile-EX0", new NFSFieldPointer(this, "17C0"));
            FieldList.Add("Projectile-EX1", new NFSFieldPointer(this, "1800"));
            FieldList.Add("Projectile-EX2", new NFSFieldPointer(this, "1840"));
            FieldList.Add("Projectile-EX3", new NFSFieldPointer(this, "1880"));
        }
    }
}
