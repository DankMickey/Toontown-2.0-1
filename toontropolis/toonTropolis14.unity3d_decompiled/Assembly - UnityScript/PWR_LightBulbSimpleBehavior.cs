using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PWR_LightBulbSimpleBehavior : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class ToggleSleepWake$72 : GenericGenerator<WaitForSeconds>
	{
		internal PWR_LightBulbSimpleBehavior $self_257;

		public ToggleSleepWake$72(PWR_LightBulbSimpleBehavior self_)
		{
			this.$self_257 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new PWR_LightBulbSimpleBehavior.ToggleSleepWake$72.$(this.$self_257);
		}
	}

	private bool lightOn;

	public float setIntensity;

	private bool prevLightOn;

	private GameObject lightBulbLight;

	private float lightTimer;

	private object sputterOnTime;

	private bool firstTime;

	public float BulbIntensityBaseValue;

	public float BulbSineCycleIntensity;

	public float BulbIntensityFlickerValue;

	public float BulbSineCycleIntensitySpeed;

	public int LRC;

	public bool CanSputter;

	private object sputtering;

	private bool Awake;

	public PWR_LightBulbSimpleBehavior()
	{
		this.CanSputter = false;
	}

	public void Start()
	{
		this.Awake = true;
		this.firstTime = true;
		this.lightOn = false;
		this.prevLightOn = false;
		this.sputtering = false;
		this.lightTimer = Time.get_time();
	}

	public IEnumerator ToggleSleepWake()
	{
		return new PWR_LightBulbSimpleBehavior.ToggleSleepWake$72(this).GetEnumerator();
	}

	public void SetLightState(object on)
	{
		this.lightOn = RuntimeServices.UnboxBoolean(on);
	}

	public bool GetLightState()
	{
		return this.lightOn;
	}

	public void SputterOn(object cycle)
	{
		if (RuntimeServices.EqualityOperator(this.LRC, "0"))
		{
			this.lightBulbLight = GameObject.Find("lightBulbLightL");
		}
		else if (this.LRC == 1)
		{
			this.lightBulbLight = GameObject.Find("lightBulbLightR");
		}
		else if (this.LRC == 2)
		{
			this.lightBulbLight = GameObject.Find("lightBulbLight");
		}
		if (this.lightBulbLight)
		{
			if (RuntimeServices.ToBool(cycle))
			{
				this.BulbSineCycleIntensity += Random.Range(0.01f, (float)1) * this.BulbSineCycleIntensitySpeed;
				this.setIntensity = this.BulbIntensityBaseValue + (Mathf.Sin(this.BulbSineCycleIntensity * 0.0174532924f) * (this.BulbIntensityFlickerValue / 2f) + this.BulbIntensityFlickerValue / 2f);
			}
			else
			{
				this.setIntensity = (float)1;
			}
			this.lightBulbLight.get_light().set_intensity(this.setIntensity);
		}
	}

	public void SputterOff()
	{
		if (this.LRC == 0)
		{
			this.lightBulbLight = GameObject.Find("lightBulbLightL");
		}
		else if (this.LRC == 1)
		{
			this.lightBulbLight = GameObject.Find("lightBulbLightR");
		}
		else if (this.LRC == 2)
		{
			this.lightBulbLight = GameObject.Find("lightBulbLight");
		}
		if (this.lightBulbLight)
		{
			this.lightBulbLight.get_light().set_intensity((float)0);
		}
	}

	public void Update()
	{
		if (this.firstTime)
		{
			this.firstTime = false;
			this.SputterOff();
		}
		else if (this.lightOn)
		{
			if (!this.prevLightOn && !RuntimeServices.ToBool(this.sputtering))
			{
				this.sputtering = true;
				this.sputterOnTime = Time.get_time();
				this.CallAudio();
			}
			if (!this.prevLightOn && RuntimeServices.ToBool(RuntimeServices.InvokeBinaryOperator("op_LessThan", RuntimeServices.InvokeBinaryOperator("op_Subtraction", Time.get_time(), this.sputterOnTime), 1.25f)) && this.CanSputter)
			{
				this.SputterOn(true);
			}
			else if (!this.prevLightOn)
			{
				this.sputtering = false;
				this.prevLightOn = true;
				this.SputterOn(false);
			}
		}
		else if (this.prevLightOn)
		{
			this.prevLightOn = false;
			this.SputterOff();
		}
	}

	public void CallAudio()
	{
		this.get_audio().Play();
	}

	public void Main()
	{
	}
}
