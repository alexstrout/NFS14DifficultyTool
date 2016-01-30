namespace NFS14DifficultyTool {
    partial class DifficultyForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.lblRacerClass = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblRacerSkill = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.lblRacerDensity = new System.Windows.Forms.Label();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.cmbRacerClass = new System.Windows.Forms.ComboBox();
            this.grpMiscSettings = new System.Windows.Forms.GroupBox();
            this.chkSpikeStripFix = new System.Windows.Forms.CheckBox();
            this.chkEqualWeaponUse = new System.Windows.Forms.CheckBox();
            this.numRacerSkill = new System.Windows.Forms.NumericUpDown();
            this.grpRacerSettings = new System.Windows.Forms.GroupBox();
            this.cmbRacerDensity = new System.Windows.Forms.ComboBox();
            this.pnlRacerCustom = new System.Windows.Forms.Panel();
            this.txtRacerDifficultyDescription = new System.Windows.Forms.TextBox();
            this.lblRacerDifficulty = new System.Windows.Forms.Label();
            this.cmbRacerDifficulty = new System.Windows.Forms.ComboBox();
            this.grpCopSettings = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblCopHeatIntensity = new System.Windows.Forms.Label();
            this.cmbCopHeatIntensity = new System.Windows.Forms.ComboBox();
            this.lblCopDensity = new System.Windows.Forms.Label();
            this.cmbCopDensity = new System.Windows.Forms.ComboBox();
            this.pnlCopCustom = new System.Windows.Forms.Panel();
            this.lblCopSkill = new System.Windows.Forms.Label();
            this.lblCopClass = new System.Windows.Forms.Label();
            this.cmbCopClass = new System.Windows.Forms.ComboBox();
            this.numCopSkill = new System.Windows.Forms.NumericUpDown();
            this.txtCopDifficultyDescription = new System.Windows.Forms.TextBox();
            this.lblCopDifficulty = new System.Windows.Forms.Label();
            this.cmbCopDifficulty = new System.Windows.Forms.ComboBox();
            this.lblInfoName = new System.Windows.Forms.Label();
            this.lnkBitbucket = new System.Windows.Forms.LinkLabel();
            this.tmrFindProcess = new System.Windows.Forms.Timer(this.components);
            this.grpMiscSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRacerSkill)).BeginInit();
            this.grpRacerSettings.SuspendLayout();
            this.pnlRacerCustom.SuspendLayout();
            this.grpCopSettings.SuspendLayout();
            this.pnlCopCustom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCopSkill)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRacerClass
            // 
            this.lblRacerClass.AutoSize = true;
            this.lblRacerClass.Location = new System.Drawing.Point(3, 6);
            this.lblRacerClass.Name = "lblRacerClass";
            this.lblRacerClass.Size = new System.Drawing.Size(48, 13);
            this.lblRacerClass.TabIndex = 4;
            this.lblRacerClass.Text = "AI Class:";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 388);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(46, 13);
            this.lblStatus.TabIndex = 21;
            this.lblStatus.Text = "Status...";
            // 
            // lblRacerSkill
            // 
            this.lblRacerSkill.AutoSize = true;
            this.lblRacerSkill.Location = new System.Drawing.Point(134, 6);
            this.lblRacerSkill.Name = "lblRacerSkill";
            this.lblRacerSkill.Size = new System.Drawing.Size(42, 13);
            this.lblRacerSkill.TabIndex = 5;
            this.lblRacerSkill.Text = "AI Skill:";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Enabled = false;
            this.btnSaveSettings.Location = new System.Drawing.Point(170, 383);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSettings.TabIndex = 20;
            this.btnSaveSettings.Text = "Save As...";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // lblRacerDensity
            // 
            this.lblRacerDensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRacerDensity.AutoSize = true;
            this.lblRacerDensity.Location = new System.Drawing.Point(110, 95);
            this.lblRacerDensity.Name = "lblRacerDensity";
            this.lblRacerDensity.Size = new System.Drawing.Size(77, 13);
            this.lblRacerDensity.TabIndex = 6;
            this.lblRacerDensity.Text = "Racer Density:";
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSettings.Enabled = false;
            this.btnLoadSettings.Location = new System.Drawing.Point(251, 383);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(75, 23);
            this.btnLoadSettings.TabIndex = 19;
            this.btnLoadSettings.Text = "Load...";
            this.btnLoadSettings.UseVisualStyleBackColor = true;
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // cmbRacerClass
            // 
            this.cmbRacerClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRacerClass.FormattingEnabled = true;
            this.cmbRacerClass.Items.AddRange(new object[] {
            "Easy",
            "Normal",
            "Hard",
            "V. Hard"});
            this.cmbRacerClass.Location = new System.Drawing.Point(57, 3);
            this.cmbRacerClass.Name = "cmbRacerClass";
            this.cmbRacerClass.Size = new System.Drawing.Size(61, 21);
            this.cmbRacerClass.TabIndex = 3;
            this.cmbRacerClass.SelectedIndexChanged += new System.EventHandler(this.cmbRacerClass_SelectedIndexChanged);
            // 
            // grpMiscSettings
            // 
            this.grpMiscSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMiscSettings.Controls.Add(this.chkSpikeStripFix);
            this.grpMiscSettings.Controls.Add(this.chkEqualWeaponUse);
            this.grpMiscSettings.Location = new System.Drawing.Point(12, 335);
            this.grpMiscSettings.Name = "grpMiscSettings";
            this.grpMiscSettings.Size = new System.Drawing.Size(320, 42);
            this.grpMiscSettings.TabIndex = 18;
            this.grpMiscSettings.TabStop = false;
            this.grpMiscSettings.Text = "Misc Settings";
            // 
            // chkSpikeStripFix
            // 
            this.chkSpikeStripFix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSpikeStripFix.AutoSize = true;
            this.chkSpikeStripFix.Location = new System.Drawing.Point(79, 19);
            this.chkSpikeStripFix.Name = "chkSpikeStripFix";
            this.chkSpikeStripFix.Size = new System.Drawing.Size(117, 17);
            this.chkSpikeStripFix.TabIndex = 1;
            this.chkSpikeStripFix.Text = "AI Use Spike Strips";
            this.chkSpikeStripFix.UseVisualStyleBackColor = true;
            this.chkSpikeStripFix.CheckedChanged += new System.EventHandler(this.chkSpikeStripFix_CheckedChanged);
            // 
            // chkEqualWeaponUse
            // 
            this.chkEqualWeaponUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEqualWeaponUse.AutoSize = true;
            this.chkEqualWeaponUse.Location = new System.Drawing.Point(202, 19);
            this.chkEqualWeaponUse.Name = "chkEqualWeaponUse";
            this.chkEqualWeaponUse.Size = new System.Drawing.Size(112, 17);
            this.chkEqualWeaponUse.TabIndex = 0;
            this.chkEqualWeaponUse.Text = "AI Target Other AI";
            this.chkEqualWeaponUse.UseVisualStyleBackColor = true;
            this.chkEqualWeaponUse.CheckedChanged += new System.EventHandler(this.chkEqualWeaponUse_CheckedChanged);
            // 
            // numRacerSkill
            // 
            this.numRacerSkill.DecimalPlaces = 2;
            this.numRacerSkill.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numRacerSkill.Location = new System.Drawing.Point(182, 4);
            this.numRacerSkill.Name = "numRacerSkill";
            this.numRacerSkill.Size = new System.Drawing.Size(60, 20);
            this.numRacerSkill.TabIndex = 2;
            this.numRacerSkill.ValueChanged += new System.EventHandler(this.numRacerSkill_ValueChanged);
            // 
            // grpRacerSettings
            // 
            this.grpRacerSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpRacerSettings.Controls.Add(this.lblRacerDensity);
            this.grpRacerSettings.Controls.Add(this.cmbRacerDensity);
            this.grpRacerSettings.Controls.Add(this.pnlRacerCustom);
            this.grpRacerSettings.Controls.Add(this.txtRacerDifficultyDescription);
            this.grpRacerSettings.Controls.Add(this.lblRacerDifficulty);
            this.grpRacerSettings.Controls.Add(this.cmbRacerDifficulty);
            this.grpRacerSettings.Location = new System.Drawing.Point(12, 210);
            this.grpRacerSettings.Name = "grpRacerSettings";
            this.grpRacerSettings.Size = new System.Drawing.Size(320, 119);
            this.grpRacerSettings.TabIndex = 17;
            this.grpRacerSettings.TabStop = false;
            this.grpRacerSettings.Text = "Racer AI Settings";
            // 
            // cmbRacerDensity
            // 
            this.cmbRacerDensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRacerDensity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRacerDensity.FormattingEnabled = true;
            this.cmbRacerDensity.Items.AddRange(new object[] {
            "None (Event Only)",
            "Low",
            "Normal",
            "High",
            "Very High *"});
            this.cmbRacerDensity.Location = new System.Drawing.Point(193, 92);
            this.cmbRacerDensity.Name = "cmbRacerDensity";
            this.cmbRacerDensity.Size = new System.Drawing.Size(121, 21);
            this.cmbRacerDensity.TabIndex = 5;
            this.cmbRacerDensity.SelectedIndexChanged += new System.EventHandler(this.cmbRacerDensity_SelectedIndexChanged);
            // 
            // pnlRacerCustom
            // 
            this.pnlRacerCustom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlRacerCustom.Controls.Add(this.lblRacerSkill);
            this.pnlRacerCustom.Controls.Add(this.lblRacerClass);
            this.pnlRacerCustom.Controls.Add(this.cmbRacerClass);
            this.pnlRacerCustom.Controls.Add(this.numRacerSkill);
            this.pnlRacerCustom.Location = new System.Drawing.Point(38, 52);
            this.pnlRacerCustom.Name = "pnlRacerCustom";
            this.pnlRacerCustom.Size = new System.Drawing.Size(245, 27);
            this.pnlRacerCustom.TabIndex = 4;
            // 
            // txtRacerDifficultyDescription
            // 
            this.txtRacerDifficultyDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRacerDifficultyDescription.Enabled = false;
            this.txtRacerDifficultyDescription.Location = new System.Drawing.Point(6, 46);
            this.txtRacerDifficultyDescription.Multiline = true;
            this.txtRacerDifficultyDescription.Name = "txtRacerDifficultyDescription";
            this.txtRacerDifficultyDescription.Size = new System.Drawing.Size(308, 40);
            this.txtRacerDifficultyDescription.TabIndex = 3;
            this.txtRacerDifficultyDescription.Text = "Sample Difficulty: Prepare to be difficulty\'d!";
            // 
            // lblRacerDifficulty
            // 
            this.lblRacerDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRacerDifficulty.AutoSize = true;
            this.lblRacerDifficulty.Location = new System.Drawing.Point(105, 22);
            this.lblRacerDifficulty.Name = "lblRacerDifficulty";
            this.lblRacerDifficulty.Size = new System.Drawing.Size(82, 13);
            this.lblRacerDifficulty.TabIndex = 1;
            this.lblRacerDifficulty.Text = "Racer Difficulty:";
            // 
            // cmbRacerDifficulty
            // 
            this.cmbRacerDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRacerDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRacerDifficulty.FormattingEnabled = true;
            this.cmbRacerDifficulty.Items.AddRange(new object[] {
            "Novice",
            "Average",
            "Experienced",
            "Skilled",
            "Adept",
            "Masterful",
            "Inhuman",
            "Godlike",
            "(Custom...) *"});
            this.cmbRacerDifficulty.Location = new System.Drawing.Point(193, 19);
            this.cmbRacerDifficulty.Name = "cmbRacerDifficulty";
            this.cmbRacerDifficulty.Size = new System.Drawing.Size(121, 21);
            this.cmbRacerDifficulty.TabIndex = 0;
            this.cmbRacerDifficulty.SelectedIndexChanged += new System.EventHandler(this.cmbRacerDifficulty_SelectedIndexChanged);
            // 
            // grpCopSettings
            // 
            this.grpCopSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCopSettings.Controls.Add(this.textBox1);
            this.grpCopSettings.Controls.Add(this.lblCopHeatIntensity);
            this.grpCopSettings.Controls.Add(this.cmbCopHeatIntensity);
            this.grpCopSettings.Controls.Add(this.lblCopDensity);
            this.grpCopSettings.Controls.Add(this.cmbCopDensity);
            this.grpCopSettings.Controls.Add(this.pnlCopCustom);
            this.grpCopSettings.Controls.Add(this.txtCopDifficultyDescription);
            this.grpCopSettings.Controls.Add(this.lblCopDifficulty);
            this.grpCopSettings.Controls.Add(this.cmbCopDifficulty);
            this.grpCopSettings.Location = new System.Drawing.Point(12, 12);
            this.grpCopSettings.Name = "grpCopSettings";
            this.grpCopSettings.Size = new System.Drawing.Size(320, 192);
            this.grpCopSettings.TabIndex = 16;
            this.grpCopSettings.TabStop = false;
            this.grpCopSettings.Text = "Cop AI Settings";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(6, 146);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(308, 40);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "Sample Heat: Prepate to be mildly warm!";
            // 
            // lblCopHeatIntensity
            // 
            this.lblCopHeatIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopHeatIntensity.AutoSize = true;
            this.lblCopHeatIntensity.Location = new System.Drawing.Point(112, 122);
            this.lblCopHeatIntensity.Name = "lblCopHeatIntensity";
            this.lblCopHeatIntensity.Size = new System.Drawing.Size(75, 13);
            this.lblCopHeatIntensity.TabIndex = 8;
            this.lblCopHeatIntensity.Text = "Heat Intensity:";
            // 
            // cmbCopHeatIntensity
            // 
            this.cmbCopHeatIntensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCopHeatIntensity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCopHeatIntensity.FormattingEnabled = true;
            this.cmbCopHeatIntensity.Items.AddRange(new object[] {
            "Cool",
            "Normal",
            "Hot",
            "Very Hot",
            "Blazing"});
            this.cmbCopHeatIntensity.Location = new System.Drawing.Point(193, 119);
            this.cmbCopHeatIntensity.Name = "cmbCopHeatIntensity";
            this.cmbCopHeatIntensity.Size = new System.Drawing.Size(121, 21);
            this.cmbCopHeatIntensity.TabIndex = 7;
            this.cmbCopHeatIntensity.SelectedIndexChanged += new System.EventHandler(this.cmbCopHeatIntensity_SelectedIndexChanged);
            // 
            // lblCopDensity
            // 
            this.lblCopDensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopDensity.AutoSize = true;
            this.lblCopDensity.Location = new System.Drawing.Point(120, 95);
            this.lblCopDensity.Name = "lblCopDensity";
            this.lblCopDensity.Size = new System.Drawing.Size(67, 13);
            this.lblCopDensity.TabIndex = 6;
            this.lblCopDensity.Text = "Cop Density:";
            // 
            // cmbCopDensity
            // 
            this.cmbCopDensity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCopDensity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCopDensity.FormattingEnabled = true;
            this.cmbCopDensity.Items.AddRange(new object[] {
            "None (Event Only) *",
            "Low",
            "Normal",
            "High",
            "Very High *",
            "Most Wanted *"});
            this.cmbCopDensity.Location = new System.Drawing.Point(193, 92);
            this.cmbCopDensity.Name = "cmbCopDensity";
            this.cmbCopDensity.Size = new System.Drawing.Size(121, 21);
            this.cmbCopDensity.TabIndex = 5;
            this.cmbCopDensity.SelectedIndexChanged += new System.EventHandler(this.cmbCopDensity_SelectedIndexChanged);
            // 
            // pnlCopCustom
            // 
            this.pnlCopCustom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlCopCustom.Controls.Add(this.lblCopSkill);
            this.pnlCopCustom.Controls.Add(this.lblCopClass);
            this.pnlCopCustom.Controls.Add(this.cmbCopClass);
            this.pnlCopCustom.Controls.Add(this.numCopSkill);
            this.pnlCopCustom.Location = new System.Drawing.Point(38, 52);
            this.pnlCopCustom.Name = "pnlCopCustom";
            this.pnlCopCustom.Size = new System.Drawing.Size(245, 27);
            this.pnlCopCustom.TabIndex = 4;
            // 
            // lblCopSkill
            // 
            this.lblCopSkill.AutoSize = true;
            this.lblCopSkill.Location = new System.Drawing.Point(134, 6);
            this.lblCopSkill.Name = "lblCopSkill";
            this.lblCopSkill.Size = new System.Drawing.Size(42, 13);
            this.lblCopSkill.TabIndex = 5;
            this.lblCopSkill.Text = "AI Skill:";
            // 
            // lblCopClass
            // 
            this.lblCopClass.AutoSize = true;
            this.lblCopClass.Location = new System.Drawing.Point(3, 6);
            this.lblCopClass.Name = "lblCopClass";
            this.lblCopClass.Size = new System.Drawing.Size(48, 13);
            this.lblCopClass.TabIndex = 4;
            this.lblCopClass.Text = "AI Class:";
            // 
            // cmbCopClass
            // 
            this.cmbCopClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCopClass.FormattingEnabled = true;
            this.cmbCopClass.Items.AddRange(new object[] {
            "Easy",
            "Normal",
            "Hard"});
            this.cmbCopClass.Location = new System.Drawing.Point(57, 3);
            this.cmbCopClass.Name = "cmbCopClass";
            this.cmbCopClass.Size = new System.Drawing.Size(61, 21);
            this.cmbCopClass.TabIndex = 3;
            this.cmbCopClass.SelectedIndexChanged += new System.EventHandler(this.cmbCopClass_SelectedIndexChanged);
            // 
            // numCopSkill
            // 
            this.numCopSkill.DecimalPlaces = 2;
            this.numCopSkill.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numCopSkill.Location = new System.Drawing.Point(182, 4);
            this.numCopSkill.Name = "numCopSkill";
            this.numCopSkill.Size = new System.Drawing.Size(60, 20);
            this.numCopSkill.TabIndex = 2;
            this.numCopSkill.ValueChanged += new System.EventHandler(this.numCopSkill_ValueChanged);
            // 
            // txtCopDifficultyDescription
            // 
            this.txtCopDifficultyDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCopDifficultyDescription.Enabled = false;
            this.txtCopDifficultyDescription.Location = new System.Drawing.Point(6, 46);
            this.txtCopDifficultyDescription.Multiline = true;
            this.txtCopDifficultyDescription.Name = "txtCopDifficultyDescription";
            this.txtCopDifficultyDescription.Size = new System.Drawing.Size(308, 40);
            this.txtCopDifficultyDescription.TabIndex = 3;
            this.txtCopDifficultyDescription.Text = "Sample Difficulty: Prepare to be difficulty\'d!";
            // 
            // lblCopDifficulty
            // 
            this.lblCopDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopDifficulty.AutoSize = true;
            this.lblCopDifficulty.Location = new System.Drawing.Point(115, 22);
            this.lblCopDifficulty.Name = "lblCopDifficulty";
            this.lblCopDifficulty.Size = new System.Drawing.Size(72, 13);
            this.lblCopDifficulty.TabIndex = 1;
            this.lblCopDifficulty.Text = "Cop Difficulty:";
            // 
            // cmbCopDifficulty
            // 
            this.cmbCopDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCopDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCopDifficulty.FormattingEnabled = true;
            this.cmbCopDifficulty.Items.AddRange(new object[] {
            "Novice",
            "Average",
            "Experienced",
            "Skilled",
            "Adept",
            "Masterful",
            "Inhuman",
            "Godlike",
            "(Custom...) *"});
            this.cmbCopDifficulty.Location = new System.Drawing.Point(193, 19);
            this.cmbCopDifficulty.Name = "cmbCopDifficulty";
            this.cmbCopDifficulty.Size = new System.Drawing.Size(121, 21);
            this.cmbCopDifficulty.TabIndex = 0;
            this.cmbCopDifficulty.SelectedIndexChanged += new System.EventHandler(this.cmbCopDifficulty_SelectedIndexChanged);
            // 
            // lblInfoName
            // 
            this.lblInfoName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfoName.AutoSize = true;
            this.lblInfoName.Location = new System.Drawing.Point(71, 413);
            this.lblInfoName.Name = "lblInfoName";
            this.lblInfoName.Size = new System.Drawing.Size(261, 13);
            this.lblInfoName.TabIndex = 22;
            this.lblInfoName.Text = "Need for Speed Rivals Difficulty Selector Tool - By fox";
            // 
            // lnkBitbucket
            // 
            this.lnkBitbucket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkBitbucket.AutoSize = true;
            this.lnkBitbucket.Location = new System.Drawing.Point(91, 430);
            this.lnkBitbucket.Name = "lnkBitbucket";
            this.lnkBitbucket.Size = new System.Drawing.Size(241, 13);
            this.lnkBitbucket.TabIndex = 23;
            this.lnkBitbucket.TabStop = true;
            this.lnkBitbucket.Text = "https://bitbucket.org/alexstrout/nfs14difficultytool";
            this.lnkBitbucket.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBitbucket_LinkClicked);
            // 
            // tmrFindProcess
            // 
            this.tmrFindProcess.Interval = 1000;
            this.tmrFindProcess.Tick += new System.EventHandler(this.tmrFindProcess_Tick);
            // 
            // DifficultyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 452);
            this.Controls.Add(this.lnkBitbucket);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.grpMiscSettings);
            this.Controls.Add(this.grpRacerSettings);
            this.Controls.Add(this.grpCopSettings);
            this.Controls.Add(this.lblInfoName);
            this.MinimumSize = new System.Drawing.Size(294, 482);
            this.Name = "DifficultyForm";
            this.Text = "NFS Rivals Difficulty Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DifficultyForm_FormClosing);
            this.grpMiscSettings.ResumeLayout(false);
            this.grpMiscSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRacerSkill)).EndInit();
            this.grpRacerSettings.ResumeLayout(false);
            this.grpRacerSettings.PerformLayout();
            this.pnlRacerCustom.ResumeLayout(false);
            this.pnlRacerCustom.PerformLayout();
            this.grpCopSettings.ResumeLayout(false);
            this.grpCopSettings.PerformLayout();
            this.pnlCopCustom.ResumeLayout(false);
            this.pnlCopCustom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCopSkill)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRacerClass;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblRacerSkill;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Label lblRacerDensity;
        private System.Windows.Forms.Button btnLoadSettings;
        private System.Windows.Forms.ComboBox cmbRacerClass;
        private System.Windows.Forms.GroupBox grpMiscSettings;
        private System.Windows.Forms.CheckBox chkSpikeStripFix;
        private System.Windows.Forms.CheckBox chkEqualWeaponUse;
        private System.Windows.Forms.NumericUpDown numRacerSkill;
        private System.Windows.Forms.GroupBox grpRacerSettings;
        private System.Windows.Forms.ComboBox cmbRacerDensity;
        private System.Windows.Forms.Panel pnlRacerCustom;
        private System.Windows.Forms.TextBox txtRacerDifficultyDescription;
        private System.Windows.Forms.Label lblRacerDifficulty;
        private System.Windows.Forms.ComboBox cmbRacerDifficulty;
        private System.Windows.Forms.GroupBox grpCopSettings;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblCopHeatIntensity;
        private System.Windows.Forms.ComboBox cmbCopHeatIntensity;
        private System.Windows.Forms.Label lblCopDensity;
        private System.Windows.Forms.ComboBox cmbCopDensity;
        private System.Windows.Forms.Panel pnlCopCustom;
        private System.Windows.Forms.Label lblCopSkill;
        private System.Windows.Forms.Label lblCopClass;
        private System.Windows.Forms.ComboBox cmbCopClass;
        private System.Windows.Forms.NumericUpDown numCopSkill;
        private System.Windows.Forms.TextBox txtCopDifficultyDescription;
        private System.Windows.Forms.Label lblCopDifficulty;
        private System.Windows.Forms.ComboBox cmbCopDifficulty;
        private System.Windows.Forms.Label lblInfoName;
        private System.Windows.Forms.LinkLabel lnkBitbucket;
        private System.Windows.Forms.Timer tmrFindProcess;

    }
}