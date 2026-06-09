using System;
using System.Linq;
using Gameplay.Abilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Gameplay.Core.Abilities.Editor
{
    [CustomEditor(typeof(AbilityConfig))]
    public class AbilityConfigEditor : UnityEditor.Editor
    {
        private Type[] _componentTypes;
        private int _selectedTypeIndex;
        
        private ReorderableList _reorderableList;
        private SerializedProperty _componentsProp;

        private void OnEnable()
        {
            // Reflection: Gather all concrete types implementing IAbilityComponentData for the dropdown
            _componentTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IAbilityComponentData).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToArray();

            // Cache the serialized property of the components list
            _componentsProp = serializedObject.FindProperty("_abilities");
            
            if (_componentsProp != null)
            {
                // Initialize ReorderableList: (serializedObject, property, draggable, displayHeader, displayAddButton, displayRemoveButton)
                _reorderableList = new ReorderableList(serializedObject, _componentsProp, true, true, false, true);

                // 1. Draw Header Callback
                _reorderableList.drawHeaderCallback = (Rect rect) => {
                    EditorGUI.LabelField(rect, "Ability Components", EditorStyles.boldLabel);
                };

                // 2. Draw Element Callback: Customizes how each item inside the list is rendered
                _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                    SerializedProperty elementProp = _componentsProp.GetArrayElementAtIndex(index);
                    
                    // Extract the clean C# class name from the managed reference type string
                    string typeName = GetFriendlyTypeName(elementProp.managedReferenceFullTypename);

                    // Offset the rect slightly to the right to prevent overlapping with the drag handles
                    rect.x += 10;
                    rect.width -= 10;

                    // Draw the element properties using the class name instead of the generic "Element X" label
                    EditorGUI.PropertyField(rect, elementProp, new GUIContent(typeName), true);
                };

                // 3. Element Height Callback: Dynamically calculates height so expanded fields don't overlap
                _reorderableList.elementHeightCallback = (int index) => {
                    SerializedProperty elementProp = _componentsProp.GetArrayElementAtIndex(index);
                    return EditorGUI.GetPropertyHeight(elementProp, true) + EditorGUIUtility.standardVerticalSpacing;
                };
            }
        }

        public override void OnInspectorGUI()
        {
            // Sync the serialized object's representation with the actual asset data
            serializedObject.Update();

            if (_reorderableList != null)
            {
                // Render the entire list with built-in drag handles, borders, and sorting behaviors
                _reorderableList.DoLayoutList();
            }
            else
            {
                EditorGUILayout.HelpBox("The field '_abilities' was not found! Please check its name in AbilityConfig.cs", MessageType.Warning);
            }

            // --- ADD NEW COMPONENT INTERFACE ---
            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("Selected Ability", EditorStyles.boldLabel);

            if (_componentTypes == null || _componentTypes.Length == 0) return;

            // Render a popup dropdown displaying all available component type names
            string[] typeNames = _componentTypes.Select(t => t.Name).ToArray();
            _selectedTypeIndex = EditorGUILayout.Popup("Component Type", _selectedTypeIndex, typeNames);

            if (GUILayout.Button("Add Component"))
            {
                if (_componentsProp == null) return;

                // Instantiate the selected C# data class via reflection
                var newComponent = Activator.CreateInstance(_componentTypes[_selectedTypeIndex]) as IAbilityComponentData;
                
                // Safely insert the new instance into the SerializeReference array
                int lastIndex = _componentsProp.arraySize;
                _componentsProp.InsertArrayElementAtIndex(lastIndex);
                _componentsProp.GetArrayElementAtIndex(lastIndex).managedReferenceValue = newComponent;
            }

            // Apply all modified properties back to the target ScriptableObject asset
            serializedObject.ApplyModifiedProperties();
        }

        // Helper method to strip namespace prefixes and assemblies from the full type name string
        private string GetFriendlyTypeName(string fullTypeName)
        {
            if (string.IsNullOrEmpty(fullTypeName)) return "Empty Component";
            
            // The string typically formatted as: "Assembly-CSharp Namespace.SubNamespace.ClassName"
            // We split by '.' and extract only the final substring (the class name itself)
            string[] parts = fullTypeName.Split('.');
            return parts.Length > 0 ? parts[parts.Length - 1] : fullTypeName;
        }
    }
}