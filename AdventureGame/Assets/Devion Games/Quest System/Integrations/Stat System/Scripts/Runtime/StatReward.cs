using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevionGames.StatSystem;
using UnityEngine.UI;

namespace DevionGames.QuestSystem.Integrations.StatSystem
{
    public class StatReward : Reward
    {
        [SerializeField]
        private string m_StatsHandler = "Player Stats";
        [SerializeField]
        private string m_StatName = "Exp";
        [SerializeField]
        private float m_Value = 35f;
        [SerializeField]
        private StatRewardType m_RewardType = StatRewardType.CurrentValue;
        public override bool GiveReward()
        {

            StatsHandler handler = StatsManager.GetStatsHandler(this.m_StatsHandler);
            if (handler is null) return false;
            Stat stat = handler.GetStat(this.m_StatName);
            if (stat == null) return false;

            switch (this.m_RewardType)
            {
                case StatRewardType.CurrentValue:
                    handler.ApplyDamage(this.m_StatName, -this.m_Value);
                    break;
                case StatRewardType.Value:
                    stat.Add(this.m_Value);
                    break;
            }
            return true;
        }

        public override void DisplayReward(GameObject reward)
        {
            Text text = reward.GetComponent<Text>();

            text.text = (this.m_Value > 0 ? "+" : "-") + Mathf.Abs(this.m_Value).ToString() + " " + this.m_StatName;
        }

        public enum StatRewardType { 
            CurrentValue,
            Value
        }
    }
}