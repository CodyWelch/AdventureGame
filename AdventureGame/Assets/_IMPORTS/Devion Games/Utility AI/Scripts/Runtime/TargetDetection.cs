using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevionGames.AI
{
    public class TargetDetection : MonoBehaviour
    {
        [SerializeField]
        protected List<string> m_Tags = new List<string>() {"Player"};
        [SerializeField]
        protected float m_Radius = 20f;
        [SerializeField]
        protected string m_TargetsVariable = "Targets";

        [SerializeField]
        protected string m_SeenTargetsVariable = "SeenTargets";
        [Range(0f,360f)]
        [SerializeField]
        protected float m_FieldOfView = 90f;
        [SerializeField]
        protected float m_UpdateInterval = 0.3f;

        protected List<GameObject> m_Targets;
        protected List<GameObject> m_SeenTargets;

        protected Blackboard m_Blackboard;


        private void Start()
        {
            this.m_Blackboard = GetComponent<Blackboard>();
            this.m_Targets = new List<GameObject>();
            this.m_SeenTargets = new List<GameObject>();
            CreateDetectionCollider();
            InvokeRepeating("UpdateSeenTargets",0f, this.m_UpdateInterval);
            EventHandler.Register<GameObject, string, float>(gameObject,"OnGetHit", OnGetHit);
        }

        private void OnGetHit(GameObject from, string stat, float damage)
        {
            Debug.Log("OnGetHit "+from.name);
            if (!this.m_Targets.Contains(from))
            {
                this.m_Targets.Add(from);
                m_Blackboard.SetValue<ArrayList>(this.m_TargetsVariable, new ArrayList(this.m_Targets));
            }
            if (!this.m_SeenTargets.Contains(from))
            {
                this.m_SeenTargets.Add(from);
                this.m_Blackboard.SetValue<ArrayList>(this.m_SeenTargetsVariable, new ArrayList(this.m_SeenTargets));
            }
        }

        protected virtual void UpdateSeenTargets() {
            this.m_SeenTargets.RemoveAll(item => !this.m_Targets.Contains(item));
            for (int i = 0; i < this.m_Targets.Count; i++) {
                GameObject target = this.m_Targets[i]; 
                if (!this.m_SeenTargets.Contains(target) && CheckFieldOfView(target) && CheckLineOfSight(target) ) {
                    this.m_SeenTargets.Add(target);
                }
            }
            this.m_Blackboard.SetValue<ArrayList>(this.m_SeenTargetsVariable, new ArrayList(this.m_SeenTargets));
        }

        protected virtual bool CheckFieldOfView(GameObject target) {
            // Get direction to target
            Vector3 directionToTarget = target.transform.position - transform.position;
            // Get angle between forward and look direction
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            // Is target within field of view?
            if (angle <= this.m_FieldOfView*0.5f)
                return true;

            // Not within view
            return false;
        }

        protected virtual bool CheckLineOfSight(GameObject target)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out hit, this.m_Radius))
            {
                if (hit.transform == target.transform)
                    return true;
            }
            return false;
        }

        //OnTriggerEnter is called when the Collider other enters the trigger.
        protected virtual void OnTriggerEnter(Collider other)
        {
            //Check if the collider other has tag 
            if (isActiveAndEnabled && this.m_Tags.Contains(other.tag) && !this.m_Targets.Contains(other.gameObject))
            {
                this.m_Targets.Add(other.gameObject);
                m_Blackboard.SetValue<ArrayList>(this.m_TargetsVariable,new ArrayList(this.m_Targets));
            }
        }

        //OnTriggerExit is called when the Collider other has stopped touching the trigger.
        protected virtual void OnTriggerExit(Collider other)
        {
            //Check if the collider other has tag
            if (isActiveAndEnabled && this.m_Tags.Contains(other.tag))
            {
                this.m_Targets.Remove(other.gameObject);
                m_Blackboard.SetValue<ArrayList>(this.m_TargetsVariable, new ArrayList(this.m_Targets));
            }
        }

        protected virtual void CreateDetectionCollider()
        {
            Vector3 position = Vector3.zero;
            GameObject detectionGameObject = new GameObject("Target Detection");
            detectionGameObject.transform.SetParent(transform, false);
            detectionGameObject.layer = 2;

            Collider collider = GetComponent<Collider>();
            if (collider != null)
            {
                position = collider.bounds.center;
                position.y = (collider.bounds.center - collider.bounds.extents).y;
                position = transform.InverseTransformPoint(position);
            }

            SphereCollider sphereCollider = detectionGameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.center = position;
            Vector3 scale = transform.lossyScale;
            sphereCollider.radius = this.m_Radius / Mathf.Max(scale.x, scale.y, scale.z);

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.isKinematic = true;
            }
        }

        protected virtual void OnDrawGizmosSelected() {
#if UNITY_EDITOR
            var forward = transform.forward;
            Vector3 newForward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward);
            Vector3 up = Vector3.Cross(forward, right);
            Vector3 fromVec = Quaternion.AngleAxis(-0.5f * this.m_FieldOfView, up) * forward;
            UnityEditor.Handles.color = new Color(0.937f,0.498f,0.247f,0.15f);
            UnityEditor.Handles.DrawSolidArc(transform.position, up, fromVec, this.m_FieldOfView, this.m_Radius);

            UnityEditor.Handles.color= new Color(0.937f, 0.498f, 0.247f, 1f);
            UnityEditor.Handles.DrawWireArc(transform.position, up, fromVec, this.m_FieldOfView, this.m_Radius);
            if (this.m_FieldOfView < 360f)
            {
                UnityEditor.Handles.DrawLine(transform.position, transform.position + transform.forward * Mathf.Cos(this.m_FieldOfView * 0.5f * Mathf.Deg2Rad) * this.m_Radius + transform.right * Mathf.Sin(this.m_FieldOfView * 0.5f * Mathf.Deg2Rad) * this.m_Radius);
                UnityEditor.Handles.DrawLine(transform.position, transform.position + transform.forward * Mathf.Cos(-this.m_FieldOfView * 0.5f * Mathf.Deg2Rad) * this.m_Radius + transform.right * Mathf.Sin(-this.m_FieldOfView * 0.5f * Mathf.Deg2Rad) * this.m_Radius);
            }
#endif
        }

    }
}