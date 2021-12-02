namespace ScotlandYard.CustomEditor
{
	using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Transform), true)]
    [CanEditMultipleObjects]
	public class CustomTransformEditor : Editor
	{
		#region Members
		protected Editor defaultEditor;
		protected Transform transform;
		#endregion

		#region Methods
		protected void OnEnable()
		{
			defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
			transform = target as Transform;
		}

		protected void OnDisable()
		{
			MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (disableMethod != null)
            {
				disableMethod.Invoke(defaultEditor, null);
			}
				
			DestroyImmediate(defaultEditor);
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Local Space", EditorStyles.boldLabel);
			defaultEditor.OnInspectorGUI();

			//Show World Space Transform
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("World Space", EditorStyles.boldLabel);

			GUI.enabled = false;
			Vector3 localPosition = transform.localPosition;
			transform.localPosition = transform.position;

			Quaternion localRotation = transform.localRotation;
			transform.localRotation = transform.rotation;

			Vector3 localScale = transform.localScale;
			transform.localScale = transform.lossyScale;

			defaultEditor.OnInspectorGUI();
			transform.localPosition = localPosition;
			transform.localRotation = localRotation;
			transform.localScale = localScale;
			GUI.enabled = true;
		}
		#endregion
	}
}