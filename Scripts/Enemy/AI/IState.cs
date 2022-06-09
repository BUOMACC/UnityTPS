using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
	void OnStateEnter(Enemy newOwner);
	void OnStateUpdate();
	void OnStateExit();
}
