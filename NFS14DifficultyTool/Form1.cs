using System.Threading;
using System.Windows.Forms;

namespace NFS14DifficultyTool {
    public partial class Form1 : Form {
        public MemoryManager MemManager { get; set; }
        public NFSAiDirectorEntityData AiDirectorEntityData { get; set; }
        public NFSPacingLibraryEntityData PacingLibraryEntityData { get; set; }
        public NFSObjectBlob HealthProfilesListEntityData { get; set; }
        public NFSObjectBlob PersonaLibraryPrefab { get; set; }
        public NFSGameTime GameTime { get; set; }
        public NFSSpikestripWeapon SpikestripWeapon { get; set; }
        public NFSHeliSpikestripWeapon HeliSpikestripWeapon { get; set; }

        public Form1() {
            InitializeComponent();

            //TODO TEST
            //btnApply_Click(null, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            ResetAll();
            if (MemManager != null)
                MemManager.CloseHandle();
        }

        private void btnApply_Click(object sender, System.EventArgs e) {
            Thread thread = new Thread(RunTests);
            thread.Start();
        }

        delegate void SetStatusCallback(string text);
        private void SetStatus(string text) {
            if (lblStatus.InvokeRequired) {
                SetStatusCallback d = new SetStatusCallback(SetStatus);
                Invoke(d, new object[] { text });
            }
            else {
                lblStatus.Text = text;
            }
        }

        public void RunTests() {
            MemManager = new MemoryManager();
            if (!MemManager.OpenProcess("nfs14")) { // && !MemManager.OpenProcess("nfs14_x86")
                SetStatus("Could not open nfs14!"); // or nfs14_x86
                return;
            }

            try {
                //StdAIPrefab -> AiDirectorEntityData
                SetStatus("Finding StdAIPrefab -> AiDirectorEntityData...");
                AiDirectorEntityData = new NFSAiDirectorEntityData(MemManager, "2d774798942db34e960cd083ace16340");
                //AiDirectorEntityData.FieldList["MaxHeat"].Field = 0;
                AiDirectorEntityData.FieldList["HeatTime"].Field = 300;
                AiDirectorEntityData.FieldList["BonusStartingHeat"].Field = 6;
                AiDirectorEntityData.FieldList["PullAheadHeatThreshold"].Field = 1;
                AiDirectorEntityData.FieldList["BlockHeatThreshold"].Field = 1;
                //AiDirectorEntityData.FieldList["SpawnDelay"].Field = 0f;
                //AiDirectorEntityData.FieldList["MaxAiVisibilityDistance"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat1 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat2 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat3 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat4 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat5 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat6 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat7 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat8 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat9 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat10 - MinHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat1 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat2 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat3 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat4 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat5 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat6 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat7 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat8 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat9 - MaxHeat"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat10 - MaxHeat"].Field = 0;
                AiDirectorEntityData.FieldList["Heat1 - CopCountHeatBased"].Field = 2;
                AiDirectorEntityData.FieldList["Heat2 - CopCountHeatBased"].Field = 3;
                AiDirectorEntityData.FieldList["Heat3 - CopCountHeatBased"].Field = 3;
                AiDirectorEntityData.FieldList["Heat4 - CopCountHeatBased"].Field = 3;
                AiDirectorEntityData.FieldList["Heat5 - CopCountHeatBased"].Field = 3;
                AiDirectorEntityData.FieldList["Heat6 - CopCountHeatBased"].Field = 4;
                AiDirectorEntityData.FieldList["Heat7 - CopCountHeatBased"].Field = 4;
                AiDirectorEntityData.FieldList["Heat8 - CopCountHeatBased"].Field = 5;
                AiDirectorEntityData.FieldList["Heat9 - CopCountHeatBased"].Field = 5;
                AiDirectorEntityData.FieldList["Heat10 - CopCountHeatBased"].Field = 6;
                //AiDirectorEntityData.FieldList["Heat1 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat2 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat3 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat4 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat5 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat6 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat7 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat8 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat9 - Basic"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat10 - Basic"].Field = 0;
                AiDirectorEntityData.FieldList["Heat1 - Chaser"].Field = 1;
                AiDirectorEntityData.FieldList["Heat2 - Chaser"].Field = 1;
                //AiDirectorEntityData.FieldList["Heat3 - Chaser"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat4 - Chaser"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat5 - Chaser"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat6 - Chaser"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat7 - Chaser"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat8 - Chaser"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat9 - Chaser"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat10 - Chaser"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat1 - Brute"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat2 - Brute"].Field = 0;
                AiDirectorEntityData.FieldList["Heat3 - Brute"].Field = 1;
                //AiDirectorEntityData.FieldList["Heat4 - Brute"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat5 - Brute"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat6 - Brute"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat7 - Brute"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat8 - Brute"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat9 - Brute"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat10 - Brute"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat1 - Aggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat2 - Aggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat3 - Aggressor"].Field = 0;
                AiDirectorEntityData.FieldList["Heat4 - Aggressor"].Field = 1;
                AiDirectorEntityData.FieldList["Heat5 - Aggressor"].Field = 1;
                //AiDirectorEntityData.FieldList["Heat6 - Aggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat7 - Aggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat8 - Aggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat9 - Aggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat10 - Aggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat1 - AdvancedAggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat2 - AdvancedAggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat3 - AdvancedAggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat4 - AdvancedAggressor"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat5 - AdvancedAggressor"].Field = 0;
                AiDirectorEntityData.FieldList["Heat6 - AdvancedAggressor"].Field = 1;
                AiDirectorEntityData.FieldList["Heat7 - AdvancedAggressor"].Field = 1;
                AiDirectorEntityData.FieldList["Heat8 - AdvancedAggressor"].Field = 3;
                AiDirectorEntityData.FieldList["Heat9 - AdvancedAggressor"].Field = 4;
                AiDirectorEntityData.FieldList["Heat10 - AdvancedAggressor"].Field = 5;
                AiDirectorEntityData.FieldList["Heat1 - ChanceOfSpawningRoamingCopHeatBased"].Field = 9;
                AiDirectorEntityData.FieldList["Heat2 - ChanceOfSpawningRoamingCopHeatBased"].Field = 18;
                AiDirectorEntityData.FieldList["Heat3 - ChanceOfSpawningRoamingCopHeatBased"].Field = 27;
                AiDirectorEntityData.FieldList["Heat4 - ChanceOfSpawningRoamingCopHeatBased"].Field = 36;
                AiDirectorEntityData.FieldList["Heat5 - ChanceOfSpawningRoamingCopHeatBased"].Field = 45;
                AiDirectorEntityData.FieldList["Heat6 - ChanceOfSpawningRoamingCopHeatBased"].Field = 54;
                AiDirectorEntityData.FieldList["Heat7 - ChanceOfSpawningRoamingCopHeatBased"].Field = 63;
                AiDirectorEntityData.FieldList["Heat8 - ChanceOfSpawningRoamingCopHeatBased"].Field = 72;
                AiDirectorEntityData.FieldList["Heat9 - ChanceOfSpawningRoamingCopHeatBased"].Field = 81;
                AiDirectorEntityData.FieldList["Heat10 - ChanceOfSpawningRoamingCopHeatBased"].Field = 90;
                AiDirectorEntityData.FieldList["Heat1 - MinimumHelicopterSpawnInterval"].Field = 50f;
                AiDirectorEntityData.FieldList["Heat2 - MinimumHelicopterSpawnInterval"].Field = 50f;
                AiDirectorEntityData.FieldList["Heat3 - MinimumHelicopterSpawnInterval"].Field = 50f;
                AiDirectorEntityData.FieldList["Heat4 - MinimumHelicopterSpawnInterval"].Field = 50f;
                //AiDirectorEntityData.FieldList["Heat5 - MinimumHelicopterSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat6 - MinimumHelicopterSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat7 - MinimumHelicopterSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat8 - MinimumHelicopterSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat9 - MinimumHelicopterSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat10 - MinimumHelicopterSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat1 - MaxHelicoptersPerBubble"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat2 - MaxHelicoptersPerBubble"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat3 - MaxHelicoptersPerBubble"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat4 - MaxHelicoptersPerBubble"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat5 - MaxHelicoptersPerBubble"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat6 - MaxHelicoptersPerBubble"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat7 - MaxHelicoptersPerBubble"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat8 - MaxHelicoptersPerBubble"].Field = 0;
                //AiDirectorEntityData.FieldList["Heat9 - MaxHelicoptersPerBubble"].Field = 0;
                AiDirectorEntityData.FieldList["Heat10 - MaxHelicoptersPerBubble"].Field = 2;
                AiDirectorEntityData.FieldList["Heat1 - MinimumRoadblockSpawnInterval"].Field = 50f;
                AiDirectorEntityData.FieldList["Heat2 - MinimumRoadblockSpawnInterval"].Field = 50f;
                //AiDirectorEntityData.FieldList["Heat3 - MinimumRoadblockSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat4 - MinimumRoadblockSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat5 - MinimumRoadblockSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat6 - MinimumRoadblockSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat7 - MinimumRoadblockSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat8 - MinimumRoadblockSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat9 - MinimumRoadblockSpawnInterval"].Field = 0f;
                //AiDirectorEntityData.FieldList["Heat10 - MinimumRoadblockSpawnInterval"].Field = 0f;
                AiDirectorEntityData.FieldList["Heat1 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 30f;
                AiDirectorEntityData.FieldList["Heat2 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 27f;
                AiDirectorEntityData.FieldList["Heat3 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 24f;
                AiDirectorEntityData.FieldList["Heat4 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 21f;
                AiDirectorEntityData.FieldList["Heat5 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 18f;
                AiDirectorEntityData.FieldList["Heat6 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 15f;
                AiDirectorEntityData.FieldList["Heat7 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 12f;
                AiDirectorEntityData.FieldList["Heat8 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 9f;
                AiDirectorEntityData.FieldList["Heat9 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 6f;
                AiDirectorEntityData.FieldList["Heat10 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 3f;
                //AiDirectorEntityData.FieldList["ClientViewSpawnRadius"].Field = 0;
                //AiDirectorEntityData.FieldList["ClientViewCullRadius"].Field = 0;
                AiDirectorEntityData.FieldList["MaxNumberOfAiOnlySpontaneousRaces"].Field = 2;
                //AiDirectorEntityData.FieldList["NumberOfPawnRacersWanted"].Field = 0;
                //AiDirectorEntityData.FieldList["NumberOfPawnCopsWanted"].Field = 0;
                //AiDirectorEntityData.FieldList["NumberOfRacers"].Field = 0;
                //AiDirectorEntityData.FieldList["GlobalNumberOfCops"].Field = 0;
                //AiDirectorEntityData.FieldList["GlobalChanceOfSpawningRoamingCop"].Field = 0;
                //AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnRacer"].Field = 0f;
                //AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnCop"].Field = 0f;
                //AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnRacer"].Field = 0f;
                AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCop"].Field = 4f;
                AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringPursuit"].Field = 4f;
                AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringHPRacer"].Field = 4f;
                AiDirectorEntityData.FieldList["IsEnabled"].Field = true;

                //PacingLibraryPrefab -> PacingLibraryEntityData
                SetStatus("Finding PacingLibraryPrefab -> PacingLibraryEntityData...");
                PacingLibraryEntityData = new NFSPacingLibraryEntityData(MemManager, "706cd7f0bc65284cf0a747f5ec29ce7d");
                PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Default"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Medium"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                //PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].Field = null;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_AroundTheWorld"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_AroundTheWorld"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_Hard"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_AroundTheWorld"].FieldDefault;
                //PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_AroundTheWorld"].Field = null;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_AroundTheWorld"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit_Hard"].FieldDefault;
                //PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit_Hard"].Field = null;
                PacingLibraryEntityData.FieldList["PacingScheduleEscape_Default"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleEscape_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                //PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].Field = null;
                PacingLibraryEntityData.FieldList["PacingScheduleEscape_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Default"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Easy"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;

                //HealthProfilesList -> HealthProfilesListEntityData
                SetStatus("Finding HealthProfilesList -> HealthProfilesListEntityData...");
                HealthProfilesListEntityData = new NFSHealthProfilesListEntityData(MemManager, "6ef1bfcc79f73ef1377db6b1fdce2da6");
                //HealthProfilesListEntityData.FieldList["CopHealthProfile_AI_Default"].Field = null;
                //HealthProfilesListEntityData.FieldList["CopHealthProfile_CopTutorial"].Field = null;
                //HealthProfilesListEntityData.FieldList["CopHealthProfile_RacerTutorial"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopHotPursuit_Easy"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopHotPursuit_Hard"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopHotPursuit_Medium"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Easy"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Medium"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_AI_Default"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopTutorial"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_RacerTutorial"].Field = null;
                //HealthProfilesListEntityData.FieldList["TutorialHealthProfile"].Field = null;
                //HealthProfilesListEntityData.FieldList["RacerHealthProfile_AI_AroundTheWorld"].Field = null;

                //PersonaLibraryPrefab
                SetStatus("Finding PersonaLibraryPrefab...");
                PersonaLibraryPrefab = new NFSPersonaLibraryPrefab(MemManager, "097d331254a092347db8c7f677cb620d");
                //PersonaLibraryPrefab.FieldList["AggressorCopPersonality - HealthProfile"].Field = null;
                //PersonaLibraryPrefab.FieldList["BruteCopPersonality - HealthProfile"].Field = null;
                //PersonaLibraryPrefab.FieldList["BasicCopPersonality - HealthProfile"].Field = null;
                //PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - HealthProfile"].Field = null;
                //PersonaLibraryPrefab.FieldList["ChaserCopPersonality - HealthProfile"].Field = null;
                PersonaLibraryPrefab.FieldList["RacerTutorialCop - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_AI_Default"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CopTutorialCop - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_AI_Default"].FieldDefault;
                PersonaLibraryPrefab.FieldList["AggressorCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["BruteCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["BasicCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["ChaserCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["RacerTutorialCop - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CopTutorialCop - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["AggressorCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["BruteCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["BasicCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["ChaserCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["RacerTutorialCop - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CopTutorialCop - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["AggressorCopPersonality - PacingSkill"].Field = 1.1f;
                PersonaLibraryPrefab.FieldList["BruteCopPersonality - PacingSkill"].Field = 1.1f;
                PersonaLibraryPrefab.FieldList["BasicCopPersonality - PacingSkill"].Field = 1.1f;
                PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - PacingSkill"].Field = 1.1f;
                PersonaLibraryPrefab.FieldList["ChaserCopPersonality - PacingSkill"].Field = 1.1f;
                PersonaLibraryPrefab.FieldList["RacerTutorialCop - PacingSkill"].Field = 1.1f;
                PersonaLibraryPrefab.FieldList["CopTutorialCop - PacingSkill"].Field = 1.1f;
                //PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkill"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkill"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkill"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkill"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkill"].Field = 0f;
                PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkill"].Field = 1f;
                //PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkillVsHumanCop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkillVsHumanCop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkillVsHumanCop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkillVsHumanCop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkillVsHumanCop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkillVsHumanCop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkillVsHumanCop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkillVsHumanRacer"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkillVsHumanRacer"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkillVsHumanRacer"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkillVsHumanRacer"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkillVsHumanRacer"].Field = 0f;
                PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkillVsHumanRacer"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkillVsHumanRacer"].Field = 1f;
                //PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkillVsAICop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkillVsAICop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkillVsAICop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkillVsAICop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkillVsAICop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkillVsAICop"].Field = 0f;
                //PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkillVsAICop"].Field = 0f;
                PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkillVsAIRacer"].Field = 1f;
                PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkillVsAIRacer"].Field = 1f;
                PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkillVsAIRacer"].Field = 1f;
                PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkillVsAIRacer"].Field = 1f;
                PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkillVsAIRacer"].Field = 1f;
                PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkillVsAIRacer"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkillVsAIRacer"].Field = 1f;
                PersonaLibraryPrefab.FieldList["AggressorCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["BruteCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["BasicCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["ChaserCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["RacerTutorialCop - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["CopTutorialCop - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["RecklessRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CleanRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["WeaponRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CautiousRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["ViolentRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_CopInterceptor_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                //PersonaLibraryPrefab.FieldList["RecklessRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = null;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                //PersonaLibraryPrefab.FieldList["CleanRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = null;
                PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                //PersonaLibraryPrefab.FieldList["WeaponRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = null;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].FieldDefault;
                //PersonaLibraryPrefab.FieldList["CautiousRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = null;
                //PersonaLibraryPrefab.FieldList["ViolentRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = null;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["RecklessRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CleanRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["WeaponRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["CautiousRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["ViolentRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].FieldDefault;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["RecklessRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["CleanRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["WeaponRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["CautiousRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["ViolentRacer - PacingSkill"].Field = 0.9f;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkill"].Field = 1f;
                //PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkill"].Field = 0f;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkill"].Field = 1f;
                //PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkill"].Field = 0f;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkill"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkillVsHumanCop"].Field = 1f;
                //PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkillVsHumanCop"].Field = 0f;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkillVsHumanCop"].Field = 1f;
                //PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkillVsHumanCop"].Field = 0f;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkillVsHumanCop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkillVsHumanRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkillVsAICop"].Field = 1f;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkillVsAIRacer"].Field = 0.6f;
                PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["RecklessRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["RacerTutorialRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["CleanRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier1CleanRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["Tier2CleanRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["WeaponRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["CopTutorialRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["CautiousRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                PersonaLibraryPrefab.FieldList["ViolentRacer - AvoidanceSpeedMatchWhenBlocked"].Field = false;

                //GameTime
                SetStatus("Finding GameTime...");
                GameTime = new NFSGameTime(MemManager, "c8d0247b61bcc2314b5679507d0416e2");
                GameTime.FieldList["VariableSimTickTimeEnable"].Field = true;

                //SpikestripWeapon
                SetStatus("Finding SpikestripWeapon...");
                SpikestripWeapon = new NFSSpikestripWeapon(MemManager, "073c76e4864aec065409eff77d578b2c");
                SpikestripWeapon.FieldList["Classification"].Field = NFSSpikestripWeapon.VehicleWeaponClassification.VehicleWeaponClassification_BackwardFiring;
                //SpikestripWeapon.FieldList["ConeLength-Low"].Field = 100f;
                //SpikestripWeapon.FieldList["ConeLength-High"].Field = 100f;
                SpikestripWeapon.FieldList["MinimumTriggerDistance-Low"].Field = 2f;
                SpikestripWeapon.FieldList["MinimumTriggerDistance-High"].Field = 1f;
                //SpikestripWeapon.FieldList["Projectile-EX0"].Field = null;
                //SpikestripWeapon.FieldList["Projectile-EX1"].Field = null;
                //SpikestripWeapon.FieldList["Projectile-EX2"].Field = null;
                //SpikestripWeapon.FieldList["Projectile-EX3"].Field = null;

                //HeliSpikestripWeapon
                SetStatus("Finding HeliSpikestripWeapon...");
                HeliSpikestripWeapon = new NFSHeliSpikestripWeapon(MemManager, "7008a3427f4a252a36025d741e2112c4");
                //HeliSpikestripWeapon.FieldList["Projectile-EX0"].Field = SpikestripWeapon.FieldList["Projectile-EX0"].FieldDefault;
                //HeliSpikestripWeapon.FieldList["Projectile-EX1"].Field = SpikestripWeapon.FieldList["Projectile-EX1"].FieldDefault;
                //HeliSpikestripWeapon.FieldList["Projectile-EX2"].Field = SpikestripWeapon.FieldList["Projectile-EX2"].FieldDefault;
                //HeliSpikestripWeapon.FieldList["Projectile-EX3"].Field = SpikestripWeapon.FieldList["Projectile-EX3"].FieldDefault;

                SetStatus("Done! Close this window to revert all changes.");
            }
            catch (System.Exception e) {
                SetStatus(e.Message);
            }
        }

        public void ResetAll() {
            SetStatus("Reverting changes...");
            if (AiDirectorEntityData != null)
                AiDirectorEntityData.ResetFieldsToDefault();
            if (PacingLibraryEntityData != null)
                PacingLibraryEntityData.ResetFieldsToDefault();
            if (HealthProfilesListEntityData != null)
                HealthProfilesListEntityData.ResetFieldsToDefault();
            if (PersonaLibraryPrefab != null)
                PersonaLibraryPrefab.ResetFieldsToDefault();
            if (GameTime != null)
                GameTime.ResetFieldsToDefault();
            if (SpikestripWeapon != null)
                SpikestripWeapon.ResetFieldsToDefault();
            if (HeliSpikestripWeapon != null)
                HeliSpikestripWeapon.ResetFieldsToDefault();
        }
    }
}
