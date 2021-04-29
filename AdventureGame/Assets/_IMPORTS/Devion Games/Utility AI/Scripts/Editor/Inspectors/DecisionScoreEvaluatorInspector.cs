using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames.AI
{
    [CustomEditor(typeof(DecisionScoreEvaluator),true)]
    public class DecisionScoreEvaluatorInspector : Editor
    {
        private SerializedProperty m_Scirpt;
       /* private SerializedProperty m_Name;
        private SerializedProperty m_Description;
        private SerializedProperty m_Weight;
        private SerializedProperty m_Threshold;
        private SerializedProperty m_Cooldown;
        private SerializedProperty m_Decision;*/
        private SerializedProperty m_Considerations;

        protected virtual void OnEnable() {
            if (target == null) return;
            this.m_Scirpt = serializedObject.FindProperty("m_Script");
            this.m_Considerations = serializedObject.FindProperty("m_Considerations");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(this.m_Scirpt);
            EditorGUI.EndDisabledGroup();

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject,this.m_Scirpt.propertyPath,this.m_Considerations.propertyPath);
            GUILayout.Space(5f);
            ConsiderationGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected void ConsiderationGUI()
        {
            EditorGUIUtility.wideMode = true;
            for (int i = 0; i < this.m_Considerations.arraySize; i++)
            {
                SerializedProperty action = this.m_Considerations.GetArrayElementAtIndex(i);

                object value = action.GetValue();
                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(target, "Consideration");
                if (EditorTools.Titlebar(value, ElementContextMenu(this.m_Considerations.GetValue() as IList, i)))
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
                if (EditorGUI.EndChangeCheck())
                    EditorUtility.SetDirty(target);
            }

            GUILayout.FlexibleSpace();
            DoActionAddButton();
            GUILayout.Space(10f);
        }

        private void AddConsideration(Type type)
        {
            object value = System.Activator.CreateInstance(type);
            this.m_Considerations.serializedObject.Update();
            this.m_Considerations.arraySize++;
            this.m_Considerations.GetArrayElementAtIndex(this.m_Considerations.arraySize - 1).managedReferenceValue = value;
            this.m_Considerations.serializedObject.ApplyModifiedProperties();
        }


        private void CreateConsiderationScript(string scriptName)
        {
            Debug.LogWarning("Not implemented yet.");
        }

        private void DoActionAddButton()
        {
            GUIStyle buttonStyle = new GUIStyle("AC Button");
            GUIContent buttonContent = new GUIContent("Add Consideration");
            Rect buttonRect = GUILayoutUtility.GetRect(buttonContent, buttonStyle, GUILayout.ExpandWidth(true));
            buttonRect.x = buttonRect.width * 0.5f - buttonStyle.fixedWidth * 0.5f;
            buttonRect.width = buttonStyle.fixedWidth;
            if (GUI.Button(buttonRect, buttonContent, buttonStyle))
            {
                AddObjectWindow.ShowWindow(buttonRect, typeof(IConsideration), AddConsideration, CreateConsiderationScript);
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
                EditorUtility.SetDirty(target);
            });
            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Remove"), false, delegate {
                list.RemoveAt(index);
                EditorUtility.SetDirty(target);
            });

            if (index > 0)
            {
                menu.AddItem(new GUIContent("Move Up"), false, delegate {
                    object value = list[index];
                    list.RemoveAt(index);
                    list.Insert(index - 1, value);
                    EditorUtility.SetDirty(target);
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
                    EditorUtility.SetDirty(target);
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