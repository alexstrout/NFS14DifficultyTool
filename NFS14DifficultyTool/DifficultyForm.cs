using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NFS14DifficultyTool {
    public partial class DifficultyForm : Form {
        protected DifficultyFormWorker worker;
        protected List<string> statusList;

        public DifficultyFormTimers Timers { get; protected set; }

        public DifficultyForm() {
            InitializeComponent();

            //Initialize our worker
            worker = new DifficultyFormWorker(this);
            statusList = new List<string>();

            //Initialize our timers
            Timers = new DifficultyFormTimers(this, worker);
            SetStatus();

            //Load previous settings, or set defaults if none present
            //TODO Add "Defaults" button to reset defaults at any time, will need to shuffle UI around
            if (System.IO.File.Exists("Settings.ini"))
                LoadSettings("Settings.ini");
            else {
                cmbCopDifficulty.SelectedIndex = (int)DifficultyEnum.Adept;
                cmbRacerDifficulty.SelectedIndex = (int)DifficultyEnum.Skilled;
                cmbCopDensity.SelectedIndex = (int)DensityEnum.Normal;
                cmbRacerDensity.SelectedIndex = (int)DensityEnum.Normal;
                numCopMinHeat.Value = 1;
                cmbCopHeatIntensity.SelectedIndex = (int)HeatEnum.Normal;
                chkSpikeStripFix.Checked = true;
                chkEqualWeaponUse.Checked = true;
            }
        }

        //Status callbacks
        protected delegate void SetStatusCallback(string text, bool preserveStatusList);
        public void SetStatus(string text = "Ready...", bool preserveStatusList = false) {
            if (!preserveStatusList)
                lock (statusList)
                    statusList.Clear();
            if (txtStatus.InvokeRequired) {
                try {
                    Invoke(new SetStatusCallback(SetStatus), new object[] { text, preserveStatusList });
                }
                catch (ObjectDisposedException) {
                    //May happen as threads are aborting, don't care
                }
            }
            else
                txtStatus.Text = text;
        }
        public void PushStatus(string text) {
            lock (statusList)
                statusList.Add(text);
            SetStatus(text, true);
        }
        public void PopStatus(string text) {
            lock (statusList)
                statusList.Remove(text);
            PopStatus();
        }
        public void PopStatus() {
            string status = null;
            lock (statusList)
                if (statusList.Count > 0)
                    status = statusList[0];
            if (status != null)
                SetStatus(status, true);
            else
                SetStatus();
        }

        //Timer callbacks
        public void ApplyAllSettings() {
            cmbCopClass_SelectedIndexChanged(null, null);
            cmbRacerClass_SelectedIndexChanged(null, null);
            numCopSkill_ValueChanged(null, null);
            numRacerSkill_ValueChanged(null, null);
            cmbCopDensity_SelectedIndexChanged(null, null);
            cmbRacerDensity_SelectedIndexChanged(null, null);
            numCopMinHeat_ValueChanged(null, null);
            cmbCopHeatIntensity_SelectedIndexChanged(null, null);
            chkSpikeStripFix_CheckedChanged(null, null);
            //chkEqualWeaponUse_CheckedChanged(null, null); //Not needed, only calls cmb[Cop/Racer]Class_SelectedIndexChanged()
        }

        public void ValidatePublicGameOptions() {
            ComboBox[] cmbs = { cmbCopDifficulty, cmbRacerDifficulty, cmbCopDensity, cmbRacerDensity, cmbCopHeatIntensity };
            foreach (ComboBox cmb in cmbs) {
                int index = cmb.SelectedIndex;
                int direction = -1;
                while (cmb.Items[index].ToString().Contains("*")) {
                    if (index == 0)
                        direction = 1;
                    index += direction;
                }
                cmb.SelectedIndex = index;
            }
        }

        //Form events
        private void DifficultyForm_FormClosing(object sender, FormClosingEventArgs e) {
            SaveSettings("Settings.ini");
            worker.ResetAll(true);
        }

        //Save / load / link events
        private void btnSaveSettings_Click(object sender, EventArgs e) {
            if (dlgSaveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SaveSettings(dlgSaveFile.FileName);
        }
        private void SaveSettings(string filePath) {
            INIParser ini = new INIParser();
            ini.SetValue("Settings", "CopDifficulty", cmbCopDifficulty.SelectedIndex);
            if (cmbCopDifficulty.SelectedIndex == (int)DifficultyEnum.Custom) {
                ini.SetValue("Settings", "CopClass", cmbCopClass.SelectedIndex);
                ini.SetValue("Settings", "CopSkill", numCopSkill.Value);
            }

            ini.SetValue("Settings", "RacerDifficulty", cmbRacerDifficulty.SelectedIndex);
            if (cmbRacerDifficulty.SelectedIndex == (int)DifficultyEnum.Custom) {
                ini.SetValue("Settings", "RacerClass", cmbRacerClass.SelectedIndex);
                ini.SetValue("Settings", "RacerSkill", numRacerSkill.Value);
            }

            ini.SetValue("Settings", "CopDensity", cmbCopDensity.SelectedIndex);
            ini.SetValue("Settings", "RacerDensity", cmbRacerDensity.SelectedIndex);

            ini.SetValue("Settings", "CopMinHeat", numCopMinHeat.Value);
            ini.SetValue("Settings", "CopHeatIntensity", cmbCopHeatIntensity.SelectedIndex);

            ini.SetValue("Settings", "SpikeStripFix", chkSpikeStripFix.Checked);
            ini.SetValue("Settings", "EqualWeaponUse", chkEqualWeaponUse.Checked);

            try {
                ini.WriteFile(filePath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Save Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLoadSettings_Click(object sender, EventArgs e) {
            if (dlgOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadSettings(dlgOpenFile.FileName);
        }
        private bool LoadSettings(string filePath) {
            INIParser ini = new INIParser();
            try {
                ini.ReadFile(filePath);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Load Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int iField;
            decimal dField;
            if (int.TryParse(ini.GetValue("Settings", "CopDifficulty"), out iField))
                cmbCopDifficulty.SelectedIndex = iField;
            if (cmbCopDifficulty.SelectedIndex == (int)DifficultyEnum.Custom) {
                if (int.TryParse(ini.GetValue("Settings", "CopClass"), out iField))
                    cmbCopClass.SelectedIndex = iField;
                if (decimal.TryParse(ini.GetValue("Settings", "CopSkill"), out dField))
                    numCopSkill.Value = dField;
            }

            if (int.TryParse(ini.GetValue("Settings", "RacerDifficulty"), out iField))
                cmbRacerDifficulty.SelectedIndex = iField;
            if (cmbRacerDifficulty.SelectedIndex == (int)DifficultyEnum.Custom) {
                if (int.TryParse(ini.GetValue("Settings", "RacerClass"), out iField))
                    cmbRacerClass.SelectedIndex = iField;
                if (decimal.TryParse(ini.GetValue("Settings", "RacerSkill"), out dField))
                    numRacerSkill.Value = dField;
            }

            if (int.TryParse(ini.GetValue("Settings", "CopDensity"), out iField))
                cmbCopDensity.SelectedIndex = iField;
            if (int.TryParse(ini.GetValue("Settings", "RacerDensity"), out iField))
                cmbRacerDensity.SelectedIndex = iField;

            if (int.TryParse(ini.GetValue("Settings", "CopMinHeat"), out iField))
                numCopMinHeat.Value = iField;
            if (int.TryParse(ini.GetValue("Settings", "CopHeatIntensity"), out iField))
                cmbCopHeatIntensity.SelectedIndex = iField;

            bool bField;
            if (bool.TryParse(ini.GetValue("Settings", "SpikeStripFix"), out bField))
                chkSpikeStripFix.Checked = bField;
            if (bool.TryParse(ini.GetValue("Settings", "EqualWeaponUse"), out bField))
                chkEqualWeaponUse.Checked = bField;

            return true;
        }

        private void lnkProjectPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Cursor.Current = Cursors.AppStarting;
            try {
                System.Diagnostics.Process.Start(lnkProjectPage.Text);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "URL Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Cursor.Current = Cursors.Default;
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
                    txtCopDifficultyDescription.Text = "Don't get cocky.";
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
                    numCopSkill.Value = 1.0m;
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
                    txtRacerDifficultyDescription.Text = "Don't get cocky.";
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
                    numRacerSkill.Value = 1.0m;
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
        private void numCopMinHeat_ValueChanged(object sender, EventArgs e) {
            worker.UpdateCopMinHeat((int)numCopMinHeat.Value);
        }

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
