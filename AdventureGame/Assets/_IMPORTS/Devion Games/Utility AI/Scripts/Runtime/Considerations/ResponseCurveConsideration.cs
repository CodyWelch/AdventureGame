using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    [System.Serializable]
    public abstract class ResponseCurveConsideration : Consideration
    {
        [SerializeField]
        protected AnimationCurve m_Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1, 1) });

        public float ComputeResponseCurve(float score)
        {
            return this.m_Curve.Evaluate(score);
        }
    }
}