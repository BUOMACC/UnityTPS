using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PoolData
{
	public string poolName;		// pool 이름
	public GameObject prefab;	// 만들어둘 오브젝트
	public int count;			// pool 개수
}


public class ObjectPooling : MonoBehaviour
{
	// 인스턴스 (사용자가 직접 설정해줄 내용이 많고, 필요없는 씬도 있으므로 싱글톤 X)
	public static ObjectPooling instance;

	// 미리 만들어둘 오브젝트
	[SerializeField] PoolData[] poolData;

	// 만들어진 오브젝트 풀
	Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();


	void Awake()
	{
		instance = this;
	}


	void Start()
	{
		InitPool();
	}


	// 풀에서 게임오브젝트를 가져옴
	public GameObject Instantiate(string poolName, Vector3 position, Quaternion rotation)
	{
		// 존재하지 않는풀이면 null을 반환
		if (pool.ContainsKey(poolName) == false)
			return null;

		// 존재하는 풀이지만, 큐에있는 모든 오브젝트가 사용된경우 null
		if (pool[poolName].Count <= 0)
			return null;

		// Queue에서 오브젝트를 꺼내 반환
		GameObject tmp = pool[poolName].Dequeue();
		tmp.transform.position = position;
		tmp.transform.rotation = rotation;
		tmp.SetActive(true);
		return tmp;
	}


	// 풀로 되돌리기
	public void ReturnPool(GameObject target)
	{
		// 해당되는 풀이 없다면 중단
		if (pool.ContainsKey(target.name) == false)
			return;

		// Enqueue
		pool[target.name].Enqueue(target);
	}


	// 풀 대상 오브젝트 생성
	private void InitPool()
	{
		foreach (PoolData data in poolData)
		{
			// Pool Object 생성
			// - 오브젝트 정리를 위한 Empty GameObject 추가
			GameObject parent = new GameObject(data.poolName);
			Queue<GameObject> poolQueue = new Queue<GameObject>();
			for (int i = 0; i < data.count; i++)
			{
				GameObject tmp = Instantiate(data.prefab, Vector3.zero, Quaternion.identity);
				tmp.name = data.poolName;
				tmp.SetActive(false);
				tmp.transform.SetParent(parent.transform);
				poolQueue.Enqueue(tmp);
			}

			// 만들어진 Queue를 pool Dictonary에 추가
			pool.Add(data.poolName, poolQueue);
		}
	}
}
