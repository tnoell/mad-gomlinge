using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace EditorRelated
{
    public class PropertyWindow : EditorWindow
    {
        private SerializedObject owningComponentObject;
        private MonoBehaviour owningComponent;
        private string propertyPath;

        private Vector2 propertyViewScrollPos;

        [MenuItem("Window/Property Editor")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(PropertyWindow)).Show();
        }

        public static void OpenProperty(SerializedProperty property)
        {
            PropertyWindow window = EditorWindow.GetWindow<PropertyWindow>();
            window.Initialize(property);
        }

        private void Initialize(SerializedProperty property)
        {
            owningComponentObject = property.serializedObject;
            owningComponent = owningComponentObject.targetObject as MonoBehaviour;
            owningComponentObject = new SerializedObject(owningComponent); // the old one will be invalidated some time after closing the inspector
            propertyPath = property.propertyPath;
            propertyViewScrollPos = Vector2.zero;
            Show(); // doesn't trigger OnEnable() call
        }

        public void OnEnable()
        {
            titleContent = new GUIContent("Property");
            if (owningComponent != null)
            {
                owningComponentObject = new SerializedObject(owningComponent); // the old one is invalidated after recompiling
            }
        }

        private void OnInspectorUpdate() // can't directly react to value being changed, for example through an undo, so this is the best option
        {
            Repaint();
        }

        void OnGUI()
        {
            SerializedProperty property = RefreshProperty();
            if (property == null) return;

            if (HandleNavigation()) return;

            propertyViewScrollPos = EditorGUILayout.BeginScrollView(propertyViewScrollPos);
            EditorGUILayout.PropertyField(property);
            EditorGUILayout.EndScrollView();

            owningComponentObject.ApplyModifiedProperties();
        }

        private SerializedProperty RefreshProperty()
        {
            if (owningComponent == null)
            {
                GUILayout.Label("No property selected", EditorStyles.boldLabel);
                return null;
            }
            owningComponentObject.Update();
            SerializedProperty property;

            try
            {
                property = owningComponentObject.FindProperty(propertyPath);
            }
            catch (ObjectDisposedException) // object has disappeared, for example if the parent of a Behavior had its type changed
            {
                property = null;
                GUILayout.Label("No property selected", EditorStyles.boldLabel);
                return null;
            }
            return property;
        }

        private bool HandleNavigation()
        {
            SerializedProperty nextHigherProperty = GetNextHigherProperty();

            if (nextHigherProperty == null)
            {
                if (EditorGUILayout.LinkButton("Open property owner in Inspector"))
                {
                    Selection.activeObject = owningComponentObject.targetObject;
                }
            }
            else
            {
                if (EditorGUILayout.LinkButton("Go up"))
                {
                    Initialize(nextHigherProperty);
                    return true;
                }
            }
            return false;
        }

        private SerializedProperty GetNextHigherProperty()
        {
            SerializedProperty nextHigherProperty;
            for (int pathLastPeriodPos = propertyPath.LastIndexOf('.');
                 pathLastPeriodPos != -1;
                 pathLastPeriodPos = propertyPath.LastIndexOf('.', pathLastPeriodPos - 1))
            {
                string newPath = propertyPath.Substring(0, pathLastPeriodPos);
                nextHigherProperty = owningComponentObject.FindProperty(newPath);
                if (nextHigherProperty.isArray) continue;
                return nextHigherProperty;
            }
            return null;
        }
    }
}
