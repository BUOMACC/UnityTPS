using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}


	public void PlayEndAnimation(bool isClear)
	{
		if (isClear)
		{
			anim.SetTrigger("Clear");
		}
		else
		{
			anim.SetTrigger("Fail");
		}
	}
}
