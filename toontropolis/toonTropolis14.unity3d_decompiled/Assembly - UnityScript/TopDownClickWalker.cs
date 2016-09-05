using Boo.Lang.Runtime;
using System;
using UnityEngine;

[AddComponentMenu("ControlPrototype/Top Down Click Walker"), RequireComponent(typeof(CharacterController))]
[Serializable]
public class TopDownClickWalker : MonoBehaviour
{
	public float maxSpeed;

	public float gravity;

	public Transform indicator;

	public float stopDistance;

	public bool instantTurn;

	public float turnSpeed;

	public string navigationMask;

	private int hasTarget;

	private Vector3 targetPoint;

	private Vector3 moveDirection;

	private bool grounded;

	private float startTime;

	private bool canDrag;

	private bool firstClick;

	public float singleClickAllowance;

	public TopDownClickWalker()
	{
		this.maxSpeed = 10f;
		this.gravity = 9.8f;
		this.stopDistance = 1f;
		this.instantTurn = true;
		this.turnSpeed = 10f;
		this.hasTarget = 0;
		this.targetPoint = Vector3.get_zero();
		this.moveDirection = Vector3.get_zero();
		this.grounded = false;
		this.startTime = (float)0;
		this.canDrag = false;
		this.firstClick = false;
		this.singleClickAllowance = 0.25f;
	}

	public void Start()
	{
		if (this.indicator)
		{
			this.indicator.get_gameObject().set_active(false);
		}
	}

	public void FixedUpdate()
	{
		if (this.hasTarget != 0)
		{
			float num = Vector3.Distance(new Vector3(this.targetPoint.x, this.get_transform().get_position().y, this.targetPoint.z), this.get_transform().get_position());
			this.moveDirection = new Vector3((float)0, (float)0, this.maxSpeed);
			this.moveDirection = this.get_transform().TransformDirection(this.moveDirection);
			if (num < this.stopDistance)
			{
				if (this.indicator)
				{
					this.indicator.get_gameObject().set_active(false);
				}
				this.hasTarget = 0;
			}
			Quaternion quaternion = Quaternion.LookRotation(new Vector3(this.targetPoint.x, this.get_transform().get_position().y, this.targetPoint.z) - this.get_transform().get_position());
			if (this.instantTurn)
			{
				this.get_transform().set_rotation(quaternion);
			}
			else
			{
				this.get_transform().set_rotation(Quaternion.Slerp(this.get_transform().get_rotation(), quaternion, Time.get_deltaTime() * this.turnSpeed));
			}
		}
		else
		{
			this.moveDirection = new Vector3((float)0, (float)0, (float)0);
		}
		this.moveDirection.y = this.moveDirection.y - this.gravity;
		CharacterController characterController = (CharacterController)this.GetComponent(typeof(CharacterController));
		CollisionFlags collisionFlags = characterController.Move(this.moveDirection * Time.get_deltaTime());
		this.grounded = ((collisionFlags & 4) != 0);
	}

	public void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			this.startTime = Time.get_time();
			this.canDrag = true;
			this.firstClick = true;
		}
		else if (Input.GetButtonUp("Fire1"))
		{
			this.canDrag = false;
		}
		if (this.firstClick || (this.canDrag && Time.get_time() > this.startTime + this.singleClickAllowance))
		{
			this.firstClick = false;
			Ray ray = Camera.get_main().ScreenPointToRay(Input.get_mousePosition());
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(ray, ref raycastHit) && RuntimeServices.EqualityOperator(RuntimeServices.GetProperty(RuntimeServices.GetProperty(raycastHit.get_collider(), "gameObject"), "layer"), LayerMask.NameToLayer(this.navigationMask)) && (raycastHit.get_point() - this.get_transform().get_position()).get_magnitude() > this.stopDistance)
			{
				this.setTargetPos(raycastHit.get_point());
			}
		}
	}

	public void setTargetPos(Vector3 target)
	{
		this.hasTarget = 1;
		this.targetPoint = target;
		if (this.indicator)
		{
			this.indicator.set_position(this.targetPoint);
			this.indicator.get_gameObject().set_active(true);
		}
		this.targetPoint.y = this.get_transform().get_position().y;
	}

	public void Main()
	{
	}
}
