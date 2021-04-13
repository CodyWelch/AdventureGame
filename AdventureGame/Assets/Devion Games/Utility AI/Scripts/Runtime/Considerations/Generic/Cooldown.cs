using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Generic/Cooldown")]
    [System.Serializable]
    public class Cooldown : BoolConsideration
    {
        [SerializeField]
        protected FloatVariable m_Min = 1f;
        [SerializeField]
        protected FloatVariable m_Max = 3f;

        protected float m_TimeStamp = 0f;

        public override float Score(Blackboard blackboard)
        {
            if (Time.time < this.m_TimeStamp)
                return this.m_InvertResult?this.m_Score : 0f;

            return this.m_InvertResult? 0f : this.m_Score;
        }

        public override void OnSelect(Blackboard blackboard)
        {
          //  this.m_TimeStamp = float.PositiveInfinity;
        }

        public override void OnDeselect(Blackboard blackboard)
        {
            float min = blackboard.GetValue<float>(this.m_Min);
            float max = blackboard.GetValue<float>(this.m_Max);
            float cooldown = Random.Range(min, max);
            this.m_TimeStamp = Time.time + cooldown;
        }
    }
}