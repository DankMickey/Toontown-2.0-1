using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class BubbleGen : MonoBehaviour
{
	public Rigidbody thisBubble;

	public float speed;

	private bool oneShot;

	private float startBubbles;

	private float endBubbles;

	private Transform mainBody;

	private GameObject gagShop;

	private GS_RandomAnim randomAnimScript;

	private bool localAwake;

	private float age;

	private int nBubbles;

	public void Start()
	{
		this.nBubbles = 0;
		this.gagShop = GameObject.Find("tt_a_ara_ttc_gagShop");
		this.startBubbles = (float)2 / this.gagShop.get_animation().get_Item("sleepSnore2").get_length();
		this.endBubbles = 2.2f / this.gagShop.get_animation().get_Item("sleepSnore2").get_length();
		this.mainBody = this.get_transform().Find("tt_a_ara_ttc_gagShop");
		this.gagShop = GameObject.Find("tt_a_ara_ttc_gagShop");
		this.randomAnimScript = (GS_RandomAnim)this.gagShop.GetComponentInChildren(typeof(GS_RandomAnim));
	}

	public float GetAge()
	{
		return this.age;
	}

	public void FixedUpdate()
	{
		if (this.nBubbles <= 25)
		{
			this.localAwake = RuntimeServices.UnboxBoolean(RuntimeServices.GetProperty(this.gagShop.GetComponentInChildren(typeof(GS_RandomAnim)), "Awake"));
			if (!this.localAwake)
			{
				float num = 0f;
				num = this.gagShop.get_animation().get_Item("sleepSnore2").get_normalizedTime() % (float)1;
				if (num > this.startBubbles && num < this.endBubbles)
				{
					int num2 = 0;
					Rigidbody rigidbody = (Rigidbody)Object.Instantiate(this.thisBubble, this.get_transform().get_position(), this.get_transform().get_rotation());
					float num3 = (float)Random.Range(-6, 6);
					rigidbody.set_velocity(new Vector3((float)-20, (float)25, num3));
					GameObject gameObject = GameObject.Find("gagS:def_chimney_A4");
					Physics.IgnoreCollision((Collider)RuntimeServices.Coerce(rigidbody.get_collider(), typeof(Collider)), (Collider)RuntimeServices.Coerce(gameObject.get_collider(), typeof(Collider)));
				}
			}
		}
	}

	public void Main()
	{
	}
}
