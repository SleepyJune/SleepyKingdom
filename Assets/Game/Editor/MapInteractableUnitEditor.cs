using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapInteractableUnit))]
public class MapInteractableUnitEditor : Editor
{
    MapInteractableUnit unit;

    private void OnEnable()
    {
        unit = target as MapInteractableUnit;        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        DrawDefaultInspector();

        if (EditorGUI.EndChangeCheck())
        {
            OnChangeCountryData();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void OnChangeCountryData()
    {
        if(unit.render != null && unit.image != null)
        {
            unit.render.sprite = unit.image;
        }
    }
}