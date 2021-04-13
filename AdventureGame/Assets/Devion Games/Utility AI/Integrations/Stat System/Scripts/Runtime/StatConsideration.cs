using DevionGames.StatSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Stats/Stat Consideration")]
    [System.Serializable]
    public class StatConsideration : ResponseCurveConsideration, IOptionConsideration
    {
        [SerializeField]
        protected string m_Target = "PickerEntity";
        [SerializeField]
        protected StringVariable m_Stat = "Health";
        public override float Score(Blackboard blackboard)
        {
            GameObject target = blackboard.GetValue<GameObject>(this.m_Target);
            if (target == null)
                return 0f;

            StatsHandler handler = target.GetComponent<StatsHandler>();
            if (handler == null)
                return 0f;

            Attribute stat = handler.GetStat(blackboard.GetValue<string>((Variable)this.m_Stat)) as Attribute;
            if (stat == null)
                return 0f;

            return ComputeResponseCurve(stat.CurrentValue/stat.Value);
        }
    }
}