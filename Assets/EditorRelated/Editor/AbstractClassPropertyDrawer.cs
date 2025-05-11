using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorRelated
{
    [CustomPropertyDrawer(typeof(DrawableAbstractClass))]
    public class AbstractClassPropertyDrawer : PropertyDrawer
    {
        private Rect rect;

        // If there are multiple objects using the same PropertyDrawer class
        // (meaning the objects have the same type) within an array, Unity uses
        // the same PropertyDrawer object for all of them.
        // It asks the height for each only after calling OnGUI on all of them,
        // so we need to store all the heights in a way that we can find them
        // from within the GetPropertyHeight call.
        private Dictionary<string, InstanceData> instanceDataByPath = new Dictionary<string, InstanceData>();

        private static object clipboard;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            if (!instanceDataByPath.TryGetValue(property.propertyPath, out var data)) // check if the property has a value/data assigned yet
            {
                data = new InstanceData();
                instanceDataByPath[property.propertyPath] = data;
            }

            GUIStyle buttonStyle = GUI.skin.button;
            GUIStyle previousButtonStyle = new GUIStyle(buttonStyle);
            buttonStyle.fixedWidth = 0;

            // The functions called from a context menu are run at a different time than the
            // function assigning them, so to gain control over the timing, we only set a marker
            // that something should be done, then actually execute it here. Execute before
            // drawing the button, so that the new changes appear immediately.
            ExecuteButtonActions(property, data);

            float startHeight = position.y;
            float lineStep = EditorGUIUtility.singleLineHeight;
            position.height = lineStep - EditorGUIUtility.standardVerticalSpacing;

            // Draw label, positioning rect.x behind it
            rect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive),
                label, GetLabelStyle(property));

            DrawTypeButton(ref position, property, data, lineStep); // sets up the context menu functions
            DisplaySubProperties(ref position, property);

            instanceDataByPath[property.propertyPath].uiHeight = Mathf.RoundToInt(position.y - startHeight);

            GUI.skin.button = previousButtonStyle;
            EditorGUI.EndProperty();
        }

        protected virtual GUIStyle GetLabelStyle(SerializedProperty property)
        {
            return GUI.skin.label;
        }

        private void DrawTypeButton(ref Rect position, SerializedProperty property, InstanceData data, float lineStep)
        {
            rect.width = position.x + position.width - rect.x; // use entire remaining width

            string buttonText;
            try
            {
                buttonText = property.managedReferenceValue?.GetType().Name ?? "None";
            }
            catch (System.InvalidOperationException)
            {
                if (!typeof(DrawableAbstractClass).IsAssignableFrom(GetRealFieldType()))
                    return; // Unity used the wrong drawer, happens sometimes for some reason
                GUI.Label(rect, "Not a reference!");
                position.y += lineStep;
                return;
            }
            if (GUI.Button(rect, buttonText))
            {
                if (Event.current.button == 0) // left click
                {
                    PropertyWindow.OpenProperty(property);
                }
                else // right click/something else
                {
                    GenericMenu menu = new GenericMenu();
                    Type type = GetRealFieldType();
                    List<Type> types = Util.GetSubTypes(type);
                    foreach (Type subType in types)
                    {
                        if (subType.IsAbstract) continue;
                        string name = "Set Type/" + GetCategory(subType) + subType.Name;
                        menu.AddItem(new GUIContent(name), false,
                            () => { data.newValue = Activator.CreateInstance(subType); });
                    }

                    menu.AddItem(new GUIContent("Copy"), false, _ => data.action = Action.copy,
                        property.managedReferenceValue != null);
                    menu.AddItem(new GUIContent("Paste"), false, _ => data.action = Action.paste, clipboard != null);

                    menu.ShowAsContext();
                }
            }
            position.y += lineStep;
        }

        private string GetCategory(Type subType)
        {
            CategoryAttribute category = subType.GetAttribute<CategoryAttribute>(true);
            if (category == null) return "";
            return category.Category + "/";
        }

        private void ExecuteButtonActions(SerializedProperty property, InstanceData data)
        {
            Action action = data.action;
            data.action = Action.none; // do this before the execution, so we don't try again next frame in case of an exception
            switch (action)
            {
                case Action.copy:
                    clipboard = Util.DeepCopy(property.managedReferenceValue);
                    break;
                case Action.paste:
                    if (!GetRealFieldType().IsAssignableFrom(clipboard.GetType()))
                    {
                        Debug.LogWarning("Can't assign copied object, which has type "
                                         + clipboard.GetType().Name + ", to target field of type "
                                         + GetRealFieldType().Name + ".");
                        break;
                    }
                    data.newValue = Util.DeepCopy(clipboard);
                    break;
                default:
                    break;
            }
            if (data.newValue != null)
            {
                property.managedReferenceValue = data.newValue;
                data.newValue = null;
            }
        }

        private void DisplaySubProperties(ref Rect position, SerializedProperty property)
        {
            EditorGUI.indentLevel++;

            var currentProperty = property.Copy();
            if (currentProperty.Next(true))
            {
                var nextProperty = property.Copy();
                if (!nextProperty.Next(false)) nextProperty = null;
                string str = "";
                while (!SerializedProperty.EqualContents(currentProperty, nextProperty))
                {
                    //if (currentProperty.propertyType == SerializedPropertyType.ManagedReference)
                    //    str += currentProperty.managedReferenceValue.GetType().Name + "/" + nextProperty.managedReferenceValue.GetType().Name + "\n";
                    str += currentProperty.displayName + "/" + nextProperty?.displayName + "\n";
                    EditorGUI.PropertyField(position, currentProperty, true);
                    position.y += EditorGUI.GetPropertyHeight(currentProperty, true);
                    if (!currentProperty.Next(false)) break;
                }
            }

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!instanceDataByPath.ContainsKey(property.propertyPath))
            {
                // Sometimes this happens, because Unity calls
                // GetPropertyHeight before OnGUI. But the next time it gets
                // called, we will have the height available. Unity seems to
                // redraw it multiple times in the beginning and when it gets
                // changed, so it's not noticeable that it's wrong the first
                // time.
                // Debug.Log("Height missing");
                return 1;
            }
            return instanceDataByPath[property.propertyPath].uiHeight;
        }

        protected Type GetRealFieldType()
        {
            Type type = fieldInfo.FieldType;
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(List<>))
            {
                // Workaround: This is the drawer for DrawableAbstractClass,
                // so fieldInfo.FieldType should always be a sub-type of
                // that. However, Unity reports the field type of a list
                // element as the type of the list, instead of the actual
                // type of the element. Since the actual List type doesn't
                // inherit from DrawableAbstractClass, this drawer will never
                // accidentally be used for an actual list. Therefore, if the
                // above clause is true, we need to adjust the type.
                type = type.GetGenericArguments()[0];
            }
            if (type.IsArray)
            {
                // same for arrays
                type = type.GetElementType();
            }
            return type;
        }

        protected class InstanceData
        {
            public object newValue = null;
            public Action action = Action.none;
            public SerializedProperty property;
            public int uiHeight;
        }

        protected enum Action
        {
            none, copy, paste
        }
    }
}
