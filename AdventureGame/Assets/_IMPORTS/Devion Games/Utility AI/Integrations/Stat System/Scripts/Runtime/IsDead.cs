using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevionGames.StatSystem;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Stats/Is Dead")]
    [System.Serializable]
    public class IsDead : BoolConsideration, IOptionConsideration
    {
        [SerializeField]
        protected string m_Target = "PickerEntity";
        [SerializeField]
        protected StringVariable m_HealthStat = "Health";

        public override float Score(Blackboard blackboard)
        {
            GameObject target = blackboard.GetValue<GameObject>(this.m_Target);
            if (target == null)
                return this.m_InvertResult ? this.m_Score:0f;

            StatsHandler handler = target.GetComponent<StatsHandler>();
            if (handler == null)
                return this.m_InvertResult ? this.m_Score : 0f;

            Attribute stat = handler.GetStat(blackboard.GetValue<string>((Variable)this.m_HealthStat)) as Attribute;
            if (stat == null)
                return this.m_InvertResult ? this.m_Score : 0f;

            if(stat != null && stat.CurrentValue == 0f)
                return this.m_InvertResult ? 0f: this.m_Score;

            return this.m_InvertResult ? this.m_Score : 0f;
        }
    }
}