using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Target/Distance To Target")]
    [System.Serializable]
    public class DistanceToTarget : ResponseCurveConsideration, IOptionConsideration
    {
        [SerializeField]
        protected string m_Target = "PickerEntity";
        [SerializeField]
        protected FloatVariable m_Min = 0f;
        [SerializeField]
        protected FloatVariable m_Max = 1f;

        public override float Score(Blackboard blackboard)
        {
            GameObject target = blackboard.GetValue<GameObject>(this.m_Target);
            if (target == null)
                return 0f;

            Transform self = blackboard.GetValue<GameObject>("Self").transform;
            float min = blackboard.GetValue<float>(this.m_Min);
            float max = blackboard.GetValue<float>(this.m_Max);
            float distance = Vector3.Distance(self.position, target.transform.position);

            return ComputeResponseCurve(Mathf.Clamp01((distance - min) / (max - min)));
        }
    }
}