using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevionGames.AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        protected IntelligenceDefinition m_ÍntelligenceDefinition;
        protected Blackboard m_Blackboard;

        private void Start()
        {
            this.m_Blackboard = GetComponent<Blackboard>();
            this.m_ÍntelligenceDefinition = this.m_ÍntelligenceDefinition.Clone(this.m_Blackboard);
            this.m_Blackboard.SetValue<GameObject>("Self", gameObject);
            this.m_Blackboard.SetValue<Vector3>("StartPosition", transform.position);
        }

        [System.NonSerialized]
        public DecisionScoreEvaluator m_CurrentDecisionScoreEvaluator;
        private Decision m_CurrentDecision;

        // Update is called once per frame
        private void Update()
        {
            if (this.m_CurrentDecision == null) {
                this.m_CurrentDecisionScoreEvaluator = this.m_ÍntelligenceDefinition.Select();
                this.m_CurrentDecisionScoreEvaluator.OnSelect();
                this.m_CurrentDecision = this.m_CurrentDecisionScoreEvaluator.Decision;
            }

            DecisionScoreEvaluator decisionScoreEvaluator = this.m_ÍntelligenceDefinition.Select();
            

            if (decisionScoreEvaluator != null)
            {
                Decision decision = decisionScoreEvaluator.Decision;

                if (this.m_CurrentDecision.IsFinished() || decision != this.m_CurrentDecision && decisionScoreEvaluator.Group > this.m_CurrentDecisionScoreEvaluator.Group)//this.m_CurrentDecision.interruptable)
                {
                    this.m_CurrentDecisionScoreEvaluator = decisionScoreEvaluator;
                    this.m_CurrentDecision = decision;
                    this.m_CurrentDecisionScoreEvaluator.OnSelect();     
                }
            }
            this.m_CurrentDecision.Execute();
            if (this.m_CurrentDecision.IsFinished())
            {
                this.m_CurrentDecisionScoreEvaluator.OnDeselect();
            }
        }
    }
}