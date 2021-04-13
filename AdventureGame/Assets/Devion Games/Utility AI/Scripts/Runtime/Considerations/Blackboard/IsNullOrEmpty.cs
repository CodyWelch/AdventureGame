using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Blackboard/Is Null Or Empty")]
    [System.Serializable]
    public class IsNullOrEmpty : BoolConsideration
    {
        [SerializeField]
        protected string m_Variable;

        public override float Score(Blackboard blackboard)
        {
            Variable variable = blackboard.GetVariable(this.m_Variable);
            if(variable == null)
                return this.m_InvertResult ? 0f : this.m_Score;

            if(variable is StringVariable && string.IsNullOrEmpty((string)variable.RawValue))
                return this.m_InvertResult ? 0f : this.m_Score;

            if (variable is ArrayListVariable && ((ArrayList)variable.RawValue).Count == 0)
                return this.m_InvertResult ? 0f : this.m_Score;

            if (variable.RawValue == null)
                return this.m_InvertResult ? 0f : this.m_Score;


            return this.m_InvertResult ? this.m_Score : 0f;

        }
    }
}