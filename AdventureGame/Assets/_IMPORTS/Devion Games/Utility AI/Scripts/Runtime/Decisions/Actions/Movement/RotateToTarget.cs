using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.AI
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon(typeof(NavMeshAgent))]
    [ComponentMenu("AI/Movement/RotateToTarget")]
    [System.Serializable]
    public class RotateToTarget : Action
    {
        [SerializeField]
        protected string m_Target = "Target";
        [SerializeField]
        protected float m_RotationSpeed = 120f;

        protected Transform m_Transform;
        protected Transform m_TargetTransform;

        public override void OnStart()
        {
            this.m_Transform = gameObject.transform;
            this.m_TargetTransform = blackboard.GetValue<GameObject>(this.m_Target).transform;


        } 

        public override ActionStatus OnUpdate()
        {
            Vector3 targetDirection = (this.m_TargetTransform.position - this.m_Transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            this.m_Transform.rotation = Quaternion.RotateTowards(this.m_Transform.rotation, targetRotation, Time.deltaTime*this.m_RotationSpeed);

            return ActionStatus.Success;
        }
    }
}
