using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myRPG
{
    public class EnemyStats : CharacterStats
    {
        public int ID;
        public void Start()
        {
            ID = 0;
        }
        public override void Die()
        {
            base.Die();
            CombatEvents.EnemyDied(this);

            // add ragdoll effect / death animation

            Destroy(gameObject);
        }

    }

}


