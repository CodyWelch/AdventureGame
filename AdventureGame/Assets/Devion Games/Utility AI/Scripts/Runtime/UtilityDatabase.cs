using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    [System.Serializable]
    public class UtilityDatabase : ScriptableObject
    {
        public List<Decision> decisions=new List<Decision>();
        public List<DecisionScoreEvaluator> decisionScoreEvaluators = new List<DecisionScoreEvaluator>();
        public List<DecisionMaker> decisionMakers = new List<DecisionMaker>();
        
    }
}