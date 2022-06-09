using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponHandlerState
{
	Default,	// �⺻����
	Reload,		// ������ ����
	Draw,		// ���⸦ ������ ����
	Aim,		// ���ػ���
}

public class WeaponHandler : MonoBehaviour
{
	public Weapon currWeapon { get; private set; }  // ���� ������� ����

	[Header("* Handler Setting")]
	[SerializeField] float aimCameraZ = -0.7f;
	[SerializeField] float aimSmoothScale = 0.15f;
	[SerializeField] Weapon[] weaponList;
	[SerializeField] ShakeObject shakeTarget;
	Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();

	Animator anim;
	Camera cam;									// �Ѿ� �߻縦 ���� ī�޶�
	Coroutine reloadCoroutine;
	Coroutine aimingCoroutine;

	float fireDelay;							// �߻� ������
	float originCameraZ;						// �⺻ ī�޶� Z
	public bool isAiming { get; private set; }	// ���� ��������

	WeaponHandlerState state = WeaponHandlerState.Default;  // �ڵ鷯 ����

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
		// ���Ⱑ ���°��
		if (currWeapon == null)
			return false;

		// �߻� �����̰ų�, �ڵ鷯�� �߻��Ҽ� �ִ� ���°� �ƴѰ��
		if (fireDelay > 0.0f || state != WeaponHandlerState.Default)
			return false;

		// �Ѿ��� ���°�� - ����
		if (currWeapon.currAmmo <= 0)
		{
			Reload();
			return false;
		}

		// �Ѿ� Ÿ�Կ� ���� ó��
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

		// ���� ��� & �ִϸ��̼� ó��
		SoundPlayer.instance.PlaySound2D(currWeapon.fireSound);
		anim.SetTrigger("Fire");

		// * �̺�Ʈ ȣ��
		if (OnUpdateWeapon != null)
			OnUpdateWeapon.Invoke(currWeapon);

		// ī�޶� ��鸲 ȿ���ֱ�
		if (shakeTarget)
			shakeTarget.StartShake(new Vector3(0.02f, 0.02f, 0.02f), 0.05f);

		return true;
	}


	private void Shoot_Raycast()
	{
		// Muzzle���� ī�޶� �ٶ󺸴� ������ ���
		Vector3 shootDir = cam.transform.forward;
		Vector3 spread = (isAiming) ? currWeapon.aimSpread : currWeapon.defaultSpread;

		float spreadX = Random.Range(-spread.x, spread.x);
		float spreadY = Random.Range(-spread.y, spread.y);

		// ź���� ���, ����ȭ
		shootDir += new Vector3(spreadX, spreadY, 0.0f);
		shootDir = shootDir.normalized;

		// Raycast
		RaycastHit hitRes;
		if (Physics.Raycast(cam.transform.position, shootDir, out hitRes, 10000.0f, currWeapon.ammoCulling))
		{
			// - Entity�ΰ�� -> ������
			IDamageObject victim = hitRes.transform.GetComponentInParent<IDamageObject>();
			if (victim != null)
			{
				// -- �±׸� �̿��� ��弦 ����
				if (hitRes.collider.tag.Equals("EntityHead"))
				{
					victim.TakeDamage(currWeapon.headDamage, hitRes.point);
					return;
				}

				// -- ��弦�� �ƴϸ� �Ϲ����� ������ó��
				victim.TakeDamage(currWeapon.damage, hitRes.point);
			}
			// - Entity�� �ƴѰ�� -> �Ѿ� ����ũ
			else
			{
				ObjectPooling.instance.Instantiate("HitSpark", hitRes.point, Quaternion.Euler(hitRes.normal));
			}
		}
	}


	private void Shoot_Projectile()
	{
		// TODO: Projectile ó��
	}


	// ���ⱳü
	public void ChangeWeapon(string weaponName)
	{
		Weapon newWeapon = (weaponName == null) ? null : weapons[weaponName];

		if (currWeapon != newWeapon)
		{
			if (currWeapon)
			{
				// ���� ��ü�� ���ػ��� ����
				Aiming(false);
				currWeapon.gameObject.SetActive(false);
			}
			if (newWeapon)
				newWeapon.gameObject.SetActive(true);

			currWeapon = newWeapon;
			state = WeaponHandlerState.Draw;

			// * �̺�Ʈ ȣ��
			if (OnUpdateWeapon != null)
				OnUpdateWeapon.Invoke(currWeapon);

			// �������߿� ���ⱳü�� �ڷ�ƾ �ߴ�
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


	// ������
	public void Reload()
	{
		// ���⸦ ����ְ� �������� ������ ����, �Ѿ��� �ѹ��̶� ����ߴٸ�
		if (currWeapon && state != WeaponHandlerState.Reload
			&& currWeapon.currAmmo < currWeapon.maxAmmo)
		{
			// �������� ���ػ��� ����
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

		// * �̺�Ʈ ȣ��
		if (OnUpdateWeapon != null)
			OnUpdateWeapon.Invoke(currWeapon);
	}


	// ���� (����)
	public void Aiming(bool flag)
	{
		if (currWeapon != null && state == WeaponHandlerState.Default)
		{
			isAiming = flag;

			// �����ڷ�ƾ�� ������ ������� �ʵ��� �����ѵ� ����
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
