using System;
using UnityEngine;

[Serializable]
public class AstronautBehavior : MonoBehaviour
{
	private bool canAnimate;

	private int choice;

	private bool first_time;

	private float startIdleTime;

	private float wait_for;

	public bool alwaysIdle;

	public float dead_time;

	public bool gameOver;

	private bool cheerOn;

	private bool duckOn;

	private float lastTimeCheered;

	public float cheerDelay;

	private float lastTimeDucked;

	public float duckDelay;

	private float lastTimeLook;

	public float lookDelay;

	public float lookAllowedTime;

	public AstronautBehavior()
	{
		this.canAnimate = true;
		this.alwaysIdle = true;
		this.cheerDelay = (float)8;
		this.duckDelay = (float)4;
		this.lookDelay = (float)10;
		this.lookAllowedTime = (float)3;
	}

	public override void Start()
	{
		this.lastTimeCheered = Time.get_time();
		this.lastTimeDucked = Time.get_time();
		this.lastTimeLook = Time.get_time();
		this.first_time = true;
		this.canAnimate = true;
		this.wait_for = 10f;
		this.get_animation().CrossFade("idle");
		this.startIdleTime = Time.get_time();
		this.gameOver = false;
	}

	public override void PlayGameOver()
	{
		this.gameOver = true;
		this.get_animation().Play("gameOver");
	}

	public override void PlayGameOverWin()
	{
		this.gameOver = true;
		this.get_animation().Play("victoryDance");
	}

	public override void PlayCheer()
	{
		if (Time.get_time() - this.lastTimeCheered > this.cheerDelay)
		{
			this.lastTimeCheered = Time.get_time();
			this.cheerOn = true;
		}
	}

	public override void PlayDuck()
	{
		if (Time.get_time() - this.lastTimeDucked > this.duckDelay)
		{
			this.duckOn = true;
		}
	}

	public override void Update()
	{
		if (!this.gameOver)
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
					if (this.cheerOn)
					{
						this.choice = 8;
					}
					else if (this.duckOn)
					{
						this.choice = 9;
					}
					else
					{
						this.choice = Random.Range(1, 7);
					}
					int num = this.choice;
					if (num == 1)
					{
						this.get_animation().CrossFade("pullLever");
						this.wait_for = this.get_animation().get_Item("pullLever").get_length();
						this.startIdleTime = Time.get_time();
					}
					else if (num == 2)
					{
						this.get_animation().CrossFade("idle");
						this.wait_for = this.get_animation().get_Item("idle").get_length();
						this.startIdleTime = Time.get_time();
					}
					else if (num == 3)
					{
						this.get_animation().CrossFade("tapConsole");
						this.wait_for = this.get_animation().get_Item("tapConsole").get_clip().get_length();
						this.startIdleTime = Time.get_time();
					}
					else if (num == 4)
					{
						this.get_animation().CrossFade("twistKnobsHi");
						this.wait_for = this.get_animation().get_Item("twistKnobsHi").get_clip().get_length();
						this.startIdleTime = Time.get_time();
					}
					else if (num == 5)
					{
						this.get_animation().CrossFade("lookR");
						this.wait_for = this.get_animation().get_Item("lookR").get_clip().get_length();
						this.startIdleTime = Time.get_time();
					}
					else if (num == 6)
					{
						this.get_animation().CrossFade("lookL");
						this.wait_for = this.get_animation().get_Item("lookL").get_clip().get_length();
						this.startIdleTime = Time.get_time();
					}
					else if (num == 7)
					{
						this.get_animation().CrossFade("twistKnobsLo");
						this.wait_for = this.get_animation().get_Item("twistKnobsLo").get_clip().get_length();
						this.startIdleTime = Time.get_time();
					}
					else if (num == 8)
					{
						this.get_animation().CrossFade("cheerR");
						this.wait_for = this.get_animation().get_Item("cheerR").get_clip().get_length();
						this.startIdleTime = Time.get_time();
						this.cheerOn = false;
					}
					else if (num == 9)
					{
						this.get_animation().CrossFade("duck");
						this.wait_for = this.get_animation().get_Item("duck").get_clip().get_length();
						this.startIdleTime = Time.get_time();
						this.lastTimeDucked = Time.get_time();
						this.duckOn = false;
					}
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
	}

	public override void Main()
	{
	}
}
