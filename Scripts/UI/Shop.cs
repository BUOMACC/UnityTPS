using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	[Header("* Shop Setting")]
	[SerializeField] Camera weaponCam;
	[SerializeField] GameObject elementPrefab;
	[SerializeField] GameObject content;		// Element�� ���� Parent
	[SerializeField] GameObject previewParent;	// ���� ���� ������ Parent
	[SerializeField] GameObject btn_Buy;
	[SerializeField] GameObject btn_Equip;
	[SerializeField] Text text_Money;

	[Space]
	[Header("* Shop Items")]
	[SerializeField] ShopData[] shopItems;

	List<ShopElement> allElements = new List<ShopElement>();
	ShopElement selectElement_pre;	// ������ �����ߴ� Element
	ShopElement selectElement;		// ������ Shop Element


	void Awake()
	{
		CreateShopElement();
	}

	
	void OnEnable()
	{
		weaponCam.gameObject.SetActive(true);

		// MoneyText ������Ʈ
		text_Money.text = string.Format("{0:#,0}", GameData.instance.money);
	}


	void Update()
	{
		// Preview ��Ʈ��
		PreviewControl();
	}


	public void OnClick_Buy()
	{
		if (selectElement == null)
			return;

		// �ݾ� Ȯ��
		if (GameData.instance.money < selectElement.data.shopPrice)
			return;

		GameData.instance.money -= selectElement.data.shopPrice;
		GameData.instance.buyItems.Add(selectElement.data, true);
		selectElement.UpdateShopElement();

		// MoneyText ������Ʈ
		text_Money.text = string.Format("{0:#,0}", GameData.instance.money);
	}


	public void OnClick_Equip()
	{
		if (selectElement == null)
			return;

		switch (selectElement.data.equipType)
		{
			case (EquipType.Main):
				GameData.instance.mainWeaponName = selectElement.data.weaponName;
				break;

			case (EquipType.Sub):
				GameData.instance.subWeaponName = selectElement.data.weaponName;
				break;
		}

		// ��ü Element ������� ������Ʈ
		for (int i = 0; i < allElements.Count; i++)
			allElements[i].UpdateShopElement();
	}


	public void OnClick_ShopElement(ShopElement clickElement)
	{
		if (clickElement == null)
			return;

		selectElement_pre = selectElement;
		selectElement = clickElement;

		// ���ſ��ο� ���� ��ư��ȭ
		bool alreadyBuy = GameData.instance.buyItems.TryGetValue(selectElement.data, out alreadyBuy);
		if (selectElement.data.shopPrice <= 0 || alreadyBuy == true)
		{
			btn_Buy.SetActive(false);
			btn_Equip.SetActive(true);
		}
		else
		{
			btn_Buy.SetActive(true);
			btn_Equip.SetActive(false);
		}

		// preview ǥ��
		if (selectElement_pre != null)
			selectElement_pre.go_Preview.SetActive(false);
		selectElement.go_Preview.SetActive(true);
	}


	public void OnClick_Close()
	{
		this.gameObject.SetActive(false);
		weaponCam.gameObject.SetActive(false);
	}


	private void CreateShopElement()
	{
		// ShopItem��ŭ Element ����
		for (int i = 0; i < shopItems.Length; i++)
		{
			GameObject tmp = Instantiate(elementPrefab, Vector3.zero, Quaternion.identity);
			ShopElement tmpElement = tmp.GetComponent<ShopElement>();
			if (tmpElement)
			{
				tmpElement.SetShopElement(shopItems[i], previewParent);
				tmpElement.button.onClick.AddListener(() => OnClick_ShopElement(tmpElement));
				allElements.Add(tmpElement);
				tmp.transform.SetParent(content.transform);
			}
		}
	}


	private void PreviewControl()
	{
		// Ŭ���߿��� �����ϼ�����
		if (Input.GetButton("Fire1"))
		{
			float mouseX = Input.GetAxis("Mouse X") * 2.0f;
			float mouseY = Input.GetAxis("Mouse Y") * 2.0f;

			previewParent.transform.Rotate(new Vector3(0, -mouseX, -mouseY));
		}
	}
}
