using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	[Header("* Player Setting")]
	[SerializeField] ShakeObject shakeTarget;   // �ǰݽ� ī�޶� ��鸲
	[SerializeField] Vector3 shakeScale;		// ī�޶� ��鸲 ����

	PlayerInput playerInput;
	CharacterMovement movement;
	WeaponHandler weaponHandler;

	protected override void Awake()
	{
		base.Awake();

		playerInput = GetComponent<PlayerInput>();
		movement = GetComponent<CharacterMovement>();
		weaponHandler = GetComponentInChildren<WeaponHandler>();
	}


	void Update()
    {
		// Movement
		float extraMoveSpeed = 1.0f;

		// ���⿡ ���� �̵��ӵ� ����
		if (weaponHandler.currWeapon)
		{
			// - ���ػ�� / �⺻��ݿ� ���� �̵��ӵ� ����
			extraMoveSpeed = (weaponHandler.isAiming) ? weaponHandler.currWeapon.slowAiming : weaponHandler.currWeapon.slow;
		}

		movement.Move(playerInput.inputVec, extraMoveSpeed);
		if (playerInput.jump)
			movement.Jump();

		// Weapon
		TryShoot();
		TryWeaponChange();
		TryReload();
		TryAiming();
	}


	private void TryShoot()
	{
		if (weaponHandler.currWeapon == null)
			return;

		// ����
		if (weaponHandler.currWeapon.fireType == FireType.Auto
			&& playerInput.leftClick)
		{
			weaponHandler.Shoot();
		}
		// �ܹ�
		else if (weaponHandler.currWeapon.fireType == FireType.Default
			&& playerInput.leftClick_Single)
		{
			weaponHandler.Shoot();
		}
	}


	private void TryWeaponChange()
	{
		if (playerInput.quickSlot1)
		{
			weaponHandler.ChangeWeapon(GameData.instance.mainWeaponName);
		}
		else if (playerInput.quickSlot2)
		{
			weaponHandler.ChangeWeapon(GameData.instance.subWeaponName);
		}
		else if (playerInput.quickSlot3)
		{
			weaponHandler.ChangeWeapon(null);
		}
	}


	private void TryReload()
	{
		if (weaponHandler.currWeapon == null)
			return;

		if (playerInput.reload)
		{
			weaponHandler.Reload();
		}
	}


	private void TryAiming()
	{
		if (weaponHandler.currWeapon == null)
			return;

		if (playerInput.rightClick_Single)
		{
			weaponHandler.Aiming(!weaponHandler.isAiming);
		}
	}


	public override void TakeDamage(int amount, Vector3 hitPosition)
	{
		base.TakeDamage(amount, hitPosition);

		// �ǰ�ȿ�� (ī�޶� ��鸲)
		if (shakeTarget)
		{
			shakeTarget.StartShake(shakeScale, 0.08f);
		}
	}
}
