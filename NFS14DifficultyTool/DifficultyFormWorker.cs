using System;
using System.Collections.Concurrent;
using System.Linq;
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
    public enum MatchmakingModeEnum {
        Unknown = -1,
        Public = 0,
        Friends = 1,
        Private = 2,
        SinglePlayer = 3
    }

    public class DifficultyFormWorker {
        protected MemoryManager memManager;
        protected ConcurrentDictionary<string, NFSObject> objectList;
        protected ConcurrentDictionary<string, Thread> threadList;
        protected DifficultyForm parent;

        protected MatchmakingModeEnum lastMatchmakingMode;

        protected string[] copPersonalityList;
        protected string[] racerPersonalityList;
        protected string[] copTypeList;

        public DifficultyFormWorker(DifficultyForm parent) {
            memManager = new MemoryManager();
            objectList = new ConcurrentDictionary<string, NFSObject>();
            threadList = new ConcurrentDictionary<string, Thread>();
            this.parent = parent;

            lastMatchmakingMode = MatchmakingModeEnum.Unknown;

            copPersonalityList = new string[] {
                "AggressorCopPersonality",
                "BruteCopPersonality",
                "BasicCopPersonality",
                "AdvAggressorCopPersonality",
                "ChaserCopPersonality",
                "RacerTutorialCop",
                "CopTutorialCop"
            };

            racerPersonalityList = new string[] {
                "Tier1WeaponRacer",
                "RecklessRacer",
                "Tier2CautiousRacer",
                "Tier1ViolentRacer",
                "Tier1RecklessRacer",
                "Tier1CautiousRacer",
                "RacerTutorialRacer",
                "CleanRacer",
                "Tier2WeaponRacer",
                "Tier1CleanRacer",
                "Tier2ViolentRacer",
                "Tier2RecklessRacer",
                "Tier2CleanRacer",
                "WeaponRacer",
                "CopTutorialRacer",
                "CautiousRacer",
                "ViolentRacer"
            };

            copTypeList = new string[] {
                "Basic",
                "Chaser",
                "Brute",
                "Aggressor",
                "AdvancedAggressor"
            };
        }

        //Useful functions
        public void ResetAll(bool isClosing = false) {
            parent.SetStatus("Signalling threads...");
            foreach (Thread t in threadList.Values)
                if (t.IsAlive)
                    t.Priority = ThreadPriority.Normal;

            parent.SetStatus("Aborting searches...");
            memManager.AbortFindObject();

            parent.SetStatus("Reverting changes...");
            foreach (NFSObject o in objectList.Values)
                o.ResetFieldsToDefault();

            parent.SetStatus("Closing nfs14 handle...");
            memManager.CloseHandle();

            //If we're not closing, start looking for our process again (e.g. we've found out game has closed but we haven't)
            //Also clear our objectList, as the game may have also just launched a new session without closing
            if (!isClosing) {
                parent.SetStatus();
                objectList.Clear();
                parent.Timers.SessionChangeTimer.Stop();
                parent.Timers.FindProcessTimer.Start();
            }
        }

        public bool TryGetObject(string name, out NFSObject obj) {
            obj = GetObject(name);
            return obj != null;
        }
        public NFSObject GetObject(string name) {
            if (!memManager.IsProcessOpen())
                return null;

            //Also avoid hammering on the game while it's still loading
            if (objectList.Count == 0 && name != "UIRootController")
                return null;

            NFSObject type = null;
            if (!objectList.TryGetValue(name, out type) || !type.IsValid()) {
                //If we haven't found any objects yet, we're still waiting for the game world to load
                string status = (objectList.Count == 0) ? "Waiting for game world..." : "Finding " + name + "...";
                parent.PushStatus(status);

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
                        case "AttackHeliEntityData":
                            type = objectList.GetOrAdd(name, new NFSObjectAttackHeliEntityData(memManager)); break;
                        case "UIRootController":
                            type = objectList.GetOrAdd(name, new NFSObjectUIRootController(memManager)); break;
                        case "ProfileOptions":
                            type = objectList.GetOrAdd(name, new NFSObjectProfileOptions(memManager)); break;
                        case "PacingPursuitScheduleHard":
                            type = objectList.GetOrAdd(name, new NFSObjectPacingPursuitScheduleHard(memManager)); break;
                        default:
                            return null;
                    }
                }
                catch (Exception ex) {
                    //Don't start printing errors until we've at least found something once, otherwise we probably aren't even in the game world yet
                    if (objectList.Count > 0)
                        parent.SetStatus(ex.Message);
                }

                parent.PopStatus(status);
            }

            return type;
        }

        protected void LaunchThread(ThreadStart obj) {
            string threadName = obj.Method.ToString();
            Thread thread;

            //If the current thread for this method is waiting, don't bother spawning a new one at all
            //Note: https://msdn.microsoft.com/en-us/library/system.threading.thread.threadstate.aspx states this is bad practice
            //      However, as this is merely a convenience shortcut and not anything critical, I'm using it anyway :)
            if (threadList.TryGetValue(threadName, out thread)
            && thread.ThreadState == ThreadState.WaitSleepJoin)
                return;

            thread = new Thread(obj);
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Name = threadName;
            thread.Start();
        }

        protected bool CheckThread() {
            //If our process isn't open, don't even bother
            if (!memManager.IsProcessOpen())
                return true;

            //If we successfully add our thread (no thread for this name ever existed before), we're good to go
            Thread oldThread = threadList.GetOrAdd(Thread.CurrentThread.Name, Thread.CurrentThread);
            if (oldThread == Thread.CurrentThread)
                return false;

            //Also as a convenience shortcut, if the old thread is waiting, don't bother (see warnings on this above)
            if (oldThread.ThreadState == ThreadState.WaitSleepJoin)
                return true;

            //Otherwise, wait on the old thread - while waiting, threads created after will then wait on us, forming a queue
            threadList[Thread.CurrentThread.Name] = Thread.CurrentThread;
            if (oldThread.IsAlive) {
                //Thus, before we wait, tell our old thread we're waiting on it by bumping its priority
                oldThread.Priority = ThreadPriority.Normal;
                oldThread.Join();
            }

            //If we're deferring to a later thread (per above), let our caller know so that it might exit early
            return Thread.CurrentThread.Priority > ThreadPriority.BelowNormal;
        }

        public bool CheckIfReady() {
            //If we have no loaded objects, game must be closed or loading
            if (objectList.Count == 0) {
                //If we're waiting on game, say so
                if (!memManager.IsProcessOpen())
                    parent.SetStatus("Waiting for game...");

                //Open the process - also make sure it hasn't closed on us
                //This can happen if game has started, but quit while hunting game objects
                if (!memManager.OpenProcess("nfs14")) { // && !MemManager.OpenProcess("nfs14_x86")
                    memManager.CloseHandle(); //Safe, this will only do anything if open
                    return false;
                }

                //Found it, we can slow down our checks for hunting game objects
                parent.SetStatus("Found it!");
                parent.Timers.FindProcessTimer.Interval = 10000;

                //Get ready to start hunting for game objects
                LaunchThread(() => {
                    //This should only run at start, while we're waiting for the game to be ready, on a loop
                    //If it's been this long we probably want to start new search from beginning
                    //So run AbortFindObject before even checking our old thread, because we don't care about its search
                    memManager.AbortFindObject();
                    if (CheckThread())
                        return;

                    //Look for UIRootController, which definitely won't be ready until we've loaded in
                    GetObject("UIRootController");
                });
            }

            //We'll know we're ready when we've loaded game objects
            return objectList.Count > 0;
        }

        public MatchmakingModeEnum GetMatchmakingMode() {
            LaunchThread(() => {
                //If any of our objects are no longer valid, we have probably changed sessions
                //In this case we'll just blow everything away and start over
                foreach (NFSObject o in objectList.Values) {
                    if (!o.IsValid()) {
                        ResetAll();
                        return;
                    }
                }

                NFSObject ProfileOptions;
                if (!TryGetObject("ProfileOptions", out ProfileOptions))
                    return;
                MatchmakingModeEnum matchmakingMode = MatchmakingModeEnum.Unknown; //(MatchmakingModeEnum)ProfileOptions.FieldList["MatchmakingMode"].Field;
                string status;
                switch (matchmakingMode) {
                    case MatchmakingModeEnum.Public:
                        status = "Public Game"; break;
                    case MatchmakingModeEnum.Friends:
                        status = "Friends Game"; break;
                    case MatchmakingModeEnum.Private:
                        status = "Private Game"; break;
                    case MatchmakingModeEnum.SinglePlayer:
                        status = "Single Player"; break;
                    default: //Unknown
                        status = "Unknown!?"; break;
                }

                //Temporarily show a MatckmakingMode change, will be cleared next pass or by something else
                if (lastMatchmakingMode == matchmakingMode)
                    parent.PopStatus();
                else {
                    lastMatchmakingMode = matchmakingMode;
                    parent.SetStatus("Migrating to " + status + "...", true);
                }
            });
            return lastMatchmakingMode;
        }

        //Class events
        public void UpdateCopClass(int index, bool eqWeapUse, bool noSideSlam) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

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
                foreach (string s in copPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;

                //PacingSchedulePursuit
                PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Default"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Easy"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Hard"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingSchedulePursuit_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;

                foreach (string s in copPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingSchedulePursuit_" + difficulty].FieldDefault;

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
                foreach (string s in copPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["CopHealthProfile_" + difficulty].FieldDefault;

                //Adjust WeaponSkill values inside PersonaLibraryPrefab objects
                float skillVsCop = 0f;
                float skillVsRacer = (float)((1 + index) * 0.25);
                float skill = skillVsRacer;
                foreach (string s in copPersonalityList) {
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkill"].Field = skill;
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkillVsHumanCop"].Field = skillVsCop;
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
                }
                if (!eqWeapUse)
                    skillVsRacer /= 2f;
                foreach (string s in copPersonalityList) {
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkillVsAICop"].Field = skillVsCop;
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkillVsAIRacer"].Field = skillVsRacer;
                }

                //Don't use SideSlam, it's kinda buggy
                foreach (string s in copPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - SideSlam-Chance"].Field = (noSideSlam)
                        ? 0f : PersonaLibraryPrefab.FieldList[s + " - SideSlam-Chance"].FieldDefault;

                //Adjust PacingPursuitScheduleHard to tougher values to make a Very Hard class
                NFSObject PacingPursuitScheduleHard;
                if (!TryGetObject("PacingPursuitScheduleHard", out PacingPursuitScheduleHard))
                    return;
                if ((ClassEnum)index == ClassEnum.AroundTheWorld) {
                    PacingPursuitScheduleHard.FieldList["DistanceCurve-1-X"].Field = 999999f;
                    PacingPursuitScheduleHard.FieldList["SkillScalarOverTimeCurve-1-X"].Field = 999999f;

                    //More effecient routing
                    foreach (string s in copPersonalityList) {
                        PersonaLibraryPrefab.FieldList[s + " - RoutingRoadBiasMin"].Field = 9999f;
                        PersonaLibraryPrefab.FieldList[s + " - RoutingRoadBiasMax"].Field = 9999f;
                        PersonaLibraryPrefab.FieldList[s + " - AvoidanceSpeedMatchWhenBlocked"].Field = false;
                    }
                }
                else {
                    PacingPursuitScheduleHard.ResetFieldsToDefault(); //Safe as these are the only things we change

                    //Revert routing
                    foreach (string s in copPersonalityList) {
                        PersonaLibraryPrefab.FieldList[s + " - RoutingRoadBiasMin"].Field = PersonaLibraryPrefab.FieldList[s + " - RoutingRoadBiasMin"].FieldDefault;
                        PersonaLibraryPrefab.FieldList[s + " - RoutingRoadBiasMax"].Field = PersonaLibraryPrefab.FieldList[s + " - RoutingRoadBiasMax"].FieldDefault;
                        PersonaLibraryPrefab.FieldList[s + " - AvoidanceSpeedMatchWhenBlocked"].Field = PersonaLibraryPrefab.FieldList[s + " - AvoidanceSpeedMatchWhenBlocked"].FieldDefault; ;
                    }
                }
            });
        }

        public void UpdateRacerClass(int index, bool eqWeapUse, bool noSideSlam) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

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
                foreach (string s in racerPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - UsedSpontaneousRacePacingScheduleGroup"].Field = PacingLibraryEntityData.FieldList["PacingScheduleGroupSpontaneousRace_" + difficulty].FieldDefault;

                //PacingScheduleEscape
                PacingLibraryEntityData.FieldList["PacingScheduleEscape_Default"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleEscape_Easy"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleEscape_Hard"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;
                PacingLibraryEntityData.FieldList["PacingScheduleEscape_Tutorial"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;

                foreach (string s in racerPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - UsedPursuitAndEscapePacingSchedule"].Field = PacingLibraryEntityData.FieldList["PacingScheduleEscape_" + difficulty].FieldDefault;

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
                foreach (string s in racerPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - HealthProfile"].Field = HealthProfilesListEntityData.FieldList["RacerHealthProfile_" + difficulty].FieldDefault;

                //Adjust WeaponSkill values inside PersonaLibraryPrefab objects
                float skillVsCop = (float)(0.01 + (1 + Math.Min(3, index)) * 0.33);
                float skillVsRacer = (float)((1 + index) * 0.25);
                float skill = skillVsCop;
                foreach (string s in racerPersonalityList) {
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkill"].Field = skill;
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkillVsHumanCop"].Field = skillVsCop;
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkillVsHumanRacer"].Field = skillVsRacer;
                }
                if (!eqWeapUse) {
                    skillVsCop /= 2f;
                    skillVsRacer = 0f;
                }
                foreach (string s in racerPersonalityList) {
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkillVsAICop"].Field = skillVsCop;
                    PersonaLibraryPrefab.FieldList[s + " - WeaponSkillVsAIRacer"].Field = skillVsRacer;
                }

                //Don't use SideSlam, it's kinda buggy
                foreach (string s in racerPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - SideSlam-Chance"].Field = (noSideSlam)
                        ? 0f : PersonaLibraryPrefab.FieldList[s + " - SideSlam-Chance"].FieldDefault;
            });
        }

        //Skill events
        public void UpdateCopSkill(float skill) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

                //Adjust PacingSkill values inside PersonaLibraryPrefab objects
                NFSObject PersonaLibraryPrefab;
                if (!TryGetObject("PersonaLibraryPrefab", out PersonaLibraryPrefab))
                    return;
                foreach (string s in copPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - PacingSkill"].Field = skill;
            });
        }

        public void UpdateRacerSkill(float skill) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

                //Adjust HeatTime based on skill
                NFSObject AiDirectorEntityData;
                if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                    return;
                AiDirectorEntityData.FieldList["HeatTime"].Field = Convert.ToInt32((int)AiDirectorEntityData.FieldList["HeatTime"].FieldDefault / Math.Pow(Math.Max(0.34d, skill) * 3d, 2));

                //Adjust PacingSkill values inside PersonaLibraryPrefab objects
                NFSObject PersonaLibraryPrefab;
                if (!TryGetObject("PersonaLibraryPrefab", out PersonaLibraryPrefab))
                    return;
                foreach (string s in racerPersonalityList)
                    PersonaLibraryPrefab.FieldList[s + " - PacingSkill"].Field = skill;
            });
        }

        //Density events
        public void UpdateCopDensity(int density) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

                //Adjust spawn caps and times based on density
                NFSObject AiDirectorEntityData;
                if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                    return;
                //AiDirectorEntityData.FieldList["NumberOfPawnCopsWanted"].Field = ((int)AiDirectorEntityData.FieldList["NumberOfPawnRacersWanted"].FieldDefault * density) / 2;
                AiDirectorEntityData.FieldList["GlobalNumberOfCops"].Field = (density == 0) ? 0 : Math.Max(1, density - 2); //0, 1 (with low spawn rates), 1 (normal), 1 (high), 2 (v. high), 3 (most wanted)
                AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnCop"].Field = (float)AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnCop"].FieldDefault / Math.Max(0.01f, density / 2f);
                AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCop"].Field = (float)AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCop"].FieldDefault / Math.Max(0.01f, density / 2f);
                AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringPursuit"].Field = (float)AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringPursuit"].FieldDefault / Math.Max(0.01f, density / 2f);
                AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringHPRacer"].Field = (float)AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnCopDuringHPRacer"].FieldDefault / Math.Max(0.01f, density / 2f);

                AiDirectorEntityData.FieldList["GlobalChanceOfSpawningRoamingCop"].Field = ((int)AiDirectorEntityData.FieldList["GlobalChanceOfSpawningRoamingCop"].FieldDefault * density) / 2;
                for (int i = 10; i >= 1; i--) {
                    AiDirectorEntityData.FieldList["Heat" + i + " - ChanceOfSpawningRoamingCopHeatBased"].Field = ((int)AiDirectorEntityData.FieldList["Heat" + i + " - ChanceOfSpawningRoamingCopHeatBased"].FieldDefault * density) / 2;
                }
            });
        }

        public void UpdateRacerDensity(int density) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

                //Adjust spawn caps and times based on density
                NFSObject AiDirectorEntityData;
                if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                    return;
                AiDirectorEntityData.FieldList["NumberOfPawnRacersWanted"].Field = ((int)AiDirectorEntityData.FieldList["NumberOfPawnRacersWanted"].FieldDefault * density) / 2;
                AiDirectorEntityData.FieldList["NumberOfRacers"].Field = (density == 0) ? 0 : Math.Max(1, density - 2); //0, 1 (with low spawn rates), 1 (normal), 1 (high), 2 (v. high)
                AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnRacer"].Field = (float)AiDirectorEntityData.FieldList["InitialTimeIntervalForTryingToSpawnRacer"].FieldDefault / Math.Max(0.01f, density / 2f);
                AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnRacer"].Field = (float)AiDirectorEntityData.FieldList["TimeIntervalForTryingToSpawnRacer"].FieldDefault / Math.Max(0.01f, density / 2f);

                AiDirectorEntityData.FieldList["MaxNumberOfAiOnlySpontaneousRaces"].Field = Math.Max(1, density - 2);
            });
        }

        //Other events
        public void UpdateCopMinHeat(int heat) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

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
            });
        }

        public void UpdateCopHeatIntensity(int index) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

                //Go crazy! Default everything to selectively override it later
                NFSObject AiDirectorEntityData;
                if (!TryGetObject("AiDirectorEntityData", out AiDirectorEntityData))
                    return;
                AiDirectorEntityData.FieldList["PullAheadHeatThreshold"].Field = AiDirectorEntityData.FieldList["PullAheadHeatThreshold"].FieldDefault;
                AiDirectorEntityData.FieldList["BlockHeatThreshold"].Field = AiDirectorEntityData.FieldList["BlockHeatThreshold"].FieldDefault;
                for (int i = 1; i <= 10; i++) {
                    //Adjust CopCount based on option (up one for every option above Normal, down one for Cool)
                    AiDirectorEntityData.FieldList["Heat" + i + " - CopCountHeatBased"].Field = Math.Max(1,
                        (int)AiDirectorEntityData.FieldList["Heat" + i + " - CopCountHeatBased"].FieldDefault + index - (int)HeatEnum.Normal);

                    //Everything else just gets reset
                    foreach (string s in copTypeList)
                        AiDirectorEntityData.FieldList["Heat" + i + " - " + s].Field = AiDirectorEntityData.FieldList["Heat" + i + " - " + s].FieldDefault;
                    AiDirectorEntityData.FieldList["Heat" + i + " - MinimumHelicopterSpawnInterval"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - MinimumHelicopterSpawnInterval"].FieldDefault;
                    AiDirectorEntityData.FieldList["Heat" + i + " - MaxHelicoptersPerBubble"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - MaxHelicoptersPerBubble"].FieldDefault;
                    AiDirectorEntityData.FieldList["Heat" + i + " - MinimumRoadblockSpawnInterval"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - MinimumRoadblockSpawnInterval"].FieldDefault;
                    AiDirectorEntityData.FieldList["Heat" + i + " - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = AiDirectorEntityData.FieldList["Heat" + i + " - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].FieldDefault;
                }

                //Now do specific overrides by heat intensity
                int count, lastCount;
                switch ((HeatEnum)index) {
                    case HeatEnum.Cool:
                        for (int i = 1; i <= 10; i++) {
                            //We want to remove one of the strongest cops from each heat
                            foreach (string s in copTypeList.Reverse()) {
                                count = (int)AiDirectorEntityData.FieldList["Heat" + i + " - " + s].FieldDefault;
                                if (count > 1) {
                                    AiDirectorEntityData.FieldList["Heat" + i + " - " + s].Field = count - 1;
                                    break;
                                }
                            }

                            //Also shift various heat values up one heat to relax things a little
                            if (i > 1) {
                                AiDirectorEntityData.FieldList["Heat" + i + " - MinimumHelicopterSpawnInterval"].Field = AiDirectorEntityData.FieldList["Heat" + (i - 1) + " - MinimumHelicopterSpawnInterval"].FieldDefault;
                                AiDirectorEntityData.FieldList["Heat" + i + " - MinimumRoadblockSpawnInterval"].Field = AiDirectorEntityData.FieldList["Heat" + (i - 1) + " - MinimumRoadblockSpawnInterval"].FieldDefault;
                            }
                        }
                        break;
                    case HeatEnum.Normal:
                        //Do nothing, already reset above
                        break;
                    default: //Hot, Very Hot, Blazing
                        AiDirectorEntityData.FieldList["PullAheadHeatThreshold"].Field = 1;
                        AiDirectorEntityData.FieldList["BlockHeatThreshold"].Field = 1;
                        for (int i = 10; i >= 1; i--) {
                            //For each intensity Hot and above, add the strongest cop to each heat
                            for (int j = (int)HeatEnum.Hot; j <= index; j++) {
                                lastCount = 0;
                                foreach (string s in copTypeList) {
                                    //We want these to compound, so use Field instead of FieldDefault - safe as we already reset above
                                    count = (int)AiDirectorEntityData.FieldList["Heat" + i + " - " + s].Field;
                                    if (s == "AdvancedAggressor" || (count == 0 && lastCount > 0)) {
                                        AiDirectorEntityData.FieldList["Heat" + i + " - " + s].Field = count + 1;
                                        break;
                                    }
                                    lastCount = count;
                                }
                            }

                            //We also want to get rid of minimum heat requirements for roadblocks
                            if (i < 10) {
                                //We want these to trickle down heat levels, so use Field instead of FieldDefault - safe as we already reset above
                                if ((float)AiDirectorEntityData.FieldList["Heat" + i + " - MinimumHelicopterSpawnInterval"].FieldDefault == -1f)
                                    AiDirectorEntityData.FieldList["Heat" + i + " - MinimumHelicopterSpawnInterval"].Field = AiDirectorEntityData.FieldList["Heat" + (i + 1) + " - MinimumHelicopterSpawnInterval"].Field;
                                if ((float)AiDirectorEntityData.FieldList["Heat" + i + " - MinimumRoadblockSpawnInterval"].FieldDefault == -1f)
                                    AiDirectorEntityData.FieldList["Heat" + i + " - MinimumRoadblockSpawnInterval"].Field = AiDirectorEntityData.FieldList["Heat" + (i + 1) + " - MinimumRoadblockSpawnInterval"].Field;


                            }

                            //Also tweak spawn times after escape
                            AiDirectorEntityData.FieldList["Heat" + i + " - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop"].Field = 30f - (i - 1) * 3;
                        }

                        //For Blazing, rain down choppers for Heat 10 and let things get weird
                        if (index >= (int)HeatEnum.Blazing)
                            AiDirectorEntityData.FieldList["Heat10 - MaxHelicoptersPerBubble"].Field = 2;
                        break;
                }
            });
        }

        public void UpdateSpikeStripFix(bool useFix) {
            LaunchThread(() => {
                if (CheckThread())
                    return;

                NFSObject SpikestripWeapon;
                if (!TryGetObject("SpikestripWeapon", out SpikestripWeapon))
                    return;
                if (useFix) {
                    SpikestripWeapon.FieldList["Classification"].Field = NFSObjectSpikestripWeapon.VehicleWeaponClassification.VehicleWeaponClassification_BackwardFiring;
                    SpikestripWeapon.FieldList["ConeLength-Low"].Field = 70f;
                    SpikestripWeapon.FieldList["ConeLength-High"].Field = 80f;
                    SpikestripWeapon.FieldList["ConeWidth-Low"].Field = 45f;
                    SpikestripWeapon.FieldList["ConeWidth-High"].Field = 30f;
                    SpikestripWeapon.FieldList["MinimumTriggerDistance-Low"].Field = 2f;
                    SpikestripWeapon.FieldList["MinimumTriggerDistance-High"].Field = 1f;
                }
                else
                    SpikestripWeapon.ResetFieldsToDefault(); //Safe as these are the only things we change
            });
        }
    }
}
