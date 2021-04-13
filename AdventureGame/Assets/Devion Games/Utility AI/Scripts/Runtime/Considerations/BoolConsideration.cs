using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [System.Serializable]
    public abstract class BoolConsideration : Consideration
    {
        [SerializeField]
        protected bool m_InvertResult = false;
        [Range(0f, 1f)]
        [SerializeField]
        protected float m_Score = 1f;
    }
}