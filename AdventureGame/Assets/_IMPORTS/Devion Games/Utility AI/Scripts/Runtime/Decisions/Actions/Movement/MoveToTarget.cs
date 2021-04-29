using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.AI
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon(typeof(NavMeshAgent))]
    [ComponentMenu("AI/Movement/MoveToTarget")]
    [System.Serializable]
    public class MoveToTarget : Action
    {
        [SerializeField]
        protected string m_Target = "Target";
        [SerializeField]
        protected float m_Speed = 2.5f;
        [SerializeField]
        protected float m_AngularSpeed = 120f;

        protected GameObject m_TargetObject;
        protected NavMeshAgent m_Agent;
        

        public override void OnStart()
        {
            this.m_Agent = gameObject.GetComponent<NavMeshAgent>();
            this.m_Agent.speed = this.m_Speed;
            this.m_Agent.angularSpeed = this.m_AngularSpeed;
            this.m_TargetObject = blackboard.GetValue<GameObject>(this.m_Target);
        } 

        public override ActionStatus OnUpdate()
        {
            Transform target = this.m_TargetObject.transform;

            NavMeshPath path = new NavMeshPath();
            NavMeshHit hit;
            if (NavMesh.SamplePosition(target.position, out hit,float.PositiveInfinity, NavMesh.AllAreas))
            {
                if (!NavMesh.CalculatePath(this.m_Agent.transform.position, hit.position, NavMesh.AllAreas, path))
                {
                    Debug.Log(path.status);
                }
                this.m_Agent.SetPath(path);
            }
            return ActionStatus.Success;
        }
    }
}
