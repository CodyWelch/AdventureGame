using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    [System.Serializable]
    public class SequenceDecision : Decision
    {
        [SerializeReference]
        private List<Action> m_Actions = new List<Action>();

        public List<Action> Actions { get => this.m_Actions; set => this.m_Actions = value; }


        private Sequence m_Sequence;
        private bool m_Finished;

        public override bool IsFinished()
        {
            return this.m_Finished;
        }

        public override void OnDeselect() {
            this.m_Sequence.Stop();
        }

        public override void OnSelect()
        {
            this.m_Sequence = new Sequence(blackboard.gameObject, new PlayerInfo("Player"), blackboard, Actions.ToArray());
            this.m_Sequence.Start();
        }

        public override void Execute()
        {
            this.m_Finished = !this.m_Sequence.Tick();
            this.m_Sequence.Update();
        }
    }
}