using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	[Header("* Vehicle Setting")]
	[SerializeField] TrackingPath currPath;			// 현재(시작) 길
	[SerializeField] float vehicleSpeed = 1.0f;		// 이동속도
	[SerializeField] float smoothRotation = 45.0f;	// 회전속도
	[SerializeField] float detectRange = 3.0f;		// 감지거리
	[SerializeField] LayerMask playerLayer;
	[SerializeField] LayerMask enemyLayer;

	[SerializeField] GameObject[] wheels;

	public float targetDist { get; private set; }	// 목적지까지 거리
	public float currDist { get; private set; }     // 현재 거리

	Collider[] playerCollider = new Collider[4];
	Collider[] enemyCollider = new Collider[100];


	void Awake()
	{
		// 현재위치에서 목적지까지 거리 계산
		TrackingPath tmpPath = currPath;
		while (tmpPath.nextPath != null)
		{
			targetDist += Vector3.Distance(tmpPath.transform.position, tmpPath.nextPath.transform.position);
			tmpPath = tmpPath.nextPath;
		}

		// 거리 초기화
		currDist = targetDist;

		// Path 위치로 이동
		if (currPath)
			transform.position = currPath.transform.position;
	}


	void Update()
	{
		// 주변에 플레이어가 있으면 이동
		int foundPlayer = Physics.OverlapSphereNonAlloc(transform.position, detectRange, playerCollider, playerLayer);
		int foundEnemy = Physics.OverlapSphereNonAlloc(transform.position, detectRange, enemyCollider, enemyLayer);
		if (foundPlayer > 0)
		{
			// 주변에 적이있는경우 움직이지 않음
			if (foundEnemy > 0)
				return;
			Move();
		}
	}


	private void RollingWheel()
	{
		for (int i = 0; i < wheels.Length; i++)
		{
			wheels[i].transform.Rotate(new Vector3(360 * (vehicleSpeed / 2), 0, 0) * Time.deltaTime);
		}
	}


	private void Move()
	{
		if (currPath)
		{
			TrackingPath targetPath = currPath.nextPath;
			if (!targetPath)
			{
				currDist = 0.0f;
				return;
			}

			Vector3 dir = (targetPath.transform.position - transform.position).normalized;

			currDist -= vehicleSpeed * Time.deltaTime;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), smoothRotation * (vehicleSpeed / 2) * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, targetPath.transform.position, vehicleSpeed * Time.deltaTime);
			RollingWheel();

			// 다음 목적지에 도착한경우
			if (Vector3.Distance(transform.position, targetPath.transform.position) <= 0.01f)
			{
				transform.position = targetPath.transform.position;
				currPath = targetPath;
			}
		}
	}
}
