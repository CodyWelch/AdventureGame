using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace myRPG
{
    public class Captureable : Interactable
    {
        public int timeToCapture = 0;
        public Slider slider;
        public GameObject target;
        public GameObject slidertarget;
        Transform cam;

        public override void Interact()
        {

        //public virtual void Use()
        //{
            base.Interact();
            Debug.Log("Using " + name);
            Renderer renderer = this.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.red);

            //StartCoroutine("Capture");
        }
        public void Start()
        {
            cam = Camera.main.transform;
        }

        IEnumerator Capture()
        {
            for (int i = timeToCapture; i>0; i--)
            {
                slider.value = i;

                if (slider != null)
                {   
                    slider.transform.position = target.transform.position;
                    slider.transform.forward = -cam.forward;

                  //  if (Time.time - lastMadeVisibleTime > visibleTime)
                    //{
                      //  ui.gameObject.SetActive(false);
                    //}
                }

                yield return new WaitForSeconds(.1f);

            }
            this.GetComponent<Material>().SetColor("_Color,", Color.blue);
        }
    }
}

