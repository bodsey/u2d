using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ingredient))]
public class IngredientEditor : Editor {

    
    public override void OnInspectorGUI()
    {
        GameObject obj = ((Ingredient)target).gameObject;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            if(GUILayout.Button("Add Physics to Object"))
            {
                obj.AddComponent<Rigidbody2D>();
            }
        }
        else
        {
            if (GUILayout.Button("Remove Physics from Object"))
            {
                DestroyImmediate(rb);
            }
        }

        GUILayout.BeginHorizontal();
        HorizontalMoveRoutine hm = obj.GetComponent<HorizontalMoveRoutine>();
        if (hm == null)
        {
            if (GUILayout.Button("Add Horizontal Move Routine"))
            {
                obj.AddComponent<HorizontalMoveRoutine>();
            }
        }
        else
        {
            if (GUILayout.Button("Remove Horizontal Move Routine"))
            {
                DestroyImmediate(hm);
            }
        }
        VerticalMoveRoutine vm = obj.GetComponent<VerticalMoveRoutine>();
        if (vm == null)
        {
            if (GUILayout.Button("Add Vertical Move Routine"))
            {
                obj.AddComponent<VerticalMoveRoutine>();
            }
        }
        else
        {
            if (GUILayout.Button("Remove Vertical Move Routine"))
            {
                DestroyImmediate(vm);
            }
        }
        GUILayout.EndHorizontal();

        Hitable h = obj.GetComponent<Hitable>();
        if(h == null)
        {
            if (GUILayout.Button("Mark as killable"))
            {
                obj.AddComponent<Hitable>();
            }
        }
        else
        {
            if (GUILayout.Button("Remove killable"))
            {
                DestroyImmediate(h);
            }
        }

        base.OnInspectorGUI();
    }
}
