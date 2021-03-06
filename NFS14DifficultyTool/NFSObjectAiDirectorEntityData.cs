﻿namespace NFS14DifficultyTool {
    public class NFSObjectAiDirectorEntityData : NFSObject {
        public NFSObjectAiDirectorEntityData(MemoryManager memManager)
            : base(memManager, "2d774798942db34e960cd083ace16340") {
            //foxAiDirEntData
            FieldList.Add("MaxHeat", new NFSFieldInt(this, "30"));
            FieldList.Add("HeatTime", new NFSFieldInt(this, "34"));
            FieldList.Add("BonusStartingHeat", new NFSFieldInt(this, "38"));
            FieldList.Add("PullAheadHeatThreshold", new NFSFieldInt(this, "3C"));
            FieldList.Add("BlockHeatThreshold", new NFSFieldInt(this, "40"));
            FieldList.Add("SpawnDelay", new NFSFieldFloat(this, "44"));
            FieldList.Add("MaxAiVisibilityDistance", new NFSFieldFloat(this, "48"));
            FieldList.Add("Heat1 - MinHeat", new NFSFieldInt(this, "4CC"));
            FieldList.Add("Heat2 - MinHeat", new NFSFieldInt(this, "4CC+34"));
            FieldList.Add("Heat3 - MinHeat", new NFSFieldInt(this, "4CC+68"));
            FieldList.Add("Heat4 - MinHeat", new NFSFieldInt(this, "4CC+9C"));
            FieldList.Add("Heat5 - MinHeat", new NFSFieldInt(this, "4CC+D0"));
            FieldList.Add("Heat6 - MinHeat", new NFSFieldInt(this, "4CC+104"));
            FieldList.Add("Heat7 - MinHeat", new NFSFieldInt(this, "4CC+138"));
            FieldList.Add("Heat8 - MinHeat", new NFSFieldInt(this, "4CC+16C"));
            FieldList.Add("Heat9 - MinHeat", new NFSFieldInt(this, "4CC+1A0"));
            FieldList.Add("Heat10 - MinHeat", new NFSFieldInt(this, "4CC+1D4"));
            FieldList.Add("Heat1 - MaxHeat", new NFSFieldInt(this, "4D0"));
            FieldList.Add("Heat2 - MaxHeat", new NFSFieldInt(this, "4D0+34"));
            FieldList.Add("Heat3 - MaxHeat", new NFSFieldInt(this, "4D0+68"));
            FieldList.Add("Heat4 - MaxHeat", new NFSFieldInt(this, "4D0+9C"));
            FieldList.Add("Heat5 - MaxHeat", new NFSFieldInt(this, "4D0+D0"));
            FieldList.Add("Heat6 - MaxHeat", new NFSFieldInt(this, "4D0+104"));
            FieldList.Add("Heat7 - MaxHeat", new NFSFieldInt(this, "4D0+138"));
            FieldList.Add("Heat8 - MaxHeat", new NFSFieldInt(this, "4D0+16C"));
            FieldList.Add("Heat9 - MaxHeat", new NFSFieldInt(this, "4D0+1A0"));
            FieldList.Add("Heat10 - MaxHeat", new NFSFieldInt(this, "4D0+1D4"));
            FieldList.Add("Heat1 - CopCountHeatBased", new NFSFieldInt(this, "4D4"));
            FieldList.Add("Heat2 - CopCountHeatBased", new NFSFieldInt(this, "4D4+34"));
            FieldList.Add("Heat3 - CopCountHeatBased", new NFSFieldInt(this, "4D4+68"));
            FieldList.Add("Heat4 - CopCountHeatBased", new NFSFieldInt(this, "4D4+9C"));
            FieldList.Add("Heat5 - CopCountHeatBased", new NFSFieldInt(this, "4D4+D0"));
            FieldList.Add("Heat6 - CopCountHeatBased", new NFSFieldInt(this, "4D4+104"));
            FieldList.Add("Heat7 - CopCountHeatBased", new NFSFieldInt(this, "4D4+138"));
            FieldList.Add("Heat8 - CopCountHeatBased", new NFSFieldInt(this, "4D4+16C"));
            FieldList.Add("Heat9 - CopCountHeatBased", new NFSFieldInt(this, "4D4+1A0"));
            FieldList.Add("Heat10 - CopCountHeatBased", new NFSFieldInt(this, "4D4+1D4"));
            FieldList.Add("Heat1 - Basic", new NFSFieldInt(this, "4D8"));
            FieldList.Add("Heat2 - Basic", new NFSFieldInt(this, "4D8+34"));
            FieldList.Add("Heat3 - Basic", new NFSFieldInt(this, "4D8+68"));
            FieldList.Add("Heat4 - Basic", new NFSFieldInt(this, "4D8+9C"));
            FieldList.Add("Heat5 - Basic", new NFSFieldInt(this, "4D8+D0"));
            FieldList.Add("Heat6 - Basic", new NFSFieldInt(this, "4D8+104"));
            FieldList.Add("Heat7 - Basic", new NFSFieldInt(this, "4D8+138"));
            FieldList.Add("Heat8 - Basic", new NFSFieldInt(this, "4D8+16C"));
            FieldList.Add("Heat9 - Basic", new NFSFieldInt(this, "4D8+1A0"));
            FieldList.Add("Heat10 - Basic", new NFSFieldInt(this, "4D8+1D4"));
            FieldList.Add("Heat1 - Chaser", new NFSFieldInt(this, "4DC"));
            FieldList.Add("Heat2 - Chaser", new NFSFieldInt(this, "4DC+34"));
            FieldList.Add("Heat3 - Chaser", new NFSFieldInt(this, "4DC+68"));
            FieldList.Add("Heat4 - Chaser", new NFSFieldInt(this, "4DC+9C"));
            FieldList.Add("Heat5 - Chaser", new NFSFieldInt(this, "4DC+D0"));
            FieldList.Add("Heat6 - Chaser", new NFSFieldInt(this, "4DC+104"));
            FieldList.Add("Heat7 - Chaser", new NFSFieldInt(this, "4DC+138"));
            FieldList.Add("Heat8 - Chaser", new NFSFieldInt(this, "4DC+16C"));
            FieldList.Add("Heat9 - Chaser", new NFSFieldInt(this, "4DC+1A0"));
            FieldList.Add("Heat10 - Chaser", new NFSFieldInt(this, "4DC+1D4"));
            FieldList.Add("Heat1 - Brute", new NFSFieldInt(this, "4E0"));
            FieldList.Add("Heat2 - Brute", new NFSFieldInt(this, "4E0+34"));
            FieldList.Add("Heat3 - Brute", new NFSFieldInt(this, "4E0+68"));
            FieldList.Add("Heat4 - Brute", new NFSFieldInt(this, "4E0+9C"));
            FieldList.Add("Heat5 - Brute", new NFSFieldInt(this, "4E0+D0"));
            FieldList.Add("Heat6 - Brute", new NFSFieldInt(this, "4E0+104"));
            FieldList.Add("Heat7 - Brute", new NFSFieldInt(this, "4E0+138"));
            FieldList.Add("Heat8 - Brute", new NFSFieldInt(this, "4E0+16C"));
            FieldList.Add("Heat9 - Brute", new NFSFieldInt(this, "4E0+1A0"));
            FieldList.Add("Heat10 - Brute", new NFSFieldInt(this, "4E0+1D4"));
            FieldList.Add("Heat1 - Aggressor", new NFSFieldInt(this, "4E4"));
            FieldList.Add("Heat2 - Aggressor", new NFSFieldInt(this, "4E4+34"));
            FieldList.Add("Heat3 - Aggressor", new NFSFieldInt(this, "4E4+68"));
            FieldList.Add("Heat4 - Aggressor", new NFSFieldInt(this, "4E4+9C"));
            FieldList.Add("Heat5 - Aggressor", new NFSFieldInt(this, "4E4+D0"));
            FieldList.Add("Heat6 - Aggressor", new NFSFieldInt(this, "4E4+104"));
            FieldList.Add("Heat7 - Aggressor", new NFSFieldInt(this, "4E4+138"));
            FieldList.Add("Heat8 - Aggressor", new NFSFieldInt(this, "4E4+16C"));
            FieldList.Add("Heat9 - Aggressor", new NFSFieldInt(this, "4E4+1A0"));
            FieldList.Add("Heat10 - Aggressor", new NFSFieldInt(this, "4E4+1D4"));
            FieldList.Add("Heat1 - AdvancedAggressor", new NFSFieldInt(this, "4E8"));
            FieldList.Add("Heat2 - AdvancedAggressor", new NFSFieldInt(this, "4E8+34"));
            FieldList.Add("Heat3 - AdvancedAggressor", new NFSFieldInt(this, "4E8+68"));
            FieldList.Add("Heat4 - AdvancedAggressor", new NFSFieldInt(this, "4E8+9C"));
            FieldList.Add("Heat5 - AdvancedAggressor", new NFSFieldInt(this, "4E8+D0"));
            FieldList.Add("Heat6 - AdvancedAggressor", new NFSFieldInt(this, "4E8+104"));
            FieldList.Add("Heat7 - AdvancedAggressor", new NFSFieldInt(this, "4E8+138"));
            FieldList.Add("Heat8 - AdvancedAggressor", new NFSFieldInt(this, "4E8+16C"));
            FieldList.Add("Heat9 - AdvancedAggressor", new NFSFieldInt(this, "4E8+1A0"));
            FieldList.Add("Heat10 - AdvancedAggressor", new NFSFieldInt(this, "4E8+1D4"));
            FieldList.Add("Heat1 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC"));
            FieldList.Add("Heat2 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+34"));
            FieldList.Add("Heat3 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+68"));
            FieldList.Add("Heat4 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+9C"));
            FieldList.Add("Heat5 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+D0"));
            FieldList.Add("Heat6 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+104"));
            FieldList.Add("Heat7 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+138"));
            FieldList.Add("Heat8 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+16C"));
            FieldList.Add("Heat9 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+1A0"));
            FieldList.Add("Heat10 - ChanceOfSpawningRoamingCopHeatBased", new NFSFieldInt(this, "4EC+1D4"));
            FieldList.Add("Heat1 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0"));
            FieldList.Add("Heat2 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+34"));
            FieldList.Add("Heat3 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+68"));
            FieldList.Add("Heat4 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+9C"));
            FieldList.Add("Heat5 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+D0"));
            FieldList.Add("Heat6 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+104"));
            FieldList.Add("Heat7 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+138"));
            FieldList.Add("Heat8 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+16C"));
            FieldList.Add("Heat9 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+1A0"));
            FieldList.Add("Heat10 - MinimumHelicopterSpawnInterval", new NFSFieldFloat(this, "4F0+1D4"));
            FieldList.Add("Heat1 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4"));
            FieldList.Add("Heat2 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+34"));
            FieldList.Add("Heat3 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+68"));
            FieldList.Add("Heat4 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+9C"));
            FieldList.Add("Heat5 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+D0"));
            FieldList.Add("Heat6 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+104"));
            FieldList.Add("Heat7 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+138"));
            FieldList.Add("Heat8 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+16C"));
            FieldList.Add("Heat9 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+1A0"));
            FieldList.Add("Heat10 - MaxHelicoptersPerBubble", new NFSFieldInt(this, "4F4+1D4"));
            FieldList.Add("Heat1 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8"));
            FieldList.Add("Heat2 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+34"));
            FieldList.Add("Heat3 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+68"));
            FieldList.Add("Heat4 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+9C"));
            FieldList.Add("Heat5 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+D0"));
            FieldList.Add("Heat6 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+104"));
            FieldList.Add("Heat7 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+138"));
            FieldList.Add("Heat8 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+16C"));
            FieldList.Add("Heat9 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+1A0"));
            FieldList.Add("Heat10 - MinimumRoadblockSpawnInterval", new NFSFieldFloat(this, "4F8+1D4"));
            FieldList.Add("Heat1 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC"));
            FieldList.Add("Heat2 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+34"));
            FieldList.Add("Heat3 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+68"));
            FieldList.Add("Heat4 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+9C"));
            FieldList.Add("Heat5 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+D0"));
            FieldList.Add("Heat6 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+104"));
            FieldList.Add("Heat7 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+138"));
            FieldList.Add("Heat8 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+16C"));
            FieldList.Add("Heat9 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+1A0"));
            FieldList.Add("Heat10 - TimeIntervalAfterSuccessfulEscapeBeforeTryingToSpawnCop", new NFSFieldFloat(this, "4FC+1D4"));
            FieldList.Add("ClientViewSpawnRadius", new NFSFieldFloat(this, "58"));
            FieldList.Add("ClientViewCullRadius", new NFSFieldFloat(this, "5C"));
            FieldList.Add("MaxNumberOfAiOnlySpontaneousRaces", new NFSFieldInt(this, "60"));
            FieldList.Add("NumberOfPawnRacersWanted", new NFSFieldInt(this, "64"));
            FieldList.Add("NumberOfPawnCopsWanted", new NFSFieldInt(this, "68"));
            FieldList.Add("NumberOfRacers", new NFSFieldInt(this, "6C"));
            FieldList.Add("GlobalNumberOfCops", new NFSFieldInt(this, "70"));
            FieldList.Add("GlobalChanceOfSpawningRoamingCop", new NFSFieldInt(this, "74"));
            FieldList.Add("InitialTimeIntervalForTryingToSpawnRacer", new NFSFieldFloat(this, "78"));
            FieldList.Add("InitialTimeIntervalForTryingToSpawnCop", new NFSFieldFloat(this, "7C"));
            FieldList.Add("TimeIntervalForTryingToSpawnRacer", new NFSFieldFloat(this, "80"));
            FieldList.Add("TimeIntervalForTryingToSpawnCop", new NFSFieldFloat(this, "84"));
            FieldList.Add("TimeIntervalForTryingToSpawnCopDuringPursuit", new NFSFieldFloat(this, "88"));
            FieldList.Add("TimeIntervalForTryingToSpawnCopDuringHPRacer", new NFSFieldFloat(this, "8C"));
            FieldList.Add("IsEnabled", new NFSFieldBool(this, "90"));
        }
    }
}
