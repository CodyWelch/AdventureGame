using DevionGames.CharacterSystem.Configuration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace DevionGames.CharacterSystem
{
    public class CharacterManager : MonoBehaviour
    {

        /// Don't destroy this object instance when loading new scenes.
        /// </summary>
        public bool dontDestroyOnLoad = true;

        private static CharacterManager m_Current;

        /// <summary>
        /// The InventoryManager singleton object. This object is set inside Awake()
        /// </summary>
        public static CharacterManager current
        {
            get
            {
                Assert.IsNotNull(m_Current, "Requires a Character Manager.Create one from Tools > Devion Games > Character System > Create Character Manager!");
                return m_Current;
            }
        }


        [SerializeField]
        private CharacterDatabase m_Database = null;

        /// <summary>
        /// Gets the item database. Configurate it inside the editor.
        /// </summary>
        /// <value>The database.</value>
        public static CharacterDatabase Database
        {
            get
            {
                if (CharacterManager.current != null)
                {
                    Assert.IsNotNull(CharacterManager.current.m_Database, "Please assign CharacterDatabase to the Character Manager!");
                    return CharacterManager.current.m_Database;
                }
                return null;
            }
        }

        private static Default m_DefaultSettings;
        public static Default DefaultSettings
        {
            get
            {
                if (m_DefaultSettings == null)
                {
                    m_DefaultSettings = GetSetting<Default>();
                }
                return m_DefaultSettings;
            }
        }

        private static UI m_UI;
        public static UI UI
        {
            get
            {
                if (m_UI == null)
                {
                    m_UI = GetSetting<UI>();
                }
                return m_UI;
            }
        }

        private static Notifications m_Notifications;
        public static Notifications Notifications
        {
            get
            {
                if (m_Notifications == null)
                {
                    m_Notifications = GetSetting<Notifications>();
                }
                return m_Notifications;
            }
        }

        private static SavingLoading m_SavingLoading;
        public static SavingLoading SavingLoading
        {
            get
            {
                if (m_SavingLoading == null)
                {
                    m_SavingLoading = GetSetting<SavingLoading>();
                }
                return m_SavingLoading;
            }
        }

        private static T GetSetting<T>() where T : Configuration.Settings
        {
            if (CharacterManager.Database != null)
            {
                return (T)CharacterManager.Database.settings.Where(x => x.GetType() == typeof(T)).FirstOrDefault();
            }
            return default(T);
        }

        private Character m_SelectedCharacter;
        public Character SelectedCharacter {
            get { return this.m_SelectedCharacter; }
        }
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (CharacterManager.m_Current != null)
            {
               // Debug.Log("Multiple Character Manager in scene...this is not supported. Destroying instance!");
                Destroy(gameObject);
                return;
            }
            else
            {
                CharacterManager.m_Current = this;
                if (dontDestroyOnLoad){
                    if (transform.parent != null)
                    {
                        if (CharacterManager.DefaultSettings.debugMessages)
                            Debug.Log("Character Manager with DontDestroyOnLoad can't be a child transform. Unparent!");
                        transform.parent = null;
                    }
                    DontDestroyOnLoad(gameObject);
                }
                Debug.Log("Character Manager initialized.");
            }
        }

        public static void StartPlayScene(Character selected) {

            CharacterManager.current.m_SelectedCharacter = selected;
            PlayerPrefs.SetString("Player",selected.CharacterName);
            PlayerPrefs.SetString("Profession", selected.Name);
            string scene = selected.FindProperty("Scene").stringValue;
            if (string.IsNullOrEmpty(scene))
            {
                scene = CharacterManager.DefaultSettings.playScene;
            }
            if (CharacterManager.DefaultSettings.debugMessages)
                Debug.Log("[Character System] Loading scene "+scene +" for "+selected.CharacterName);
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ChangedActiveScene;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }

        private static void ChangedActiveScene(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene next)
        {
            Vector3 position = CharacterManager.current.m_SelectedCharacter.FindProperty("Spawnpoint").vector3Value;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //Player already in scene
            if (player != null) {
                //Is it the player prefab we selected?
                if (player.name == CharacterManager.current.m_SelectedCharacter.Prefab.name) {
                    return;
                }
                DestroyImmediate(player);
            }


            player = GameObject.Instantiate(CharacterManager.current.m_SelectedCharacter.Prefab, position, Quaternion.identity);
            player.name = player.name.Replace("(Clone)", "").Trim();
        }


        public static void CreateCharacter(Character character)
        {
            string key = PlayerPrefs.GetString(CharacterManager.SavingLoading.accountKey);
            string serializedCharacterData = PlayerPrefs.GetString(key);
            List<Character> list = JsonSerializer.Deserialize<Character>(serializedCharacterData);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CharacterName == character.CharacterName)
                {
                    EventHandler.Execute("OnFailedToCreateCharacter", character);
                    return;
                }
            }

            list.Add(character);
            string data = JsonSerializer.Serialize(list.ToArray());
            PlayerPrefs.SetString(key, data);
            EventHandler.Execute("OnCharacterCreated", character);
        }

        public static void LoadCharacters() {
            string key = PlayerPrefs.GetString(CharacterManager.SavingLoading.accountKey);

            string data = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(data)) return;

            List<object> l = MiniJSON.Deserialize(data) as List<object>;
            for (int i = 0; i < l.Count; i++) {
                Dictionary<string, object> characterData = l[i] as Dictionary<string, object>;
                EventHandler.Execute("OnCharacterDataLoaded",characterData);
            }
            List<Character> list = JsonSerializer.Deserialize<Character>(data);
            for (int i = 0; i < list.Count; i++)
            {
                Character character = list[i];
                EventHandler.Execute("OnCharacterLoaded", character);
            }
        }

        public static void DeleteCharacter(Character character) {
            string key = PlayerPrefs.GetString(CharacterManager.SavingLoading.accountKey);

            string serializedCharacterData = PlayerPrefs.GetString(key);
            List<Character> list = JsonSerializer.Deserialize<Character>(serializedCharacterData);

            string data = JsonSerializer.Serialize(list.Where(x => x.CharacterName != character.CharacterName).ToArray());
            PlayerPrefs.SetString(key, data);

            DeleteInventorySystemForCharacter(character.CharacterName);
            DeleteStatSystemForCharacter(character.CharacterName);
            EventHandler.Execute("OnCharacterDeleted", character);
        }

        private static void DeleteInventorySystemForCharacter(string character) {
            PlayerPrefs.DeleteKey(character + ".UI");
            List<string> scenes = PlayerPrefs.GetString(character + ".Scenes").Split(';').ToList();
            scenes.RemoveAll(x => string.IsNullOrEmpty(x));
            for (int i = 0; i < scenes.Count; i++)
            {
                PlayerPrefs.DeleteKey(character + "." + scenes[i]);
            }
            PlayerPrefs.DeleteKey(character + ".Scenes");
        }

        private static void DeleteStatSystemForCharacter(string character) {
            PlayerPrefs.DeleteKey(character + ".Stats");
            List<string> keys = PlayerPrefs.GetString("StatSystemSavedKeys").Split(';').ToList();
            keys.RemoveAll(x => string.IsNullOrEmpty(x));
            List<string> allKeys = new List<string>(keys);
            allKeys.Remove(character);
            PlayerPrefs.SetString("StatSystemSavedKeys", string.Join(";", allKeys));
        }
    }
}