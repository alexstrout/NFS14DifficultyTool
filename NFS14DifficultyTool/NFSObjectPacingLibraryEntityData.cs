namespace NFS14DifficultyTool {
    public class NFSObjectPacingLibraryEntityData : NFSObject {
        public NFSObjectPacingLibraryEntityData(MemoryManager memManager)
            : base(memManager, "706cd7f0bc65284cf0a747f5ec29ce7d") {
            //foxPacLibEntData
            FieldList.Add("PacingScheduleGroupSpontaneousRace_Default", new NFSFieldPointer(this, "D0"));
            FieldList.Add("PacingScheduleGroupSpontaneousRace_Tutorial", new NFSFieldPointer(this, "118"));
            FieldList.Add("PacingScheduleGroupSpontaneousRace_Easy", new NFSFieldPointer(this, "120"));
            FieldList.Add("PacingScheduleGroupSpontaneousRace_Medium", new NFSFieldPointer(this, "128"));
            FieldList.Add("PacingScheduleGroupSpontaneousRace_Hard", new NFSFieldPointer(this, "130"));
            FieldList.Add("PacingScheduleGroupDirectedRace", new NFSFieldPointer(this, "D8"));
            FieldList.Add("PacingScheduleGroupDirectedRace_Easy", new NFSFieldPointer(this, "E0"));
            FieldList.Add("PacingScheduleGroupDirectedRace_Hard", new NFSFieldPointer(this, "E8"));
            FieldList.Add("PacingScheduleGroupDirectedRace_AroundTheWorld", new NFSFieldPointer(this, "F0"));
            FieldList.Add("PacingScheduleGroupDirectedRace_Tutorial", new NFSFieldPointer(this, "F8"));
            FieldList.Add("PacingScheduleGroupCopHotPursuit", new NFSFieldPointer(this, "100"));
            FieldList.Add("PacingScheduleGroupCopHotPursuit_Easy", new NFSFieldPointer(this, "108"));
            FieldList.Add("PacingScheduleGroupCopHotPursuit_Hard", new NFSFieldPointer(this, "110"));
            FieldList.Add("PacingScheduleEscape_Default", new NFSFieldPointer(this, "140"));
            FieldList.Add("PacingScheduleEscape_Easy", new NFSFieldPointer(this, "150"));
            FieldList.Add("PacingScheduleEscape_Hard", new NFSFieldPointer(this, "158"));
            FieldList.Add("PacingScheduleEscape_Tutorial", new NFSFieldPointer(this, "160"));
            FieldList.Add("PacingSchedulePursuit_Default", new NFSFieldPointer(this, "148"));
            FieldList.Add("PacingSchedulePursuit_Easy", new NFSFieldPointer(this, "168"));
            FieldList.Add("PacingSchedulePursuit_Hard", new NFSFieldPointer(this, "170"));
            FieldList.Add("PacingSchedulePursuit_Tutorial", new NFSFieldPointer(this, "178"));
        }
    }
}
