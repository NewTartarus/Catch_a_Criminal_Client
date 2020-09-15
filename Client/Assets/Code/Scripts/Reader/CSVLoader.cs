using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ScotlandYard.Scripts.FileReader
{
    public class CSVLoader
    {
        private TextAsset csvFile;
        private string[] lineSeperator = { "\n", "\r", "\r\n" };
        private char surround = '"';
        private string[] fieldSeperator = { "\",\"" };

        public void LoadCSVFile()
        {
            csvFile = Resources.Load<TextAsset>("localisation");
        }

        public Dictionary<string, string> GetDictionaryValues(string attributeId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();


            string[] lines = csvFile.text.Split(lineSeperator, StringSplitOptions.None);
            int attributeIndex = -1;

            string[] headers = lines[0].Split(fieldSeperator, StringSplitOptions.None);
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Contains(attributeId))
                {
                    attributeIndex = i;
                    break;
                }
            }

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = CSVParser.Split(line);
                for (int f = 0; f < fields.Length; f++)
                {
                    fields[f] = fields[f].TrimStart(' ', surround);
                    fields[f] = fields[f].TrimEnd('"');
                }

                if (fields.Length > attributeIndex)
                {
                    var key = fields[0];

                    if (dictionary.ContainsKey(key))
                    {
                        continue;
                    }

                    dictionary.Add(key, fields[attributeIndex]);
                }
            }

            return dictionary;
        }

#if UNITY_EDITOR
        public void Add(string key, string value)
        {
            string appended = $"\n\"{key}\",\"{value}\",\"\"";
            File.AppendAllText("Assets/Resources/localisation.csv", appended);

            UnityEditor.AssetDatabase.Refresh();
        }

        public void Remove(string key)
        {
            string[] lines = csvFile.text.Split(lineSeperator, StringSplitOptions.None);
            int index = -1;

            for (int i = 0; i < lines.Length; i++)
            {
                string tempKey = lines[i].Split(fieldSeperator, StringSplitOptions.None)[0];
                if (tempKey.Contains(key))
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                string[] newLines;
                newLines = lines.Where(w => w != lines[index] && !string.IsNullOrEmpty(lines[index])).ToArray();

                string replaced = string.Join(lineSeperator[0], newLines);
                File.WriteAllText("Assets/Resources/localisation.csv", replaced);
            }
        }

        public void Edit(string key, string value)
        {
            Remove(key);
            Add(key, value);
        }
#endif
    }
}