using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        VeryHard = 3
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

    public partial class DifficultyForm : Form {
        public MemoryManager MemManager { get; set; }
        public Dictionary<String, NFSObject> ObjectList { get; protected set; }

        public DifficultyForm() {
            InitializeComponent();

            //Initialize our MemoryManager and object list
            MemManager = new MemoryManager();
            ObjectList = new Dictionary<String, NFSObject>();

            //Start looking for our game
            SetStatus();
            tmrFindProcess.Start();

            //Set defaults -- TODO hook this up to save/load system
            cmbCopDifficulty.SelectedIndex = (int)DifficultyEnum.Skilled;
            cmbRacerDifficulty.SelectedIndex = (int)DifficultyEnum.Skilled;
            cmbCopDensity.SelectedIndex = (int)DensityEnum.Normal;
            cmbRacerDensity.SelectedIndex = (int)DensityEnum.Normal;
            cmbCopHeatIntensity.SelectedIndex = (int)HeatEnum.Normal;
            chkSpikeStripFix.Checked = true;
            chkEqualWeaponUse.Checked = true;
        }

        //Status callbacks
        delegate void SetStatusCallback(string text);
        private void SetStatus(string text = "") {
            if (lblStatus.InvokeRequired) {
                SetStatusCallback d = new SetStatusCallback(SetStatus);
                Invoke(d, new object[] { text });
            }
            else {
                lblStatus.Text = text;
            }
        }

        //Useful functions
        public void ResetAll(bool isClosing = false) {
            SetStatus("Reverting changes...");
            foreach (NFSObject n in ObjectList.Values)
                n.ResetFieldsToDefault();

            SetStatus("Closing nfs14 handle...");
            if (MemManager != null)
                MemManager.CloseHandle();

            SetStatus();

            //If we're not closing, start looking for our process again (e.g. we've found out game has closed but we haven't)
            if (!isClosing)
                tmrFindProcess.Start();
        }

        public NFSObject GetObject(string name) {
            NFSObject type;
            lock (ObjectList) {
                SetStatus("Finding " + name + "...");

                if (ObjectList.ContainsKey(name))
                    type = ObjectList[name];
                else {
                    switch (name) {
                        case "AiDirectorEntityData":
                            type = new NFSObjectAiDirectorEntityData(MemManager); break;
                        case "PacingLibraryEntityData":
                            type = new NFSObjectPacingLibraryEntityData(MemManager); break;
                        case "HealthProfilesListEntityData":
                            type = new NFSObjectHealthProfilesListEntityData(MemManager); break;
                        case "PersonaLibraryPrefab":
                            type = new NFSObjectPersonaLibraryPrefab(MemManager); break;
                        case "GameTime":
                            type = new NFSObjectGameTime(MemManager); break;
                        case "SpikestripWeapon":
                            type = new NFSObjectSpikestripWeapon(MemManager); break;
                        case "HeliSpikestripWeapon":
                            type = new NFSObjectHeliSpikestripWeapon(MemManager); break;
                        default:
                            return null;
                    }
                    ObjectList.Add(name, type);
                }

                SetStatus();
            }

            return type;
        }

        //Form events
        private void DifficultyForm_FormClosing(object sender, FormClosingEventArgs e) {
            ResetAll(true);
        }

        private void tmrFindProcess_Tick(object sender, EventArgs e) {
            SetStatus("Waiting for game...");
            if (!MemManager.OpenProcess("nfs14")) // && !MemManager.OpenProcess("nfs14_x86")
                return;

            //We've found it, no need to keep searching
            SetStatus("Found it!");
            tmrFindProcess.Stop();
        }

        //Save / load / link events
        private void btnSaveSettings_Click(object sender, EventArgs e) {
            //TODO...
        }

        private void btnLoadSettings_Click(object sender, EventArgs e) {
            //TODO...
        }

        private void lnkBitbucket_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            //TODO...
        }

        //Difficulty events
        private void cmbCopDifficulty_SelectedIndexChanged(object sender, EventArgs e) {
            pnlCopCustom.Visible = cmbCopDifficulty.SelectedIndex == (int)DifficultyEnum.Custom;
            switch ((DifficultyEnum)cmbCopDifficulty.SelectedIndex) {
                case DifficultyEnum.Novice:
                    txtCopDifficultyDescription.Text = "They won't crash into you... much.";
                    cmbCopClass.SelectedIndex = (int)ClassEnum.Easy;
                    numCopSkill.Value = 0.3m;
                    break;
                case DifficultyEnum.Average:
                    txtCopDifficultyDescription.Text = "They know how to drift.";
                    cmbCopClass.SelectedIndex = (int)ClassEnum.Easy;
                    numCopSkill.Value = 0.4m;
                    break;
                case DifficultyEnum.Experienced:
                    txtCopDifficultyDescription.Text = "Don't get airy.";
                    cmbCopClass.SelectedIndex = (int)ClassEnum.Normal;
                    numCopSkill.Value = 0.4m;
                    break;
                case DifficultyEnum.Skilled:
                    txtCopDifficultyDescription.Text = "You think you're fast?";
                    cmbCopClass.SelectedIndex = (int)ClassEnum.Normal;
                    numCopSkill.Value = 0.5m;
                    break;
                case DifficultyEnum.Adept:
                    txtCopDifficultyDescription.Text = "You'd better be in gear.";
                    cmbCopClass.SelectedIndex = (int)ClassEnum.Normal;
                    numCopSkill.Value = 0.7m;
                    break;
                case DifficultyEnum.Masterful:
                    txtCopDifficultyDescription.Text = "I hope you like to repair.";
                    cmbCopClass.SelectedIndex = (int)ClassEnum.Hard;
                    numCopSkill.Value = 0.7m;
                    break;
                case DifficultyEnum.Inhuman:
                    txtCopDifficultyDescription.Text = "You're already busted.";
                    cmbCopClass.SelectedIndex = (int)ClassEnum.Hard;
                    numCopSkill.Value = 0.9m;
                    break;
                case DifficultyEnum.Godlike:
                    txtCopDifficultyDescription.Text = "I am the Law and the Omega.";
                    cmbCopClass.SelectedIndex = (int)ClassEnum.Hard;
                    numCopSkill.Value = 1.1m;
                    break;
                case DifficultyEnum.Custom:
                    txtCopDifficultyDescription.Text = "";
                    break;
            }
        }

        private void cmbRacerDifficulty_SelectedIndexChanged(object sender, EventArgs e) {
            pnlRacerCustom.Visible = cmbRacerDifficulty.SelectedIndex == (int)DifficultyEnum.Custom;
            switch ((DifficultyEnum)cmbRacerDifficulty.SelectedIndex) {
                case DifficultyEnum.Novice:
                    txtRacerDifficultyDescription.Text = "They won't crash into you... much.";
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.Easy;
                    numRacerSkill.Value = 0.3m;
                    break;
                case DifficultyEnum.Average:
                    txtRacerDifficultyDescription.Text = "They know how to drift.";
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.Easy;
                    numRacerSkill.Value = 0.4m;
                    break;
                case DifficultyEnum.Experienced:
                    txtRacerDifficultyDescription.Text = "Don't get airy.";
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.Normal;
                    numRacerSkill.Value = 0.4m;
                    break;
                case DifficultyEnum.Skilled:
                    txtRacerDifficultyDescription.Text = "You think you're fast?";
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.Normal;
                    numRacerSkill.Value = 0.5m;
                    break;
                case DifficultyEnum.Adept:
                    txtRacerDifficultyDescription.Text = "You'd better be in gear.";
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.Normal;
                    numRacerSkill.Value = 0.7m;
                    break;
                case DifficultyEnum.Masterful:
                    txtRacerDifficultyDescription.Text = "I hope you like to repair.";
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.Hard;
                    numRacerSkill.Value = 0.7m;
                    break;
                case DifficultyEnum.Inhuman:
                    txtRacerDifficultyDescription.Text = "You're already wrecked.";
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.Hard;
                    numRacerSkill.Value = 0.9m;
                    break;
                case DifficultyEnum.Godlike:
                    txtRacerDifficultyDescription.Text = "I am not you, and also the Omega.";
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.VeryHard;
                    numRacerSkill.Value = 1.1m;
                    break;
                case DifficultyEnum.Custom:
                    txtRacerDifficultyDescription.Text = "";
                    break;
            }
        }

        //Class events
        private void cmbCopClass_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void cmbRacerClass_SelectedIndexChanged(object sender, EventArgs e) {

        }

        //Skill events
        private void numCopSkill_ValueChanged(object sender, EventArgs e) {

        }

        private void numRacerSkill_ValueChanged(object sender, EventArgs e) {

        }

        //Density events
        private void cmbCopDensity_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void cmbRacerDensity_SelectedIndexChanged(object sender, EventArgs e) {

        }

        //Other events
        private void cmbCopHeatIntensity_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void chkSpikeStripFix_CheckedChanged(object sender, EventArgs e) {

        }

        private void chkEqualWeaponUse_CheckedChanged(object sender, EventArgs e) {

        }
    }
}
