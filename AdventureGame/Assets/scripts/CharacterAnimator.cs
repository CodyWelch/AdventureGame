using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace myRPG
{
    public class CharacterAnimator : MonoBehaviour
    {
        public AnimationClip replaceableAttackAnim;
        public AnimationClip[] defaultAttackAnimSet;
        protected AnimationClip[] currentAttackAnimSet;

        const float locomotionAnimationSmoothTime = .1f;

        NavMeshAgent agent;
        protected Animator animator;
        protected CharacterCombat combat;
        protected AnimatorOverrideController overrideController;

        protected virtual void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            combat = GetComponent<CharacterCombat>();

            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;
            
            currentAttackAnimSet = defaultAttackAnimSet;
            combat.OnAttack += OnAttack;
            //overrideController("Punch");
           
        }

        protected virtual void Update()
        {
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);

            animator.SetBool("inCombat", combat.InCombat);
        }

        protected virtual void OnAttack()
        {
            animator.SetTrigger("attack");
            int attackIndex = Random.Range(0, currentAttackAnimSet.Length);
            overrideController[replaceableAttackAnim.name] = currentAttackAnimSet[attackIndex];
        }
    }
}