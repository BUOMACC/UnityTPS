using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
	[SerializeField] Text text_Health;
	[SerializeField] Text text_Ammo;

	Player targetPlayer;
	WeaponHandler targetWeaponHandler;


	void Awake()
	{
		targetPlayer = FindObjectOfType<Player>();
		targetWeaponHandler = FindObjectOfType<WeaponHandler>();

		// * 이벤트 등록
		if (targetPlayer)
			targetPlayer.OnHealthChange += UpdateHealthText;

		if (targetWeaponHandler)
			targetWeaponHandler.OnUpdateWeapon += UpdateAmmoText;
	}


	private void UpdateAmmoText(Weapon targetWeapon)
	{
		int currAmmo = (targetWeapon) ? targetWeapon.currAmmo : 0;
		int maxAmmo = (targetWeapon) ? targetWeapon.maxAmmo : 0;
		text_Ammo.text = currAmmo + " / " + maxAmmo;
	}


	private void UpdateHealthText(int amount)
	{
		text_Health.text = amount.ToString();
	}
}
