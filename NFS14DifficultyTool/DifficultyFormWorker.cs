using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace NFS14DifficultyTool {
    //Useful enums (Note: Not all dropdowns will have all options!)
    public enum DifficultyEnum {
        Novice = 0,
        Average = 1,
        Experienced = 2,
        Skilled = 3,
        Adept = 4,
        Masterful = 5,
        Inhuman = 6,
        Godlike = 7,
        Custom = 8
    }
    public enum ClassEnum {
        Easy = 0,
        Normal = 1,
        Hard = 2,
        AroundTheWorld = 3
    }
    public enum DensityEnum {
        None = 0,
        Low = 1,
        Normal = 2,
        High = 3,
        VeryHigh = 4,
        MostWanted = 5
    }
    public enum HeatEnum {
        Cool = 0,
        Normal = 1,
        Hot = 2,
        VeryHot = 3,
        Blazing = 4
    }

    public class DifficultyFormWorker {
        //TODO use ConcurrentDictionary instead of locking Dictionary, would be much faster for these uses
        protected MemoryManager memManager;
        protected ConcurrentDictionary<string, NFSObject> objectList;
        protected ConcurrentDictionary<string, Thread> threadList;
        protected List<string> statusList;
        protected DifficultyForm parent;

        public DifficultyFormWorker(DifficultyForm parent) {
            memManager = new MemoryManager();
            objectList = new ConcurrentDictionary<string, NFSObject>();
            threadList = new ConcurrentDictionary<string, Thread>();
            statusList = new List<string>();
            this.parent = parent;
        }

        //Useful functions
        public void ResetAll(bool isClosing = false) {
            parent.SetStatus("Signalling threads...");
            foreach (Thread t in threadList.Values)
                if (t.IsAlive)
                    t.Priority = ThreadPriority.AboveNormal;

            parent.SetStatus("Aborting searches...");
            memManager.AbortFindObject();

            parent.SetStatus("Reverting changes...");
            foreach (NFSObject n in objectList.Values)
                n.ResetFieldsToDefault();

            parent.SetStatus("Closing nfs14 handle...");
            if (memManager != null)
                memManager.CloseHandle();

            parent.SetStatus();

            //If we're not closing, start looking for our process again (e.g. we've found out game has closed but we haven't)
            if (!isClosing)
                parent.FindProcessTimer.Start();
        }

        public bool TryGetObject(string name, out NFSObject obj) {
            obj = GetObject(name);
            return obj != null;
        }
        public NFSObject GetObject(string name) {
            if (!memManager.ProcessOpen)
                return null;

            NFSObject type = null;
            if (!objectList.TryGetValue(name, out type)) {
                //If we haven't found any objects yet, we're still waiting for the game world to load
                string status = (objectList.Count == 0) ? "Waiting for game world..." : "Finding " + name + "...";
                lock (statusList)
                    statusList.Add(status);
                parent.SetStatus(status);

                try {
                    switch (name) {
                        case "AiDirectorEntityData":
                            type = objectList.GetOrAdd(name, new NFSObjectAiDirectorEntityData(memManager)); break;
                        case "PacingLibraryEntityData":
                            type = objectList.GetOrAdd(name, new NFSObjectPacingLibraryEntityData(memManager)); break;
                        case "HealthProfilesListEntityData":
                            type = objectList.GetOrAdd(name, new NFSObjectHealthProfilesListEntityData(memManager)); break;
                        case "PersonaLibraryPrefab":
                            type = objectList.GetOrAdd(name, new NFSObjectPersonaLibraryPrefab(memManager)); break;
                        case "GameTime":
                            type = objectList.GetOrAdd(name, new NFSObjectGameTime(memManager)); break;
                        case "SpikestripWeapon":
                            type = objectList.GetOrAdd(name, new NFSObjectSpikestripWeapon(memManager)); break;
                        case "HeliSpikestripWeapon":
                            type = objectList.GetOrAdd(name, new NFSObjectHeliSpikestripWeapon(memManager)); break;
                        default:
                            return null;
                    }
                    lock (statusList) {
                        statusList.Remove(status);
                        if (statusList.Count > 0)
                            parent.SetStatus(statusList[0]);
                        else
                            parent.SetStatus();
                    }
                }
                catch (Exception e) {
                    //Don't start printing errors until we've at least found something once, otherwise we probably aren't even in the game world yet
                    if (objectList.Count > 0)
                        parent.SetStatus(e.Message);
                }
            }

            return type;
        }

        protected void LaunchThread(ThreadStart obj) {
            Thread thread = new Thread(obj);
            thread.Name = obj.Method.ToString();
            thread.Start();
        }

        protected bool CheckThread() {
            //If we successfully add our thread (no thread for this name ever existed before), we're good to go
            Thread oldThread = threadList.GetOrAdd(Thread.CurrentThread.Name, Thread.CurrentThread);
            if (oldThread == Thread.CurrentThread)
                return false;

            //Otherwise, wait on the old thread - while waiting, threads created after will then wait on us, forming a queue
            threadList[Thread.CurrentThread.Name] = Thread.CurrentThread;
            if (oldThread.IsAlive) {
                //Thus, before we wait, tell our old thread we're waiting on it by bumping its priority
                oldThread.Priority = ThreadPriority.AboveNormal;
                oldThread.Join();
            }

            //If we're deferring to a later thread (per above), let our caller know so that it might exit early
            return Thread.CurrentThread.Priority > ThreadPriority.Normal;
        }

        public bool CheckIfReady() {
            if (!memManager.ProcessOpen) {
                parent.SetStatus("Waiting for game...");
                if (!memManager.OpenProcess("nfs14")) // && !MemManager.OpenProcess("nfs14_x86")
                    return false;

                //Found it, we can slow down our checks
                parent.SetStatus("Found it!");
                parent.FindProcessTimer.Interval = 10000;
            }

            if (objectList.Count == 0)
                LaunchThread(CheckGameTime);
            return objectList.Count > 0;
        }
        protected void CheckGameTime() {
            //This should only run at start, while we're waiting for the game to be ready, on a loop
            //If it's been this long we probably want to start new search from beginning
            //So run AbortFindObject before even checking our old thread, because we don't care about its search
            memManager.AbortFindObject();
            if (CheckThread())
                return;

            //I'd use GameTime here, but that seems to sometimes be created before the game world is actually ready
            //So in this case we'll use SpikestripWeapon, which definitely won't be ready until we've loaded in
            GetObject("SpikestripWeapon");
        }

        //Class events
        public void UpdateCopClass(int index, bool eqWeapUse) {
            copClass = index;
            equalWeaponUse = eqWeapUse;
            LaunchThread(UpdateCopClass);
        }
        protected int copClass;
        protected void UpdateCopClass() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            int index = copClass;
            bool eqWeapUse = equalWeaponUse;

            //Swap PacingLibraryEntityData pointers, both directly and inside PersonaLibraryPrefab objects
            //PacingScheduleGroupSpontaneousRace (note we don't swap these directly - only set the (probably unused) PersonaLibraryPrefab values)
            string difficulty;
            switch ((ClassEnum)index) {
                case ClassEnum.Easy:
                    difficulty = "Easy"; break;
                case ClassEnum.Normal:
                    difficulty = "Default"; break;
                case ClassEnum.Hard:
                case ClassEnum.AroundTheWorld:
                    difficulty = "Hard"; break;
                default:
                    return;
            }
            NFSObject PacingLibraryEntityData, PersonaLibraryPrefab;
            if (!TryGetObject("PacingLibraryEntityData", out PacingLibraryEntityData) || !TryGetObject("PersonaLibraryPrefab", out PersonaLibraryPrefab))
                return;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;

            //PacingSchedulePursuit
            PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Default"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Easy"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;

            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;

            //Swap HealthProfilesListEntityData pointers inside PersonaLibraryPrefab objects (but not directly, for now, as that causes weird HUD issues)
            switch ((ClassEnum)index) {
                case ClassEnum.Easy:
                case ClassEnum.Normal:
                case ClassEnum.Hard:
                case ClassEnum.AroundTheWorld:
                    difficulty = "AI_Default"; break;
                default:
                    return;
            }
            NFSObject HealthProfilesListEntityData;
            if (!TryGetObject("HealthProfilesListEntityData", out HealthProfilesListEntityData))
                return;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_" + difficulty].FieldDefault;

            //Adjust WeaponSkill values inside PersonaLibraryPrefab objects
            float skillVsCop = 0f;
            float skillVsRacer = (float)(0.01 + (1 + index) * 0.33);
            float skill = skillVsRacer;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            if (!eqWeapUse)
                skillVsRacer /= 2f;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - WeaponSkillVsAIRacer"].Field = skillVsRacer;

            //Set speed matching inside PersonaLibraryPrefab objects based on class
            bool matchSpeed = index < (int)ClassEnum.Hard;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
        }

        public void UpdateRacerClass(int index, bool eqWeapUse) {
            racerClass = index;
            equalWeaponUse = eqWeapUse;
            LaunchThread(UpdateRacerClass);
        }
        protected int racerClass;
        protected void UpdateRacerClass() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            int index = racerClass;
            bool eqWeapUse = equalWeaponUse;

            //Adjust some AiDirectorEntityData values based on class
            NFSObject AiDirectorEntityData;
            if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                return;
            AiDirectorEntityData.FieldList["BonusStartingHeat"].Field = (int)AiDirectorEntityData.FieldList["BonusStartingHeat"].FieldDefault + Math.Max(0, 2 * (index - 1));

            //Swap PacingLibraryEntityData pointers, both directly and inside PersonaLibraryPrefab objects
            //PacingScheduleGroupSpontaneousRace
            string difficulty;
            switch ((ClassEnum)index) {
                case ClassEnum.Easy:
                    difficulty = "Easy"; break;
                case ClassEnum.Normal:
                    difficulty = "Default"; break;
                case ClassEnum.Hard:
                case ClassEnum.AroundTheWorld:
                    difficulty = "Hard"; break;
                default:
                    return;
            }
            NFSObject PacingLibraryEntityData;
            if (!TryGetObject("PacingLibraryEntityData", out PacingLibraryEntityData))
                return;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Default"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Medium"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_Hard"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;

            NFSObject PersonaLibraryPrefab;
            if (!TryGetObject("PersonaLibraryPrefab", out PersonaLibraryPrefab))
                return;
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RecklessRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CleanRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["WeaponRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CautiousRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["ViolentRacer - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;

            //PacingScheduleEscape
            PacingLibraryEntityData.FieldList["PacingScheduleEscape_Default"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleEscape_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleEscape_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;

            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RecklessRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CleanRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["WeaponRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CautiousRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["ViolentRacer - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;

            //PacingScheduleGroupCopHotPursuit
            switch ((ClassEnum)index) {
                case ClassEnum.Easy:
                    difficulty = "_Easy"; break;
                case ClassEnum.Normal:
                    difficulty = ""; break;
                case ClassEnum.Hard:
                case ClassEnum.AroundTheWorld:
                    difficulty = "_Hard"; break;
                default:
                    return;
            }
            PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit_Hard"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupCopHotPursuit" + difficulty].FieldDefault;

            //PacingScheduleGroupDirectedRace
            switch ((ClassEnum)index) {
                //In addition to above, we also have "_AroundTheWorld" available for these
                case ClassEnum.AroundTheWorld:
                    difficulty = "_AroundTheWorld"; break;
            }
            PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_Hard"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_AroundTheWorld"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace" + difficulty].FieldDefault;
            PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupDirectedRace" + difficulty].FieldDefault;

            //Swap HealthProfilesListEntityData pointers inside PersonaLibraryPrefab objects (but not directly, for now, as that causes weird HUD issues)
            switch ((ClassEnum)index) {
                case ClassEnum.Easy:
                    difficulty = "CopInterceptor_Easy"; break;
                case ClassEnum.Normal:
                    difficulty = "CopInterceptor_Medium"; break;
                case ClassEnum.Hard:
                    difficulty = "CopInterceptor_Hard"; break;
                case ClassEnum.AroundTheWorld:
                    difficulty = "AI_AroundTheWorld"; break;
                default:
                    return;
            }
            NFSObject HealthProfilesListEntityData;
            if (!TryGetObject("HealthProfilesListEntityData", out HealthProfilesListEntityData))
                return;
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RecklessRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CleanRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["WeaponRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["CautiousRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;
            PersonaLibraryPrefab.FieldList["ViolentRacer - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;

            //Adjust WeaponSkill values inside PersonaLibraryPrefab objects
            float skillVsCop = Math.Min(1f, (float)(0.01 + (1 + index) * 0.33));
            float skillVsRacer = Math.Min(1f, (float)((1 + index) * 0.25));
            float skill = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkillVsHumanCop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
            if (!eqWeapUse) {
                skillVsCop /= 2f;
                skillVsRacer = 0f;
            }
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkillVsAICop"].Field = skillVsCop;
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["RecklessRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["CleanRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["WeaponRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["CautiousRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;
            PersonaLibraryPrefab.FieldList["ViolentRacer - WeaponSkillVsAIRacer"].Field = skillVsRacer;

            //Set speed matching inside PersonaLibraryPrefab objects based on class
            bool matchSpeed = index < (int)ClassEnum.Hard;
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["RecklessRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["CleanRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["WeaponRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["CautiousRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
            PersonaLibraryPrefab.FieldList["ViolentRacer - AvoidanceSpeedMatchWhenBlocked"].Field = matchSpeed;
        }

        //Skill events
        public void UpdateCopSkill(float skill) {
            copSkill = skill;
            LaunchThread(UpdateCopSkill);
        }
        protected float copSkill;
        protected void UpdateCopSkill() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            float skill = copSkill;

            //Adjust PacingSkill values inside PersonaLibraryPrefab objects
            NFSObject PersonaLibraryPrefab;
            if (!TryGetObject("PersonaLibraryPrefab", out PersonaLibraryPrefab))
                return;
            PersonaLibraryPrefab.FieldList["AggressorCopPersonality - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["BruteCopPersonality - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["BasicCopPersonality - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["AdvAggressorCopPersonality - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["ChaserCopPersonality - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["RacerTutorialCop - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["CopTutorialCop - PacingSkill"].Field = skill;
        }

        public void UpdateRacerSkill(float skill) {
            racerSkill = skill;
            LaunchThread(UpdateRacerSkill);
        }
        protected float racerSkill;
        protected void UpdateRacerSkill() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            float skill = racerSkill;

            //Adjust HeatTime based on skill
            NFSObject AiDirectorEntityData;
            if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                return;
            AiDirectorEntityData.FieldList["HeatTime"].Field = Convert.ToInt32((int)AiDirectorEntityData.FieldList["HeatTime"].FieldDefault / Math.Pow(Math.Max(0.34d, skill) * 3d, 2));

            //Adjust PacingSkill values inside PersonaLibraryPrefab objects
            NFSObject PersonaLibraryPrefab;
            if (!TryGetObject("PersonaLibraryPrefab", out PersonaLibraryPrefab))
                return;
            PersonaLibraryPrefab.FieldList["Tier1WeaponRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["RecklessRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2CautiousRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1ViolentRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1RecklessRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1CautiousRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["RacerTutorialRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["CleanRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2WeaponRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier1CleanRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2ViolentRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2RecklessRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["Tier2CleanRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["WeaponRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["CopTutorialRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["CautiousRacer - PacingSkill"].Field = skill;
            PersonaLibraryPrefab.FieldList["ViolentRacer - PacingSkill"].Field = skill;
        }

        //Density events
        public void UpdateCopDensity(int density) {
            copDensity = density;
            LaunchThread(UpdateCopDensity);
        }
        protected int copDensity;
        protected void UpdateCopDensity() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            int density = copDensity;

            //Adjust spawn caps and times based on density
            NFSObject AiDirectorEntityData;
            if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                return;
            //AiDirectorEntityData.FieldList["NumberOfPawnCopsWanted"].Field = (int)((int)AiDirectorEntityData.FieldList["NumberOfPawnRacersWanted"].FieldDefault * (density / 2f));
            AiDirectorEntityData.FieldList["GlobalNumberOfCops"].Field = (density == 0) ? 0 : Math.Max(1, density - 1); //0, 1 (with low spawn rates), 1 (normal), 2 (high), 3 (v. high), 4 (most wanted)
            AiDirectorEntityData.FieldList["GlobalChanceOfSpawningRoamingCop"].Field = Math.Min(100, (int)AiDirectorEntityData.FieldList["GlobalChanceOfSpawningRoamingCop"].FieldDefault * density);
            AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnCop"].Field = (float)AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnCop"].FieldDefault / (density / 2f + 0.01f);
            AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCop"].Field = (float)AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCop"].FieldDefault / (density / 2f + 0.01f);
            AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringPursuit"].Field = (float)AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringPursuit"].FieldDefault / (density / 2f + 0.01f);
            AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringHPRacer"].Field = (float)AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringHPRacer"].FieldDefault / (density / 2f + 0.01f);
        }

        public void UpdateRacerDensity(int density) {
            racerDensity = density;
            LaunchThread(UpdateRacerDensity);
        }
        protected int racerDensity;
        protected void UpdateRacerDensity() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            int density = racerDensity;

            //Adjust spawn caps and times based on density
            NFSObject AiDirectorEntityData;
            if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                return;
            AiDirectorEntityData.FieldList["MaxNumberOfAiOnlySpontaneousRaces"].Field = density;
            AiDirectorEntityData.FieldList["NumberOfPawnRacersWanted"].Field = (int)((int)AiDirectorEntityData.FieldList["NumberOfPawnRacersWanted"].FieldDefault * (density / 2f));
            AiDirectorEntityData.FieldList["NumberOfRacers"].Field = (density == 0) ? 0 : Math.Max(1, density - 1); //0, 1 (with low spawn rates), 1 (normal), 2 (high), 3 (v. high)
            AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnRacer"].Field = (float)AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnRacer"].FieldDefault / (density / 2f + 0.01f);
            AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnRacer"].Field = (float)AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnRacer"].FieldDefault / (density / 2f + 0.01f);
        }

        //Other events
        public void UpdateCopMinHeat(int heat) {
            copMinHeat = heat;
            LaunchThread(UpdateCopMinHeat);
        }
        protected int copMinHeat;
        protected void UpdateCopMinHeat() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            int heat = copMinHeat;

            //Set Min/MaxHeat of each heat appropriately...
            NFSObject AiDirectorEntityData;
            if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                return;
            for (int i = 1; i < heat; i++) {
                AiDirectorEntityData.FieldList["Heat" + i + " - MinHeat"].Field = 0;
                AiDirectorEntityData.FieldList["Heat" + i + " - MaxHeat"].Field = 0;
            }
            AiDirectorEntityData.FieldList["Heat" + heat + " - MinHeat"].Field = 1;
            AiDirectorEntityData.FieldList["Heat" + heat + " - MaxHeat"].Field = AiDirectorEntityData.FieldList["Heat" + heat + " - MaxHeat"].FieldDefault;

            //... and default the rest
            for (int i = heat + 1; i <= 10; i++) {
                AiDirectorEntityData.FieldList["Heat" + i + " - MinHeat"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - MinHeat"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - MaxHeat"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - MaxHeat"].FieldDefault;
            }
        }

        public void UpdateCopHeatIntensity(int index) {
            copHeatIntensity = index;
            LaunchThread(UpdateCopHeatIntensity);
        }
        protected int copHeatIntensity;
        protected void UpdateCopHeatIntensity() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            int index = copHeatIntensity;

            //Go crazy! Default everything to selectively override it later
            NFSObject AiDirectorEntityData;
            if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                return;
            AiDirectorEntityData.FieldList["PullAheadHeatThreshold"].Field = AiDirectorEntityData.FieldList["PullAheadHeatThreshold"].FieldDefault;
            AiDirectorEntityData.FieldList["BlockHeatThreshold"].Field = AiDirectorEntityData.FieldList["BlockHeatThreshold"].FieldDefault;
            for (int i = 1; i <= 10; i++) {
                //Adjust CopCount based on option (up one for every option above Normal, down one for Cool)
                AiDirectorEntityData.FieldList["Heat" + i + " - CopCountHeatBased"].Field = (int)AiDirectorEntityData.FieldList["Heat" + i + " - CopCountHeatBased"].FieldDefault
                    + index - (int)HeatEnum.Normal;

                //Everything else just gets reset
                AiDirectorEntityData.FieldList["Heat" + i + " - Basic"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - Basic"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - Chaser"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - Chaser"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - Brute"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - Brute"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - Aggressor"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - Aggressor"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - AdvancedAggressor"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - AdvancedAggressor"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - ChanceOfSpawningRoamingCopHeatBased"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - ChanceOfSpawningRoamingCopHeatBased"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - MinimumHelicopterSpawnInterval"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - MinimumHelicopterSpawnInterval"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - MaxHelicoptersPerBubble"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - MaxHelicoptersPerBubble"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - MinimumRoadblockSpawnInterval"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - MinimumRoadblockSpawnInterval"].FieldDefault;
                AiDirectorEntityData.FieldList["Heat" + i + " - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].FieldDefault;
            }

            //Now do specific overrides by heat intensity
            switch ((HeatEnum)index) {
                case HeatEnum.Cool:
                    AiDirectorEntityData.FieldList["Heat2 - Basic"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat3 - Chaser"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat4 - Brute"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat5 - Brute"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat6 - Aggressor"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat7 - Aggressor"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat8 - AdvancedAggressor"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat9 - AdvancedAggressor"].Field = 2;
                    AiDirectorEntityData.FieldList["Heat10 - AdvancedAggressor"].Field = 3;
                    AiDirectorEntityData.FieldList["Heat1 - ChanceOfSpawningRoamingCopHeatBased"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat2 - ChanceOfSpawningRoamingCopHeatBased"].Field = 1;
                    AiDirectorEntityData.FieldList["Heat3 - ChanceOfSpawningRoamingCopHeatBased"].Field = 2;
                    AiDirectorEntityData.FieldList["Heat4 - ChanceOfSpawningRoamingCopHeatBased"].Field = 2;
                    AiDirectorEntityData.FieldList["Heat5 - ChanceOfSpawningRoamingCopHeatBased"].Field = 3;
                    AiDirectorEntityData.FieldList["Heat6 - ChanceOfSpawningRoamingCopHeatBased"].Field = 3;
                    AiDirectorEntityData.FieldList["Heat7 - ChanceOfSpawningRoamingCopHeatBased"].Field = 5;
                    AiDirectorEntityData.FieldList["Heat8 - ChanceOfSpawningRoamingCopHeatBased"].Field = 7;
                    AiDirectorEntityData.FieldList["Heat9 - ChanceOfSpawningRoamingCopHeatBased"].Field = 10;
                    AiDirectorEntityData.FieldList["Heat10 - ChanceOfSpawningRoamingCopHeatBased"].Field = 15;
                    AiDirectorEntityData.FieldList["Heat5 - MinimumHelicopterSpawnInterval"].Field = -1f;
                    AiDirectorEntityData.FieldList["Heat6 - MinimumHelicopterSpawnInterval"].Field = 50f;
                    AiDirectorEntityData.FieldList["Heat7 - MinimumHelicopterSpawnInterval"].Field = 40f;
                    AiDirectorEntityData.FieldList["Heat8 - MinimumHelicopterSpawnInterval"].Field = 30f;
                    AiDirectorEntityData.FieldList["Heat9 - MinimumHelicopterSpawnInterval"].Field = 25f;
                    AiDirectorEntityData.FieldList["Heat10 - MinimumHelicopterSpawnInterval"].Field = 20f;
                    AiDirectorEntityData.FieldList["Heat3 - MinimumRoadblockSpawnInterval"].Field = -1f;
                    AiDirectorEntityData.FieldList["Heat4 - MinimumRoadblockSpawnInterval"].Field = 50f;
                    AiDirectorEntityData.FieldList["Heat5 - MinimumRoadblockSpawnInterval"].Field = 45f;
                    AiDirectorEntityData.FieldList["Heat6 - MinimumRoadblockSpawnInterval"].Field = 30f;
                    AiDirectorEntityData.FieldList["Heat7 - MinimumRoadblockSpawnInterval"].Field = 25f;
                    AiDirectorEntityData.FieldList["Heat8 - MinimumRoadblockSpawnInterval"].Field = 20f;
                    AiDirectorEntityData.FieldList["Heat9 - MinimumRoadblockSpawnInterval"].Field = 15f;
                    AiDirectorEntityData.FieldList["Heat10 - MinimumRoadblockSpawnInterval"].Field = 10f;
                    break;
                case HeatEnum.Normal:
                    //Do nothing, already reset above
                    break;
                default: //Hot, Very Hot, Blazing
                    if (index >= (int)HeatEnum.Hot) {
                        AiDirectorEntityData.FieldList["PullAheadHeatThreshold"].Field = 1;
                        AiDirectorEntityData.FieldList["BlockHeatThreshold"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat1 - Chaser"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat2 - Chaser"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat3 - Brute"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat4 - Aggressor"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat5 - Aggressor"].Field = 1;
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
                        AiDirectorEntityData.FieldList["Heat1 - MinimumRoadblockSpawnInterval"].Field = 50f;
                        AiDirectorEntityData.FieldList["Heat2 - MinimumRoadblockSpawnInterval"].Field = 50f;
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
                    }
                    if (index >= (int)HeatEnum.VeryHot) {
                        AiDirectorEntityData.FieldList["Heat1 - Brute"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat2 - Brute"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat3 - Aggressor"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat4 - AdvancedAggressor"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat5 - AdvancedAggressor"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat6 - AdvancedAggressor"].Field = 2;
                        AiDirectorEntityData.FieldList["Heat7 - AdvancedAggressor"].Field = 2;
                        AiDirectorEntityData.FieldList["Heat8 - AdvancedAggressor"].Field = 4;
                        AiDirectorEntityData.FieldList["Heat9 - AdvancedAggressor"].Field = 5;
                        AiDirectorEntityData.FieldList["Heat10 - AdvancedAggressor"].Field = 6;
                        AiDirectorEntityData.FieldList["Heat1 - ChanceOfSpawningRoamingCopHeatBased"].Field = 18;
                        AiDirectorEntityData.FieldList["Heat2 - ChanceOfSpawningRoamingCopHeatBased"].Field = 27;
                        AiDirectorEntityData.FieldList["Heat3 - ChanceOfSpawningRoamingCopHeatBased"].Field = 36;
                        AiDirectorEntityData.FieldList["Heat4 - ChanceOfSpawningRoamingCopHeatBased"].Field = 45;
                        AiDirectorEntityData.FieldList["Heat5 - ChanceOfSpawningRoamingCopHeatBased"].Field = 54;
                        AiDirectorEntityData.FieldList["Heat6 - ChanceOfSpawningRoamingCopHeatBased"].Field = 63;
                        AiDirectorEntityData.FieldList["Heat7 - ChanceOfSpawningRoamingCopHeatBased"].Field = 72;
                        AiDirectorEntityData.FieldList["Heat8 - ChanceOfSpawningRoamingCopHeatBased"].Field = 81;
                        AiDirectorEntityData.FieldList["Heat9 - ChanceOfSpawningRoamingCopHeatBased"].Field = 90;
                        AiDirectorEntityData.FieldList["Heat10 - ChanceOfSpawningRoamingCopHeatBased"].Field = 100;
                    }
                    if (index >= (int)HeatEnum.Blazing) {
                        AiDirectorEntityData.FieldList["Heat1 - Aggressor"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat2 - Aggressor"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat3 - AdvancedAggressor"].Field = 1;
                        AiDirectorEntityData.FieldList["Heat4 - AdvancedAggressor"].Field = 2;
                        AiDirectorEntityData.FieldList["Heat5 - AdvancedAggressor"].Field = 2;
                        AiDirectorEntityData.FieldList["Heat6 - AdvancedAggressor"].Field = 3;
                        AiDirectorEntityData.FieldList["Heat7 - AdvancedAggressor"].Field = 3;
                        AiDirectorEntityData.FieldList["Heat8 - AdvancedAggressor"].Field = 5;
                        AiDirectorEntityData.FieldList["Heat9 - AdvancedAggressor"].Field = 6;
                        AiDirectorEntityData.FieldList["Heat10 - AdvancedAggressor"].Field = 7;
                        AiDirectorEntityData.FieldList["Heat1 - ChanceOfSpawningRoamingCopHeatBased"].Field = 27;
                        AiDirectorEntityData.FieldList["Heat2 - ChanceOfSpawningRoamingCopHeatBased"].Field = 36;
                        AiDirectorEntityData.FieldList["Heat3 - ChanceOfSpawningRoamingCopHeatBased"].Field = 45;
                        AiDirectorEntityData.FieldList["Heat4 - ChanceOfSpawningRoamingCopHeatBased"].Field = 54;
                        AiDirectorEntityData.FieldList["Heat5 - ChanceOfSpawningRoamingCopHeatBased"].Field = 63;
                        AiDirectorEntityData.FieldList["Heat6 - ChanceOfSpawningRoamingCopHeatBased"].Field = 72;
                        AiDirectorEntityData.FieldList["Heat7 - ChanceOfSpawningRoamingCopHeatBased"].Field = 81;
                        AiDirectorEntityData.FieldList["Heat8 - ChanceOfSpawningRoamingCopHeatBased"].Field = 90;
                        AiDirectorEntityData.FieldList["Heat9 - ChanceOfSpawningRoamingCopHeatBased"].Field = 100;
                        AiDirectorEntityData.FieldList["Heat10 - MaxHelicoptersPerBubble"].Field = 2;
                    }
                    break;
            }
        }

        public void UpdateSpikeStripFix(bool useFix) {
            useSpikeStripFix = useFix;
            LaunchThread(UpdateSpikeStripFix);
        }
        protected bool useSpikeStripFix;
        protected void UpdateSpikeStripFix() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            bool useFix = useSpikeStripFix;

            NFSObject SpikestripWeapon;
            if (!TryGetObject("SpikestripWeapon", out SpikestripWeapon))
                return;
            if (useFix) {
                SpikestripWeapon.FieldList["Classification"].Field = NFSObjectSpikestripWeapon.VehicleWeaponClassification.VehicleWeaponClassification_BackwardFiring;
                SpikestripWeapon.FieldList["MinimumTriggerDistance-Low"].Field = 2f;
                SpikestripWeapon.FieldList["MinimumTriggerDistance-High"].Field = 1f;
            }
            else
                SpikestripWeapon.ResetFieldsToDefault(); //Safe as these are the only things we change
        }

        public void UpdateEqualWeaponUse(bool eqWeapUse) {
            equalWeaponUse = eqWeapUse;
            LaunchThread(UpdateEqualWeaponUse);
        }
        protected bool equalWeaponUse;
        protected void UpdateEqualWeaponUse() {
            if (!memManager.ProcessOpen || CheckThread())
                return;
            bool weaponUse = equalWeaponUse;

            UpdateCopClass(copClass, equalWeaponUse);
            UpdateRacerClass(racerClass, equalWeaponUse);
        }
    }
}
