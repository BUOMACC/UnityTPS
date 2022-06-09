using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	[Header("* Spawner Setting")]
	[SerializeField] string poolName;       // 소환할 몬스터의 풀 이름
	[SerializeField] Vector3 spawnerSize;   // 소환 범위
	[SerializeField] float firstSpawnTime;	// 처음 소환시 딜레이 (0 = 바로소환됨)
	[SerializeField] float spawnTime;       // 소환시간
	[SerializeField] int spawnCount;		// 얼마나 소환할지 (다 소환하면 더이상 동작하지않음)

	[Header("* Detect Setting")]
	[SerializeField] Vector3 detectSize;    // 해당 범위만큼 감지되면 소환
	[SerializeField] Vector3 detectPos;     // 감지 좌표
	[SerializeField] Color detectGizmoColor;
	[SerializeField] LayerMask detectLayer; // 감지할 레이어


	float currSpawnTime;    // 소환까지 남은시간
	Collider[] detectCollider = new Collider[4];

	void Awake()
	{
		currSpawnTime = firstSpawnTime;
	}


	void Update()
	{
		// 다 소환했으면 스포너 비활성화 -> 자원낭비 최소화
		if (spawnCount <= 0)
		{
			this.gameObject.SetActive(false);
			return;
		}

		// 범위체크 (Detect Size가 모두 0이면 범위는 무한)
		if (detectSize.x > 0.0f || detectSize.y > 0.0f || detectSize.z > 0.0f)
		{
			int detectCnt = Physics.OverlapBoxNonAlloc(transform.position + detectPos, detectSize / 2, detectCollider, Quaternion.identity, detectLayer);
			if (detectCnt <= 0)
				return;
		}

		// 모든 조건이 만족하면
		// - 소환카운트가 남음 & 범위안에 있음
		currSpawnTime -= Time.deltaTime;
		if (currSpawnTime <= 0)
		{
			currSpawnTime = spawnTime;
			spawnCount--;

			// Spawn
			// - 소환위치 지정
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
