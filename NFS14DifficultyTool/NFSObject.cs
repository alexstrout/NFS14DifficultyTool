using System;
using System.Collections.Generic;

namespace NFS14DifficultyTool {
    abstract public class NFSObject {
        public MemoryManager MemManager { get; protected set; }
        public Dictionary<String, NFSField> FieldList { get; protected set; }
        public string GUID { get; protected set; }
        public IntPtr Address { get; protected set; }

        public NFSObject(MemoryManager memManager) { }
        protected NFSObject(MemoryManager memManager, string guid) {
            MemManager = memManager;
            FieldList = new Dictionary<String, NFSField>();
            GUID = guid;
            Address = memManager.FindObject(MemoryManager.StringToByteArray(guid), IntPtr.Size);
            if (Address == IntPtr.Zero)
                throw new Exception("Could not locate guid in memory: " + guid);

            //Add a GUID field for all NFSObjects - we check this later to make sure our object is still valid in memory
            FieldList.Add("GUID", new NFSFieldByteArray(this, "0", 16, true));
        }

        public bool IsValid() {
            return MemoryManager.ByteArrayToString((byte[])FieldList["GUID"].Field) == MemoryManager.ByteArrayToString((byte[])FieldList["GUID"].FieldDefault);
        }

        public void ResetFieldsToDefault() {
            if (!IsValid())
                return;
            foreach (NFSField val in FieldList.Values)
                val.Field = val.FieldDefault;
        }
    }
}
