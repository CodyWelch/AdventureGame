using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevionGames.CharacterSystem
{
    public class CharacterSlot : UICharacter, IPointerClickHandler
    {
        [SerializeField]
        protected Image m_SelectionOutline;

        private CharacterWindow m_Container;
        /// <summary>
        /// The character window that holds this slot
        /// </summary>
        public CharacterWindow Container
        {
            get { return this.m_Container; }
            set { this.m_Container = value; }
        }

        private int m_Index = -1;
        /// <summary>
        /// Index of character
        /// </summary>
        public int Index
        {
            get { return this.m_Index; }
            set { this.m_Index = value; }
        }

        private Character m_Character;
        /// <summary>
        /// The character this slot is holding
        /// </summary>
        public Character ObservedCharacter
        {
            get
            {
                return this.m_Character;
            }
            set
            {
                this.m_Character = value;
                Repaint();
            }
        }

        /// <summary>
        /// Checks if the slot is empty ObservedCharacter == null
        /// </summary>
        public bool IsEmpty
        {
            get { return ObservedCharacter == null; }
        }

        public override void Repaint() {
            if (this.m_CharacterName != null)
            {
                this.m_CharacterName.text = (!IsEmpty ? ObservedCharacter.CharacterName : string.Empty);
            }

            if (this.m_ProfessionName != null)
            {
                this.m_ProfessionName.text = (!IsEmpty ? ObservedCharacter.Name : string.Empty);
            }

            if (this.m_Description != null && Container.SelectedCharacter == ObservedCharacter)
            {
                this.m_Description.text = (!IsEmpty ? ObservedCharacter.Description : string.Empty);
            }

            if (this.m_Ícon != null)
            {
                if (!IsEmpty)
                {
                    this.m_Ícon.overrideSprite = ObservedCharacter.CreateCharacterImage;
                    this.m_Ícon.enabled = true;
                }
                else
                {
                    this.m_Ícon.enabled = false;
                }
            }

            if (m_SelectionOutline != null) {
                this.m_SelectionOutline.enabled =  (Container.SelectedCharacter == ObservedCharacter? true : false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(ObservedCharacter != null)
                Container.OnSelectionChange(ObservedCharacter);
        }
    }
}