namespace NFS14DifficultyTool {
    public class NFSHealthProfilesListEntityData : NFSObjectBlob {
        public NFSHealthProfilesListEntityData(MemoryManager memManager, string guid)
            : base(memManager, guid) {
            //foxHealthEntData
            FieldList.Add("CopHealthProfile_AI_Default", new NFSFieldPointer(this, "120"));
            FieldList.Add("CopHealthProfile_CopTutorial", new NFSFieldPointer(this, "128"));
            FieldList.Add("CopHealthProfile_RacerTutorial", new NFSFieldPointer(this, "130"));
            FieldList.Add("RacerHealthProfile_CopHotPursuit_Easy", new NFSFieldPointer(this, "F0"));
            FieldList.Add("RacerHealthProfile_CopHotPursuit_Hard", new NFSFieldPointer(this, "F8"));
            FieldList.Add("RacerHealthProfile_CopHotPursuit_Medium", new NFSFieldPointer(this, "100"));
            FieldList.Add("RacerHealthProfile_CopInterceptor_Easy", new NFSFieldPointer(this, "108"));
            FieldList.Add("RacerHealthProfile_CopInterceptor_Hard", new NFSFieldPointer(this, "110"));
            FieldList.Add("RacerHealthProfile_CopInterceptor_Medium", new NFSFieldPointer(this, "118"));
            FieldList.Add("RacerHealthProfile_AI_Default", new NFSFieldPointer(this, "138"));
            FieldList.Add("RacerHealthProfile_CopTutorial", new NFSFieldPointer(this, "140"));
            FieldList.Add("RacerHealthProfile_RacerTutorial", new NFSFieldPointer(this, "148"));
            FieldList.Add("TutorialHealthProfile", new NFSFieldPointer(this, "150"));
            FieldList.Add("RacerHealthProfile_AI_AroundTheWorld", new NFSFieldPointer(this, "158"));
        }
    }
}
