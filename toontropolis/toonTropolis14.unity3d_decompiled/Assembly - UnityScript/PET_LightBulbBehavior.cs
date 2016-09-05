using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PET_LightBulbBehavior : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class ToggleSleepWake$67 : GenericGenerator<WaitForSeconds>
	{
		internal PET_LightBulbBehavior $self_248;

		public ToggleSleepWake$67(PET_LightBulbBehavior self_)
		{
			this.$self_248 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new PET_LightBulbBehavior.ToggleSleepWake$67.$(this.$self_248);
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

	private object sputtering;

	private bool Awake;

	public void Start()
	{
		this.firstTime = true;
		this.lightOn = false;
		this.prevLightOn = false;
		this.sputtering = false;
		this.lightTimer = Time.get_time();
		this.Awake = true;
	}

	public IEnumerator ToggleSleepWake()
	{
		return new PET_LightBulbBehavior.ToggleSleepWake$67(this).GetEnumerator();
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
		this.lightBulbLight = GameObject.Find("myLightBulbLight");
		if (!this.lightBulbLight)
		{
			MonoBehaviour.print("can't find lightbulb");
		}
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

	public void SputterOff()
	{
		this.lightBulbLight = GameObject.Find("myLightBulbLight");
		this.lightBulbLight.get_light().set_intensity((float)0);
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
			if (!this.prevLightOn && RuntimeServices.ToBool(RuntimeServices.InvokeBinaryOperator("op_LessThan", RuntimeServices.InvokeBinaryOperator("op_Subtraction", Time.get_time(), this.sputterOnTime), 10)))
			{
				this.SputterOn(true);
			}
			else
			{
				this.lightOn = true;
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
