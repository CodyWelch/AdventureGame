using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.CharacterSystem
{
    public class CharacterWindow : UIWidget
    {
        [Header("Behaviour")]
        /// <summary>
        /// Sets the container as dynamic. Slots are instantiated at runtime.
        /// </summary>
        [SerializeField]
        protected bool m_DynamicContainer = false;
        /// <summary>
        /// The parent transform of slots. 
        /// </summary>
        [SerializeField]
        protected Transform m_SlotParent;
        /// <summary>
        /// The slot prefab. This game object should contain the Slot component or a child class of Slot. 
        /// </summary>
        [SerializeField]
        protected GameObject m_SlotPrefab;

        [Header("Reference")]
        [SerializeField]
        protected Transform m_Spawnpoint;

        protected List<CharacterSlot> m_Slots = new List<CharacterSlot>();
        /// <summary>
        /// Collection of slots this container is holding
        /// </summary>
        public ReadOnlyCollection<CharacterSlot> Slots
        {
            get
            {
                return this.m_Slots.AsReadOnly();
            }
        }

        /// The selected character.
        /// </summary>
        protected Character m_SelectedCharacter;
        public Character SelectedCharacter
        {
            get { return this.m_SelectedCharacter; }
        }

        private Dictionary<Character, GameObject> m_CharacterCache;


        protected override void OnAwake()
        {
            RefreshSlots();
            this.m_CharacterCache = new Dictionary<Character, GameObject>();
        }

        /// <summary>
        /// Called on selection change.
        /// </summary>
        /// <param name="character">Character.</param>
        public virtual void OnSelectionChange(Character character)
        {
            this.m_SelectedCharacter = character;
            ShowCharacter(character);
            Repaint();
        }

        private void ShowCharacter(Character character) {
            if (this.m_Spawnpoint == null) {
                return;
            }
            GameObject go = null;
            if (!this.m_CharacterCache.TryGetValue(character, out go))
            {
                go = Instantiate(character.Prefab, this.m_Spawnpoint.position, this.m_Spawnpoint.rotation, this.m_Spawnpoint);
                Component[] components = go.GetComponents<Component>().Where(x=>x.GetType() != typeof(Animator) && x.GetType() != typeof(Transform)).ToArray();
                for (int i = 0; i < components.Length; i++) {
                    Destroy(components[i]);
                }
                this.m_CharacterCache.Add(character, go);
            }
            foreach (KeyValuePair<Character, GameObject> kvp in this.m_CharacterCache) {
                if (kvp.Key != character)
                {
                    kvp.Value.SetActive(false);
                }
            }
            go.SetActive(true);
        }

        public virtual void Repaint()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].Repaint();
            }
        }

        /// <summary>
        /// Refreshs the slot list and reorganize indices. This method is slow!
        /// </summary>
        public void RefreshSlots()
        {
            if (this.m_DynamicContainer && this.m_SlotParent != null)
            {
                this.m_Slots = this.m_SlotParent.GetComponentsInChildren<CharacterSlot>(true).Where(x => x.GetComponentsInParent<CharacterWindow>(true).FirstOrDefault() == this).ToList();
                this.m_Slots.Remove(this.m_SlotPrefab.GetComponent<CharacterSlot>());
            }
            else
            {
                this.m_Slots = GetComponentsInChildren<CharacterSlot>(true).Where(x => x.GetComponentsInParent<CharacterWindow>(true).FirstOrDefault() == this).ToList();
            }

            for (int i = 0; i < this.m_Slots.Count; i++)
            {
                CharacterSlot slot = this.m_Slots[i];
                slot.Index = i;
                slot.Container = this;
            }
        }

        /// Creates a new slot
        /// </summary>
        protected virtual CharacterSlot CreateSlot()
        {
            if (this.m_SlotPrefab != null && this.m_SlotParent != null)
            {
                GameObject go = (GameObject)Instantiate(this.m_SlotPrefab);
                go.SetActive(true);
                go.transform.SetParent(this.m_SlotParent, false);
                CharacterSlot slot = go.GetComponent<CharacterSlot>();
                this.m_Slots.Add(slot);
                slot.Index = Slots.Count - 1;
                slot.Container = this;
                return slot;
            }
            Debug.LogWarning("Please ensure that the slot prefab and slot parent is set in the inspector.");
            return null;
        }

        /// <summary>
        /// Destroy the slot and reorganize indices.
        /// </summary>
        /// <param name="slotID">Slot I.</param>
        protected virtual void DestroySlot(int index)
        {
            if (index < this.m_Slots.Count)
            {
                DestroyImmediate(this.m_Slots[index].gameObject);
                RefreshSlots();
            }
        }


        public void Add(Character[] characters)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                Add(characters[i]);
            }
        }

        public CharacterSlot Add(Character character)
        {
            for (int i = 0; i < this.Slots.Count; i++)
            {
                if (this.Slots[i].IsEmpty)
                {
                    this.Slots[i].ObservedCharacter = character;
                    return this.Slots[i];
                }
            }

            if (this.m_DynamicContainer)
            {
                CharacterSlot slot = CreateSlot();
                slot.ObservedCharacter = character;
                return slot;
            }
            return null;
        }

        public bool Remove(Character character)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (Slots[i].ObservedCharacter == character)
                {
                    Slots[i].ObservedCharacter = null;
                    if (m_DynamicContainer)
                    {
                        DestroySlot(i);
                    }
                    return true;
                }
            }
            return false;
        }

        public void Insert(int index, Character child)
        {
            Slots[index].ObservedCharacter = child;
        }

        public void RemoveAt(int index)
        {
            Slots[index].ObservedCharacter = null;
        }

        public void Clear()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                Slots[i].ObservedCharacter = null;
            }
            if (m_DynamicContainer)
            {
                for (int i = this.Slots.Count - 1; i >= 0; i--)
                {
                    DestroySlot(i);
                }
            }
        }
    }
}