using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DevionGames.AI
{
    [CreateAssetMenu(fileName = "IntelligenceDefinition", menuName = "Devion Games/AI/Intelligence Definition")]
    [System.Serializable]
    public class IntelligenceDefinition : ScriptableObject
    {
        [TextArea(3,6)]
        [SerializeField]
        protected string m_Description = string.Empty;

        [DecisionMakerPicker(true)]
        [SerializeField]
        protected List<DecisionMaker> m_DecisionMakers = new List<DecisionMaker>();

        public List<DecisionMaker> DecisionMakers
        {
            get => this.m_DecisionMakers;
            set => this.m_DecisionMakers = value;
        }

        private Blackboard m_Blackboard;

        public DecisionScoreEvaluator Select() {
            float maximumScore = 0f;

            DecisionScoreEvaluator selectedDecisionScoreEvaluator = null;
            for (int i = 0; i < this.m_DecisionMakers.Count; i++)
            {
                DecisionScoreEvaluator decisionScoreEvaluator;
                float score = this.m_DecisionMakers[i].Score(out decisionScoreEvaluator);
                if (score > maximumScore && decisionScoreEvaluator != null)
                {
                    maximumScore = score;
                    selectedDecisionScoreEvaluator = decisionScoreEvaluator;
                }
            }
            return selectedDecisionScoreEvaluator;
        }

        public IntelligenceDefinition Clone(Blackboard blackboard) {
            IntelligenceDefinition intelligenceDefinition = ScriptableObject.CreateInstance<IntelligenceDefinition>();
            intelligenceDefinition.m_Blackboard = blackboard;

            for (int i = 0; i < DecisionMakers.Count; i++) {
                DecisionMaker decisionMaker = DecisionMakers[i].Clone(blackboard);
                intelligenceDefinition.m_DecisionMakers.Add(decisionMaker);
            }
            return intelligenceDefinition;
        }
    }
}