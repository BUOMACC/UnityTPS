using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapCard : MonoBehaviour
{
	[SerializeField] Image mapIcon;
	[SerializeField] Text text_Name;
	[SerializeField] Text text_Desc;

	public string mapName { get; private set; }

	public void SetMapCard(MapData newData)
	{
		if (newData == null)
			return;

		mapIcon.sprite = newData.mapIcon;
		text_Name.text = newData.cardName;
		text_Desc.text = newData.cardDesc;
		mapName = newData.mapName;
	}
}
