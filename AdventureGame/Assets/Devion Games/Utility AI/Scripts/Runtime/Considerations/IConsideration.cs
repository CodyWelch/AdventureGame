using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    public interface IConsideration 
    {
        bool enabled { get; }
        float Score(Blackboard blackboard);

        void OnSelect(Blackboard blackboard);
        void OnDeselect(Blackboard blackboard);
    }
}