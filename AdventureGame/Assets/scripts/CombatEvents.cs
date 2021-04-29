using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myRPG
{
    public class CombatEvents : MonoBehaviour
    {
        public delegate void EnemyStatsEventHandler(EnemyStats enemyStats);
        public static event EnemyStatsEventHandler OnEnemyDeath;

        public static void EnemyDied(EnemyStats enemyStats)
        {
            if (OnEnemyDeath != null)
                OnEnemyDeath(enemyStats);
        }
    }
}
