using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DevionGames.AI
{
    [CustomPropertyDrawer(typeof(OptionPicker))]
    public class OptionPickerDrawer : PropertyDrawer
    {
        private SerializedProperty m_Options;
      
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.m_Options = property.FindPropertyRelative("m_Considerations");
            //EditorGUILayout.PropertyField(property.FindPropertyRelative("m_Curve"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("m_Options"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("m_Variable"));
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            ConsiderationGUI();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0f;
        }

        protected void ConsiderationGUI()
        {
            EditorGUIUtility.wideMode = true;
            for (int i = 0; i < this.m_Options.arraySize; i++)
            {
                SerializedProperty action = this.m_Options.GetArrayElementAtIndex(i);

                object value = action.GetValue();
                EditorGUI.BeginChangeCheck();

                if (EditorTools.Titlebar(value, ElementContextMenu(this.m_Options.GetValue() as IList, i)))
                {
                    EditorGUI.indentLevel += 1;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField("Script", value != null ? EditorTools.FindMonoScript(value.GetType()) : null, typeof(MonoScript), true);
                    EditorGUI.EndDisabledGroup();
                    if (value == null)
                    {
                        EditorGUILayout.HelpBox("Managed reference values can't be removed or replaced. Only way to fix it is to recreate the renamed or deleted script file or delete and recreate the Consideration. Unity throws an error: Unknown managed type referenced: [Assembly-CSharp] + Type which has been removed.", MessageType.Error);
                    }

                    if (EditorTools.HasCustomPropertyDrawer(value.GetType()))
                    {
                        EditorGUILayout.PropertyField(action, true);
                    }
                    else
                    {
                        foreach (var child in action.EnumerateChildProperties())
                        {
                            EditorGUILayout.PropertyField(
                                child,
                                includeChildren: true
                            );
                        }
                    }
                    EditorGUI.indentLevel -= 1;
                }
            }


            DoAddButton();
            GUILayout.Space(10f);
        }

        private void AddConsideration(Type type)
        {
            object value = System.Activator.CreateInstance(type);
            this.m_Options.serializedObject.Update();
            this.m_Options.arraySize++;
            this.m_Options.GetArrayElementAtIndex(this.m_Options.arraySize - 1).managedReferenceValue = value;
            this.m_Options.serializedObject.ApplyModifiedProperties();
        }


        private void CreateConsiderationScript(string scriptName)
        {
            Debug.LogWarning("Not implemented yet.");
        }

        private void DoAddButton()
        {
            GUIStyle buttonStyle = new GUIStyle("AC Button");
            GUIContent buttonContent = new GUIContent("Add Option Consideration");
            Rect buttonRect = GUILayoutUtility.GetRect(buttonContent, buttonStyle, GUILayout.ExpandWidth(true));
            buttonRect.x = buttonRect.width * 0.5f - buttonStyle.fixedWidth * 0.5f;
            buttonRect.width = buttonStyle.fixedWidth;
            if (GUI.Button(buttonRect, buttonContent, buttonStyle))
            {

                Type type = typeof(IOptionConsideration);
                AddObjectWindow.ShowWindow(buttonRect, type, AddConsideration, CreateConsiderationScript);
            }
        }

        private GenericMenu ElementContextMenu(IList list, int index)
        {

            GenericMenu menu = new GenericMenu();
            if (list[index] == null)
            {
                return menu;
            }
            Type elementType = list[index].GetType();
            menu.AddItem(new GUIContent("Reset"), false, delegate {

                object value = System.Activator.CreateInstance(list[index].GetType());
                list[index] = value;
                EditorUtility.SetDirty(this.m_Options.serializedObject.targetObject);
            });
            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Remove"), false, delegate {
                list.RemoveAt(index);
                EditorUtility.SetDirty(this.m_Options.serializedObject.targetObject);
            });

            if (index > 0)
            {
                menu.AddItem(new GUIContent("Move Up"), false, delegate {
                    object value = list[index];
                    list.RemoveAt(index);
                    list.Insert(index - 1, value);
                    EditorUtility.SetDirty(this.m_Options.serializedObject.targetObject);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Move Up"));
            }

            if (index < list.Count - 1)
            {
                menu.AddItem(new GUIContent("Move Down"), false, delegate
                {
                    object value = list[index];
                    list.RemoveAt(index);
                    list.Insert(index + 1, value);
                    EditorUtility.SetDirty(this.m_Options.serializedObject.targetObject);
                });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Move Down"));
            }

            if (list[index] != null)
            {
                MonoScript script = EditorTools.FindMonoScript(list[index].GetType());
                if (script != null)
                {
                    menu.AddSeparator(string.Empty);
                    menu.AddItem(new GUIContent("Edit Script"), false, delegate { AssetDatabase.OpenAsset(script); });
                }
            }
            return menu;
        }
    }
}