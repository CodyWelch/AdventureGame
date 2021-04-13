using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.AI
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon(typeof(NavMeshAgent))]
    [ComponentMenu("AI/Movement/MoveToPosition")]
    [System.Serializable]
    public class MoveToPosition : Action
    {
        [SerializeField]
        protected Vector3Variable m_PositionVariable;
        [SerializeField]
        protected float m_Speed = 2.5f;
        [SerializeField]
        protected float m_AngularSpeed = 120f;
        [SerializeField]
        private float m_Threshold = 1f;

        protected Vector3 m_TargetPosition;
        protected NavMeshAgent m_Agent;

        public override void OnStart()
        {
            this.m_Agent = gameObject.GetComponent<NavMeshAgent>();
            this.m_Agent.speed = this.m_Speed;
            this.m_Agent.angularSpeed = this.m_AngularSpeed;
            this.m_TargetPosition = blackboard.GetValue<Vector3>(this.m_PositionVariable);

            NavMeshPath path = new NavMeshPath();
            NavMeshHit hit;
            if (NavMesh.SamplePosition(this.m_TargetPosition, out hit, float.PositiveInfinity, NavMesh.AllAreas))
            {
                if (!NavMesh.CalculatePath(this.m_Agent.transform.position, hit.position, NavMesh.AllAreas, path))
                {
                    Debug.Log(path.status);
                }
                this.m_Agent.SetPath(path);
            }

        } 

        public override ActionStatus OnUpdate()
        {
            return this.m_Agent.remainingDistance < this.m_Threshold ? ActionStatus.Success : ActionStatus.Running;
        }
    }
}
