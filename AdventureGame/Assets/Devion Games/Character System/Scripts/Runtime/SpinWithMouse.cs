using UnityEngine;
using System.Collections;

namespace DevionGames.CharacterSystem{
	public class SpinWithMouse : MonoBehaviour {
		public Transform target;
		public float speed = 5f;
		
		Transform mTrans;
		
		void Start ()
		{
			mTrans = transform;
		}
		
		void Update ()
		{
			if (Input.GetMouseButton (0)) {
				float input=Input.GetAxis("Mouse X")*-speed;
				if (target != null) {
					target.Rotate(0,input,0,Space.World);
				} else {
					mTrans.Rotate(0,input,0,Space.World);
				}
			}
		}
	}
}