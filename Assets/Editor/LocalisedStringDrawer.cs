using UnityEngine;
using UnityEditor;
using ScotlandYard.Scripts.Localisation;

namespace ScotlandYard.Editor.Localisation
{
    [CustomPropertyDrawer(typeof(LocalizedString))]
    public class LocalisedStringDrawer : PropertyDrawer
    {
        bool dropdown;
        float height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (dropdown)
            {
                return height + 25;
            }

            return 20;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.width -= 34;
            position.height = 18;

            Rect valueRect = new Rect(position);
            valueRect.x += 15;
            valueRect.y -= 15;

            Rect foldButtonRect = new Rect(position);
            foldButtonRect.width = 15;

            dropdown = EditorGUI.Foldout(foldButtonRect, dropdown, "");

            position.x += 15;
            position.width -= 70;

            SerializedProperty key = property.FindPropertyRelative("key");
            key.stringValue = EditorGUI.TextField(position, key.stringValue);

            position.x += position.width + 2;
            position.width = 70;

            if (GUI.Button(position, "Open"))
            {
                TextLocaliserEditorWindow.Open();
            }

            if (dropdown)
            {
                var value = LocalisationSystem.GetLocalisedValue(key.stringValue);
                GUIStyle style = GUI.skin.box;
                height = style.CalcHeight(new GUIContent(value), valueRect.width);

                valueRect.height = height;
                valueRect.y += 40;
                EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
            }

            EditorGUI.EndProperty();
        }
    }
}