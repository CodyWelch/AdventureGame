using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.CharacterSystem
{
    public class UICharacter : MonoBehaviour
    {
        /// The text to display character name.
        /// </summary>
        [InspectorLabel("Name")]
        [SerializeField]
        protected Text m_ProfessionName;
        /// <summary>
        /// The Image to display character icon.
        /// </summary>
        [SerializeField]
        protected Image m_Ícon;
        /// The text to display character description.
        /// </summary>
        [SerializeField]
        protected Text m_Description;
        /// The text to display character name.
        /// </summary>
        [SerializeField]
        protected Text m_CharacterName;


        protected virtual void Start() {
            if(CharacterManager.current.SelectedCharacter != null)
            Repaint();
        }

        public virtual void Repaint() {
            if (this.m_CharacterName != null)
            {
                this.m_CharacterName.text = CharacterManager.current.SelectedCharacter.CharacterName;
            }

            if (this.m_ProfessionName != null)
            {
                this.m_ProfessionName.text = CharacterManager.current.SelectedCharacter.Name;
            }

            if (this.m_Description != null)
            {
                this.m_Description.text = CharacterManager.current.SelectedCharacter.Description;
            }

            if (this.m_Ícon != null)
            {
                this.m_Ícon.overrideSprite = CharacterManager.current.SelectedCharacter.CreateCharacterImage;
            }
        }
    }
}