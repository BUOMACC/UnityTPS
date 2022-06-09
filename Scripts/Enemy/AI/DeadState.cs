using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState
{
	Enemy owner;

	public void OnStateEnter(Enemy newOwner)
	{
		owner = newOwner;
	}


	public void OnStateExit()
	{
		owner = null;
	}


	public void OnStateUpdate()
	{

	}
}
