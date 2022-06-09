using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[Header("* Movement Setting")]
	[SerializeField] float walkSpeed = 5.0f;
	[Range(0.05f, 1.0f)]
	[SerializeField] float smoothScale = 1.0f;
	[SerializeField] float jumpForce = 8.0f;
	[SerializeField] LayerMask groundCheckLayer;

	float gravVelocity;
	bool isGrounded = true;

	CharacterController characterController;
	Animator anim;


	void Awake()
	{
		characterController = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
	}


	void Update()
    {
		GroundCheck();
		Gravity();
    }


	private void GroundCheck()
	{
		// 구체와 충돌이 정상적으로 되도록 오프셋조정
		// -> 캐릭터의 포지션이 땅에 완전히 붙어있으므로 SphereCast가 동작하지 않았음
		//	  위치를 조금위로 잡아주는것으로 해결
		RaycastHit hitRes;
		isGrounded = Physics.SphereCast(transform.position + (transform.up * 0.2f), 0.2f, -transform.up, out hitRes, 0.1f, groundCheckLayer);
	}


	public void Move(Vector2 moveInput, float extraSpeed = 1.0f)
	{
		float AxisForward = moveInput.y;
		float AxisRight = moveInput.x;

		AxisForward = Mathf.Clamp(AxisForward, -smoothScale, smoothScale) / smoothScale;
		AxisRight = Mathf.Clamp(AxisRight, -smoothScale, smoothScale) / smoothScale;

		// * 이동방향 계산 & 대각선 이동 제어
		Vector3 inputDir = AxisForward * transform.forward + AxisRight * transform.right;
		inputDir = Vector3.ClampMagnitude(inputDir, 1.0f);

		// * 최종 이동방향 계산
		Vector3 moveDir = inputDir * walkSpeed * extraSpeed;
		characterController.Move(moveDir * Time.deltaTime);

		UpdateAnimation(moveInput.y, moveInput.x, extraSpeed);
	}


	public void Jump()
	{
		if (isGrounded)
		{
			isGrounded = false;
			gravVelocity = jumpForce;
		}
	}


	private void Gravity()
	{
		// * 중력계산
		gravVelocity += Physics.gravity.y * Time.deltaTime;
		gravVelocity = Mathf.Clamp(gravVelocity, -8.0f, gravVelocity);
		if (characterController.isGrounded)
			gravVelocity = 0.0f;

		characterController.Move(gravVelocity * Vector3.up * Time.deltaTime);
	}


	private void UpdateAnimation(float forward, float right, float playSpeed)
	{
		if (forward != 0.0f || right != 0.0f)
			anim.SetBool("IsMove", true);
		else
			anim.SetBool("IsMove", false);

		if (!isGrounded)
			anim.SetBool("IsJump", true);
		else
			anim.SetBool("IsJump", false);

		anim.SetFloat("WalkAnimSpeed", playSpeed);
		anim.SetFloat("Forward", forward, 0.05f, Time.deltaTime);
		anim.SetFloat("Right", right, 0.05f, Time.deltaTime);
	}
}
