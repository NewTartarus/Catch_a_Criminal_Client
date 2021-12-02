namespace ScotlandYard.CustomEditor
{
    using ScotlandYard.Scripts.Street;
    using System;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(StreetPath), true)]
    [CanEditMultipleObjects]
    public class StreetEditor : Editor
    {
        StreetPath streetPath;

        protected void OnSceneGUI()
        {
            if (streetPath != null && streetPath.drawMeshInEditor && Event.current.type == EventType.Repaint && streetPath.CrossSection != null)
            {
                streetPath.GenerateMesh();
            }
        }

        protected void OnEnable()
        {
            streetPath = (StreetPath)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Separator();
            if (streetPath.transform.childCount == 0)
            {
                if (GUILayout.Button("Initialize Street"))
                {
                    AddControlPoints(4);
                }
            }
            else
            {
                if (GUILayout.Button("Add Segment"))
                {
                    AddControlPoints(3);
                }
            }

            if (GUILayout.Button("Remove Segment"))
            {
                int childCount = streetPath.transform.childCount;

                if (childCount > 4)
                {
                    RemoveControlPoints(3);
                }
                else if(childCount == 4)
                {
                    RemoveControlPoints(4);
                }
            }
        }

        protected void AddControlPoints(int count)
        {
            int lastIndex = streetPath.transform.childCount-1;
            GameObject prefab = Resources.Load<GameObject>("Prefabs/StreetControlPoint");

            for (int i = 0; i < count; i++)
            {
                GameObject emptyGO = GameObject.Instantiate(prefab);
                emptyGO.name = "ControlPoint";

                emptyGO.transform.parent = streetPath.transform;
                if(lastIndex > -1)
                {
                    emptyGO.transform.localPosition = streetPath.transform.GetChild(lastIndex).transform.localPosition;
                }
                else
                {
                    emptyGO.transform.localPosition = Vector3.zero;
                }

                streetPath.AddControlPoint(emptyGO.transform);
            }
        }

        protected void RemoveControlPoints(int count)
        {
            int childCount = streetPath.transform.childCount;

            for (int i = 1; i < (count+1); i++)
            {
                DestroyImmediate(streetPath.transform.GetChild(childCount - i).gameObject);
                streetPath.RemoveControlPoint(childCount - i);
            }
        }
    }
}
