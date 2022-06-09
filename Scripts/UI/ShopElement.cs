using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
	[Header("* Shop Element Setting")]
	[SerializeField] GameObject go_Price;
	[SerializeField] GameObject go_Equip;
	[Space]
	[SerializeField] Text text_Name;
	[SerializeField] Text text_Price;

	public Button button;
	public GameObject go_Preview { get; private set; }
	public ShopData data { get; private set; }


	public void Start()
	{
		UpdateShopElement();
	}


	public void SetShopElement(ShopData newData, GameObject previewParent)
	{
		if (newData == null)
			return;

		data = newData;
		text_Name.text = data.shopName;
		text_Price.text = string.Format("{0:#,0}", data.shopPrice);

		// Preview 생성
		go_Preview = Instantiate(data.itemPrefab);
		go_Preview.transform.SetParent(previewParent.transform);
		go_Preview.transform.localPosition = Vector3.zero;
		go_Preview.transform.localRotation = Quaternion.identity;
	}


	public void UpdateShopElement()
	{
		// 가격이 없거나 구매한 아이템이면 가격을 보여주지 않음
		bool alreadyBuy = GameData.instance.buyItems.TryGetValue(data, out alreadyBuy);
		if (data.shopPrice <= 0 || alreadyBuy == true)
			go_Price.SetActive(false);
		else
			go_Price.SetActive(true);

		// 장착한경우 장착텍스트 표시
		string weaponName = data.weaponName;
		if (GameData.instance.mainWeaponName.Equals(weaponName))
			go_Equip.SetActive(true);
		else
			go_Equip.SetActive(false);
	}
}
