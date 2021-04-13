using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DevionGames.AI
{
    [CustomEditor(typeof(CompositeDecision),true)]
    public class CompositeDecisionInspector : Editor
    {
        protected SerializedProperty m_Script;
        protected SerializedProperty m_CompositeDecisions;
        protected ReorderableList m_CompositeDecisionList;


        protected virtual void OnEnable() {
            this.m_Script = serializedObject.FindProperty("m_Script");
            this.m_CompositeDecisions = serializedObject.FindProperty("m_Decisions");
            CreateCompositeDecisionList(serializedObject,this.m_CompositeDecisions);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(this.m_Script);
            EditorGUI.EndDisabledGroup();

            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject,this.m_Script.propertyPath, this.m_CompositeDecisions.propertyPath);
            this.m_CompositeDecisionList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void CreateCompositeDecisionList(SerializedObject serializedObject, SerializedProperty elements)
        {
            this.m_CompositeDecisionList = new ReorderableList(serializedObject, elements, true, true, true, true);
            this.m_CompositeDecisionList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Decisions");
            };

            this.m_CompositeDecisionList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                float verticalOffset = (rect.height - EditorGUIUtility.singleLineHeight) * 0.5f;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y = rect.y + verticalOffset;
                SerializedProperty element = elements.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, element, GUIContent.none, true);
            };
        }

    }
}