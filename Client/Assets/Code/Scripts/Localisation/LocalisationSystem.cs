using System;
using System.Collections.Generic;
using System.Linq;
using ScotlandYard.Enums;
using ScotlandYard.Scripts.FileReader;
using UnityEngine;

namespace ScotlandYard.Scripts.Localisation
{
    public class LocalisationSystem
    {
        public static ELanguages language = ELanguages.English;

        private static Dictionary<string, string> localisationEN;
        private static Dictionary<string, string> localisationDE;

        public static bool isInit;

        public static CSVLoader csvLoader;
        public static void Init()
        {
            csvLoader = new CSVLoader();
            csvLoader.LoadCSVFile();

            UpdateDictionaries();

            string cultureName = System.Globalization.CultureInfo.CurrentCulture.NativeName.Split(' ')[0];

            try
            {
                language = (ELanguages)Enum.Parse(typeof(ELanguages), cultureName, true);
            }
            catch(Exception ex)
            {
                Debug.Log($"The language {cultureName} could not be found.\n{ex.Message}");
                language = ELanguages.English;
            }

            isInit = true;
        }

        public static void UpdateDictionaries()
        {
            localisationEN = csvLoader.GetDictionaryValues("en");
            localisationDE = csvLoader.GetDictionaryValues("de");
        }

        public static string GetLocalisedValue(string key)
        {
            if (!isInit)
            {
                Init();
            }

            string value;

            switch (language)
            {
                case ELanguages.English:
                    localisationEN.TryGetValue(key, out value);
                    break;
                case ELanguages.Deutsch:
                    localisationDE.TryGetValue(key, out value);
                    break;
                default:
                    localisationEN.TryGetValue(key, out value);
                    break;
            }

            return value;
        }

        public static List<string> GetLanguages()
        {
            List<string> languages = new List<string>();

            foreach(string name in Enum.GetNames(typeof(ELanguages)))
            {
                languages.Add(name);
            }

            return languages;
        }

#if UNITY_EDITOR
        public static void Add(string key, string value)
        {
            if (value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSVFile();
            csvLoader.Add(key, value);
            csvLoader.LoadCSVFile();

            UpdateDictionaries();
        }

        public static void Replace(string key, string value)
        {
            if (value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSVFile();
            csvLoader.Edit(key, value);
            csvLoader.LoadCSVFile();

            UpdateDictionaries();
        }

        public static Dictionary<string, string> GetDictionaryForEditor()
        {
            if (!isInit)
            {
                Init();
            }
            return localisationEN;
        }

        public static void Remove(string key)
        {
            if (csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSVFile();
            csvLoader.Remove(key);
            csvLoader.LoadCSVFile();

            UpdateDictionaries();
        }
#endif
    }
}

