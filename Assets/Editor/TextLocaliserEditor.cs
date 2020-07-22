using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ScotlandYard.Scripts.Localisation;

namespace ScotlandYard.Editor.Localisation
{
    public class TextLocaliserEditorWindow : EditorWindow
    {
        int index = 0;

        public string searchValue;
        public string key;
        public string value;

        public Vector2 scroll;
        public Dictionary<string, string> dictionary;

        [MenuItem("Window/Custom Editor/Localisation")]
        public static void Open()
        {
            TextLocaliserEditorWindow window = GetWindow<TextLocaliserEditorWindow>("Localisation Editor");
        }

        public void OnEnable()
        {
            dictionary = LocalisationSystem.GetDictionaryForEditor();
        }

        public void OnGUI()
        {

            string[] test = { "Add", "Search" };
            index = GUILayout.SelectionGrid(index, test, 2);
            EditorGUILayout.Space();

            switch (index)
            {
                case 0:
                    GetAddValuesField();
                    break;
                case 1:
                    GetSearchResultsField();
                    break;
            }
        }

        private void GetSearchResultsField()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Search:", EditorStyles.boldLabel, GUILayout.MaxWidth(50));
            GUILayout.Space(40);
            searchValue = GUILayout.TextField(searchValue);
            EditorGUILayout.EndHorizontal();

            if (searchValue == null)
            {
                searchValue = "";
            }

            GUILayout.BeginVertical();
            scroll = GUILayout.BeginScrollView(scroll);

            foreach (var element in dictionary)
            {
                if (element.Key.ToLower().Contains(searchValue.ToLower()) || element.Value.ToLower().Contains(searchValue.ToLower()))
                {
                    GUILayout.BeginHorizontal("Box");

                    GUILayout.TextField(element.Key, GUILayout.MaxWidth(300));
                    GUILayout.Label(element.Value);

                    if (GUILayout.Button("X", GUILayout.MaxHeight(18), GUILayout.MaxWidth(18)))
                    {
                        if (EditorUtility.DisplayDialog($"Remove Key: {element.Key}?", "This will remove the element from localisation, are you sure?", "YES", "NO"))
                        {
                            LocalisationSystem.Remove(element.Key);
                            AssetDatabase.Refresh();
                            LocalisationSystem.Init();
                            dictionary = LocalisationSystem.GetDictionaryForEditor();
                        }
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void GetAddValuesField()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Key:", EditorStyles.boldLabel, GUILayout.MaxWidth(50));
            GUILayout.Space(40);
            key = GUILayout.TextField(key);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Value:", EditorStyles.boldLabel, GUILayout.MaxWidth(50));
            GUILayout.Space(40);
            EditorStyles.textArea.wordWrap = true;
            value = EditorGUILayout.TextArea(value, EditorStyles.textArea, GUILayout.Height(100));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if (GUILayout.Button("Add"))
            {
                if (LocalisationSystem.GetLocalisedValue(key) != string.Empty)
                {
                    LocalisationSystem.Replace(key, value);
                }
                else
                {
                    LocalisationSystem.Add(key, value);
                }

                key = "";
                value = "";
                OnEnable();
            }
        }
    }
}