using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGameMode : GameMode
{
	protected override void Start()
	{
		GameData.instance.SetShowMouseCursor(true);
	}
}
