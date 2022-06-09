using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipType
{
	Main,	// �ֹ���
	Sub,	// ��������
}


[CreateAssetMenu(fileName = "New ShopData", menuName = "Create New ShopData")]
public class ShopData : ScriptableObject
{
	public GameObject itemPrefab;   // ������(����) ��
	public EquipType equipType;     // ������ ��ġ(�ֹ���? ��������?)
	public string weaponName;		// ������ �����̸�
	public string shopName;			// ������ ǥ�õ� �̸�
	public int shopPrice;			// ������ ǥ�õ� ����
}


