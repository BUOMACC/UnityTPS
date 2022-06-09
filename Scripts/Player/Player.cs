using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	[Header("* Player Setting")]
	[SerializeField] ShakeObject shakeTarget;   // 피격시 카메라 흔들림
	[SerializeField] Vector3 shakeScale;		// 카메라 흔들림 강도

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

		// 무기에 따른 이동속도 감소
		if (weaponHandler.currWeapon)
		{
			// - 조준사격 / 기본사격에 따른 이동속도 감소
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

		// 연사
		if (weaponHandler.currWeapon.fireType == FireType.Auto
			&& playerInput.leftClick)
		{
			weaponHandler.Shoot();
		}
		// 단발
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

		// 피격효과 (카메라 흔들림)
		if (shakeTarget)
		{
			shakeTarget.StartShake(shakeScale, 0.08f);
		}
	}
}
