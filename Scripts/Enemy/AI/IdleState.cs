using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
	Enemy owner;
	Collider[] colliders = new Collider[4];
	float remainTime;

	public void OnStateEnter(Enemy newOwner)
	{
		owner = newOwner;
		remainTime = owner.findTargetRepeatTime;
	}


	public void OnStateExit()
	{
		owner = null;
	}


	public void OnStateUpdate()
	{
		// �������Ӹ��� Ÿ���� ã���ʰ� �����ð����� ã��
		remainTime -= Time.deltaTime;
		if (remainTime <= 0)
		{
			remainTime = owner.findTargetRepeatTime;

			// - Ÿ��ã�� & Ÿ���� ã�Ҵٸ� (Idle -> Chase)
			int foundNum = Physics.OverlapSphereNonAlloc(owner.transform.position, owner.detectRange, colliders, owner.cullTarget);
			if (foundNum > 0)
			{
				owner.target = colliders[0].GetComponent<Entity>();
				owner.SetState(SkeletonState.Chase);
			}
		}
	}
}
