using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(MovingBox))]
public class MovingBoxEditor : Editor
{
    private SerializedProperty speed;
    private SerializedProperty offsets;

    private void OnEnable()
    {
        speed = serializedObject.FindProperty("speed");
        offsets = serializedObject.FindProperty("offsets");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.Slider(speed, 1, 5);

        while (offsets.arraySize < 2) offsets.InsertArrayElementAtIndex(0);


        GUILayout.Label("Control points (first is constant current position)");
        for (int i = 0; i < offsets.arraySize; i++)
        {
            if (!DrawProperty(offsets.GetArrayElementAtIndex(i), i)) continue;
            if (offsets.arraySize > 2)
                offsets.DeleteArrayElementAtIndex(i);
        }

        if (GUILayout.Button("ADD Control point")) offsets.InsertArrayElementAtIndex(offsets.arraySize);


        serializedObject.ApplyModifiedProperties();
    }

    private bool DrawProperty(SerializedProperty p, int index)
    {
        // make the with short so it fits on one line
        EditorGUIUtility.labelWidth = 70;
        EditorGUILayout.BeginHorizontal();

        var val = p.FindPropertyRelative("offset");
        var h = p.FindPropertyRelative("horizontal");

        EditorGUI.BeginDisabledGroup(index == 0);

        EditorGUILayout.PropertyField(val, new GUIContent("Offset",
                "Offset from world position of current line segment (x if horizontal, y otherwise)"));

        EditorGUILayout.PropertyField(h);


        if (GUILayout.Button("Remove"))
        {
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            return true;
        }

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        return false;
    }
}
