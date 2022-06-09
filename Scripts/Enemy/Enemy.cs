using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum SkeletonState
{
	Idle,
	Chase,
	Attack,
	Dead,
}


public class Enemy : Entity
{
	public LayerMask cullTarget;
	public Entity target;

	[Header("* Idle")]
	public float detectRange = 10.0f;
	public float findTargetRepeatTime = 2.0f;

	[Header("* Attack")]
	public float attackRange = 2.0f;
	public float attackRepeatTime = 2.0f;
	public int damage = 0;

	Rigidbody[] rb;
	Animator anim;
	CapsuleCollider capsule;
	NavMeshAgent nav;

	// 현재 State
	IState currState;

	// State 캐싱
	Dictionary<SkeletonState, IState> stateCache = new Dictionary<SkeletonState, IState>();

	protected override void Awake()
	{
		base.Awake();

		nav = GetComponent<NavMeshAgent>();
		capsule = GetComponent<CapsuleCollider>();
		anim = GetComponent<Animator>();
		stateCache.Add(SkeletonState.Idle, new IdleState());
		stateCache.Add(SkeletonState.Chase, new ChaseState());
		stateCache.Add(SkeletonState.Attack, new AttackState());

		rb = GetComponentsInChildren<Rigidbody>();
	}


	// --------------ObjectPooling 처리--------------
	// ObjectPooling의 대상이 될 수 있기때문에 OnEnable에서 초기화작업 수행
	void OnEnable()
	{
		SetState(SkeletonState.Idle);
		health = maxHealth;
		anim.enabled = true;
		capsule.enabled = true;
		isDead = false;
	}

	// OnDisable에서 풀로 반환
	void OnDisable()
	{
		ObjectPooling.instance.ReturnPool(this.gameObject);
	}
	// ----------------------------------------------


	void Update()
	{
		if (isDead)
			return;

		if (currState != null)
			currState.OnStateUpdate();

		// 애니메이션 업데이트
		anim.SetFloat("Speed", nav.velocity.magnitude);
	}


	// 상태 변경
	public void SetState(SkeletonState newState)
	{
		if (currState != null)
			currState.OnStateExit();

		currState = stateCache[newState];
		currState.OnStateEnter(this);
	}


	// Animation Event에서 호출
	public void Attack()
	{
		RaycastHit hit;
		if (Physics.BoxCast(transform.position + new Vector3(0, 1.0f, 0), new Vector3(0.25f, 0.25f, 0.25f), transform.forward, out hit, Quaternion.identity, attackRange, cullTarget))
		{
			Entity entity = hit.collider.GetComponent<Entity>();
			entity.TakeDamage(damage, hit.point);
		}
	}


	protected override void Dead()
	{
		base.Dead();
		anim.enabled = false;
		capsule.enabled = false;

		Invoke("DeadAction", 3.0f);
	}

	
	private void DeadAction()
	{
		this.gameObject.SetActive(false);
	}
}
