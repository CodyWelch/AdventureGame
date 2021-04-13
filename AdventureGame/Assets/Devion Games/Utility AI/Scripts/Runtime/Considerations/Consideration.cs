using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    /// <summary>
    /// Consideration evaluates the input and parameters to return a score.
    /// </summary>
    [System.Serializable]
    public abstract class Consideration : IConsideration
    {

        [HideInInspector]
        [SerializeField]
        private bool m_Enabled = true;
        public bool enabled
        {
            get { return this.m_Enabled; }
            set { this.m_Enabled = value; }
        }

        public abstract float Score(Blackboard blackboard);

        public virtual void OnSelect(Blackboard blackboard) { }
        public virtual void OnDeselect(Blackboard blackboard) { }

    }
}