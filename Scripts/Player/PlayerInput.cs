using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	// 입력을 받기위한 매핑이름
	[Header("* Mapping Names")]
	[SerializeField] string name_horizontal = "Horizontal";
	[SerializeField] string name_vertical = "Vertical";
	[SerializeField] string name_mouseX = "Mouse X";
	[SerializeField] string name_mouseY = "Mouse Y";
	[SerializeField] string name_leftClick = "Fire1";
	[SerializeField] string name_rightClick = "Fire2";
	[SerializeField] KeyCode key_jump;
	[SerializeField] KeyCode key_quickSlot1;
	[SerializeField] KeyCode key_quickSlot2;
	[SerializeField] KeyCode key_quickSlot3;
	[SerializeField] KeyCode key_reload;

	public Vector2 inputVec { get; private set; }
	public Vector2 mouseVec { get; private set; }

	public bool leftClick { get; private set; }
	public bool leftClick_Single { get; private set; }

	public bool rightClick { get; private set; }
	public bool rightClick_Single { get; private set; }

	public bool jump { get; private set; }
	public bool quickSlot1 { get; private set; }
	public bool quickSlot2 { get; private set; }
	public bool quickSlot3 { get; private set; }
	public bool reload { get; private set; }


	void Update()
	{
		inputVec = new Vector2(Input.GetAxis(name_horizontal), Input.GetAxis(name_vertical));
		mouseVec = new Vector2(Input.GetAxisRaw(name_mouseX), Input.GetAxisRaw(name_mouseY));

		leftClick = Input.GetButton(name_leftClick);
		leftClick_Single = Input.GetButtonDown(name_leftClick);

		rightClick = Input.GetButton(name_rightClick);
		rightClick_Single = Input.GetButtonDown(name_rightClick);

		jump = Input.GetKeyDown(key_jump);
		quickSlot1 = Input.GetKeyDown(key_quickSlot1);
		quickSlot2 = Input.GetKeyDown(key_quickSlot2);
		quickSlot3 = Input.GetKeyDown(key_quickSlot3);
		reload = Input.GetKeyDown(key_reload);
	}
}
