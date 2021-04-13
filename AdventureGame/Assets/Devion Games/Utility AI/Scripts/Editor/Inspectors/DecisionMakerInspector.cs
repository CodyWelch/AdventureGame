using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DevionGames.AI
{
    [CustomEditor(typeof(DecisionMaker), true)]
    public class DecitionMakerInspector : Editor
    {
        protected SerializedProperty m_Name;
        protected SerializedProperty m_DecisionScoreEvaluators;
        protected ReorderableList m_DecisionScoreEvaluatorList;
        protected System.Action m_RemoveElement;


        protected virtual void OnEnable() {
            if (target == null) return;
            this.m_Name = serializedObject.FindProperty("m_DecisionMakerName");
            this.m_DecisionScoreEvaluators = serializedObject.FindProperty("m_DecisionScoreEvaluators");
            CreateDecisionScoreEvaluatorList();
        }

        public override void OnInspectorGUI()
        {
            if (target == null) return;
            serializedObject.Update();
            EditorGUILayout.PropertyField(this.m_Name);

            this.m_DecisionScoreEvaluatorList.DoLayoutList();

            GUILayout.FlexibleSpace();
            DoDecisionScoreEvaluatorAddButton();
            GUILayout.Space(10f);
            m_RemoveElement?.Invoke();
            m_RemoveElement = null;
            serializedObject.ApplyModifiedProperties();
        }

        

        protected void CreateDecisionScoreEvaluatorList()
        {
            this.m_DecisionScoreEvaluatorList = new ReorderableList(serializedObject, this.m_DecisionScoreEvaluators, true, false, false, false);
            this.m_DecisionScoreEvaluatorList.headerHeight = 0f;
            this.m_DecisionScoreEvaluatorList.footerHeight = 0f;
            this.m_DecisionScoreEvaluatorList.showDefaultBackground = false;
            this.m_DecisionScoreEvaluatorList.elementHeight=25;//default 21
            this.m_DecisionScoreEvaluatorList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.y += 2f;
                rect.height = 21f;
                rect.width -= 27f;
                SerializedProperty element = this.m_DecisionScoreEvaluators.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect,element, GUIContent.none);
                rect.x = rect.width + 27f;
                rect.width = 25f;
                if (GUI.Button(rect,EditorGUIUtility.IconContent("d_Toolbar Minus"),"label"))
                {
                    int i = index;
                    this.m_RemoveElement =()=> {
                        element.objectReferenceValue = null;
                        this.m_DecisionScoreEvaluators.DeleteArrayElementAtIndex(i); 
                    };
                }

            };
            this.m_DecisionScoreEvaluatorList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

                if (Event.current.type == EventType.Repaint)
                {
                    GUIStyle style = new GUIStyle("AnimItemBackground");
                    style.Draw(rect, false, isActive, isActive, isFocused);

                    GUIStyle style2 = new GUIStyle("RL Element");
                    style2.Draw(rect, false, isActive, isActive, isFocused);
                }
            };
        }

        private void DoDecisionScoreEvaluatorAddButton()
        {
            GUIStyle buttonStyle = new GUIStyle("AC Button");
            GUIContent buttonContent = new GUIContent("Add Decision Score Evaluator");
            Rect buttonRect = GUILayoutUtility.GetRect(buttonContent, buttonStyle, GUILayout.ExpandWidth(true));
            buttonRect.x = buttonRect.width * 0.5f - buttonStyle.fixedWidth * 0.5f;
            buttonRect.width = buttonStyle.fixedWidth;
            if (GUI.Button(buttonRect, buttonContent, buttonStyle))
            {
                ReorderableList.defaultBehaviours.DoAddButton(this.m_DecisionScoreEvaluatorList);
            }
        }

    }
}