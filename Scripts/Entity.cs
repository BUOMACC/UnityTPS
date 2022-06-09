using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageObject
{
	[SerializeField] protected int maxHealth = 100;
	[SerializeField] protected string hitEffect;
	[SerializeField] protected string hitSound;

	public int health { get; protected set; }
	public bool isDead { get; protected set; }

	// Events
	public delegate void EntityDeath();
	public event EntityDeath OnEntityDeath;

	public delegate void HealthChange(int amount);
	public event HealthChange OnHealthChange;


	protected virtual void Awake()
	{
		health = maxHealth;
	}


	public virtual void TakeDamage(int amount, Vector3 hitPosition)
	{
		if (isDead)
			return;

		health -= amount;
		if (OnHealthChange != null)
			OnHealthChange.Invoke(health);

		// �ǰ�ȿ�� ���
		if (hitEffect.Length > 0)
		{
			ObjectPooling.instance.Instantiate(hitEffect, hitPosition, Quaternion.identity);
			SoundPlayer.instance.PlaySound2D(hitSound);
		}

		// ü���� ���ٸ� ����
		if (health <= 0)
		{
			health = 0;
			Dead();
		}
	}


	protected virtual void Dead()
	{
		isDead = true;
		if (OnEntityDeath != null)
			OnEntityDeath.Invoke();
	}
}
