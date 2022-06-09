using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New MapData", menuName = "Create New MapData")]
public class MapData : ScriptableObject
{
	public Sprite mapIcon;		// �ʼ��ÿ� ǥ�õ� �̹���
	public string cardName;		// �ʼ��ÿ� ǥ�õ� �̸�
	[TextArea(1, 3)]
	public string cardDesc;     // �ʼ��ÿ� ǥ�õ� ����
	public string mapName;		// �̵��� ��(Scene)�̸�
}
