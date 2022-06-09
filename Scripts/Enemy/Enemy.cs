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

	// ���� State
	IState currState;

	// State ĳ��
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


	// --------------ObjectPooling ó��--------------
	// ObjectPooling�� ����� �� �� �ֱ⶧���� OnEnable���� �ʱ�ȭ�۾� ����
	void OnEnable()
	{
		SetState(SkeletonState.Idle);
		health = maxHealth;
		anim.enabled = true;
		capsule.enabled = true;
		isDead = false;
	}

	// OnDisable���� Ǯ�� ��ȯ
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

		// �ִϸ��̼� ������Ʈ
		anim.SetFloat("Speed", nav.velocity.magnitude);
	}


	// ���� ����
	public void SetState(SkeletonState newState)
	{
		if (currState != null)
			currState.OnStateExit();

		currState = stateCache[newState];
		currState.OnStateEnter(this);
	}


	// Animation Event���� ȣ��
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
