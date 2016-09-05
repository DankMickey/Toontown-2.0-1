using System;
using UnityEngine;

[Serializable]
public class OwlBehaviorScript : MonoBehaviour
{
	private bool canAnimate;

	private int choice;

	public AnimationClip animA;

	public AnimationClip animB;

	public AnimationClip animC;

	public AnimationClip animD;

	private bool first_time;

	private float startIdleTime;

	private float wait_for;

	public bool alwaysIdle;

	public float dead_time;

	public OwlBehaviorScript()
	{
		this.canAnimate = true;
		this.alwaysIdle = true;
		this.dead_time = (float)0;
	}

	public void Start()
	{
		this.first_time = true;
		this.canAnimate = true;
		this.wait_for = 10f;
		this.get_animation().CrossFade(this.animA.get_name());
		this.startIdleTime = Time.get_time();
	}

	public void Update()
	{
		if (this.first_time)
		{
			this.first_time = false;
			this.canAnimate = true;
		}
		else
		{
			if (this.canAnimate)
			{
				this.choice = Random.Range(1, 6);
				int num = this.choice;
				if (num == 1)
				{
					this.get_animation().CrossFade(this.animA.get_name());
					this.wait_for = this.get_animation().get_Item(this.animA.get_name()).get_length();
					this.startIdleTime = Time.get_time();
				}
				else if (num == 2)
				{
					this.get_animation().CrossFade(this.animB.get_name());
					this.wait_for = this.get_animation().get_Item(this.animB.get_name()).get_clip().get_length();
					this.startIdleTime = Time.get_time();
				}
				else if (num == 3)
				{
					this.get_animation().CrossFade(this.animC.get_name());
					this.wait_for = this.get_animation().get_Item(this.animC.get_name()).get_clip().get_length();
					this.startIdleTime = Time.get_time();
				}
				else if (num == 4)
				{
					this.get_animation().CrossFade(this.animD.get_name());
					this.wait_for = this.get_animation().get_Item(this.animD.get_name()).get_clip().get_length();
					this.startIdleTime = Time.get_time();
				}
				else if (num == 5)
				{
					this.get_animation().CrossFade(this.animA.get_name());
					int num2 = Random.Range(3, 10);
					this.wait_for = (float)num2 * this.get_animation().get_Item(this.animA.get_name()).get_length();
					this.startIdleTime = Time.get_time();
				}
				this.wait_for += (float)Random.Range(3, 11);
			}
			if (Time.get_time() - this.startIdleTime >= this.wait_for)
			{
				this.canAnimate = true;
			}
			else
			{
				this.canAnimate = false;
			}
		}
	}

	public void Main()
	{
	}
}
