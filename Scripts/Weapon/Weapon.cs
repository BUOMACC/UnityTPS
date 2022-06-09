using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponAnimType
{
	Default,	// 기본모션
	Pistol,		// 피스톨 모션
	Rifle,		// 라이플 모션
}


public enum FireType
{
	Auto,		// 연사
	Default,    // 단발
}


public enum AmmoType
{
	Raycast,
	Projectile,
}


public class Weapon : MonoBehaviour
{
	public WeaponAnimType weaponAnimType;	// 무기 애니메이션
	public FireType fireType;				// 발사타입 (연사 / 단발...)
	public AmmoType ammoType;				// 총알종류 (Raycast / Projectile)
	public LayerMask ammoCulling;           // 총알이 어떤 오브젝트를 무시할지

	public string weaponName;				// 무기 이름
	public int damage = 1;					// 데미지
	public int headDamage;					// 머리 데미지
	public int currAmmo = 15;				// 현재 총알
	public int maxAmmo = 15;				// 최대 총알
	public int reloadAmmo = 15;				// 재장전시 충전되는 총알

	public float fireDelay = 0.8f;			// 발사 시간
	public float reloadDelay = 1.5f;		// 재장전 시간
	public float drawDelay = 1.0f;          // 총을 꺼내기까지의 시간

	public float slow = 1.0f;				// 총을 들고있을시 느려지는 속도(0.6 = 60%로 감소함)
	public float slowAiming = 1.0f;			// 조준시 느려지는 속도 비율(0.6 = 60%로 감소함)

	public string fireSound;				// 발사 소리
	public string reloadSound;				// 재장전 소리
	public GameObject muzzle;				// 발사 총구효과
	public ParticleSystem hitImpact;		// 총알이 벽에 박혔을때 이펙트

	public Vector2 defaultSpread;			// 기본발사시 총알 퍼짐정도
	public Vector2 aimSpread;				// 조준발사시 총알 퍼짐정도


	public void AddAmmo(int amount)
	{
		currAmmo += amount;
		currAmmo = Mathf.Clamp(currAmmo, 0, maxAmmo);
	}
}
