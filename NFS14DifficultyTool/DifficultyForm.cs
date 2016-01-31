using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace NFS14DifficultyTool {
    public partial class DifficultyForm : Form {
        protected DifficultyFormWorker worker;

        public DifficultyForm() {
            InitializeComponent();

            //Initialize our worker
            worker = new DifficultyFormWorker(this);

            //Start looking for our game
            SetStatus();
            FindProcessTimer.Start();

            //Set defaults -- TODO hook this up to save/load system
            cmbCopDifficulty.SelectedIndex = (int)DifficultyEnum.Adept;
            cmbRacerDifficulty.SelectedIndex = (int)DifficultyEnum.Skilled;
            cmbCopDensity.SelectedIndex = (int)DensityEnum.Normal;
            cmbRacerDensity.SelectedIndex = (int)DensityEnum.Normal;
            cmbCopHeatIntensity.SelectedIndex = (int)HeatEnum.Normal;
            chkSpikeStripFix.Checked = true;
            chkEqualWeaponUse.Checked = true;
        }

        //Status callbacks
        delegate void SetStatusCallback(string text);
        public void SetStatus(string text = "Ready...") {
            if (txtStatus.InvokeRequired) {
                SetStatusCallback d = new SetStatusCallback(SetStatus);
                try {
                    Invoke(d, new object[] { text });
                }
                catch (Exception) {
                    //May happen as threads are aborting, don't care
                }
            }
            else {
                txtStatus.Text = text;
            }
        }

        //Form events
        private void DifficultyForm_FormClosing(object sender, FormClosingEventArgs e) {
            worker.ResetAll(true);
        }

        private void FindProcessTimer_Tick(object sender, EventArgs e) {
            if (!worker.MemManager.ProcessOpen) {
                SetStatus("Waiting for game...");
                if (!worker.MemManager.OpenProcess("nfs14")) // && !MemManager.OpenProcess("nfs14_x86")
                    return;

                //Found it, we can slow down our checks
                SetStatus("Found it!");
                FindProcessTimer.Interval = 10000;
            }

            //Wait until we're ready to go
            if (!worker.CheckIfReady())
                return;

            //All set! No use for timer now (until we have an error or something at least)
            FindProcessTimer.Stop();

            //Fire off the rest of the settings events now that we're ready
            cmbCopClass_SelectedIndexChanged(null, null);
            cmbRacerClass_SelectedIndexChanged(null, null);
            numCopSkill_ValueChanged(null, null);
            numRacerSkill_ValueChanged(null, null);
            cmbCopDensity_SelectedIndexChanged(null, null);
            cmbRacerDensity_SelectedIndexChanged(null, null);
            cmbCopHeatIntensity_SelectedIndexChanged(null, null);
            chkSpikeStripFix_CheckedChanged(null, null);
            //chkEqualWeaponUse_CheckedChanged(null, null); //Not needed, only calls cmb[Cop/Racer]Class_SelectedIndexChanged()
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
                default: //Custom
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
                    cmbRacerClass.SelectedIndex = (int)ClassEnum.AroundTheWorld;
                    numRacerSkill.Value = 1.1m;
                    break;
                default: //Custom
                    txtRacerDifficultyDescription.Text = "";
                    break;
            }
        }

        //Class events
        private void cmbCopClass_SelectedIndexChanged(object sender, EventArgs e) {
            worker.UpdateCopClass(cmbCopClass.SelectedIndex, chkEqualWeaponUse.Checked);
        }

        private void cmbRacerClass_SelectedIndexChanged(object sender, EventArgs e) {
            worker.UpdateRacerClass(cmbRacerClass.SelectedIndex, chkEqualWeaponUse.Checked);
        }

        //Skill events
        private void numCopSkill_ValueChanged(object sender, EventArgs e) {
            worker.UpdateCopSkill((float)numCopSkill.Value);
        }

        private void numRacerSkill_ValueChanged(object sender, EventArgs e) {
            worker.UpdateRacerSkill((float)numRacerSkill.Value);
        }

        //Density events
        private void cmbCopDensity_SelectedIndexChanged(object sender, EventArgs e) {
            worker.UpdateCopDensity(cmbCopDensity.SelectedIndex);
        }

        private void cmbRacerDensity_SelectedIndexChanged(object sender, EventArgs e) {
            worker.UpdateRacerDensity(cmbRacerDensity.SelectedIndex);
        }

        //Other events
        private void cmbCopHeatIntensity_SelectedIndexChanged(object sender, EventArgs e) {
            switch ((HeatEnum)cmbCopHeatIntensity.SelectedIndex) {
                case HeatEnum.Cool:
                    txtCopHeatIntensityDescription.Text = "A slightly less agressive experience. One less cop per heat and higher requirements for roadblocks and helicopters."; break;
                case HeatEnum.Normal:
                    txtCopHeatIntensityDescription.Text = "The vanilla police chase experience. Recommended when combined with higher AI difficulty levels."; break;
                case HeatEnum.Hot:
                    txtCopHeatIntensityDescription.Text = "Hot! A single additional cop per heat, with each cop tier introduced one heat early."; break;
                case HeatEnum.VeryHot:
                    txtCopHeatIntensityDescription.Text = "Very hot! Two additional cops per heat, with each cop tier introduced two heats early."; break;
                case HeatEnum.Blazing:
                    txtCopHeatIntensityDescription.Text = "Blazing! Three additional cops per heat, with each cop tier introduced three heats early. Two helicopters on Heat 10."; break;
            }

            worker.UpdateCopHeatIntensity(cmbCopHeatIntensity.SelectedIndex);
        }

        private void chkSpikeStripFix_CheckedChanged(object sender, EventArgs e) {
            worker.UpdateSpikeStripFix(chkSpikeStripFix.Checked);
        }

        private void chkEqualWeaponUse_CheckedChanged(object sender, EventArgs e) {
            worker.UpdateEqualWeaponUse(chkEqualWeaponUse.Checked);
        }
    }
}
