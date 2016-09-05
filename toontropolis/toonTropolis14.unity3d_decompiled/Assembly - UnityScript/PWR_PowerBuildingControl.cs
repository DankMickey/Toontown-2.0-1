using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class PWR_PowerBuildingControl : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class ToggleSleepWake$53 : GenericGenerator<WaitForSeconds>
	{
		internal PWR_PowerBuildingControl $self_259;

		public ToggleSleepWake$53(PWR_PowerBuildingControl self_)
		{
			this.$self_259 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new PWR_PowerBuildingControl.ToggleSleepWake$53.$(this.$self_259);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class HonkHorn$54 : GenericGenerator<WaitForSeconds>
	{
		internal PWR_PowerBuildingControl $self_261;

		public HonkHorn$54(PWR_PowerBuildingControl self_)
		{
			this.$self_261 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new PWR_PowerBuildingControl.HonkHorn$54.$(this.$self_261);
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

	public PWR_PowerBuildingControl()
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
		return new PWR_PowerBuildingControl.ToggleSleepWake$53(this).GetEnumerator();
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
						this.get_animation().CrossFade("sleepIntoIdle", 0.1f);
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
						float arg_12A_0 = Time.get_time();
						float[] expr_11D = this.idleLastTimeRun;
						float arg_13E_0 = arg_12A_0 - expr_11D[RuntimeServices.NormalizeArrayIndex(expr_11D, this.choice)];
						float[] expr_131 = this.idleActionRateFloat;
						if (arg_13E_0 < expr_131[RuntimeServices.NormalizeArrayIndex(expr_131, this.choice)])
						{
							this.choice = 0;
							this.wait_for = this.get_animation().get_Item(this.IdleActions[0].anim).get_length();
						}
						bool arg_18C_0;
						if (arg_18C_0 = (num <= (float)this.activationDist))
						{
							arg_18C_0 = (this.choice == 0);
						}
						bool flag = arg_18C_0 ?? (this.choice != 0);
						if (this.choice != this.prevChoice)
						{
							this.prevChoice = this.choice;
							Animation arg_1E1_0 = this.get_animation();
							AnimInfo[] expr_1CF = this.IdleActions;
							this.wait_for = arg_1E1_0.get_Item(expr_1CF[RuntimeServices.NormalizeArrayIndex(expr_1CF, this.choice)].anim).get_length();
							Animation arg_20E_0 = this.get_animation();
							AnimInfo[] expr_1FC = this.IdleActions;
							if (arg_20E_0.get_Item(expr_1FC[RuntimeServices.NormalizeArrayIndex(expr_1FC, this.choice)].anim).get_wrapMode() == 2)
							{
								float arg_260_0 = this.wait_for;
								AnimInfo[] expr_231 = this.IdleActions;
								float arg_25B_0 = expr_231[RuntimeServices.NormalizeArrayIndex(expr_231, this.choice)].min_dur;
								AnimInfo[] expr_249 = this.IdleActions;
								this.wait_for = arg_260_0 * Random.Range(arg_25B_0, expr_249[RuntimeServices.NormalizeArrayIndex(expr_249, this.choice)].max_dur);
							}
							float[] expr_26C = this.idleLastTimeRun;
							expr_26C[RuntimeServices.NormalizeArrayIndex(expr_26C, this.prevChoice)] = Time.get_time();
							if (flag)
							{
								Animation arg_2A7_0 = this.get_animation();
								AnimInfo[] expr_290 = this.IdleActions;
								arg_2A7_0.CrossFade(expr_290[RuntimeServices.NormalizeArrayIndex(expr_290, this.choice)].anim, 0.1f);
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
						this.get_animation().CrossFade("idleIntoSleep", 0.25f);
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
						float arg_378_0 = Time.get_time();
						float[] expr_36B = this.sleepLastTimeRun;
						float arg_38C_0 = arg_378_0 - expr_36B[RuntimeServices.NormalizeArrayIndex(expr_36B, this.choice)];
						float[] expr_37F = this.sleepActionRateFloat;
						if (arg_38C_0 < expr_37F[RuntimeServices.NormalizeArrayIndex(expr_37F, this.choice)])
						{
							this.choice = 0;
							this.wait_for = this.get_animation().get_Item(this.SleepActions[0].anim).get_length();
						}
						if (this.choice != this.prevChoice)
						{
							this.prevChoice = this.choice;
							Animation arg_3FE_0 = this.get_animation();
							AnimInfo[] expr_3EC = this.SleepActions;
							this.wait_for = arg_3FE_0.get_Item(expr_3EC[RuntimeServices.NormalizeArrayIndex(expr_3EC, this.choice)].anim).get_length();
							Animation arg_42B_0 = this.get_animation();
							AnimInfo[] expr_419 = this.SleepActions;
							if (arg_42B_0.get_Item(expr_419[RuntimeServices.NormalizeArrayIndex(expr_419, this.choice)].anim).get_wrapMode() == 2)
							{
								float arg_47D_0 = this.wait_for;
								AnimInfo[] expr_44E = this.SleepActions;
								float arg_478_0 = expr_44E[RuntimeServices.NormalizeArrayIndex(expr_44E, this.choice)].min_dur;
								AnimInfo[] expr_466 = this.SleepActions;
								this.wait_for = arg_47D_0 * Random.Range(arg_478_0, expr_466[RuntimeServices.NormalizeArrayIndex(expr_466, this.choice)].max_dur);
							}
							float[] expr_489 = this.sleepLastTimeRun;
							expr_489[RuntimeServices.NormalizeArrayIndex(expr_489, this.prevChoice)] = Time.get_time();
							Animation arg_4BE_0 = this.get_animation();
							AnimInfo[] expr_4A7 = this.SleepActions;
							arg_4BE_0.CrossFade(expr_4A7[RuntimeServices.NormalizeArrayIndex(expr_4A7, this.choice)].anim, 0.1f);
							this.startIdleTime = Time.get_time();
						}
					}
				}
			}
			this.canAnimate = ((Time.get_time() - this.startIdleTime >= this.wait_for) ?? (this.prevAwakeState != this.Awake));
		}
	}

	public IEnumerator HonkHorn(object delay)
	{
		return new PWR_PowerBuildingControl.HonkHorn$54(this).GetEnumerator();
	}

	public void Main()
	{
	}
}
