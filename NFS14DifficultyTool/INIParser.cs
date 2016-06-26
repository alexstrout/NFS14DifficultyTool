using System;
using System.Collections.Generic;
using System.IO;

namespace NFS14DifficultyTool {
    class INIParser {
        protected Dictionary<string, Dictionary<string, string>> fields;

        public INIParser() {
            fields = new Dictionary<string, Dictionary<string, string>>();
        }

        //In-memory get/set functions
        public string GetValue(string key) {
            return GetValue("", key);
        }
        public string GetValue(string section, string key) {
            return (fields.ContainsKey(section) && fields[section].ContainsKey(key))
                ? fields[section][key]
                : null;
        }

        public void SetValue(string key, object data) {
            SetValue("", key, data);
        }
        public void SetValue(string section, string key, object data) {
            if (!fields.ContainsKey(section))
                fields[section] = new Dictionary<string, string>();
            fields[section][key] = data.ToString();
        }

        //File read/write functions
        public void ReadFile(string filePath) {
            fields.Clear();

            //Note: We don't gcAllowVeryLargeObjects, so Length is big enough (who would want to read a >2GB INI file anyway!?)
            string[] lines = File.ReadAllLines(filePath);
            string[] lineSplit;
            string line;
            string section = ""; //Empty string, or [] in file, is treated as "Global" section
            for (int i = 0; i < lines.Length; i++) {
                //Strip comments
                //TODO allow configuring of comment fields
                line = lines[i];
                if (line.Contains(";"))
                    line = line.Substring(0, line.IndexOf(";"));

                //Hunt sections first - any following fields are considered part of that section, until we find a new section
                if (line.Contains("[") && line.Contains("]")) {
                    section = line.Substring(line.IndexOf("[") + 1);
                    section = section.Substring(0, section.IndexOf("]"));
                }
                else if (line.Contains("=")) {
                    //Just ignore blank fields
                    //TODO allow configuring of whether this should throw exception
                    if (line[0] == '=')
                        continue;

                    //Treat blank assignments as empty string (e.g. "SomeNullableValue=" should be valid)
                    //TODO allow configuring of whether this should throw exception
                    if (line[line.Length - 1] == '=')
                        line = line + " ";

                    //This should always succeed based on the above two checks
                    lineSplit = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    SetValue(section, lineSplit[0].Trim(), lineSplit[1].Trim());
                }
            }
        }

        public void WriteFile(string filePath) {
            List<string> lines = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in fields) {
                if (lines.Count > 0)
                    lines.Add("");
                lines.Add("[" + kvp.Key + "]");
                foreach (KeyValuePair<string, string> kvpSub in kvp.Value) {
                    lines.Add(kvpSub.Key + " = " + kvpSub.Value);
                }
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
