using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Target/Has Line Of Sight")]
    [System.Serializable]
    public class HasLineOfSight : BoolConsideration, IOptionConsideration
    {
        [SerializeField]
        protected string m_Target = "PickerEntity";
        [SerializeField]
        protected Vector3Variable m_Offset = Vector3.up;
        [SerializeField]
        protected LayerMask m_LayerMask = Physics.DefaultRaycastLayers;

        public override float Score(Blackboard blackboard)
        {
            GameObject target = blackboard.GetValue<GameObject>(this.m_Target);
            if (target == null) {
                return this.m_InvertResult ? this.m_Score : 0f;
            }

            Transform self = blackboard.GetValue<GameObject>("Self").transform;
            RaycastHit hit;
            if (Physics.Linecast(self.position + blackboard.GetValue<Vector3>(this.m_Offset), target.transform.position + blackboard.GetValue<Vector3>(this.m_Offset), out hit, this.m_LayerMask))
            {
                if (hit.transform == target.transform)
                {
                    return this.m_InvertResult ? 0f : this.m_Score;
                }
            }
            return this.m_InvertResult ? this.m_Score : 0f;
        }
    }
}
