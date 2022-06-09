using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	[Header("* Spawner Setting")]
	[SerializeField] string poolName;       // ��ȯ�� ������ Ǯ �̸�
	[SerializeField] Vector3 spawnerSize;   // ��ȯ ����
	[SerializeField] float firstSpawnTime;	// ó�� ��ȯ�� ������ (0 = �ٷμ�ȯ��)
	[SerializeField] float spawnTime;       // ��ȯ�ð�
	[SerializeField] int spawnCount;		// �󸶳� ��ȯ���� (�� ��ȯ�ϸ� ���̻� ������������)

	[Header("* Detect Setting")]
	[SerializeField] Vector3 detectSize;    // �ش� ������ŭ �����Ǹ� ��ȯ
	[SerializeField] Vector3 detectPos;     // ���� ��ǥ
	[SerializeField] Color detectGizmoColor;
	[SerializeField] LayerMask detectLayer; // ������ ���̾�


	float currSpawnTime;    // ��ȯ���� �����ð�
	Collider[] detectCollider = new Collider[4];

	void Awake()
	{
		currSpawnTime = firstSpawnTime;
	}


	void Update()
	{
		// �� ��ȯ������ ������ ��Ȱ��ȭ -> �ڿ����� �ּ�ȭ
		if (spawnCount <= 0)
		{
			this.gameObject.SetActive(false);
			return;
		}

		// ����üũ (Detect Size�� ��� 0�̸� ������ ����)
		if (detectSize.x > 0.0f || detectSize.y > 0.0f || detectSize.z > 0.0f)
		{
			int detectCnt = Physics.OverlapBoxNonAlloc(transform.position + detectPos, detectSize / 2, detectCollider, Quaternion.identity, detectLayer);
			if (detectCnt <= 0)
				return;
		}

		// ��� ������ �����ϸ�
		// - ��ȯī��Ʈ�� ���� & �����ȿ� ����
		currSpawnTime -= Time.deltaTime;
		if (currSpawnTime <= 0)
		{
			currSpawnTime = spawnTime;
			spawnCount--;

			// Spawn
			// - ��ȯ��ġ ����
			float spawnX = Random.Range(transform.position.x - (spawnerSize.x / 2), transform.position.x + (spawnerSize.x / 2));
			float spawnY = Random.Range(transform.position.y - (spawnerSize.y / 2), transform.position.y + (spawnerSize.y / 2));
			float spawnZ = Random.Range(transform.position.z - (spawnerSize.z / 2), transform.position.z + (spawnerSize.z / 2));
			
			Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);
			ObjectPooling.instance.Instantiate(poolName, spawnPos, Quaternion.identity);
		}
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position + new Vector3(0, spawnerSize.y/2, 0), spawnerSize);
		Gizmos.color = detectGizmoColor;
		Gizmos.DrawCube(transform.position + detectPos, detectSize);
	}
}
