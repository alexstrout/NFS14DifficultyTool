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

        public Form1() {
            InitializeComponent();

            //TODO TEST
            //btnApply_Click(null, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            ResetAll();
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
                AiDirectorEntityData.FieldList["HeatTime"].Field = 300;
                AiDirectorEntityData.FieldList["BonusStartingHeat"].Field = 6;

                //PacingLibraryPrefab -> PacingLibraryEntityData
                SetStatus("Finding PacingLibraryPrefab -> PacingLibraryEntityData...");
                PacingLibraryEntityData = new NFSPacingLibraryEntityData(MemManager, "706cd7f0bc65284cf0a747f5ec29ce7d");

                //HealthProfilesList -> HealthProfilesListEntityData
                SetStatus("Finding HealthProfilesList -> HealthProfilesListEntityData...");
                HealthProfilesListEntityData = new NFSObjectBlob(MemManager, "6ef1bfcc79f73ef1377db6b1fdce2da6");

                //PersonaLibraryPrefab
                SetStatus("Finding PersonaLibraryPrefab...");
                PersonaLibraryPrefab = new NFSObjectBlob(MemManager, "097d331254a092347db8c7f677cb620d");

                //GameTime
                SetStatus("Finding GameTime...");
                GameTime = new NFSGameTime(MemManager, "c8d0247b61bcc2314b5679507d0416e2");

                //SpikestripWeapon
                SetStatus("Finding SpikestripWeapon...");
                SpikestripWeapon = new NFSSpikestripWeapon(MemManager, "073c76e4864aec065409eff77d578b2c");
                SpikestripWeapon.FieldList["Classification"].Field = NFSSpikestripWeapon.VehicleWeaponClassification.VehicleWeaponClassification_BackwardFiring;
            }
            catch (System.Exception e) {
                SetStatus(e.Message);
            }
        }

        public void ResetAll() {
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
        }
    }
}
