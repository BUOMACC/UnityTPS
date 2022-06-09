using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InputMode
{
	Game,
	UI,
}

public class GameData : Singleton<GameData>
{
	// 구매한 아이템 목록
	public Dictionary<ShopData, bool> buyItems = new Dictionary<ShopData, bool>();

	[Header("* User Setting")]
	public float sfxVolume = 0.4f;


	[Header("* Game Setting")]
	// 착용한 주무기 / 보조무기
	public string mainWeaponName = "Rifle";
	public string subWeaponName = "Pistol";

	// 재화
	public int money = 0;


	public void SetShowMouseCursor(bool flag)
	{
		if (!flag)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}
	}
}
