using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myRPG
{

    public class KillGoal : QuestGoal
    {
        public int EnemyID { get; set; }

        public KillGoal(int enemyID, string description, bool completed, int currentAmount, int requiredAmount)
        {
            this.EnemyID = enemyID;
            this.Description = description;
            this.Completed = completed;
            this.CurrentAmount = currentAmount;
            this.RequiredAmount = requiredAmount;
        }

        public override void Init()
        {
            base.Init();
            CombatEvents.OnEnemyDeath += EnemyDied;
        }

        void EnemyDied(EnemyStats enemy)
        {
            if (enemy.ID == this.EnemyID)
            {
                this.CurrentAmount++;
                Evaluate();
            }
        }
    }
}


