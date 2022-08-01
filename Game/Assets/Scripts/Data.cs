using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SO/Data")]
[ExecuteInEditMode]
public class Data : ScriptableObject
{
    [Tooltip("Does the aim origin stick to ball or is it on the initial click location?")]
    public bool stickToBall = true;
    public float soundtrackVolume = 1f;
    public float sfxVolume = 1f;

    public List<int> levels = new List<int>(){-1, -1, -1};

    public void Reset()
    {
        stickToBall = true;
        soundtrackVolume = 1f;
        sfxVolume = 1f;
        levels = new List<int>() {-1, -1, -1};
    }

    public int GetMaxLevelComplete()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i] == -1)
                return i;
        }

        return levels.Count - 1;
    }
}


// Adds button directly to SO, simpler to use
#if UNITY_EDITOR
[CustomEditor(typeof(Data))]
public class DataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Data data = (Data)target;
        if (GUILayout.Button("Reset Data"))
        {
            data.Reset();
        }

        DrawDefaultInspector();
    }
}
#endif


