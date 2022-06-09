using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponHandlerState
{
	Default,	// 기본상태
	Reload,		// 재장전 상태
	Draw,		// 무기를 꺼내는 상태
	Aim,		// 조준상태
}

public class WeaponHandler : MonoBehaviour
{
	public Weapon currWeapon { get; private set; }  // 현재 사용중인 무기

	[Header("* Handler Setting")]
	[SerializeField] float aimCameraZ = -0.7f;
	[SerializeField] float aimSmoothScale = 0.15f;
	[SerializeField] Weapon[] weaponList;
	[SerializeField] ShakeObject shakeTarget;
	Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();

	Animator anim;
	Camera cam;									// 총알 발사를 위한 카메라
	Coroutine reloadCoroutine;
	Coroutine aimingCoroutine;

	float fireDelay;							// 발사 딜레이
	float originCameraZ;						// 기본 카메라 Z
	public bool isAiming { get; private set; }	// 조준 상태인지

	WeaponHandlerState state = WeaponHandlerState.Default;  // 핸들러 상태

	// Events
	public delegate void UpdateWeapon(Weapon newWeapon);
	public event UpdateWeapon OnUpdateWeapon;


	void Awake()
	{
		cam = Camera.main;
		originCameraZ = cam.transform.localPosition.z;
		anim = GetComponent<Animator>();

		// Init All Weapon
		foreach (Weapon w in weaponList)
		{
			weapons.Add(w.weaponName, w);
		}
	}


	void Update()
	{
		if (fireDelay > 0.0f)
			fireDelay -= Time.deltaTime;
	}


	public bool Shoot()
	{
		// 무기가 없는경우
		if (currWeapon == null)
			return false;

		// 발사 딜레이거나, 핸들러가 발사할수 있는 상태가 아닌경우
		if (fireDelay > 0.0f || state != WeaponHandlerState.Default)
			return false;

		// 총알이 없는경우 - 장전
		if (currWeapon.currAmmo <= 0)
		{
			Reload();
			return false;
		}

		// 총알 타입에 따른 처리
		switch (currWeapon.ammoType)
		{
			case AmmoType.Raycast:
				Shoot_Raycast();
				break;

			case AmmoType.Projectile:
				Shoot_Projectile();
				break;
		}

		fireDelay = currWeapon.fireDelay;
		float muzzleRotY = currWeapon.muzzle.transform.localEulerAngles.y;
		float muzzleRotZ = currWeapon.muzzle.transform.localEulerAngles.z;

		currWeapon.currAmmo--;
		currWeapon.muzzle.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(-30, 30), muzzleRotY, muzzleRotZ));
		currWeapon.muzzle.SetActive(true);

		// 사운드 재생 & 애니메이션 처리
		SoundPlayer.instance.PlaySound2D(currWeapon.fireSound);
		anim.SetTrigger("Fire");

		// * 이벤트 호출
		if (OnUpdateWeapon != null)
			OnUpdateWeapon.Invoke(currWeapon);

		// 카메라 흔들림 효과주기
		if (shakeTarget)
			shakeTarget.StartShake(new Vector3(0.02f, 0.02f, 0.02f), 0.05f);

		return true;
	}


	private void Shoot_Raycast()
	{
		// Muzzle부터 카메라가 바라보는 방향을 계산
		Vector3 shootDir = cam.transform.forward;
		Vector3 spread = (isAiming) ? currWeapon.aimSpread : currWeapon.defaultSpread;

		float spreadX = Random.Range(-spread.x, spread.x);
		float spreadY = Random.Range(-spread.y, spread.y);

		// 탄퍼짐 계산, 정규화
		shootDir += new Vector3(spreadX, spreadY, 0.0f);
		shootDir = shootDir.normalized;

		// Raycast
		RaycastHit hitRes;
		if (Physics.Raycast(cam.transform.position, shootDir, out hitRes, 10000.0f, currWeapon.ammoCulling))
		{
			// - Entity인경우 -> 데미지
			IDamageObject victim = hitRes.transform.GetComponentInParent<IDamageObject>();
			if (victim != null)
			{
				// -- 태그를 이용해 헤드샷 판정
				if (hitRes.collider.tag.Equals("EntityHead"))
				{
					victim.TakeDamage(currWeapon.headDamage, hitRes.point);
					return;
				}

				// -- 헤드샷이 아니면 일반적인 데미지처리
				victim.TakeDamage(currWeapon.damage, hitRes.point);
			}
			// - Entity가 아닌경우 -> 총알 스파크
			else
			{
				ObjectPooling.instance.Instantiate("HitSpark", hitRes.point, Quaternion.Euler(hitRes.normal));
			}
		}
	}


	private void Shoot_Projectile()
	{
		// TODO: Projectile 처리
	}


	// 무기교체
	public void ChangeWeapon(string weaponName)
	{
		Weapon newWeapon = (weaponName == null) ? null : weapons[weaponName];

		if (currWeapon != newWeapon)
		{
			if (currWeapon)
			{
				// 무기 교체시 조준상태 해제
				Aiming(false);
				currWeapon.gameObject.SetActive(false);
			}
			if (newWeapon)
				newWeapon.gameObject.SetActive(true);

			currWeapon = newWeapon;
			state = WeaponHandlerState.Draw;

			// * 이벤트 호출
			if (OnUpdateWeapon != null)
				OnUpdateWeapon.Invoke(currWeapon);

			// 재장전중에 무기교체시 코루틴 중단
			if (reloadCoroutine != null)
				StopCoroutine(reloadCoroutine);
			StartCoroutine(ChangeWeaponCoroutine(newWeapon));
		}
	}


	IEnumerator ChangeWeaponCoroutine(Weapon newWeapon)
	{
		float drawDelay = (newWeapon) ? newWeapon.drawDelay : 1.0f;
		int animType = (newWeapon) ? (int)newWeapon.weaponAnimType : 0;

		anim.SetTrigger("Draw");
		anim.SetInteger("Weapon", animType);
		yield return CoroutineCache.WaitForSecondsCache(drawDelay);
		state = WeaponHandlerState.Default;
	}


	// 재장전
	public void Reload()
	{
		// 무기를 들고있고 재장전이 가능한 상태, 총알을 한발이라도 사용했다면
		if (currWeapon && state != WeaponHandlerState.Reload
			&& currWeapon.currAmmo < currWeapon.maxAmmo)
		{
			// 재장전시 조준상태 해제
			Aiming(false);
			state = WeaponHandlerState.Reload;
			reloadCoroutine = StartCoroutine(ReloadCoroutine());
		}
	}


	IEnumerator ReloadCoroutine()
	{
		anim.SetTrigger("Reload");
		SoundPlayer.instance.PlaySound2D(currWeapon.reloadSound);
		yield return CoroutineCache.WaitForSecondsCache(currWeapon.reloadDelay);
		state = WeaponHandlerState.Default;
		currWeapon.AddAmmo(currWeapon.reloadAmmo);

		// * 이벤트 호출
		if (OnUpdateWeapon != null)
			OnUpdateWeapon.Invoke(currWeapon);
	}


	// 조준 (견착)
	public void Aiming(bool flag)
	{
		if (currWeapon != null && state == WeaponHandlerState.Default)
		{
			isAiming = flag;

			// 조준코루틴이 여러번 실행되지 않도록 중지한뒤 실행
			if (aimingCoroutine != null)
				StopCoroutine(aimingCoroutine);
			aimingCoroutine = StartCoroutine(AimingCoroutine( (flag) ? aimCameraZ : originCameraZ));
		}
	}


	IEnumerator AimingCoroutine(float targetZ)
	{
		float currZ = 0.0f;
		while (Mathf.Abs(targetZ - currZ) > 0.05f)
		{
			currZ = Mathf.Lerp(cam.transform.localPosition.z, targetZ, aimSmoothScale);
			Vector3 currVec = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, currZ);
			cam.transform.localPosition = currVec;

			yield return null;
		}

		cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, targetZ);
	}
}
