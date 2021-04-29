using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.AI
{
    public class Ragdoll : MonoBehaviour
    {
        private List<Collider> m_Ragdoll = new List<Collider>();
        private Animator m_Animator;
        private Collider m_Collider;
        private void Awake()
        {
            this.m_Animator = GetComponent<Animator>();
            this.m_Collider = GetComponent<Collider>();
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    this.m_Ragdoll.Add(collider);
                }
            }

            SetRagdollActive(false);

        }

        public void SetRagdollActive(bool state)
        {
            this.m_Animator.enabled = !state;
            this.m_Collider.enabled = !state;
            foreach (Collider collider in this.m_Ragdoll)
            {
                collider.isTrigger = !state;
                collider.attachedRigidbody.isKinematic = !state;
            }
        }
    }
}