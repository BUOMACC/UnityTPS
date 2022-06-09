using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] PlayMenu playmenu;
	[SerializeField] Shop shop;


	public void OnClick_Play()
	{
		playmenu.gameObject.SetActive(true);
	}


	public void OnClick_Shop()
	{
		shop.gameObject.SetActive(true);
	}


	public void OnClick_Exit()
	{
		Application.Quit();
	}
}
