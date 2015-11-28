namespace NFS14DifficultyTool {
    public class NFSGameTime : NFSObjectBlob {
        public NFSGameTime(MemoryManager memManager, string guid)
            : base(memManager, guid) {
            //foxGameTime
            FieldList.Add("VariableSimTickTimeEnable", new NFSFieldBool(this, "62"));
        }
    }
}
