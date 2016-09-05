using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class BubbleBehaviour : MonoBehaviour
{
	private float bubbleScale;

	private Time startTime;

	private float maxRad;

	private float growRate;

	private float popTimer;

	private bool startPopTimer;

	private float birthday;

	private float lifespan;

	private bool popMode;

	public BubbleBehaviour()
	{
		this.popMode = false;
	}

	public void Start()
	{
		this.maxRad = Random.Range(0.6f, 5f);
		this.growRate = this.maxRad / Random.Range((float)1, 100f);
		this.startPopTimer = false;
		this.birthday = Time.get_time();
		this.lifespan = (float)Random.Range(2, 8);
	}

	public void Update()
	{
		GameObject gameObject = GameObject.Find("tt_a_ara_ttc_gagShop");
		GS_RandomAnim gS_RandomAnim = (GS_RandomAnim)gameObject.GetComponentInChildren(typeof(GS_RandomAnim));
		bool flag = RuntimeServices.UnboxBoolean(RuntimeServices.GetProperty(gameObject.GetComponentInChildren(typeof(GS_RandomAnim)), "Awake"));
		if (!flag)
		{
			if (RuntimeServices.ToBool(RuntimeServices.InvokeBinaryOperator("op_GreaterThan", RuntimeServices.GetProperty(RuntimeServices.GetProperty(this.GetComponent(typeof(Rigidbody)), "velocity"), "y"), 0.05f)))
			{
			}
			if (Time.get_time() - this.birthday > this.lifespan)
			{
				this.Pop();
			}
			if (RuntimeServices.ToBool(RuntimeServices.InvokeBinaryOperator("op_LessThan", RuntimeServices.GetProperty(RuntimeServices.GetProperty(this.GetComponent(typeof(Rigidbody)), "velocity"), "y"), 0.001f)))
			{
				if (!this.startPopTimer)
				{
					this.startPopTimer = true;
					this.popTimer = Time.get_time();
				}
				else if (Time.get_time() - this.popTimer > (float)5)
				{
					this.Pop();
				}
			}
			else if (this.get_gameObject().get_transform().get_localScale().y <= this.maxRad)
			{
				float x = this.get_gameObject().get_transform().get_localScale().x + this.growRate;
				Vector3 localScale = this.get_gameObject().get_transform().get_localScale();
				float num = localScale.x = x;
				Vector3 vector;
				this.get_gameObject().get_transform().set_localScale(vector = localScale);
				float y = this.get_gameObject().get_transform().get_localScale().y + this.growRate;
				Vector3 localScale2 = this.get_gameObject().get_transform().get_localScale();
				float num2 = localScale2.y = y;
				Vector3 vector2;
				this.get_gameObject().get_transform().set_localScale(vector2 = localScale2);
				float z = this.get_gameObject().get_transform().get_localScale().z + this.growRate;
				Vector3 localScale3 = this.get_gameObject().get_transform().get_localScale();
				float num3 = localScale3.z = z;
				Vector3 vector3;
				this.get_gameObject().get_transform().set_localScale(vector3 = localScale3);
			}
		}
	}

	public void Pop()
	{
		if (!this.popMode)
		{
			this.popMode = true;
			this.get_audio().Play();
			int num = 0;
			Vector3 localScale = this.get_gameObject().get_transform().get_localScale();
			float num2 = localScale.x = (float)num;
			Vector3 vector;
			this.get_gameObject().get_transform().set_localScale(vector = localScale);
			int num3 = 0;
			Vector3 localScale2 = this.get_gameObject().get_transform().get_localScale();
			float num4 = localScale2.y = (float)num3;
			Vector3 vector2;
			this.get_gameObject().get_transform().set_localScale(vector2 = localScale2);
			int num5 = 0;
			Vector3 localScale3 = this.get_gameObject().get_transform().get_localScale();
			float num6 = localScale3.z = (float)num5;
			Vector3 vector3;
			this.get_gameObject().get_transform().set_localScale(vector3 = localScale3);
			Object.Destroy(this.get_gameObject(), (float)2);
		}
	}

	public void Main()
	{
	}
}
