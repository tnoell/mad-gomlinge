#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//from https://answers.unity.com/questions/1589226/showing-an-array-with-enum-as-keys-in-the-property.html
//+ automatic resizing
[CustomPropertyDrawer(typeof(EnumNamedArrayAttribute))]
public class DrawerEnumNamedArray : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumNamedArrayAttribute enumNames = attribute as EnumNamedArrayAttribute;
        //SerializedProperty array = property.serializedObject;
        string path = property.propertyPath;
        string arrayName = path.Substring(0, path.IndexOf(".Array"));
        SerializedProperty arrayProperty = property.serializedObject.FindProperty(arrayName);
        arrayProperty.arraySize = enumNames.names.Length;

        //propertyPath returns something like component_hp_max.Array.data[4]
        //so get the index from there
        int index = System.Convert.ToInt32(path.Substring(path.IndexOf("[")).Replace("[", "").Replace("]", ""));
        //change the label
        label.text = enumNames.names[index];
        //draw field
        EditorGUI.PropertyField(position, property, label, true);
    }
}
#endif
