using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
public class ArrayElementTitleDrawer : PropertyDrawer {
    SerializedProperty TitleNameProp;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        string FullPathName = property.propertyPath + "." + Atribute.Varname;
        TitleNameProp = property.serializedObject.FindProperty(FullPathName);
        string newlabel = TitleNameProp.enumNames[TitleNameProp.enumValueIndex];
        EditorGUI.PropertyField(position, property, new GUIContent(newlabel, label.tooltip), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    protected virtual ArrayElementTitleAttribute Atribute {
        get {
            return (ArrayElementTitleAttribute)attribute;
        }
    }
}