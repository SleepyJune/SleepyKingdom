using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;

using UnityEditor;

using System.Linq;

[CustomEditor(typeof(DevModeManager))]
public class DevModeManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();

        DrawDefaultInspector();

        if (GUILayout.Button("Delete PlayPref"))
        {
            PlayerPrefs.DeleteAll();
        }

        if (GUILayout.Button("Reset Population"))
        {            
            if (GameManager.instance != null)
            {
                GameManager.instance.globalCountryManager.myCountry.population = 100;
            }
        }

        //serializedObject.ApplyModifiedProperties();
    }
}

