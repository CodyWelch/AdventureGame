using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace myRPG
{
    [RequireComponent(typeof(CharacterStats))]
    public class HealthUI : MonoBehaviour
    {

        public GameObject uiPrefab;
        public Transform target;
        float visibleTime = 5;
        public Slider slider;

        float lastMadeVisibleTime;
        Transform ui;
        //Image healthSlider;
        Transform cam;

        // Use this for initialization
        void Start()
        {
            cam = Camera.main.transform;

            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.WorldSpace)
                {
                    ui = Instantiate(uiPrefab, c.transform).transform;
                    slider = ui.GetComponent<Slider>();
                    //healthSlider = ui.GetChild(0).GetComponent<Image>();
                    ui.gameObject.SetActive(false);
                    break;
                }
            }

            GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
        }

        void OnHealthChanged(int maxHealth, int currentHealth)
        {
            SetMaxHealth(maxHealth);

            if (ui != null)
            {
                ui.gameObject.SetActive(true);
                lastMadeVisibleTime = Time.time;

                SetHealth(currentHealth);
                //float healthPercent = currentHealth / (float)maxHealth;
                //healthSlider.fillAmount = healthPercent;
                if (currentHealth <= 0)
                {
                    Destroy(ui.gameObject);
                }
            }
        }

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
        }

        public void SetHealth(int health)
        {
            slider.value = health;
        }

        void LateUpdate()
        {
            if (ui != null)
            {
                ui.position = target.position;
                ui.forward = -cam.forward;

                if (Time.time - lastMadeVisibleTime > visibleTime)
                {
                    ui.gameObject.SetActive(false);
                }
            }
        }
    }
}
