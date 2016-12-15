using System;

namespace NFS14DifficultyTool {
    abstract public class NFSField {
        protected NFSObject parent;

        public int Offset { get; protected set; }
        public bool ReadOnly { get; set; }
        public object FieldDefault { get; protected set; }
        abstract public object Field { get; set; }

        public NFSField(NFSObject parent, int offset, bool readOnly = false) {
            this.parent = parent;
            Offset = offset;
            ReadOnly = readOnly;

            //This will trigger a memory read on Field and assign it to FieldDefault
            FieldDefault = Field;
        }
        public NFSField(NFSObject parent, string offset, bool readOnly = false) : this(parent, ParseOffset(offset), readOnly) { }

        public static int ParseOffset(string offset) {
            string[] offsets = offset.Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);

            int ret = 0;
            foreach (string s in offsets) {
                ret += Convert.ToInt32(s, 16);
            }
            return ret;
        }
    }

    public class NFSFieldByteArray : NFSField {
        public int FieldSize { get; protected set; }
        public override object Field {
            get {
                return (FieldSize == 0) ? null : parent.MemManager.Read(parent.Address + Offset, FieldSize);
            }
            set {
                if (ReadOnly)
                    return;
                parent.MemManager.Write(parent.Address + Offset, (byte[])value);
            }
        }

        public NFSFieldByteArray(NFSObject parent, int offset, int fieldSize, bool readOnly = false)
            : base(parent, offset, readOnly) {
            FieldSize = fieldSize;
            FieldDefault = Field; //HACK can't call base constructor later, so must do this again
        }
        public NFSFieldByteArray(NFSObject parent, string offset, int fieldSize, bool readOnly = false)
            : base(parent, offset, readOnly) {
            FieldSize = fieldSize;
            FieldDefault = Field; //HACK can't call base constructor later, so must do this again
        }
    }

    public class NFSFieldPointer : NFSFieldByteArray {
        public NFSFieldPointer(NFSObject parent, int offset, bool readOnly = false) : base(parent, offset, IntPtr.Size, readOnly) { }
        public NFSFieldPointer(NFSObject parent, string offset, bool readOnly = false) : base(parent, offset, IntPtr.Size, readOnly) { }
    }

    public class NFSFieldBool : NFSField {
        public override object Field {
            get {
                return parent.MemManager.ReadBool(parent.Address + Offset);
            }
            set {
                if (ReadOnly)
                    return;
                parent.MemManager.WriteBool(parent.Address + Offset, (bool)value);
            }
        }

        public NFSFieldBool(NFSObject parent, int offset, bool readOnly = false) : base(parent, offset, readOnly) { }
        public NFSFieldBool(NFSObject parent, string offset, bool readOnly = false) : base(parent, offset, readOnly) { }
    }

    public class NFSFieldInt : NFSField {
        public override object Field {
            get {
                return parent.MemManager.ReadInt(parent.Address + Offset);
            }
            set {
                if (ReadOnly)
                    return;
                parent.MemManager.WriteInt(parent.Address + Offset, (int)value);
            }
        }

        public NFSFieldInt(NFSObject parent, int offset, bool readOnly = false) : base(parent, offset, readOnly) { }
        public NFSFieldInt(NFSObject parent, string offset, bool readOnly = false) : base(parent, offset, readOnly) { }
    }

    public class NFSFieldFloat : NFSField {
        public override object Field {
            get {
                return parent.MemManager.ReadFloat(parent.Address + Offset);
            }
            set {
                if (ReadOnly)
                    return;
                parent.MemManager.WriteFloat(parent.Address + Offset, (float)value);
            }
        }

        public NFSFieldFloat(NFSObject parent, int offset, bool readOnly = false) : base(parent, offset, readOnly) { }
        public NFSFieldFloat(NFSObject parent, string offset, bool readOnly = false) : base(parent, offset, readOnly) { }
    }

    public class NFSFieldDouble : NFSField {
        public override object Field {
            get {
                return parent.MemManager.ReadDouble(parent.Address + Offset);
            }
            set {
                if (ReadOnly)
                    return;
                parent.MemManager.WriteDouble(parent.Address + Offset, (double)value);
            }
        }

        public NFSFieldDouble(NFSObject parent, int offset, bool readOnly = false) : base(parent, offset, readOnly) { }
        public NFSFieldDouble(NFSObject parent, string offset, bool readOnly = false) : base(parent, offset, readOnly) { }
    }
}
