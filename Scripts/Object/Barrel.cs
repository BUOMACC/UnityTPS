using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageObject
{
	// ���� ����Ʈ
	[SerializeField] GameObject explosionParticle;  // ����ȿ��
	[SerializeField] string hitSound;				// �ǰݼҸ�
	[SerializeField] string explosionSound;			// ���߼Ҹ�
	[SerializeField] int health = 50;				// ���߱��� ����ü��
	[SerializeField] int damage = 150;              // ���� ������
	[SerializeField] float range = 5.0f;            // ���� �ݰ�
	[SerializeField] LayerMask cullLayer;			// Ư�� ��� ������ �޵��� ���̾� ����

	Collider[] colliders = new Collider[80];		// �ִ� 80�������� ������

	public void TakeDamage(int amount, Vector3 hitPosition)
	{
		if (health > 0)
		{
			// �ǰ�Ƚ�� ����
			health -= amount;
			SoundPlayer.instance.PlaySound2D(hitSound);

			// ����
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
			// �ڱ� �ڽ��� ����
			if (colliders[i] == this)
				continue;

			// ���߹ݰ濡 ���Ե� ���鿡�� ���ظ� ����
			IDamageObject victim = colliders[i].GetComponent<IDamageObject>();
			if (victim != null)
				victim.TakeDamage(damage, colliders[i].transform.position);
		}
	}
}
