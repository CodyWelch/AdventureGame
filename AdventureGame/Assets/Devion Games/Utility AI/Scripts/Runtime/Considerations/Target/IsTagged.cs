using UnityEngine;

namespace DevionGames.AI.Considerations
{
    [ComponentMenu("Target/Is Tagged")]
    [System.Serializable]
    public class IsTagged : BoolConsideration,  IOptionConsideration
    {
        [SerializeField]
        protected string m_Target = "PickerEntity";
        [SerializeField]
        protected StringVariable m_Tag = "Player";

        public override float Score(Blackboard blackboard)
        {
            GameObject target = blackboard.GetValue<GameObject>(this.m_Target);
            string tag = blackboard.GetValue<string>((Variable)this.m_Tag);

            if (target != null && target.CompareTag(tag))
                return this.m_InvertResult ? 0f : this.m_Score;

            return this.m_InvertResult ? this.m_Score : 0f;
        }

      
    }
}