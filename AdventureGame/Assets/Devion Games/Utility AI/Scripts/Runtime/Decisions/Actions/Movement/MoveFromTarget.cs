using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.AI
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon(typeof(NavMeshAgent))]
    [ComponentMenu("AI/Movement/MoveFromTarget")]
    [System.Serializable]
    public class MoveFromTarget : Action
    {

        [SerializeField]
        protected string m_Target = "Target";
        [SerializeField]
        protected float m_Speed = 2.5f;
        [SerializeField]
        protected float m_AngularSpeed = 120f;

        protected NavMeshAgent m_Agent;
        protected GameObject m_TargetObject;

        public override void OnStart()
        {
            this.m_Agent = gameObject.GetComponent<NavMeshAgent>();
            this.m_Agent.speed = this.m_Speed;
            this.m_Agent.angularSpeed = this.m_AngularSpeed;
            this.m_TargetObject = blackboard.GetValue<GameObject>(this.m_Target);


        }

        public override ActionStatus OnUpdate()
        {
            Vector3 direction = this.m_TargetObject.transform.position - this.m_Agent.transform.position;
            Vector3 movePosition = this.m_Agent.transform.position + (direction * -1f);
            this.m_Agent.SetDestination(movePosition);
            return ActionStatus.Success;
        }
    }
}
