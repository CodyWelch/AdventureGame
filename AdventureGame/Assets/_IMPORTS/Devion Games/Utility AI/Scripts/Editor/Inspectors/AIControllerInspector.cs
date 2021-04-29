using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames.AI
{
    [CustomEditor(typeof(AIController))]
    public class AIControllerInspector : Editor
    {
        private IntelligenceDefinition m_ÍntelligenceDefinition;

        private void OnEnable()
        {
            if (target == null) return;

            this.m_ÍntelligenceDefinition = serializedObject.FindProperty("m_ÍntelligenceDefinition").objectReferenceValue as IntelligenceDefinition;
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (EditorApplication.isPlaying && this.m_ÍntelligenceDefinition != null) {

               /* float maxScore = 0f;
                for (int i = 0; i < this.m_ÍntelligenceDefinition.DecisionMakers.Count; i++)
                {
                    DecisionMaker maker = this.m_ÍntelligenceDefinition.DecisionMakers[i];
                    DecisionScoreEvaluator evaluator;
                    maxScore += maker.Score(out evaluator);
                }*/

                for (int i = 0; i < this.m_ÍntelligenceDefinition.DecisionMakers.Count; i++)
                {
                    DecisionMaker maker = this.m_ÍntelligenceDefinition.DecisionMakers[i];
                    for (int j = 0; j < maker.DecisionScoreEvaluators.Count; j++)
                    {
                        DecisionScoreEvaluator evaluator = maker.DecisionScoreEvaluators[j];
                        GUILayout.BeginHorizontal();
                        Color color = GUI.color;
                        GUI.color = (target as AIController).m_CurrentDecisionScoreEvaluator == evaluator ? Color.green : color;
                        GUILayout.Label(evaluator.Name);
                        GUI.color = color;
                        var rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
     
                        rect.x = EditorGUIUtility.labelWidth;
                        rect.width = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth-20f;
                        float currentScore = evaluator.score;
                        float progress = currentScore/ evaluator.Weight;
                        EditorGUI.ProgressBar(rect, progress, currentScore.ToString());

                        GUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
}