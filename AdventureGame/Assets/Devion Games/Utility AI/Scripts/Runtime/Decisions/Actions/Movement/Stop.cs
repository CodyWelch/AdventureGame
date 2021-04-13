using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.AI
{
    [Icon(typeof(NavMeshAgent))]
    [ComponentMenu("AI/Movement/Stop")]
    [System.Serializable]
    public class Stop : Action
    {
        protected NavMeshAgent m_Agent;
        public override void OnStart()
        {
            this.m_Agent = gameObject.GetComponent<NavMeshAgent>();
          
        }

        public override ActionStatus OnUpdate()
        {
            if(this.m_Agent.isActiveAndEnabled)
                this.m_Agent.ResetPath();
            return ActionStatus.Success;
        }
    }
}
