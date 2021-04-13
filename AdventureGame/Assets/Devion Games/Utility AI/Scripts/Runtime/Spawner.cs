using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DevionGames.AI
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        protected int m_Min = 1;
        [SerializeField]
        protected int m_Max = 3;
        [SerializeField]
        protected float m_Radius = 8f;
        [SerializeField]
        protected List<GameObject> m_Objects;

        protected List<GameObject> m_CurrentObjects;

        protected virtual void Start() {
            this.m_CurrentObjects = new List<GameObject>();
            StartCoroutine(CheckSpawn());
        }

        protected virtual IEnumerator CheckSpawn() {
            while (true) {
                this.m_CurrentObjects.RemoveAll(x => x == null);
                int amount = Random.Range(this.m_Min, this.m_Max);
                if (this.m_CurrentObjects.Count < amount) {
                    Spawn(amount);
                }
                yield return new WaitForSeconds(5f);
            }
        }

        protected virtual void Spawn(int amount) {
            for (int i = 0; i < amount; i++) {
                Vector3 position = GetSpawnPosition();
                GameObject original = this.m_Objects[Random.Range(0, this.m_Objects.Count)];
                GameObject go = Instantiate(original, position, Quaternion.Euler(0f, Random.Range(0, 360f), 0f));
                this.m_CurrentObjects.Add(go);
            }
        }

        protected virtual Vector3 GetSpawnPosition() {
            Vector3 position = transform.position + Random.insideUnitSphere * this.m_Radius;
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(position, out navHit, this.m_Radius, -1))
            {
                return navHit.position;
            }
            return position;
        }

        private void OnDrawGizmosSelected()
        {
            #if UNITY_EDITOR
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, this.m_Radius);
            #endif
        }
    }
}