using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LevelHandler2), true)]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //  DrawDefaultInspector();
        base.OnInspectorGUI();

        LevelHandler2 m_target = (LevelHandler2)target;

        if (GUILayout.Button("Genrate Level"))
        {
            m_target._GenrateLevelNow();
        }

        if (GUILayout.Button("Destroy Level"))
        {
            m_target._DestroyLevelNow();
        }

    }
}
