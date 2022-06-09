using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
	Vector3 originPosition;


	public void StartShake(Vector3 shakeScale, float shakeTime)
	{
		originPosition = transform.localPosition;
		StopAllCoroutines();
		StartCoroutine(ShakeCoroutine(shakeScale, shakeTime));
	}


	IEnumerator ShakeCoroutine(Vector3 shakeScale, float shakeTime)
	{
		while (shakeTime > 0)
		{
			float shakeX = Random.Range(-shakeScale.x, shakeScale.x);
			float shakeY = Random.Range(-shakeScale.y, shakeScale.y);
			float shakeZ = Random.Range(-shakeScale.z, shakeScale.z);

			transform.localPosition = new Vector3(shakeX, shakeY, shakeZ);

			shakeTime -= Time.deltaTime;
			yield return null;
		}
		transform.localPosition = originPosition;
	}
}
