using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPath : MonoBehaviour
{
	public TrackingPath nextPath;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 0.2f);
		if (nextPath)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, nextPath.transform.position);
		}
	}
}
