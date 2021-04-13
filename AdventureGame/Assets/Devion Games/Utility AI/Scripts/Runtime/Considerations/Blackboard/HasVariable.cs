using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Blackboard/Has Variable")]
    [System.Serializable]
    public class HasVariable : BoolConsideration
    {
        [SerializeField]
        protected string m_Variable;

        public override float Score(Blackboard blackboard)
        {
            Variable variable = blackboard.GetVariable(this.m_Variable);
            if (variable != null)
                return this.m_InvertResult ? 0f : this.m_Score;

            return this.m_InvertResult ? this.m_Score : 0f;

        }
    }
}