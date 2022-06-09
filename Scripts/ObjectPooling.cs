using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PoolData
{
	public string poolName;		// pool �̸�
	public GameObject prefab;	// ������ ������Ʈ
	public int count;			// pool ����
}


public class ObjectPooling : MonoBehaviour
{
	// �ν��Ͻ� (����ڰ� ���� �������� ������ ����, �ʿ���� ���� �����Ƿ� �̱��� X)
	public static ObjectPooling instance;

	// �̸� ������ ������Ʈ
	[SerializeField] PoolData[] poolData;

	// ������� ������Ʈ Ǯ
	Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();


	void Awake()
	{
		instance = this;
	}


	void Start()
	{
		InitPool();
	}


	// Ǯ���� ���ӿ�����Ʈ�� ������
	public GameObject Instantiate(string poolName, Vector3 position, Quaternion rotation)
	{
		// �������� �ʴ�Ǯ�̸� null�� ��ȯ
		if (pool.ContainsKey(poolName) == false)
			return null;

		// �����ϴ� Ǯ������, ť���ִ� ��� ������Ʈ�� ���Ȱ�� null
		if (pool[poolName].Count <= 0)
			return null;

		// Queue���� ������Ʈ�� ���� ��ȯ
		GameObject tmp = pool[poolName].Dequeue();
		tmp.transform.position = position;
		tmp.transform.rotation = rotation;
		tmp.SetActive(true);
		return tmp;
	}


	// Ǯ�� �ǵ�����
	public void ReturnPool(GameObject target)
	{
		// �ش�Ǵ� Ǯ�� ���ٸ� �ߴ�
		if (pool.ContainsKey(target.name) == false)
			return;

		// Enqueue
		pool[target.name].Enqueue(target);
	}


	// Ǯ ��� ������Ʈ ����
	private void InitPool()
	{
		foreach (PoolData data in poolData)
		{
			// Pool Object ����
			// - ������Ʈ ������ ���� Empty GameObject �߰�
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

			// ������� Queue�� pool Dictonary�� �߰�
			pool.Add(data.poolName, poolQueue);
		}
	}
}
