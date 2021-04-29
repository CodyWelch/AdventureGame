using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    [System.Serializable]
    public class CompositeDecision : Decision
    {
        [DecisionPicker(true)]
        [SerializeField]
        protected List<Decision> m_Decisions;

        public override bool IsFinished()
        {
            for (int i = 0; i < this.m_Decisions.Count; i++) {
                if (!this.m_Decisions[i].IsFinished())
                    return false;
            }
            return true;
        }

        public override void OnSelect()
        {
            for (int i = 0; i < this.m_Decisions.Count; i++)
            {
                this.m_Decisions[i].blackboard = blackboard;
                this.m_Decisions[i].OnSelect();
            }
        }

        public override void OnDeselect()
        {
            for (int i = 0; i < this.m_Decisions.Count; i++)
            {
                this.m_Decisions[i].OnDeselect();
            }
        }

        public override void Execute()
        {
            for (int i = 0; i < this.m_Decisions.Count; i++)
            {
                if (!this.m_Decisions[i].IsFinished())
                    this.m_Decisions[i].Execute();
            }
        }
    }
}