using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUI : MonoBehaviour
{
	[SerializeField] Vehicle vehicle;
	[SerializeField] Image bar_Front;


	void Awake()
	{
		if (vehicle == null)
			vehicle = FindObjectOfType<Vehicle>();
	}


	void Update()
    {
		float percent = 1.0f - (vehicle.currDist / vehicle.targetDist);
		bar_Front.fillAmount = percent;
	}
}
