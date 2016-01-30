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
            Address = memManager.FindObject(MemoryManager.StringToByteArray(guid));
            if (Address == IntPtr.Zero)
                throw new Exception("Could not locate guid in memory: " + guid);
        }

        public void ResetFieldsToDefault() {
            foreach (NFSField val in FieldList.Values)
                val.Field = val.FieldDefault;
        }
    }
}
