namespace NFS14DifficultyTool {
    public class NFSObjectPacingPursuitScheduleHard : NFSObject {
        public NFSObjectPacingPursuitScheduleHard(MemoryManager memManager)
            : base(memManager, "6b800570ad4fc849934164d54f9f1a56") {
            //foxPacSchedPurHard
            FieldList.Add("DistanceCurve-1-X", new NFSFieldFloat(this, "70"));
            FieldList.Add("DistanceCurve-1-Z", new NFSFieldFloat(this, "78"));
            FieldList.Add("SkillScalarOverTimeCurve-1-X", new NFSFieldFloat(this, "F0"));
            FieldList.Add("SkillScalarOverTimeCurve-1-Z", new NFSFieldFloat(this, "F8"));
        }
    }
}
