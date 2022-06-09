using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineCache
{
	private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

	// 자주 사용되는 WaitForSeconds 객체사용시 캐싱
	public static WaitForSeconds WaitForSecondsCache(float seconds)
	{
		WaitForSeconds wfs;

		// 가져올수 없다면 -> 객체가 없다면
		if (waitForSeconds.TryGetValue(seconds, out wfs) == false)
		{
			// 객체를 생성해 캐싱
			waitForSeconds.Add(seconds, new WaitForSeconds(seconds));
		}
		return waitForSeconds[seconds];
	}

}
