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
		// ĳ���� �¿� ȸ��
		Vector3 turnRightVec = new Vector3(0.0f, playerInput.mouseVec.x, 0.0f) * camSens;
		transform.rotation = Quaternion.Euler(transform.eulerAngles + turnRightVec);

		// ī�޶�(SpringArm) ���Ʒ� ȸ��
		lookUpRot -= playerInput.mouseVec.y * camSens;
		lookUpRot = Mathf.Clamp(lookUpRot, -maxLookUpRot, maxLookUpRot);
		springArm.transform.localRotation = Quaternion.Euler(new Vector3(lookUpRot, 0.0f, 0.0f));
	}
}
