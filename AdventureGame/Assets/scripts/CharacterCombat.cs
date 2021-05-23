using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace myRPG
{
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterCombat : MonoBehaviour
    {
        public float attackSpeed = 1f;
        private float attackCooldown = 0f;
        const float combatCooldown = 5;
        float lastAttackTime;
        float combatDistance = 10.0f;

        public float attackDelay = 0.6f;

        public bool InCombat { get; private set; }
        public event System.Action OnAttack;
        public float attackRange;

        CharacterStats myStats;
        CharacterStats opponentStats;
        NavMeshAgent agent;
        PlayerController controller;
        EnemyManager enemyManager;

        void Start()
        {
            enemyManager = EnemyManager.instance;
            myStats = GetComponent<CharacterStats>();
            agent = GetComponent<NavMeshAgent>();
            controller = GetComponent<PlayerController>();
        }

        void Update()
        {
            if(opponentStats!= null)
            {
                if(opponentStats.currentHealth>=0)
                {
                    Attack(opponentStats);
                }
            }
            attackCooldown -= Time.deltaTime;
            if(Time.time - lastAttackTime > combatCooldown)
            {
                InCombat = false;
            }

        }

        public void FollowEnemy()
        {

            /*this.playerAgent = playerAgent;
            playerAgent.stoppingDistance = 2f;
            playerAgent.destination = this.transform.position;

            Interact();*/
        }
        void OnDrawGizmos()
        {
            foreach (GameObject enemy in enemyManager.enemies)
            {

                if (Vector3.Distance(enemy.transform.position, this.transform.position) < combatDistance)
                {
                    // Draws a blue line from this transform to the target
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(this.gameObject.transform.position, enemy.transform.position);
                }
            }
        }

        // new attack method for animation events
        public void Target(CharacterStats targetStats)
        {
          opponentStats = targetStats;

        }

        // new attack method for animation events
        public void Attack(CharacterStats targetStats)
          {
            foreach(GameObject enemy in enemyManager.enemies)
            {
                  

                if (Vector3.Distance(targetStats.transform.position, this.transform.position) < combatDistance)
                {
                    InCombat = true;
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        if (Vector3.Distance(targetStats.transform.position, this.transform.position) < attackRange)
                        {
                            if (attackCooldown <= 0f)
                            {
                                opponentStats = targetStats;
                                if (OnAttack != null)
                                    OnAttack();

                                attackCooldown = 1f / attackSpeed;
                                lastAttackTime = Time.time;
                            }
                        }
                    }
                }
            }
          }

        // Made obsolete my AnimationEvent
        /*IEnumerator DoDamage(CharacterStats stats, float delay)
        {
            yield return new WaitForSeconds(delay);

            stats.TakeDamage(myStats.damage.GetValue());
            if (stats.currentHealth <= 0)
            {
                InCombat = false;
            }
        }*/

            // Replaces DoDamage
            // Updates damage based on timing of animation
        public void AttackHit_AnimationEvent()
        {
            opponentStats.TakeDamage(myStats.damage.GetValue());
            if (opponentStats.currentHealth <= 0)
            {
                InCombat = false;
            }
        }
        
        //old attack method 
    /*   public void Attack(CharacterStats targetStats)
        {
            if (attackCooldown <= 0f)
            {
                StartCoroutine(DoDamage(targetStats, attackDelay));

                if (OnAttack != null)
                    OnAttack();

                targetStats.TakeDamage(myStats.damage.GetValue());
                attackCooldown = 1f / attackSpeed;
                InCombat = true;
                lastAttackTime = Time.time;
            }
        }*/


        
    }
}
