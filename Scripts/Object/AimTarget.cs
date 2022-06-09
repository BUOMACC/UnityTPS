using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
	public Camera cam;

	void Awake()
	{
		cam = Camera.main;
	}


	void Update()
	{
		transform.position = cam.transform.position + cam.transform.forward * 100.0f;
	}
}
