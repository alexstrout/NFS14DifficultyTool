using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFS14DifficultyTool {
    class NFSObjectBlob {
        public MemoryManager MemManager { get; protected set; }
        public Dictionary<String, NFSField> FieldList { get; protected set; }
        public IntPtr Address { get; protected set; }

        public NFSObjectBlob(MemoryManager memManager, string guid) {
            MemManager = memManager;
            FieldList = new Dictionary<String, NFSField>();
            Address = memManager.FindObject(MemoryManager.StringToByteArray(guid));
            if (Address == IntPtr.Zero)
                throw new Exception("Could not locate guid in memory: " + guid);
        }
    }
}
