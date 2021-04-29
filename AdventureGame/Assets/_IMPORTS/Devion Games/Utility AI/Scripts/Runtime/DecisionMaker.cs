using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    [System.Serializable]
    public class DecisionMaker : ScriptableObject, INameable 
    {

        [SerializeField]
        private string m_DecisionMakerName = "New Decision Maker";
        public string Name { get => this.m_DecisionMakerName; set => this.m_DecisionMakerName = value; }

        [DecisionScoreEvaluatorPicker(true)]
        [SerializeField]
        protected List<DecisionScoreEvaluator> m_DecisionScoreEvaluators= new List<DecisionScoreEvaluator>();

        public List<DecisionScoreEvaluator> DecisionScoreEvaluators {
            get => this.m_DecisionScoreEvaluators;
            set => this.m_DecisionScoreEvaluators = value;
        }

        protected Blackboard m_Blackboard;

        public float Score(out DecisionScoreEvaluator decisionScoreEvaluator)
        {
            float maximumScore = 0f;
            decisionScoreEvaluator = null;
            for (int i = 0; i < this.m_DecisionScoreEvaluators.Count; i++)
            {
                DecisionScoreEvaluator evaluator = this.m_DecisionScoreEvaluators[i];

                float score = evaluator.Score();
                evaluator.score = score;
                if (score > maximumScore)
                {
                    maximumScore = score;
                    decisionScoreEvaluator = evaluator;
                }
            }

            return maximumScore;
        }

        public DecisionMaker Clone(Blackboard blackboard) {
            DecisionMaker decisionMaker = ScriptableObject.CreateInstance<DecisionMaker>();
            decisionMaker.m_Blackboard = blackboard;
            decisionMaker.Name = Name;
            for (int i = 0; i < DecisionScoreEvaluators.Count; i++) {
                DecisionScoreEvaluator decisionScoreEvaluator = DecisionScoreEvaluators[i].Clone(blackboard);
                decisionMaker.DecisionScoreEvaluators.Add(decisionScoreEvaluator);
            }
            return decisionMaker;
        }
    }
}