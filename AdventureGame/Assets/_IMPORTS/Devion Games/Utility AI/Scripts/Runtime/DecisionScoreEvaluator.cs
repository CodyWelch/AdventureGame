using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DevionGames.AI
{
    [System.Serializable]
    public class DecisionScoreEvaluator : ScriptableObject, INameable
    {
        [InspectorLabel("Name")]
        [SerializeField]
        protected string m_DecisionScoreEvaluatorName = "Decision Score Evaluator";
        public string Name { get => this.m_DecisionScoreEvaluatorName; set => this.m_DecisionScoreEvaluatorName = value; }
        [TextArea(3, 6)]
        [SerializeField]
        protected string m_Description;

        [DecisionPicker(true)]
        [SerializeField]
        protected Decision m_Decision;
        [SerializeField]
        protected float m_Weight = 1f;
        public float Weight { get => this.m_Weight; set => this.m_Weight=value; }

        [SerializeField]
        protected float m_Threshold=0f;
        [SerializeField]
        protected int m_Group = 0;

        public int Group { get => this.m_Group; }
        public Decision Decision { get => this.m_Decision; set => this.m_Decision = value; }

        protected Blackboard m_Blackboard;
        [System.NonSerialized]
        public float score;

        [SerializeReference]
        protected List<IConsideration> m_Considerations= new List<IConsideration>();

        public List<IConsideration> Considerations {
            get { return this.m_Considerations; }
        }

        public float Score()
        {

            float finalScore = this.m_Weight;
            for (int i = 0; i < this.m_Considerations.Count; i++)
            {
                if (finalScore <= 0f)
                {
                    break;
                }
                IConsideration consideration = this.m_Considerations[i];
                if (!consideration.enabled)
                    continue;

                float score = consideration.Score(this.m_Blackboard);
                finalScore *= Mathf.Clamp01(ApplyCompensation(score));
            }
            if (finalScore < this.m_Threshold)
                return 0f;

            return finalScore;
        }

        private float ApplyCompensation(float score)
        {
            float modificationFactor = 1f - (1f / this.m_Considerations.Count);
            float makeUpValue = (1f - score) * modificationFactor;
            return score + (makeUpValue * score);
        }

        public void OnSelect() {
            this.m_Decision.OnSelect();
            for (int i = 0; i < this.m_Considerations.Count; i++)
            {
                this.m_Considerations[i].OnSelect(this.m_Blackboard);
            } 
        }

        public void OnDeselect() {
            this.m_Decision.OnDeselect();
            for (int i = 0; i < this.m_Considerations.Count; i++)
            {
                this.m_Considerations[i].OnDeselect(this.m_Blackboard);
            }
        }

        public DecisionScoreEvaluator Clone(Blackboard blackboard) {
            DecisionScoreEvaluator decisionScoreEvaluator = Instantiate(this);
            decisionScoreEvaluator.m_Blackboard = blackboard;
            decisionScoreEvaluator.Decision = Instantiate(decisionScoreEvaluator.Decision);
            decisionScoreEvaluator.Decision.blackboard = blackboard;
            return decisionScoreEvaluator;
        }

    }
}