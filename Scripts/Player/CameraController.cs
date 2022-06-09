using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("* Camera Setting")]
	[SerializeField] GameObject springArm;
	[SerializeField] float camSens = 2.5f;
	[Range(0, 70)]
	[SerializeField] float maxLookUpRot = 60.0f;

	PlayerInput playerInput;
	float lookUpRot;


	void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
	}


	void Update()
    {
		// 캐릭터 좌우 회전
		Vector3 turnRightVec = new Vector3(0.0f, playerInput.mouseVec.x, 0.0f) * camSens;
		transform.rotation = Quaternion.Euler(transform.eulerAngles + turnRightVec);

		// 카메라(SpringArm) 위아래 회전
		lookUpRot -= playerInput.mouseVec.y * camSens;
		lookUpRot = Mathf.Clamp(lookUpRot, -maxLookUpRot, maxLookUpRot);
		springArm.transform.localRotation = Quaternion.Euler(new Vector3(lookUpRot, 0.0f, 0.0f));
	}
}
