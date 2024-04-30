using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using NUnit.Framework.Internal;
using Cinemachine.Editor;

[CustomEditor(typeof(AudioClips))]
public class AudioClipsEditor : Editor {
    private SerializedProperty savedAudioClips;
    private TypeDiconaryType TypeDiconary = new();
    private Dictionary<string, bool> ToggleChecker = new();


    private void OnEnable() {
        savedAudioClips = serializedObject.FindProperty("SavedAuidoClips");
        for (int i = 0; i < savedAudioClips.arraySize; i++) {
            SerializedProperty elementProperty = savedAudioClips.GetArrayElementAtIndex(i);
            SerializedProperty soundClip = elementProperty.FindPropertyRelative("Clip");
            string audioName = CheckAudioSourceName(soundClip);
            string enumName = GetEnumName(elementProperty);
            HandleNewElement(enumName, audioName, elementProperty, TypeDiconary);
        }
        UpdateBoolDiconary();
    }

    private void HandleNewElement(string enumName, string audioName, SerializedProperty elementProperty, TypeDiconaryType typeDictionary) {
        typeDictionary.TryGetValue(enumName, out List<(SerializedProperty, string)> list);
        if (list == null) {
            list = new() {
                (elementProperty, audioName)
            };

            typeDictionary.Add(enumName, list);
        } else {
            list.Add((elementProperty, audioName));
        }
    }

    private static string CheckAudioSourceName(SerializedProperty SoundClip) {
        if (SoundClip != null && SoundClip.objectReferenceValue != null) {
            return SoundClip.objectReferenceValue.name;
        }
        return "None";
    }


    public override void OnInspectorGUI() {
        serializedObject.Update();
        TypeDiconary = UpdateTypeDictionary();
        TypeDiconaryType updatedDiconary = new();
        List<(SerializedProperty property, string clipName)> updatedList = null;
        foreach (var list in TypeDiconary) {
            bool hasRemoved = false;
            ToggleChecker[list.Key] = EditorGUILayout.Foldout(ToggleChecker[list.Key], list.Key);
            if (ToggleChecker[list.Key]) {
                EditorGUI.indentLevel++;
                for (int i = 0; i < list.Value.Count; i++) {
                    var property = list.Value[i];
                    var name = CheckAudioSourceName(property.property.FindPropertyRelative("Clip"));
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(property.property, new GUIContent(name));
                    if (GUILayout.Button("Delete")) {
                        updatedList ??= list.Value;
                        updatedList.Remove(property);
                        RemoveEntry(list.Key, i);
                        hasRemoved = true;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            var value = list.Value;
            if (hasRemoved) {
                value = updatedList;
            }
            updatedDiconary.Add(list.Key, value);
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add New Entry")) {
            CreateNewEntry();
        } else {
            CreateNewDicionary(updatedDiconary);
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void RemoveEntry(string enumName, int index) {
        savedAudioClips.DeleteArrayElementAtIndex(index);
        serializedObject.ApplyModifiedProperties();
    }
    private static string GetEnumName(SerializedProperty elementProperty) {
        try {
            SerializedProperty typeEnum = elementProperty.FindPropertyRelative("Type");
            return typeEnum.enumNames[typeEnum.enumValueIndex];
        } catch (Exception) {
            Debug.Log("IDK what happened");
        }
        return "Exception";
    }

    private void CreateNewEntry() {
        AudioClips audioClipsObject = (AudioClips)target;
        SerializedObject serializedAudioClipsObject = new(audioClipsObject);
        SerializedProperty savedAudioClipsProperty = serializedAudioClipsObject.FindProperty("SavedAuidoClips");
        int arraySize = savedAudioClipsProperty.arraySize;
        savedAudioClipsProperty.arraySize++;
        SerializedProperty newElementProperty = savedAudioClipsProperty.GetArrayElementAtIndex(arraySize);
        SerializedProperty typeEnum = newElementProperty.FindPropertyRelative("Type");
        typeEnum.enumValueIndex = 0;
        newElementProperty.FindPropertyRelative("Clip").objectReferenceValue = null;
        newElementProperty.FindPropertyRelative("volume").floatValue = 1;
        string enumName = typeEnum.enumNames[typeEnum.enumValueIndex];
        serializedAudioClipsObject.ApplyModifiedProperties();
        HandleNewElement(enumName, "None", newElementProperty, TypeDiconary);
        TypeDiconary = UpdateTypeDictionary();
    }

    private void CreateNewDicionary(TypeDiconaryType updatedDiconary) {
        TypeDiconary.Clear();
        foreach (var item in updatedDiconary) {
            TypeDiconary.Add(item.Key, item.Value);
        }
    }

    private TypeDiconaryType UpdateTypeDictionary() {
        TypeDiconaryType returnDictionary = new();
        foreach (var pair in TypeDiconary) {
            foreach (var property in pair.Value) {
                SerializedProperty soundClip = property.property.FindPropertyRelative("Clip");
                string enumName = GetEnumName(property.property);
                HandleNewElement(enumName, CheckAudioSourceName(soundClip), property.property, returnDictionary);
            }
        }
        UpdateBoolDiconary();
        return returnDictionary;
    }

    private void UpdateBoolDiconary() {
        foreach (var item in TypeDiconary) {
            if (!ToggleChecker.TryGetValue(item.Key, out bool value)) {
                ToggleChecker.Add(item.Key, value);
            }
        }
    }

    private class TypeDiconaryType : Dictionary<string, List<(SerializedProperty property, string clipName)>> {
    }
}