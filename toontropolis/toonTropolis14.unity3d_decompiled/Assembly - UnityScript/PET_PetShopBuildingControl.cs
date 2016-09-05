using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class PET_PetShopBuildingControl : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class ToggleSleepWake$69 : GenericGenerator<WaitForSeconds>
	{
		internal PET_PetShopBuildingControl $self_251;

		public ToggleSleepWake$69(PET_PetShopBuildingControl self_)
		{
			this.$self_251 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new PET_PetShopBuildingControl.ToggleSleepWake$69.$(this.$self_251);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class HonkHorn$70 : GenericGenerator<WaitForSeconds>
	{
		internal PET_PetShopBuildingControl $self_253;

		public HonkHorn$70(PET_PetShopBuildingControl self_)
		{
			this.$self_253 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new PET_PetShopBuildingControl.HonkHorn$70.$(this.$self_253);
		}
	}

	private bool canAnimate;

	public int choice;

	public int timer;

	public int activationDist;

	public AnimInfo[] IdleActions;

	public AnimInfo[] SleepActions;

	private bool Awake;

	private bool printSwitch;

	private bool prevAwakeState;

	private float startIdleTime;

	private int prevChoice;

	private bool first_time;

	private float[] idleActionRateFloat;

	private float[] sleepActionRateFloat;

	private float[] idleLastTimeRun;

	private float[] sleepLastTimeRun;

	private float wait_for;

	private string[] animationList;

	private Animation animationComp;

	public PET_PetShopBuildingControl()
	{
		this.canAnimate = false;
		this.choice = 0;
		this.timer = 0;
		this.activationDist = 500;
	}

	public bool IsAwake()
	{
		return this.Awake;
	}

	public string[] GetAnimationNames(object anim)
	{
		Array array = new Array();
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(anim);
		while (enumerator.MoveNext())
		{
			AnimationState animationState = (AnimationState)RuntimeServices.Coerce(enumerator.Current, typeof(AnimationState));
			array.Add(animationState.get_name());
			UnityRuntimeServices.Update(enumerator, animationState);
		}
		return (string[])array.ToBuiltin(typeof(string));
	}

	public void Start()
	{
		float num = 0f;
		this.printSwitch = false;
		this.canAnimate = false;
		GameObject gameObject = GameObject.FindWithTag("Player");
		this.animationComp = (Animation)this.GetComponentInChildren(typeof(Animation));
		this.animationList = this.GetAnimationNames(this.animationComp);
		this.Awake = true;
		this.prevAwakeState = true;
		if (gameObject)
		{
			num = Vector3.Distance(gameObject.get_transform().get_position(), this.get_transform().get_position());
		}
		else
		{
			num = (float)(checked(this.activationDist + 10));
		}
		if (num <= (float)this.activationDist)
		{
			this.get_animation().Play("idle0");
		}
		this.startIdleTime = Time.get_time();
		this.prevChoice = 0;
		this.wait_for = (float)2 * this.get_animation().get_Item(this.SleepActions[0].anim).get_clip().get_length();
		this.idleLastTimeRun = new float[Extensions.get_length(this.IdleActions)];
		this.idleActionRateFloat = new float[Extensions.get_length(this.IdleActions)];
		this.sleepLastTimeRun = new float[Extensions.get_length(this.SleepActions)];
		this.sleepActionRateFloat = new float[Extensions.get_length(this.SleepActions)];
		int i = 0;
		checked
		{
			for (i = 0; i < Extensions.get_length(this.SleepActions); i++)
			{
				float[] expr_165 = this.sleepLastTimeRun;
				expr_165[RuntimeServices.NormalizeArrayIndex(expr_165, i)] = Time.get_time();
				float[] expr_178 = this.sleepActionRateFloat;
				int arg_1BB_1 = RuntimeServices.NormalizeArrayIndex(expr_178, i);
				Animation arg_198_0 = this.get_animation();
				AnimInfo[] expr_18B = this.SleepActions;
				float arg_1BA_0 = arg_198_0.get_Item(expr_18B[RuntimeServices.NormalizeArrayIndex(expr_18B, i)].anim).get_clip().get_length();
				AnimInfo[] expr_1AD = this.SleepActions;
				expr_178[arg_1BB_1] = unchecked(arg_1BA_0 * expr_1AD[RuntimeServices.NormalizeArrayIndex(expr_1AD, i)].delta);
			}
			for (i = 0; i < Extensions.get_length(this.IdleActions); i++)
			{
				float[] expr_1DE = this.idleLastTimeRun;
				expr_1DE[RuntimeServices.NormalizeArrayIndex(expr_1DE, i)] = Time.get_time();
				float[] expr_1F1 = this.idleActionRateFloat;
				int arg_234_1 = RuntimeServices.NormalizeArrayIndex(expr_1F1, i);
				Animation arg_211_0 = this.get_animation();
				AnimInfo[] expr_204 = this.IdleActions;
				float arg_233_0 = arg_211_0.get_Item(expr_204[RuntimeServices.NormalizeArrayIndex(expr_204, i)].anim).get_clip().get_length();
				AnimInfo[] expr_226 = this.IdleActions;
				expr_1F1[arg_234_1] = unchecked(arg_233_0 * expr_226[RuntimeServices.NormalizeArrayIndex(expr_226, i)].delta);
			}
			this.canAnimate = false;
			this.first_time = true;
		}
	}

	public IEnumerator ToggleSleepWake()
	{
		return new PET_PetShopBuildingControl.ToggleSleepWake$69(this).GetEnumerator();
	}

	public void FallAsleep()
	{
		this.Awake = false;
	}

	public void WakeUp()
	{
		this.Awake = true;
	}

	public void Update()
	{
		float num = 0f;
		int num2 = 0;
		if (this.first_time)
		{
			this.first_time = false;
		}
		else
		{
			if (this.canAnimate)
			{
				GameObject gameObject = GameObject.FindWithTag("Player");
				if (gameObject)
				{
					num = Vector3.Distance(gameObject.get_transform().get_position(), this.get_transform().get_position());
				}
				else
				{
					num = (float)(checked(this.activationDist + 10));
				}
				if (this.Awake)
				{
					if (!this.prevAwakeState)
					{
						this.wait_for = this.get_animation().get_Item("sleepIntoIdle").get_clip().get_length();
						this.get_animation().CrossFade("sleepIntoIdle");
						this.idleLastTimeRun[1] = Time.get_time();
						this.idleLastTimeRun[2] = Time.get_time();
						this.choice = -1;
						this.prevChoice = -1;
						this.prevAwakeState = true;
						this.startIdleTime = Time.get_time();
					}
					else
					{
						this.choice = Random.Range(1, Extensions.get_length(this.IdleActions));
						float arg_12D_0 = Time.get_time();
						float[] expr_120 = this.idleLastTimeRun;
						float arg_141_0 = arg_12D_0 - expr_120[RuntimeServices.NormalizeArrayIndex(expr_120, this.choice)];
						float[] expr_134 = this.idleActionRateFloat;
						if (arg_141_0 < expr_134[RuntimeServices.NormalizeArrayIndex(expr_134, this.choice)])
						{
							this.choice = 0;
							this.wait_for = this.get_animation().get_Item(this.IdleActions[0].anim).get_length();
						}
						bool arg_18F_0;
						if (arg_18F_0 = (num <= (float)this.activationDist))
						{
							arg_18F_0 = (this.choice == 0);
						}
						bool flag = arg_18F_0 ?? (this.choice != 0);
						if (this.choice != this.prevChoice)
						{
							this.prevChoice = this.choice;
							Animation arg_1E4_0 = this.get_animation();
							AnimInfo[] expr_1D2 = this.IdleActions;
							this.wait_for = arg_1E4_0.get_Item(expr_1D2[RuntimeServices.NormalizeArrayIndex(expr_1D2, this.choice)].anim).get_length();
							Animation arg_211_0 = this.get_animation();
							AnimInfo[] expr_1FF = this.IdleActions;
							if (arg_211_0.get_Item(expr_1FF[RuntimeServices.NormalizeArrayIndex(expr_1FF, this.choice)].anim).get_wrapMode() == 2)
							{
								AnimInfo[] expr_22D = this.IdleActions;
								float arg_257_0 = expr_22D[RuntimeServices.NormalizeArrayIndex(expr_22D, this.choice)].min_dur;
								AnimInfo[] expr_245 = this.IdleActions;
								num2 = checked((int)Random.Range(arg_257_0, expr_245[RuntimeServices.NormalizeArrayIndex(expr_245, this.choice)].max_dur));
								this.wait_for *= (float)num2;
							}
							float[] expr_273 = this.idleLastTimeRun;
							expr_273[RuntimeServices.NormalizeArrayIndex(expr_273, this.prevChoice)] = Time.get_time();
							if (flag)
							{
								Animation arg_2A9_0 = this.get_animation();
								AnimInfo[] expr_297 = this.IdleActions;
								arg_2A9_0.CrossFade(expr_297[RuntimeServices.NormalizeArrayIndex(expr_297, this.choice)].anim);
							}
							this.startIdleTime = Time.get_time();
						}
					}
				}
				else if (!this.Awake)
				{
					if (this.prevAwakeState)
					{
						this.wait_for = this.get_animation().get_Item("idleIntoSleep").get_clip().get_length();
						this.get_animation().CrossFade("idleIntoSleep");
						this.sleepLastTimeRun[1] = Time.get_time();
						this.sleepLastTimeRun[2] = Time.get_time();
						this.choice = -1;
						this.prevChoice = -1;
						this.prevAwakeState = false;
						this.startIdleTime = Time.get_time();
					}
					else
					{
						this.choice = Random.Range(1, Extensions.get_length(this.SleepActions));
						float arg_375_0 = Time.get_time();
						float[] expr_368 = this.sleepLastTimeRun;
						float arg_389_0 = arg_375_0 - expr_368[RuntimeServices.NormalizeArrayIndex(expr_368, this.choice)];
						float[] expr_37C = this.sleepActionRateFloat;
						if (arg_389_0 < expr_37C[RuntimeServices.NormalizeArrayIndex(expr_37C, this.choice)])
						{
							this.choice = 0;
							this.wait_for = this.get_animation().get_Item(this.SleepActions[0].anim).get_length();
						}
						if (this.choice != this.prevChoice)
						{
							this.prevChoice = this.choice;
							Animation arg_3FB_0 = this.get_animation();
							AnimInfo[] expr_3E9 = this.SleepActions;
							this.wait_for = arg_3FB_0.get_Item(expr_3E9[RuntimeServices.NormalizeArrayIndex(expr_3E9, this.choice)].anim).get_length();
							Animation arg_428_0 = this.get_animation();
							AnimInfo[] expr_416 = this.SleepActions;
							if (arg_428_0.get_Item(expr_416[RuntimeServices.NormalizeArrayIndex(expr_416, this.choice)].anim).get_wrapMode() == 2)
							{
								AnimInfo[] expr_444 = this.SleepActions;
								float arg_46E_0 = expr_444[RuntimeServices.NormalizeArrayIndex(expr_444, this.choice)].min_dur;
								AnimInfo[] expr_45C = this.SleepActions;
								num2 = checked((int)Random.Range(arg_46E_0, expr_45C[RuntimeServices.NormalizeArrayIndex(expr_45C, this.choice)].max_dur));
								this.wait_for *= (float)num2;
							}
							float[] expr_48A = this.sleepLastTimeRun;
							expr_48A[RuntimeServices.NormalizeArrayIndex(expr_48A, this.prevChoice)] = Time.get_time();
							Animation arg_4BA_0 = this.get_animation();
							AnimInfo[] expr_4A8 = this.SleepActions;
							arg_4BA_0.CrossFade(expr_4A8[RuntimeServices.NormalizeArrayIndex(expr_4A8, this.choice)].anim);
							this.startIdleTime = Time.get_time();
						}
					}
				}
			}
			bool arg_4FF_1;
			if (!(arg_4FF_1 = (Time.get_time() - this.startIdleTime >= this.wait_for)) && (arg_4FF_1 = !this.prevAwakeState))
			{
				arg_4FF_1 = this.Awake;
			}
			this.canAnimate = arg_4FF_1;
		}
	}

	public IEnumerator HonkHorn(object delay)
	{
		return new PET_PetShopBuildingControl.HonkHorn$70(this).GetEnumerator();
	}

	public void Main()
	{
	}
}
