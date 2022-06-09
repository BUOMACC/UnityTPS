using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New MapData", menuName = "Create New MapData")]
public class MapData : ScriptableObject
{
	public Sprite mapIcon;		// 맵선택에 표시될 이미지
	public string cardName;		// 맵선택에 표시될 이름
	[TextArea(1, 3)]
	public string cardDesc;     // 맵선택에 표시될 설명
	public string mapName;		// 이동될 맵(Scene)이름
}
