using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    [System.Serializable]
    public class OptionPicker : Consideration
    {
        [SerializeField]
        protected string m_Options = "Targets";
        [SerializeField]
        protected string m_Variable = "Target";
        [SerializeReference]
        protected List<IOptionConsideration> m_Considerations;


        public override float Score(Blackboard blackboard)
        {
            ArrayList options = blackboard.GetValue<ArrayList>(this.m_Options);
            if (options == null)
                return 0f;

            float score;
            object option = GetBest(blackboard, options, out score);
            if (option != null){
                blackboard.SetValue(this.m_Variable, option, option.GetType());
            }/*else{
                blackboard.DeleteVariable(this.m_Variable);
            }*/

            return score;
        }

        public object GetBest(Blackboard blackboard, ArrayList options, out float score)
        {
            object option = null;
            score = 0f;
            int count = options.Count;
            for (int i = 0; i < count; i++)
            {
                object item = options[i];
                float finalScore = 1f;
                int num = this.m_Considerations.Count;
                for (int j = 0; j < num; j++)
                {
                    blackboard.SetValue("PickerEntity", item, item.GetType());
                    float optionScore = this.m_Considerations[j].Score(blackboard);
                    finalScore *= Mathf.Clamp01(optionScore);
                }

                if (finalScore > score)
                {
                    option = item;
                    score = finalScore;
                }
            }
            blackboard.DeleteVariable("PickerEntity");
            return option;
        }
    }
}