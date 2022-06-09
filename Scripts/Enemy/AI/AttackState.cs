using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
	Enemy owner;
	Animator anim;
	float remainTime;

	public void OnStateEnter(Enemy newOwner)
	{
		owner = newOwner;
		anim = owner.GetComponent<Animator>();
		remainTime = 0.0f;
	}


	public void OnStateExit()
	{
		owner = null;
	}


	public void OnStateUpdate()
	{
		remainTime -= Time.deltaTime;
		if (remainTime <= 0)
		{
			remainTime = owner.attackRepeatTime;
			Attack();
		}

	}


	void Attack()
	{
		// 타겟이 없어진경우 (Attack -> Idle)
		if (owner.target == null)
		{
			owner.SetState(SkeletonState.Idle);
			return;
		}

		// 타겟이 멀어진경우 (Attack -> Chase)
		Entity target = owner.target;
		float dist = Vector3.Distance(owner.transform.position, target.transform.position);
		if (dist > owner.attackRange)
		{
			owner.SetState(SkeletonState.Idle);
			return;
		}

		anim.SetTrigger("Attack");
	}
}
