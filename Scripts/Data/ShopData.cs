using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipType
{
	Main,	// 주무기
	Sub,	// 보조무기
}


[CreateAssetMenu(fileName = "New ShopData", menuName = "Create New ShopData")]
public class ShopData : ScriptableObject
{
	public GameObject itemPrefab;   // 아이템(무기) 모델
	public EquipType equipType;     // 장착될 위치(주무기? 보조무기?)
	public string weaponName;		// 장착할 무기이름
	public string shopName;			// 상점에 표시될 이름
	public int shopPrice;			// 상점에 표시될 가격
}


