using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class MUS_MusicShopBuildingControl : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class ToggleSleepWake$65 : GenericGenerator<WaitForSeconds>
	{
		internal MUS_MusicShopBuildingControl $self_238;

		public ToggleSleepWake$65(MUS_MusicShopBuildingControl self_)
		{
			this.$self_238 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new MUS_MusicShopBuildingControl.ToggleSleepWake$65.$(this.$self_238);
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

	public MUS_MusicShopBuildingControl()
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
		return new MUS_MusicShopBuildingControl.ToggleSleepWake$65(this).GetEnumerator();
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
						float arg_125_0 = Time.get_time();
						float[] expr_118 = this.idleLastTimeRun;
						float arg_139_0 = arg_125_0 - expr_118[RuntimeServices.NormalizeArrayIndex(expr_118, this.choice)];
						float[] expr_12C = this.idleActionRateFloat;
						if (arg_139_0 < expr_12C[RuntimeServices.NormalizeArrayIndex(expr_12C, this.choice)])
						{
							this.choice = 0;
							this.wait_for = this.get_animation().get_Item(this.IdleActions[0].anim).get_length();
						}
						bool arg_187_0;
						if (arg_187_0 = (num <= (float)this.activationDist))
						{
							arg_187_0 = (this.choice == 0);
						}
						bool flag = arg_187_0 ?? (this.choice != 0);
						if (this.choice != this.prevChoice)
						{
							this.prevChoice = this.choice;
							Animation arg_1DC_0 = this.get_animation();
							AnimInfo[] expr_1CA = this.IdleActions;
							this.wait_for = arg_1DC_0.get_Item(expr_1CA[RuntimeServices.NormalizeArrayIndex(expr_1CA, this.choice)].anim).get_length();
							Animation arg_209_0 = this.get_animation();
							AnimInfo[] expr_1F7 = this.IdleActions;
							if (arg_209_0.get_Item(expr_1F7[RuntimeServices.NormalizeArrayIndex(expr_1F7, this.choice)].anim).get_wrapMode() == 2)
							{
								float arg_25B_0 = this.wait_for;
								AnimInfo[] expr_22C = this.IdleActions;
								float arg_256_0 = expr_22C[RuntimeServices.NormalizeArrayIndex(expr_22C, this.choice)].min_dur;
								AnimInfo[] expr_244 = this.IdleActions;
								this.wait_for = arg_25B_0 * Random.Range(arg_256_0, expr_244[RuntimeServices.NormalizeArrayIndex(expr_244, this.choice)].max_dur);
							}
							float[] expr_267 = this.idleLastTimeRun;
							expr_267[RuntimeServices.NormalizeArrayIndex(expr_267, this.prevChoice)] = Time.get_time();
							if (flag)
							{
								Animation arg_29D_0 = this.get_animation();
								AnimInfo[] expr_28B = this.IdleActions;
								arg_29D_0.CrossFade(expr_28B[RuntimeServices.NormalizeArrayIndex(expr_28B, this.choice)].anim);
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
						float arg_369_0 = Time.get_time();
						float[] expr_35C = this.sleepLastTimeRun;
						float arg_37D_0 = arg_369_0 - expr_35C[RuntimeServices.NormalizeArrayIndex(expr_35C, this.choice)];
						float[] expr_370 = this.sleepActionRateFloat;
						if (arg_37D_0 < expr_370[RuntimeServices.NormalizeArrayIndex(expr_370, this.choice)])
						{
							this.choice = 0;
							this.wait_for = this.get_animation().get_Item(this.SleepActions[0].anim).get_length();
						}
						if (this.choice != this.prevChoice)
						{
							this.prevChoice = this.choice;
							Animation arg_3EF_0 = this.get_animation();
							AnimInfo[] expr_3DD = this.SleepActions;
							this.wait_for = arg_3EF_0.get_Item(expr_3DD[RuntimeServices.NormalizeArrayIndex(expr_3DD, this.choice)].anim).get_length();
							Animation arg_41C_0 = this.get_animation();
							AnimInfo[] expr_40A = this.SleepActions;
							if (arg_41C_0.get_Item(expr_40A[RuntimeServices.NormalizeArrayIndex(expr_40A, this.choice)].anim).get_wrapMode() == 2)
							{
								float arg_46E_0 = this.wait_for;
								AnimInfo[] expr_43F = this.SleepActions;
								float arg_469_0 = expr_43F[RuntimeServices.NormalizeArrayIndex(expr_43F, this.choice)].min_dur;
								AnimInfo[] expr_457 = this.SleepActions;
								this.wait_for = arg_46E_0 * Random.Range(arg_469_0, expr_457[RuntimeServices.NormalizeArrayIndex(expr_457, this.choice)].max_dur);
							}
							float[] expr_47A = this.sleepLastTimeRun;
							expr_47A[RuntimeServices.NormalizeArrayIndex(expr_47A, this.prevChoice)] = Time.get_time();
							Animation arg_4AA_0 = this.get_animation();
							AnimInfo[] expr_498 = this.SleepActions;
							arg_4AA_0.CrossFade(expr_498[RuntimeServices.NormalizeArrayIndex(expr_498, this.choice)].anim);
							this.startIdleTime = Time.get_time();
						}
					}
				}
			}
			bool arg_4EF_1;
			if (!(arg_4EF_1 = (Time.get_time() - this.startIdleTime >= this.wait_for)) && (arg_4EF_1 = !this.prevAwakeState))
			{
				arg_4EF_1 = this.Awake;
			}
			this.canAnimate = arg_4EF_1;
		}
	}

	public void Main()
	{
	}
}
