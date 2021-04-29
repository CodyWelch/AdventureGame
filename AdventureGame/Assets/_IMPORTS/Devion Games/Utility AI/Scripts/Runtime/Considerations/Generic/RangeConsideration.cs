using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Generic/Range")]
    [System.Serializable]
    public class RangeConsideration : ResponseCurveConsideration, IOptionConsideration
    {
        [Shared]
        [SerializeField]
        protected FloatVariable m_Current;
        [SerializeField]
        protected FloatVariable m_Min = 0f;
        [SerializeField]
        protected FloatVariable m_Max = 10f;

        public override float Score(Blackboard blackboard)
        {
            float current = blackboard.GetValue<float>(this.m_Current);
            return Score(blackboard, current);
        }

        public float Score(Blackboard blackboard, float option)
        {
            float min = blackboard.GetValue<float>(this.m_Min);
            float max = blackboard.GetValue<float>(this.m_Max);
            if ((max - min) <= 0f) return 0f;

            return ComputeResponseCurve(Mathf.Clamp01((option - min) / (max - min)));
        }
    }
}