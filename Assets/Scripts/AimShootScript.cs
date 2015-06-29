using UnityEngine;
using System.Collections;

public class AimShootScript : MonoBehaviour {

	Animator anim;
	public bool aim;
	public bool toggle;
	public Camera cam;
	public float zoomSpeed = 30f;
	public float minZoomFOV = 45f;
	public float currentFOV;
	void Awake () {
		anim=GetComponentInChildren<Animator>();
		currentFOV=cam.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
		aim=Input.GetButton("Fire2");

		if(aim)
		{
			anim.SetBool("Sprint",false);
			cam.fieldOfView -= zoomSpeed/8;
			if (cam.fieldOfView < minZoomFOV)
			{
				cam.fieldOfView = minZoomFOV;
			}
		}
		else
		{
			cam.fieldOfView += zoomSpeed/8;
			if (cam.fieldOfView > currentFOV)
			{
				cam.fieldOfView=currentFOV;
			
			}
		}
		anim.SetBool("Aim",aim);
	}
}
