using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DevionGames.AI
{
    [CustomEditor(typeof(IntelligenceDefinition),true)]
    public class IntelligenceDefinitionInspector : Editor
    {
        protected SerializedProperty m_Description;
        protected SerializedProperty m_DecisionMakers;
        protected ReorderableList m_DecisionMakerList;

        private void OnEnable()
        {
            if (target == null) return;
            this.m_Description = serializedObject.FindProperty("m_Description");
            this.m_DecisionMakers = serializedObject.FindProperty("m_DecisionMakers");
            CreateDecisionMakerList();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Intelligence Definition is the brain of AI. All high level information will be assigned to it => It defines what a specie can do and how it acts.", MessageType.Info);
            serializedObject.Update();
            EditorGUILayout.PropertyField(this.m_Description);
            this.m_DecisionMakerList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void CreateDecisionMakerList()
        {
            this.m_DecisionMakerList = new ReorderableList(serializedObject, this.m_DecisionMakers, true, true, true, true);
            this.m_DecisionMakerList.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "Decision Makers");
            };

           /* this.m_DecisionMakerList.elementHeightCallback=(int index) => {
       
                return this.m_DecisionMakerList.elementHeight;
            };*/

            this.m_DecisionMakerList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                float verticalOffset = (rect.height - EditorGUIUtility.singleLineHeight) * 0.5f;
                rect.height = EditorGUIUtility.singleLineHeight;
                rect.y = rect.y + verticalOffset;
                SerializedProperty element = this.m_DecisionMakers.GetArrayElementAtIndex(index);

                EditorGUI.PropertyField(rect, element, GUIContent.none);
            };

            this.m_DecisionMakerList.onRemoveCallback = (ReorderableList list) =>
            {
                list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue = null;
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            };
        }
    }
}