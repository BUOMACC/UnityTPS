using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponAnimType
{
	Default,	// �⺻���
	Pistol,		// �ǽ��� ���
	Rifle,		// ������ ���
}


public enum FireType
{
	Auto,		// ����
	Default,    // �ܹ�
}


public enum AmmoType
{
	Raycast,
	Projectile,
}


public class Weapon : MonoBehaviour
{
	public WeaponAnimType weaponAnimType;	// ���� �ִϸ��̼�
	public FireType fireType;				// �߻�Ÿ�� (���� / �ܹ�...)
	public AmmoType ammoType;				// �Ѿ����� (Raycast / Projectile)
	public LayerMask ammoCulling;           // �Ѿ��� � ������Ʈ�� ��������

	public string weaponName;				// ���� �̸�
	public int damage = 1;					// ������
	public int headDamage;					// �Ӹ� ������
	public int currAmmo = 15;				// ���� �Ѿ�
	public int maxAmmo = 15;				// �ִ� �Ѿ�
	public int reloadAmmo = 15;				// �������� �����Ǵ� �Ѿ�

	public float fireDelay = 0.8f;			// �߻� �ð�
	public float reloadDelay = 1.5f;		// ������ �ð�
	public float drawDelay = 1.0f;          // ���� ����������� �ð�

	public float slow = 1.0f;				// ���� ��������� �������� �ӵ�(0.6 = 60%�� ������)
	public float slowAiming = 1.0f;			// ���ؽ� �������� �ӵ� ����(0.6 = 60%�� ������)

	public string fireSound;				// �߻� �Ҹ�
	public string reloadSound;				// ������ �Ҹ�
	public GameObject muzzle;				// �߻� �ѱ�ȿ��
	public ParticleSystem hitImpact;		// �Ѿ��� ���� �������� ����Ʈ

	public Vector2 defaultSpread;			// �⺻�߻�� �Ѿ� ��������
	public Vector2 aimSpread;				// ���ع߻�� �Ѿ� ��������


	public void AddAmmo(int amount)
	{
		currAmmo += amount;
		currAmmo = Mathf.Clamp(currAmmo, 0, maxAmmo);
	}
}
