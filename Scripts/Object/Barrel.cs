using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageObject
{
	// 폭발 이펙트
	[SerializeField] GameObject explosionParticle;  // 폭발효과
	[SerializeField] string hitSound;				// 피격소리
	[SerializeField] string explosionSound;			// 폭발소리
	[SerializeField] int health = 50;				// 폭발까지 남은체력
	[SerializeField] int damage = 150;              // 폭발 데미지
	[SerializeField] float range = 5.0f;            // 폭발 반경
	[SerializeField] LayerMask cullLayer;			// 특정 대상만 폭발을 받도록 레이어 지정

	Collider[] colliders = new Collider[80];		// 최대 80마리까지 데미지

	public void TakeDamage(int amount, Vector3 hitPosition)
	{
		if (health > 0)
		{
			// 피격횟수 감소
			health -= amount;
			SoundPlayer.instance.PlaySound2D(hitSound);

			// 폭발
			if (health <= 0)
			{
				SoundPlayer.instance.PlaySound3D(explosionSound, transform.position);
				Explosion();
				Destroy(this.gameObject);
			}
		}
	}


	private void Explosion()
	{
		Instantiate(explosionParticle, transform.position, Quaternion.identity);
		int overlapCount = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, cullLayer);
		for (int i = 0; i < overlapCount; i++)
		{
			// 자기 자신은 무시
			if (colliders[i] == this)
				continue;

			// 폭발반경에 포함된 적들에게 피해를 입힘
			IDamageObject victim = colliders[i].GetComponent<IDamageObject>();
			if (victim != null)
				victim.TakeDamage(damage, colliders[i].transform.position);
		}
	}
}
