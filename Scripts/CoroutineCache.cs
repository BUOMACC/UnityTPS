using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineCache
{
	private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

	// ���� ���Ǵ� WaitForSeconds ��ü���� ĳ��
	public static WaitForSeconds WaitForSecondsCache(float seconds)
	{
		WaitForSeconds wfs;

		// �����ü� ���ٸ� -> ��ü�� ���ٸ�
		if (waitForSeconds.TryGetValue(seconds, out wfs) == false)
		{
			// ��ü�� ������ ĳ��
			waitForSeconds.Add(seconds, new WaitForSeconds(seconds));
		}
		return waitForSeconds[seconds];
	}

}
