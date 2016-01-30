namespace NFS14DifficultyTool {
    public class NFSObjectGameTime : NFSObject {
        public NFSObjectGameTime(MemoryManager memManager)
            : base(memManager, "c8d0247b61bcc2314b5679507d0416e2") {
            //foxGameTime
            FieldList.Add("VariableSimTickTimeEnable", new NFSFieldBool(this, "62"));
        }
    }
}
