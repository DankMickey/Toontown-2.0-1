using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class GS_RandomAnim : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class FallAsleepAnim$58 : GenericGenerator<WaitForSeconds>
	{
		internal GS_RandomAnim $self_224;

		public FallAsleepAnim$58(GS_RandomAnim self_)
		{
			this.$self_224 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GS_RandomAnim.FallAsleepAnim$58.$(this.$self_224);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class WakeUpAnim$59 : GenericGenerator<WaitForSeconds>
	{
		internal GS_RandomAnim $self_228;

		public WakeUpAnim$59(GS_RandomAnim self_)
		{
			this.$self_228 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GS_RandomAnim.WakeUpAnim$59.$(this.$self_228);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class HonkHorn$60 : GenericGenerator<WaitForSeconds>
	{
		internal GS_RandomAnim $self_230;

		public HonkHorn$60(GS_RandomAnim self_)
		{
			this.$self_230 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GS_RandomAnim.HonkHorn$60.$(this.$self_230);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class HonkHornSimple$61 : GenericGenerator<WaitForSeconds>
	{
		internal GS_RandomAnim $self_232;

		public HonkHornSimple$61(GS_RandomAnim self_)
		{
			this.$self_232 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GS_RandomAnim.HonkHornSimple$61.$(this.$self_232);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class AnvilDrop$62 : GenericGenerator<WaitForSeconds>
	{
		internal GS_RandomAnim $self_235;

		public AnvilDrop$62(GS_RandomAnim self_)
		{
			this.$self_235 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GS_RandomAnim.AnvilDrop$62.$(this.$self_235);
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

	public GS_RandomAnim()
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

	public void Start()
	{
		float num = 0f;
		this.printSwitch = false;
		this.canAnimate = false;
		GameObject gameObject = GameObject.FindWithTag("Player");
		num = Vector3.Distance(gameObject.get_transform().get_position(), this.get_transform().get_position());
		this.Awake = true;
		this.prevAwakeState = true;
		MonoBehaviour.print("snore length = " + this.get_animation().get_Item("sleepSnore2").get_clip().get_length());
		if (num > (float)this.activationDist)
		{
			this.get_animation().Play("idleNoAnim");
		}
		else
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
				float[] expr_15B = this.sleepLastTimeRun;
				expr_15B[RuntimeServices.NormalizeArrayIndex(expr_15B, i)] = Time.get_time();
				float[] expr_16E = this.sleepActionRateFloat;
				int arg_1B1_1 = RuntimeServices.NormalizeArrayIndex(expr_16E, i);
				Animation arg_18E_0 = this.get_animation();
				AnimInfo[] expr_181 = this.SleepActions;
				float arg_1B0_0 = arg_18E_0.get_Item(expr_181[RuntimeServices.NormalizeArrayIndex(expr_181, i)].anim).get_clip().get_length();
				AnimInfo[] expr_1A3 = this.SleepActions;
				expr_16E[arg_1B1_1] = unchecked(arg_1B0_0 * expr_1A3[RuntimeServices.NormalizeArrayIndex(expr_1A3, i)].delta);
			}
			for (i = 0; i < Extensions.get_length(this.IdleActions); i++)
			{
				float[] expr_1D4 = this.idleLastTimeRun;
				expr_1D4[RuntimeServices.NormalizeArrayIndex(expr_1D4, i)] = Time.get_time();
				float[] expr_1E7 = this.idleActionRateFloat;
				int arg_22A_1 = RuntimeServices.NormalizeArrayIndex(expr_1E7, i);
				Animation arg_207_0 = this.get_animation();
				AnimInfo[] expr_1FA = this.IdleActions;
				float arg_229_0 = arg_207_0.get_Item(expr_1FA[RuntimeServices.NormalizeArrayIndex(expr_1FA, i)].anim).get_clip().get_length();
				AnimInfo[] expr_21C = this.IdleActions;
				expr_1E7[arg_22A_1] = unchecked(arg_229_0 * expr_21C[RuntimeServices.NormalizeArrayIndex(expr_21C, i)].delta);
			}
			MonoBehaviour.print("sleepLastTimeRun.lengt = " + Extensions.get_length(this.sleepLastTimeRun));
			MonoBehaviour.print("idleLastTimeRun.lengt = " + Extensions.get_length(this.idleLastTimeRun));
			this.canAnimate = false;
			this.first_time = false;
		}
	}

	public void ToggleSleepWake()
	{
		if (this.Awake)
		{
			this.Awake = false;
			this.prevAwakeState = true;
		}
		else
		{
			MonoBehaviour.print("GAGSHOP: wake up");
			this.prevAwakeState = false;
			this.Awake = true;
		}
	}

	public IEnumerator FallAsleepAnim()
	{
		return new GS_RandomAnim.FallAsleepAnim$58(this).GetEnumerator();
	}

	public void FallAsleep()
	{
		this.Awake = false;
	}

	public void WakeUp()
	{
		this.Awake = true;
	}

	public IEnumerator WakeUpAnim()
	{
		return new GS_RandomAnim.WakeUpAnim$59(this).GetEnumerator();
	}

	public void Update()
	{
		int num = 0;
		float num2 = 0f;
		if (this.canAnimate)
		{
			GameObject gameObject = GameObject.FindWithTag("Player");
			num2 = Vector3.Distance(gameObject.get_transform().get_position(), this.get_transform().get_position());
			if (this.Awake)
			{
				if (!this.prevAwakeState)
				{
					MonoBehaviour.print("GAGSHOP: UPDATE: WAKE UP");
					this.wait_for = this.get_animation().get_Item("sleepIntoIdle").get_clip().get_length();
					MonoBehaviour.print("GAGSHOP: sleepIntoIdle wait_for = " + this.wait_for);
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
					float arg_11C_0 = Time.get_time();
					float[] expr_10F = this.idleLastTimeRun;
					float arg_130_0 = arg_11C_0 - expr_10F[RuntimeServices.NormalizeArrayIndex(expr_10F, this.choice)];
					float[] expr_123 = this.idleActionRateFloat;
					if (arg_130_0 < expr_123[RuntimeServices.NormalizeArrayIndex(expr_123, this.choice)])
					{
						this.choice = 0;
						this.wait_for = this.get_animation().get_Item(this.IdleActions[0].anim).get_length();
						this.startIdleTime = Time.get_time();
					}
					bool arg_189_0;
					if (arg_189_0 = (num2 <= (float)this.activationDist))
					{
						arg_189_0 = (this.choice == 0);
					}
					bool flag = arg_189_0 ?? (this.choice != 0);
					if (this.choice != this.prevChoice)
					{
						this.prevChoice = this.choice;
						Animation arg_1DE_0 = this.get_animation();
						AnimInfo[] expr_1CC = this.IdleActions;
						this.wait_for = arg_1DE_0.get_Item(expr_1CC[RuntimeServices.NormalizeArrayIndex(expr_1CC, this.choice)].anim).get_length();
						Animation arg_20B_0 = this.get_animation();
						AnimInfo[] expr_1F9 = this.IdleActions;
						if (arg_20B_0.get_Item(expr_1F9[RuntimeServices.NormalizeArrayIndex(expr_1F9, this.choice)].anim).get_wrapMode() == 2)
						{
							AnimInfo[] expr_227 = this.IdleActions;
							float arg_251_0 = expr_227[RuntimeServices.NormalizeArrayIndex(expr_227, this.choice)].min_dur;
							AnimInfo[] expr_23F = this.IdleActions;
							num = checked((int)Random.Range(arg_251_0, expr_23F[RuntimeServices.NormalizeArrayIndex(expr_23F, this.choice)].max_dur));
							this.wait_for *= (float)num;
						}
						float[] expr_26D = this.idleLastTimeRun;
						expr_26D[RuntimeServices.NormalizeArrayIndex(expr_26D, this.prevChoice)] = Time.get_time();
						if (flag)
						{
							Animation arg_2A3_0 = this.get_animation();
							AnimInfo[] expr_291 = this.IdleActions;
							arg_2A3_0.CrossFade(expr_291[RuntimeServices.NormalizeArrayIndex(expr_291, this.choice)].anim);
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
					MonoBehaviour.print("DO A NEW THING, prevChoice = " + this.prevChoice);
					MonoBehaviour.print("elapsed animation time = " + (Time.get_time() - this.startIdleTime));
					MonoBehaviour.print("wait_for = " + this.wait_for);
					this.choice = Random.Range(1, Extensions.get_length(this.SleepActions));
					string arg_3E2_0 = "TEMP sleep action choice " + this.choice + " = ";
					AnimInfo[] expr_3D0 = this.SleepActions;
					MonoBehaviour.print(arg_3E2_0 + expr_3D0[RuntimeServices.NormalizeArrayIndex(expr_3D0, this.choice)].anim);
					string arg_40F_0 = "time elapsed = ";
					float arg_409_0 = Time.get_time();
					float[] expr_3FC = this.sleepLastTimeRun;
					MonoBehaviour.print(arg_40F_0 + (arg_409_0 - expr_3FC[RuntimeServices.NormalizeArrayIndex(expr_3FC, this.choice)]));
					string arg_436_0 = "sleepActionRateFloat = ";
					float[] expr_424 = this.sleepActionRateFloat;
					MonoBehaviour.print(arg_436_0 + expr_424[RuntimeServices.NormalizeArrayIndex(expr_424, this.choice)]);
					float arg_458_0 = Time.get_time();
					float[] expr_44B = this.sleepLastTimeRun;
					float arg_46C_0 = arg_458_0 - expr_44B[RuntimeServices.NormalizeArrayIndex(expr_44B, this.choice)];
					float[] expr_45F = this.sleepActionRateFloat;
					if (arg_46C_0 < expr_45F[RuntimeServices.NormalizeArrayIndex(expr_45F, this.choice)])
					{
						this.choice = 0;
						this.wait_for = this.get_animation().get_Item(this.SleepActions[0].anim).get_length();
						this.startIdleTime = Time.get_time();
						MonoBehaviour.print("NO SWITCH, SO USE SLEEP0, wait for =" + this.wait_for);
					}
					if (this.choice != this.prevChoice)
					{
						this.prevChoice = this.choice;
						Animation arg_503_0 = this.get_animation();
						AnimInfo[] expr_4F1 = this.SleepActions;
						this.wait_for = arg_503_0.get_Item(expr_4F1[RuntimeServices.NormalizeArrayIndex(expr_4F1, this.choice)].anim).get_length();
						MonoBehaviour.print("SWITCHING to NEW ACTION, wait_for length = " + this.wait_for);
						Animation arg_54A_0 = this.get_animation();
						AnimInfo[] expr_538 = this.SleepActions;
						if (arg_54A_0.get_Item(expr_538[RuntimeServices.NormalizeArrayIndex(expr_538, this.choice)].anim).get_wrapMode() == 2)
						{
							AnimInfo[] expr_566 = this.SleepActions;
							float arg_590_0 = expr_566[RuntimeServices.NormalizeArrayIndex(expr_566, this.choice)].min_dur;
							AnimInfo[] expr_57E = this.SleepActions;
							num = checked((int)Random.Range(arg_590_0, expr_57E[RuntimeServices.NormalizeArrayIndex(expr_57E, this.choice)].max_dur));
							this.wait_for *= (float)num;
						}
						float[] expr_5AC = this.sleepLastTimeRun;
						expr_5AC[RuntimeServices.NormalizeArrayIndex(expr_5AC, this.prevChoice)] = Time.get_time();
						Animation arg_5DC_0 = this.get_animation();
						AnimInfo[] expr_5CA = this.SleepActions;
						arg_5DC_0.CrossFade(expr_5CA[RuntimeServices.NormalizeArrayIndex(expr_5CA, this.choice)].anim);
						this.startIdleTime = Time.get_time();
					}
				}
			}
		}
		this.canAnimate = ((Time.get_time() - this.startIdleTime >= this.wait_for) ?? (this.prevAwakeState != this.Awake));
		if (this.printSwitch)
		{
			MonoBehaviour.print("XXXX test for animation switching");
			MonoBehaviour.print("XXXX elapsed time = " + (Time.get_time() - this.startIdleTime));
			MonoBehaviour.print("XXXX wait for = " + this.wait_for);
		}
	}

	public void AnvilBang(object delay)
	{
		MonoBehaviour.print("BANG");
		this.get_animation().get_Item("anvilBang").set_layer(9);
		this.get_animation().get_Item("anvilBang").set_blendMode(0);
		this.get_animation().CrossFade("anvilBang");
	}

	public IEnumerator HonkHorn(object delay)
	{
		return new GS_RandomAnim.HonkHorn$60(this).GetEnumerator();
	}

	public IEnumerator HonkHornSimple(object delay)
	{
		return new GS_RandomAnim.HonkHornSimple$61(this).GetEnumerator();
	}

	public IEnumerator AnvilDrop(object delay)
	{
		return new GS_RandomAnim.AnvilDrop$62(this).GetEnumerator();
	}

	public void Main()
	{
	}
}
