using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleGameMode : GameMode
{
	[SerializeField] Vehicle vehicle;

	protected override void Start()
	{
		base.Start();

		// 만약 화물을 선택해주지 않았을 경우를 대비해 찾아줌
		if (vehicle == null)
			vehicle = FindObjectOfType<Vehicle>();

		// 플레이어가 죽은경우 게임오버
		Player player = FindObjectOfType<Player>();
		player.OnEntityDeath += OnPlayerDeath;
	}


	void Update()
	{
		if (gameEnd)
			return;

		// 거리가 일정량 이하일경우 -> 도착
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
