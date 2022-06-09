using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
	[SerializeField] float disableTime;
	float currDisableTime;


	void OnEnable()
	{
		currDisableTime = disableTime;
	}


	void Update()
	{
		if (currDisableTime > 0.0f)
		{
			currDisableTime -= Time.deltaTime;
			return;
		}
		gameObject.SetActive(false);
	}
}
