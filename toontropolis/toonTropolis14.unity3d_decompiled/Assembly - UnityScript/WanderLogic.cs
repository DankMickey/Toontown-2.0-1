using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class WanderLogic : MonoBehaviour
{
	private bool wanderFirstTime;

	public string outOfAnim;

	public string intoAnim;

	public string useAction;

	public AnimInfo[] WanderActions;

	private float turnTimer;

	private Quaternion newDirection;

	private Quaternion startDirection;

	private float randomRot;

	private float timeLastTurn;

	private float turnIncr;

	public float moveSpeed;

	public WanderLogic()
	{
		this.wanderFirstTime = true;
		this.outOfAnim = string.Empty;
		this.intoAnim = string.Empty;
	}

	public void Start()
	{
		if (this.wanderFirstTime)
		{
			this.wanderFirstTime = false;
		}
	}

	public void Into()
	{
		if (Extensions.get_length(this.intoAnim) > 0)
		{
			this.get_animation().CrossFade(this.intoAnim, 0.15f);
			this.useAction = this.outOfAnim;
		}
		else
		{
			this.useAction = this.WanderActions[0].anim;
			this.get_animation().CrossFade(this.WanderActions[0].anim, 0.15f);
		}
		float time = Time.get_time();
		this.turnTimer = (float)0;
	}

	public void Outof()
	{
		if (Extensions.get_length(this.outOfAnim) > 0)
		{
			this.get_animation().CrossFade(this.outOfAnim, 0.15f);
			this.useAction = this.outOfAnim;
		}
	}

	public void WanderAnimation()
	{
		PetState petState = (PetState)this.GetComponent(typeof(PetState));
		if (!this.wanderFirstTime)
		{
			if (!this.get_animation().IsPlaying("WanderActions[0].anim"))
			{
				this.get_animation().CrossFade(this.WanderActions[0].anim);
			}
			float num = (float)20;
			float num2 = 0f;
			if (Time.get_time() - this.timeLastTurn > (float)4)
			{
				num2 = (float)0;
				this.randomRot = (float)Random.Range(25, 45);
				this.startDirection = this.get_transform().get_rotation();
				this.newDirection = Quaternion.get_identity();
				float num3 = this.get_transform().get_eulerAngles().y + this.randomRot;
				this.newDirection.set_eulerAngles(new Vector3((float)0, num3, (float)0));
				int num4 = Random.Range(1, 3);
				this.turnIncr = (float)num4 * num / Mathf.Abs(this.randomRot);
				this.turnIncr = (float)1 / Mathf.Abs(this.randomRot);
				this.turnTimer = this.turnIncr;
				this.timeLastTurn = Time.get_time();
			}
			if (this.turnTimer < this.randomRot)
			{
				this.get_transform().set_rotation(Quaternion.Slerp(this.startDirection, this.newDirection, this.turnTimer));
				this.get_transform().set_eulerAngles(new Vector3((float)0, this.get_transform().get_eulerAngles().y, (float)0));
				this.turnTimer += this.turnIncr;
				if (this.turnTimer >= this.randomRot)
				{
					this.randomRot = (float)0;
					this.turnTimer = (float)0;
				}
			}
			Vector3 vector = this.get_transform().TransformDirection(Vector3.get_forward());
			Vector3 vector2 = vector * this.moveSpeed;
			UnityRuntimeServices.Invoke(this.GetComponent(typeof(CharacterController)), "SimpleMove", new object[]
			{
				vector2
			}, typeof(MonoBehaviour));
		}
	}

	public void Main()
	{
	}
}
