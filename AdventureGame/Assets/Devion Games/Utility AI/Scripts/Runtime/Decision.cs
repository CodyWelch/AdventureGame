using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    [System.Serializable]
    public abstract class Decision : ScriptableObject, INameable
    {
        [InspectorLabel("Name")]
        [SerializeField]
        private string m_DecisionName = "New Decision";
        public string Name { get => this.m_DecisionName; set => this.m_DecisionName = value; }
        [TextArea(3, 6)]
        [SerializeField]
        protected string m_Description;

        [System.NonSerialized]
        public Blackboard blackboard;

        public abstract bool IsFinished();

        public virtual void OnSelect() {}

        public virtual void OnDeselect() { }

        public virtual void Execute() {}
    }
}