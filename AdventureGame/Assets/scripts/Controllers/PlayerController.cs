using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace myRPG
{
    [RequireComponent(typeof(PlayerMotor))]
    public class PlayerController : MonoBehaviour
    {

        public Interactable focus;
        public Camera cam;

        public NavMeshAgent agent;
        PlayerMotor motor;
        public LayerMask movementMask;
        public bool isActive;
        public Quest quest;
        public bool bMainPlayer;

        [SerializeField]
        private string playerName;

        void Start()
        {
            motor = GetComponent<PlayerMotor>();
            cam = Camera.main;
            isActive = false;
        }

        public string GetPlayerName()
        {
            return playerName;
        }

        public void SetMainPlayer(bool setActive, GameObject mainPlayer)
        {
            bMainPlayer = setActive;
            motor.SetMainPlayer(setActive,mainPlayer);
        }

        void Update()
        {
            if (bMainPlayer)
            {

                // stop player from moving when using inventory
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100, movementMask))
                    {
                        motor.MoveToPoint(hit.point);
                        //                agent.SetDestination(hit.point);'
                        RemoveFocus();

                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        Debug.Log("hit " + hit.transform.name);
                        // check if interactable
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        
                        if (interactable != null)
                        {
                            SetFocus(interactable);
                        }
                    }
                }
            }
        }

        void SetFocus(Interactable newFocus)
        {
            if (newFocus != focus)
            {
                if (focus != null)
                    focus.OnDefocused();


                focus = newFocus;
                motor.FollowTarget(newFocus);
            }
            newFocus.OnFocused(transform);
        }

        private void RemoveFocus()
        {
            if (focus != null)
                focus.OnDefocused();

            focus = null;
            motor.StopFollowingTarget();
        }


    }
}
