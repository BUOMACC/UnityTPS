using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGameMode : GameMode
{
	[SerializeField] Vehicle vehicle;

	protected override void Start()
	{
		base.Start();

		// ���� ȭ���� ���������� �ʾ��� ��츦 ����� ã����
		if (vehicle == null)
			vehicle = FindObjectOfType<Vehicle>();

		// �÷��̾ ������� ���ӿ���
		Player player = FindObjectOfType<Player>();
		player.OnEntityDeath += OnPlayerDeath;
	}


	void Update()
	{
		if (gameEnd)
			return;

		// �Ÿ��� ������ �����ϰ�� -> ����
		if (vehicle.currDist <= 0.01f)
		{
			GameEnd(true);
		}
	}


	void OnPlayerDeath()
	{
		GameEnd(false);
	}
}
