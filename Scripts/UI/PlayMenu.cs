using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
	[SerializeField] MapData[] mapList;

	[Header("* PlayMenu Setting")]
	[SerializeField] float cardSize = 350.0f;
	[SerializeField] RectTransform contentRect;
	[SerializeField] GameObject mapCardPrefab;

	List<MapCard> mapCards = new List<MapCard>();     // ī�� ���
	int selectCardIndex = 0;
	bool lockInteract = false;


	void Awake()
	{
		CreateMapCard();
	}


	public void OnClick_Play()
	{
		if (lockInteract)
			return;

		if (mapCards[selectCardIndex].mapName != "")
			SceneManager.LoadScene(mapCards[selectCardIndex].mapName);
	}


	public void OnClick_LeftMap()
	{
		// ��ȣ�ۿ��� �Ұ���, ���� ���õ�ī�尡 ���� ù��° ī��� �ߴ�
		if (lockInteract || selectCardIndex <= 0)
			return;

		Vector3 target = new Vector3(contentRect.localPosition.x + cardSize, 0.0f, 0.0f);
		selectCardIndex--;
		StopAllCoroutines();
		StartCoroutine(ScrollCoroutine(target));
	}


	public void OnClick_RightMap()
	{
		// ��ȣ�ۿ��� �Ұ���, ���� ���õ�ī�尡 ���� ������ ī��� �ߴ�
		if (lockInteract || selectCardIndex + 1 >= mapCards.Count)
			return;

		Vector3 target = new Vector3(contentRect.localPosition.x - cardSize, 0.0f, 0.0f);
		selectCardIndex++;
		StopAllCoroutines();
		StartCoroutine(ScrollCoroutine(target));
	}


	public void OnClick_Close()
	{
		this.gameObject.SetActive(false);
	}


	private void CreateMapCard()
	{
		for (int i = 0; i < mapList.Length; i++)
		{
			GameObject cardObject = Instantiate(mapCardPrefab, Vector3.zero, Quaternion.identity);
			MapCard card = cardObject.GetComponent<MapCard>();
			if (card)
			{
				card.SetMapCard(mapList[i]);
				mapCards.Add(card);
				cardObject.transform.SetParent(contentRect);
			}
		}

		// rect �ʱ�ȭ
		float rectX = contentRect.anchoredPosition.x;
		float rectY = contentRect.anchoredPosition.y;
		contentRect.anchoredPosition = new Vector2(rectX + (selectCardIndex + 1) * cardSize, 0.0f);
	}


	IEnumerator ScrollCoroutine(Vector3 target)
	{
		lockInteract = true;
		Vector3 curr = contentRect.localPosition;
		while (Vector3.Distance(contentRect.localPosition, target) > 0.1f)
		{
			curr = Vector3.Lerp(contentRect.localPosition, target, 0.1f);
			contentRect.localPosition = curr;
			yield return null;
		}
		contentRect.localPosition = target;
		lockInteract = false;
	}
}
