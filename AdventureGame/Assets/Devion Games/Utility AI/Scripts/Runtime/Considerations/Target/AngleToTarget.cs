using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Target/Angle To Target")]
    [System.Serializable]
    public class AngleToTarget : ResponseCurveConsideration, IOptionConsideration
    {
        [SerializeField]
        protected string m_Target = "PickerEntity";
        [SerializeField]
        protected FloatVariable m_MinAngle = 10f;
        [SerializeField]
        protected FloatVariable m_MaxAngle = 60f;

        public override float Score(Blackboard blackboard)
        {
            GameObject target = blackboard.GetValue<GameObject>(this.m_Target);
            if (target == null) return 0f;

            Transform self = blackboard.GetValue<GameObject>("Self").transform;
            float current = Mathf.Abs(Vector3.Angle(target.transform.position - self.position, self.forward));
            float min = blackboard.GetValue<float>(this.m_MinAngle);
            float max = blackboard.GetValue<float>(this.m_MaxAngle);
            return ComputeResponseCurve(Mathf.Clamp01((current - min) / (max - min)));
        }
    }
}