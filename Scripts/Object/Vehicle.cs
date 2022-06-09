using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	[Header("* Vehicle Setting")]
	[SerializeField] TrackingPath currPath;			// ����(����) ��
	[SerializeField] float vehicleSpeed = 1.0f;		// �̵��ӵ�
	[SerializeField] float smoothRotation = 45.0f;	// ȸ���ӵ�
	[SerializeField] float detectRange = 3.0f;		// �����Ÿ�
	[SerializeField] LayerMask playerLayer;
	[SerializeField] LayerMask enemyLayer;

	[SerializeField] GameObject[] wheels;

	public float targetDist { get; private set; }	// ���������� �Ÿ�
	public float currDist { get; private set; }     // ���� �Ÿ�

	Collider[] playerCollider = new Collider[4];
	Collider[] enemyCollider = new Collider[100];


	void Awake()
	{
		// ������ġ���� ���������� �Ÿ� ���
		TrackingPath tmpPath = currPath;
		while (tmpPath.nextPath != null)
		{
			targetDist += Vector3.Distance(tmpPath.transform.position, tmpPath.nextPath.transform.position);
			tmpPath = tmpPath.nextPath;
		}

		// �Ÿ� �ʱ�ȭ
		currDist = targetDist;

		// Path ��ġ�� �̵�
		if (currPath)
			transform.position = currPath.transform.position;
	}


	void Update()
	{
		// �ֺ��� �÷��̾ ������ �̵�
		int foundPlayer = Physics.OverlapSphereNonAlloc(transform.position, detectRange, playerCollider, playerLayer);
		int foundEnemy = Physics.OverlapSphereNonAlloc(transform.position, detectRange, enemyCollider, enemyLayer);
		if (foundPlayer > 0)
		{
			// �ֺ��� �����ִ°�� �������� ����
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

			// ���� �������� �����Ѱ��
			if (Vector3.Distance(transform.position, targetPath.transform.position) <= 0.01f)
			{
				transform.position = targetPath.transform.position;
				currPath = targetPath;
			}
		}
	}
}
