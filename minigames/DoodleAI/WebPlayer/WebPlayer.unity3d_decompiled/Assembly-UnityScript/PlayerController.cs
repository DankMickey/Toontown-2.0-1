using System;
using UnityEngine;

[AddComponentMenu("ControlPrototype/Player Controller"), RequireComponent(typeof(CharacterController))]
[Serializable]
public class PlayerController : MonoBehaviour
{
	public float forwardSpeed;

	public float backwardSpeed;

	public float turnSpeed;

	public int jumpSpeed;

	public float gravity;

	private Vector3 moveDirection;

	private CharacterController charController;

	private Status status;

	public static TopDownClickWalker tdcWalker;

	public static bool fpsWalkerMode;

	public static bool strafeWalkerMode;

	public static bool tdcWalkerMode;

	public static bool runMode;

	public static bool jumpMode;

	public bool bounce;

	public float boing;

	public PlayerController()
	{
		this.forwardSpeed = 6f;
		this.backwardSpeed = 3f;
		this.turnSpeed = 3f;
		this.jumpSpeed = 8;
		this.gravity = 20f;
		this.moveDirection = Vector3.get_zero();
		this.boing = 25f;
	}

	public override void Start()
	{
		this.charController = (CharacterController)this.GetComponent(typeof(CharacterController));
		this.status = (Status)this.GetComponent(typeof(Status));
		PlayerController.tdcWalker = (TopDownClickWalker)this.GetComponent(typeof(TopDownClickWalker));
	}

	public override void Update()
	{
		this.CheckStaus();
		if (this.charController.get_isGrounded())
		{
			if (PlayerController.fpsWalkerMode)
			{
				float y = this.get_transform().get_eulerAngles().y + Input.GetAxis("Mouse X") * this.turnSpeed;
				Vector3 eulerAngles = this.get_transform().get_eulerAngles();
				float num = eulerAngles.y = y;
				Vector3 vector;
				this.get_transform().set_eulerAngles(vector = eulerAngles);
				this.moveDirection = new Vector3(Input.GetAxis("Horizontal"), (float)0, Input.GetAxis("Vertical"));
			}
			else
			{
				if (CameraController.mouseMode)
				{
					float y2 = this.get_transform().get_eulerAngles().y + (Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X")) * this.turnSpeed;
					Vector3 eulerAngles2 = this.get_transform().get_eulerAngles();
					float num2 = eulerAngles2.y = y2;
					Vector3 vector2;
					this.get_transform().set_eulerAngles(vector2 = eulerAngles2);
				}
				else
				{
					float y3 = this.get_transform().get_eulerAngles().y + Input.GetAxis("Horizontal") * this.turnSpeed;
					Vector3 eulerAngles3 = this.get_transform().get_eulerAngles();
					float num3 = eulerAngles3.y = y3;
					Vector3 vector3;
					this.get_transform().set_eulerAngles(vector3 = eulerAngles3);
				}
				this.moveDirection = new Vector3((float)0, (float)0, Input.GetAxis("Vertical"));
			}
			this.moveDirection = this.get_transform().TransformDirection(this.moveDirection);
			this.CharMove();
		}
		this.moveDirection.y = this.moveDirection.y - this.gravity * Time.get_deltaTime();
		this.charController.Move(this.moveDirection * Time.get_deltaTime());
	}

	public override void CharMove()
	{
		if (Input.GetAxis("Vertical") > (float)0)
		{
			this.moveDirection *= this.forwardSpeed;
		}
		else if (Input.GetAxis("Vertical") < (float)0)
		{
			this.moveDirection *= this.backwardSpeed;
		}
		if (Input.GetButton("Jump"))
		{
			this.moveDirection.y = (float)this.jumpSpeed;
		}
		if (this.bounce)
		{
			this.moveDirection.y = this.boing;
			this.bounce = false;
		}
	}

	public override void CheckStaus()
	{
		if (Input.GetKeyDown(306) || Input.GetKeyDown(120))
		{
			if (PlayerController.runMode)
			{
				PlayerController.runMode = false;
			}
			else
			{
				PlayerController.runMode = true;
			}
		}
		if (Input.GetKeyDown(110))
		{
			if (PlayerController.fpsWalkerMode)
			{
				PlayerController.fpsWalkerMode = false;
			}
			else
			{
				PlayerController.fpsWalkerMode = true;
			}
		}
		if (Input.GetKeyDown(116))
		{
			if (PlayerController.tdcWalkerMode)
			{
				PlayerController.tdcWalkerMode = false;
			}
			else
			{
				PlayerController.tdcWalkerMode = true;
			}
		}
		if (Input.GetKeyDown(106))
		{
			if (PlayerController.jumpMode)
			{
				PlayerController.jumpMode = false;
			}
			else
			{
				PlayerController.jumpMode = true;
			}
		}
		if (PlayerController.tdcWalkerMode)
		{
			PlayerController.tdcWalker.set_enabled(true);
		}
		else
		{
			PlayerController.tdcWalker.set_enabled(false);
		}
	}

	public override void OnTriggerEnter(Collider col)
	{
		if (col.get_name() == "spring")
		{
			this.bounce = true;
		}
	}

	public override void Main()
	{
	}
}
