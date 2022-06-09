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
	// ������ ������ ���
	public Dictionary<ShopData, bool> buyItems = new Dictionary<ShopData, bool>();

	[Header("* User Setting")]
	public float sfxVolume = 0.4f;


	[Header("* Game Setting")]
	// ������ �ֹ��� / ��������
	public string mainWeaponName = "Rifle";
	public string subWeaponName = "Pistol";

	// ��ȭ
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
