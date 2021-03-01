using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ScotlandYard.Enums;
using ScotlandYard.Scripts.Database.DAOs;
using UnityEngine;

namespace ScotlandYard.Scripts.Localisation
{
    public class LocalisationSystem
    {
        public static Language language;

        public static Language Lang
        {
            get => language;
            set
            {
                if(value != null && !value.Equals(language))
                {
                    language = value;
                    UpdateDictionary();
                }
            }
        }

        private static Dictionary<string, string> localisation;

        public static bool isInit;

        public static void Init()
        {
            string cultureName = CultureInfo.CurrentCulture.NativeName.Split(' ')[0];

            List<Language> languages = LanguageDAO.getInstance().ReadAll();

            try
            {
                Lang = languages.Find(l => l.Name.Equals(cultureName));
            }
            catch(Exception ex)
            {
                Debug.Log($"The language {cultureName} could not be found.\n{ex.Message}");
                Lang = languages.Find(l => l.Name.Equals("English"));
            }

            UpdateDictionary();

            isInit = true;
        }

        public static void UpdateDictionary()
        {
            List<object[]> result = LocalizationDAO.getInstance().Read(Lang.ID);

            localisation = new Dictionary<string, string>();
            foreach(object[] r in result)
            {
                localisation.Add(r[0].ToString(), r[1].ToString());
            }
        }

        public static string GetLocalisedValue(string key)
        {
            if (!isInit)
            {
                Init();
            }

            string value;
            localisation.TryGetValue(key, out value);

            return value;
        }

        public static List<string> GetLanguages()
        {
            List<string> languageNames = new List<string>();

            foreach(Language lan in LanguageDAO.getInstance().ReadAll())
            {
                languageNames.Add(lan.Name);
            }

            return languageNames;
        }

#if UNITY_EDITOR
        public static void EditorInit()
        {
            Lang = new Language(0, "English");
            UpdateDictionary();
            isInit = true;
        }
        
        public static void Add(string key, string value)
        {
            if (value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            LocalizationDAO.getInstance().Insert(key, value, Lang.ID);
            UpdateDictionary();
        }

        public static void Replace(string key, string value)
        {
            LocalizationDAO.getInstance().Update(value, key, Lang.ID);
            UpdateDictionary();
        }

        public static Dictionary<string, string> GetDictionaryForEditor()
        {
            if (!isInit)
            {
                EditorInit();
            }
            return localisation;
        }

        public static void Remove(string key)
        {
            LocalizationDAO.getInstance().Delete(key);
            UpdateDictionary();
        }
#endif
    }
}

