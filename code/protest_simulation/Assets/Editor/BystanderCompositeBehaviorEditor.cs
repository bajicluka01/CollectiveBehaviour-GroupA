using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(BystanderCompositeBehavior))]

public class BystanderCompositeBehaviorEditor : Editor {
    public override void OnInspectorGUI() {

        Console.WriteLine("TESTING!");

        BystanderCompositeBehavior cb = (BystanderCompositeBehavior)target;        

        if(cb.behaviors == null || cb.behaviors.Length == 0) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
            EditorGUILayout.EndHorizontal();            
        }
        else {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Behaviors", GUILayout.MinWidth(60f), GUILayout.MaxWidth(290f));
            EditorGUILayout.LabelField("Weights", GUILayout.MinWidth(65f), GUILayout.MaxWidth(65f));
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginChangeCheck();

            for(int i = 0; i < cb.behaviors.Length; i++) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.MinWidth(20f), GUILayout.MaxWidth(20f));
                cb.behaviors[i] = (BystanderBehavior)EditorGUILayout.ObjectField(cb.behaviors[i], typeof(BystanderBehavior), false, GUILayout.MinWidth(20f));
                cb.weights[i] = EditorGUILayout.FloatField(cb.weights[i], GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.EndHorizontal();
            }
            if(EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(target);
                GUIUtility.ExitGUI();
            }
        }

        EditorGUILayout.BeginHorizontal();      
        if(GUILayout.Button("Add Behavior")) {
            AddBehavior(cb);
            GUIUtility.ExitGUI();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if(cb.behaviors != null && cb.behaviors.Length > 0) {
            if (GUILayout.Button("Remove Behavior")) {
                RemoveBehavior(cb);
                GUIUtility.ExitGUI();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void AddBehavior(BystanderCompositeBehavior cb) {
        int oldCount = (cb.behaviors != null) ? cb.behaviors.Length : 0;
        BystanderBehavior[] newBehaviors = new BystanderBehavior[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];

        for(int i = 0; i < oldCount; i++) {
            newBehaviors[i] = cb.behaviors[i];
            newWeights[i] = cb.weights[i];
        }
        newWeights[oldCount] = 1f;
        cb.behaviors = newBehaviors;
        cb.weights = newWeights;
    }

    void RemoveBehavior(BystanderCompositeBehavior cb) {
        int oldCount = cb.behaviors.Length;
        if(oldCount == 1) {
            cb.behaviors = null;
            cb.weights = null;
            return;
        }

        BystanderBehavior[] newBehaviors = new BystanderBehavior[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];

        for(int i = 0; i < oldCount -1; i++) {
            newBehaviors[i] = cb.behaviors[i];
            newWeights[i] = cb.weights[i];
        }
        cb.behaviors = newBehaviors;
        cb.weights = newWeights;
    }
}