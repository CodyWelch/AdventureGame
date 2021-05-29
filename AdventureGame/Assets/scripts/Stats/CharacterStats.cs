using UnityEngine;
using TMPro;

namespace myRPG
{
    public class CharacterStats : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth { get; private set; }

        public Stat damage;
        public Stat armor;
        public int experience;

        public event System.Action<int, int> OnHealthChanged;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(10);
            }
        }


        public void TakeDamage(int damage)
        {
            damage -= armor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
            currentHealth -= damage;

            Debug.Log(transform.name + " takes " + damage + " damage.");


            if (OnHealthChanged != null)
            {
                OnHealthChanged(maxHealth, currentHealth);
            }
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            // die in some way
        }



        public int SpawnType = 0;
        public int NumberOfNPC = 12;

        private TextMeshProFloatingText floatingText_Script;


        void Start()
        {

            for (int i = 0; i < NumberOfNPC; i++)
            {


                if (SpawnType == 0)
                {
                    // TextMesh Pro Implementation
                   // GameObject go = new GameObject();
                   // go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));

                   // TextMeshPro textMeshPro = go.AddComponent<TextMeshPro>();
                    TextMeshPro textMeshPro = this.GetComponent<TextMeshPro>();

                    textMeshPro.autoSizeTextContainer = true;
                    textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0);

                    textMeshPro.alignment = TextAlignmentOptions.Bottom;
                    textMeshPro.fontSize = 96;
                    textMeshPro.enableKerning = false;

                    textMeshPro.color = new Color32(255, 255, 0, 255);
                    textMeshPro.text = "!";
                    textMeshPro.isTextObjectScaleStatic = true;

                    // Spawn Floating Text
                    floatingText_Script = this.gameObject.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 0;
                }
                else if (SpawnType == 1)
                {
                    // TextMesh Implementation
                    GameObject go = new GameObject();
                    go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));

                    TextMesh textMesh = go.AddComponent<TextMesh>();
                    textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
                    textMesh.GetComponent<Renderer>().sharedMaterial = textMesh.font.material;

                    textMesh.anchor = TextAnchor.LowerCenter;
                    textMesh.fontSize = 96;

                    textMesh.color = new Color32(255, 255, 0, 255);
                    textMesh.text = "!";

                    // Spawn Floating Text
                    floatingText_Script = go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 1;
                }
                else if (SpawnType == 2)
                {
                    // Canvas WorldSpace Camera
                    GameObject go = new GameObject();
                    Canvas canvas = go.AddComponent<Canvas>();
                    canvas.worldCamera = Camera.main;

                    go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    go.transform.position = new Vector3(Random.Range(-95f, 95f), 5f, Random.Range(-95f, 95f));

                    TextMeshProUGUI textObject = new GameObject().AddComponent<TextMeshProUGUI>();
                    textObject.rectTransform.SetParent(go.transform, false);

                    textObject.color = new Color32(255, 255, 0, 255);
                    textObject.alignment = TextAlignmentOptions.Bottom;
                    textObject.fontSize = 96;
                    textObject.text = "!";

                    // Spawn Floating Text
                    floatingText_Script = go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 0;
                }
            }
        }
    }
}
