using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.CharacterSystem
{

    public enum Gender { 
        Male,
        Female
    }

    [System.Serializable]
    public class Character : ScriptableObject, INameable, IJsonSerializable
    {
        [HeaderLine("Default")]
        [InspectorLabel("Name")]
        [SerializeField]
        private string m_ProfessionName = string.Empty;

        public string Name
        {
            get { return this.m_ProfessionName; }
            set { this.m_ProfessionName = value; }
        }

        [SerializeField]
        private Gender m_Gender;
        public Gender Gender {
            get { return m_Gender; }
            set { this.m_Gender = value; }
        }

        [SerializeField]
        [TextArea(4,4)]
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [SerializeField]
        private Sprite m_CreateCharacterImage;

        public Sprite CreateCharacterImage
        {
            get { return this.m_CreateCharacterImage; }
            set { this.m_CreateCharacterImage = value; }
        }

        [SerializeField]
        private GameObject prefab;

        public GameObject Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

        [SerializeField]
        private List<ObjectProperty> m_Properties = new List<ObjectProperty>();

        public ObjectProperty FindProperty(string name)
        {
            return m_Properties.Find(property => property.Name == name);
        }

        public ObjectProperty[] FindProperties(string name)
        {
            return m_Properties.FindAll(property => property.Name == name).ToArray();
        }

        public ObjectProperty[] GetProperties()
        {
            return m_Properties.ToArray();
        }

        public void SetProperty(string name, object value)
        {
            ObjectProperty property = FindProperty(name);
            if (property == null)
            {
                property = new ObjectProperty();
                property.Name = name;
                property.SetValue(value);
                m_Properties.Add(property);
            }
            else
            {
                property.SetValue(value);
            }
        }

        public object GetProperty(string name)
        {
            ObjectProperty property = FindProperty(name);
            if (property != null)
            {
                return property.GetValue();
            }
            return null;
        }

        public void SetProperties(ObjectProperty[] properties)
        {
            this.m_Properties = new List<ObjectProperty>(properties);
        }

        private string m_CharacterName = string.Empty;

        public string CharacterName
        {
            get { return this.m_CharacterName; }
            set { this.m_CharacterName = value; }
        }

        public void GetObjectData(Dictionary<string, object> data)
        {
            data.Add("Name", Name);
            data.Add("Gender", (int)Gender);
            data.Add("CharacterName", CharacterName);
            Dictionary<string, object> mProperties = new Dictionary<string, object>();
            foreach (ObjectProperty property in this.m_Properties)
            {
                mProperties.Add(property.Name, property.GetValue());
            }
            data.Add("Properties", mProperties);
        }

        public void SetObjectData(Dictionary<string, object> data)
        {
            this.Name = (string)data["Name"];
            this.Gender = (Gender)System.Convert.ToInt32(data["Gender"]);
            Character character = CharacterManager.Database.items.Find(x => x.Name == this.Name && x.Gender == this.Gender);
            if (character != null) {
                this.Description = character.Description;
                this.CreateCharacterImage = character.CreateCharacterImage;
                this.Prefab = character.Prefab;
            }
            this.CharacterName = (string)data["CharacterName"];

            Dictionary<string, object> mProperties = data["Properties"] as Dictionary<string, object>;
            List<ObjectProperty> toSet = new List<ObjectProperty>();

            foreach (KeyValuePair<string, object> kvp in mProperties)
            {
                ObjectProperty property = new ObjectProperty();
                property.Name = kvp.Key;
                property.SetValue(kvp.Value);
                toSet.Add(property);
            }
            this.SetProperties(toSet.ToArray());
        }
    }
}
