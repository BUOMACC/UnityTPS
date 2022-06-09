using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
	Enemy owner;
	NavMeshAgent nav;

	public void OnStateEnter(Enemy newOwner)
	{
		owner = newOwner;
		nav = owner.GetComponent<NavMeshAgent>();
	}


	public void OnStateExit()
	{
		owner = null;
	}


	public void OnStateUpdate()
	{
		if (nav && owner.target)
		{
			// Distance Check (Chase -> Attack)
			Entity target = owner.target;
			float dist = Vector3.Distance(owner.transform.position, target.transform.position);
			if (dist <= owner.attackRange)
			{
				nav.ResetPath();
				owner.SetState(SkeletonState.Attack);
				return;
			}

			nav.SetDestination(target.transform.position);
		}
	}
}
