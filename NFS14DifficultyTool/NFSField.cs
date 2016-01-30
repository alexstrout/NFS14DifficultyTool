using System;

namespace NFS14DifficultyTool {
    abstract public class NFSField {
        protected NFSObject parent;

        public int Offset { get; protected set; }
        public object FieldDefault { get; protected set; }
        abstract public object Field { get; set; }

        public NFSField(NFSObject parent, int offset) {
            this.parent = parent;
            Offset = offset;

            //This will trigger a memory read on Field and assign it to FieldDefault
            FieldDefault = Field;
        }
        public NFSField(NFSObject parent, string offset) : this(parent, ParseOffset(offset)) { }

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
                parent.MemManager.Write(parent.Address + Offset, (byte[])value);
            }
        }

        public NFSFieldByteArray(NFSObject parent, int offset, int fieldSize)
            : base(parent, offset) {
            FieldSize = fieldSize;
            FieldDefault = Field; //HACK can't call base constructor later, so must do this again
        }
        public NFSFieldByteArray(NFSObject parent, string offset, int fieldSize)
            : base(parent, offset) {
            FieldSize = fieldSize;
            FieldDefault = Field; //HACK can't call base constructor later, so must do this again
        }
    }

    public class NFSFieldPointer : NFSFieldByteArray {
        public override object Field {
            get {
                return parent.MemManager.Read(parent.Address + Offset, IntPtr.Size);
            }
            set {
                parent.MemManager.Write(parent.Address + Offset, (byte[])value);
            }
        }

        public NFSFieldPointer(NFSObject parent, int offset) : base(parent, offset, IntPtr.Size) { }
        public NFSFieldPointer(NFSObject parent, string offset) : base(parent, offset, IntPtr.Size) { }
    }

    public class NFSFieldBool : NFSField {
        public override object Field {
            get {
                return parent.MemManager.ReadBool(parent.Address + Offset);
            }
            set {
                parent.MemManager.WriteBool(parent.Address + Offset, (bool)value);
            }
        }

        public NFSFieldBool(NFSObject parent, int offset) : base(parent, offset) { }
        public NFSFieldBool(NFSObject parent, string offset) : base(parent, offset) { }
    }

    public class NFSFieldInt : NFSField {
        public override object Field {
            get {
                return parent.MemManager.ReadInt(parent.Address + Offset);
            }
            set {
                parent.MemManager.WriteInt(parent.Address + Offset, (int)value);
            }
        }

        public NFSFieldInt(NFSObject parent, int offset) : base(parent, offset) { }
        public NFSFieldInt(NFSObject parent, string offset) : base(parent, offset) { }
    }

    public class NFSFieldFloat : NFSField {
        public override object Field {
            get {
                return parent.MemManager.ReadFloat(parent.Address + Offset);
            }
            set {
                parent.MemManager.WriteFloat(parent.Address + Offset, (float)value);
            }
        }

        public NFSFieldFloat(NFSObject parent, int offset) : base(parent, offset) { }
        public NFSFieldFloat(NFSObject parent, string offset) : base(parent, offset) { }
    }

    public class NFSFieldDouble : NFSField {
        public override object Field {
            get {
                return parent.MemManager.ReadDouble(parent.Address + Offset);
            }
            set {
                parent.MemManager.WriteDouble(parent.Address + Offset, (double)value);
            }
        }

        public NFSFieldDouble(NFSObject parent, int offset) : base(parent, offset) { }
        public NFSFieldDouble(NFSObject parent, string offset) : base(parent, offset) { }
    }
}
