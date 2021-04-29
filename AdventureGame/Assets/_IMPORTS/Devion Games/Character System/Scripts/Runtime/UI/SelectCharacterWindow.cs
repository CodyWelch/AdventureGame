using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.CharacterSystem
{
    public class SelectCharacterWindow : CharacterWindow
    {

        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>(base.Callbacks);
                callbacks.Add("OnCharacterLoaded");
                callbacks.Add("OnCharacterDeleted");
                return callbacks.ToArray();
            }
        }

        [SerializeField]
        protected Button m_CreateButton;
        [SerializeField]
        protected Button m_PlayButton;
        [SerializeField]
        protected Button m_DeleteButton;

        protected override void OnStart()
        {
            this.m_PlayButton.onClick.AddListener(delegate {
                CharacterManager.StartPlayScene(SelectedCharacter);
            });

            this.m_CreateButton.onClick.AddListener(delegate
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(CharacterManager.DefaultSettings.createCharacterScene);
            });

            this.m_DeleteButton.onClick.AddListener(delegate 
            {
                Close();
                m_CreateButton.gameObject.SetActive(false);
                m_PlayButton.gameObject.SetActive(false);
                m_DeleteButton.gameObject.SetActive(false);
                CharacterManager.Notifications.deleteConfirmation.Show(delegate (int result) {
                    switch (result) {
                        case 0:
                            CharacterManager.DeleteCharacter(SelectedCharacter); Show();
                            break;
                        case 1:
                            m_PlayButton.gameObject.SetActive(true);
                            m_DeleteButton.gameObject.SetActive(true);
                            break;

                    }
                    Show();
                    m_CreateButton.gameObject.SetActive(true);
                }, "Yes","Cancel");
                
            }
            );

            EventHandler.Register<Character>("OnCharacterLoaded", OnCharacterLoaded);
            EventHandler.Register<Character>("OnCharacterDeleted", OnCharacterDeleted);
            CharacterManager.LoadCharacters();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventHandler.Unregister<Character>("OnCharacterLoaded", OnCharacterLoaded);
            EventHandler.Unregister<Character>("OnCharacterDeleted", OnCharacterDeleted);
        }

        public override void OnSelectionChange(Character character)
        {
            base.OnSelectionChange(character);
            m_DeleteButton.gameObject.SetActive(character != null);
            m_PlayButton.gameObject.SetActive(character != null);
            if (CharacterManager.DefaultSettings.debugMessages)
                Debug.Log("[Character System] Character "+character.CharacterName+ " selected.");
        }

        private void OnCharacterLoaded(Character character) {
            CharacterSlot slot=Add(character);
            slot.name = character.CharacterName;
            CallbackEventData data = new CallbackEventData();
            data.AddData("Slot", slot);
            data.AddData("Character", character);
            data.AddData("CharacterName", character.CharacterName);
            Execute("OnCharacterLoaded", data);
            //Dirty, should do it in a diffrent way
            slot.BroadcastMessage("OnCharacterLoaded", data, SendMessageOptions.DontRequireReceiver);
        }

        private void OnCharacterDeleted(Character character) {
            Remove(character);
            CallbackEventData data = new CallbackEventData();
            data.AddData("Character", character);
            data.AddData("CharacterName", character.CharacterName);
            Execute("OnCharacterDeleted", data);
        }
    }
}