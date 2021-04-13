using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.AI
{
    [Icon(typeof(NavMeshAgent))]
    [ComponentMenu("AI/Movement/Wander")]
    [System.Serializable]
    public class Wander : Action
    {
        [SerializeField]
        protected float m_Speed = 1.5f;
        [SerializeField]
        protected float m_AngularSpeed = 100f;
        [SerializeField]
        private float m_WanderRadius = 8f;
        [SerializeField]
        private float m_Threshold = 1f;

        protected NavMeshAgent m_Agent;
        public override void OnStart()
        {
            this.m_Agent = gameObject.GetComponent<NavMeshAgent>();
            this.m_Agent.speed = this.m_Speed;
            this.m_Agent.angularSpeed = this.m_AngularSpeed;
            this.m_Agent.stoppingDistance = 0f;

            Vector3 nextPosition = GetNextPosition(blackboard.GetValue<Vector3>("StartPosition"));
            this.m_Agent.SetDestination(nextPosition);
        }

        public override ActionStatus OnUpdate()
        {
            if (this.m_Agent.pathPending) return ActionStatus.Running;

            return this.m_Agent.remainingDistance < this.m_Threshold? ActionStatus.Success: ActionStatus.Running;
        }

        private Vector3 GetNextPosition(Vector3 startPos)
        {
            Vector3 randomPosition = startPos + Random.insideUnitSphere * this.m_WanderRadius;
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomPosition, out navHit, this.m_WanderRadius, -1))
            {
                return navHit.position;
            }
            return startPos;
        }


    }
}
