using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class IdleLogic : MonoBehaviour
{
	public int indexAction;

	public float[] actionRateFloat;

	public GUIText statusGUI;

	public bool canLookAround;

	public float t;

	public float speed;

	private float rate;

	private bool run;

	public PetState petStateScript;

	public static bool firstTime = true;

	private float[] lastTimeRun;

	public AnimInfo[] IdleActions;

	private int num_iter;

	private float run_length;

	public string outOfAnim;

	public string intoAnim;

	public string useAction;

	public static int actionIndex = 0;

	public IdleLogic()
	{
		this.canLookAround = false;
		this.t = (float)0;
		this.speed = (float)12;
		this.rate = (float)5;
		this.run = false;
		this.outOfAnim = string.Empty;
		this.intoAnim = string.Empty;
	}

	public void Start()
	{
		float num = 0f;
		this.actionRateFloat = new float[Extensions.get_length(this.IdleActions)];
		this.lastTimeRun = new float[Extensions.get_length(this.IdleActions)];
		if (IdleLogic.firstTime)
		{
			IdleLogic.actionIndex = 0;
			checked
			{
				for (int i = 0; i < Extensions.get_length(this.IdleActions); i++)
				{
					float[] expr_53 = this.actionRateFloat;
					int arg_96_1 = RuntimeServices.NormalizeArrayIndex(expr_53, i);
					Animation arg_73_0 = this.get_animation();
					AnimInfo[] expr_66 = this.IdleActions;
					float arg_95_0 = arg_73_0.get_Item(expr_66[RuntimeServices.NormalizeArrayIndex(expr_66, i)].anim).get_clip().get_length();
					AnimInfo[] expr_88 = this.IdleActions;
					unchecked
					{
						expr_53[arg_96_1] = arg_95_0 * expr_88[RuntimeServices.NormalizeArrayIndex(expr_88, i)].max_dur;
						float[] expr_9D = this.lastTimeRun;
						expr_9D[RuntimeServices.NormalizeArrayIndex(expr_9D, i)] = 100f * (float)-1;
					}
				}
				IdleLogic.firstTime = false;
				this.lastTimeRun[0] = Time.get_time();
				this.num_iter = (int)Random.Range((float)1, this.IdleActions[0].max_dur);
			}
			this.run_length = (float)this.num_iter * this.get_animation().get_Item(this.IdleActions[0].anim).get_clip().get_length();
			this.get_animation().Play(this.IdleActions[0].anim);
		}
	}

	public void Update()
	{
	}

	public void Into()
	{
		int num = 0;
		if (Extensions.get_length(this.intoAnim) > 0)
		{
			this.get_animation().CrossFade(this.intoAnim, 0.15f);
			this.useAction = this.outOfAnim;
		}
		else
		{
			this.useAction = this.IdleActions[0].anim;
			num = 1;
			this.run_length = (float)num * this.get_animation().get_Item(this.IdleActions[0].anim).get_clip().get_length();
			this.get_animation().CrossFade(this.IdleActions[0].anim, 0.15f);
			this.lastTimeRun[0] = Time.get_time();
			IdleLogic.actionIndex = 0;
		}
	}

	public void Outof()
	{
		if (Extensions.get_length(this.outOfAnim) > 0)
		{
			this.get_animation().CrossFade(this.outOfAnim, 0.15f);
			this.useAction = this.outOfAnim;
		}
		else
		{
			this.useAction = this.IdleActions[0].anim;
		}
	}

	public void IdleAnimation()
	{
		float num = 0f;
		PetState petState = (PetState)this.GetComponent(typeof(PetState));
		bool flag = false;
		int num2 = 0;
		int num3 = IdleLogic.actionIndex;
		float num4 = (float)-1;
		int num5 = 0;
		if (Extensions.get_length(this.actionRateFloat) != 0)
		{
			float arg_67_0 = Time.get_time();
			float[] expr_5B = this.lastTimeRun;
			if (arg_67_0 - expr_5B[RuntimeServices.NormalizeArrayIndex(expr_5B, IdleLogic.actionIndex)] >= this.run_length - 0.15f)
			{
				int[] array = new int[Extensions.get_length(this.IdleActions)];
				Array array2 = new Array(Extensions.get_length(this.IdleActions));
				checked
				{
					for (int i = 0; i < Extensions.get_length(this.IdleActions); i++)
					{
						int[] expr_AC = array;
						expr_AC[RuntimeServices.NormalizeArrayIndex(expr_AC, i)] = 0;
						array2.set_Item(i, i);
					}
					float arg_F8_0 = Time.get_time();
					float[] expr_EC = this.lastTimeRun;
					float num6 = unchecked(arg_F8_0 - expr_EC[RuntimeServices.NormalizeArrayIndex(expr_EC, IdleLogic.actionIndex)]);
					while (!flag && num2 < array2.get_length())
					{
						int num7;
						if (num2 < Extensions.get_length(this.IdleActions) - 1)
						{
							num7 = Random.Range(0, array2.get_length());
						}
						else
						{
							num7 = 0;
						}
						num5 = RuntimeServices.UnboxInt32(array2.get_Item(num7));
						int[] expr_13E = array;
						int[] expr_324;
						unchecked
						{
							if (expr_13E[RuntimeServices.NormalizeArrayIndex(expr_13E, num5)] == 0)
							{
								float arg_160_0 = Time.get_time();
								float[] expr_157 = this.lastTimeRun;
								float arg_175_0 = arg_160_0 - expr_157[RuntimeServices.NormalizeArrayIndex(expr_157, num5)];
								AnimInfo[] expr_167 = this.IdleActions;
								if (arg_175_0 > expr_167[RuntimeServices.NormalizeArrayIndex(expr_167, num5)].delta)
								{
									float num8 = 0f;
									bool flag2 = true;
									Animation arg_1A1_0 = this.get_animation();
									AnimInfo[] expr_193 = this.IdleActions;
									if (arg_1A1_0.get_Item(expr_193[RuntimeServices.NormalizeArrayIndex(expr_193, num5)].anim).get_wrapMode() == 2)
									{
										AnimInfo[] expr_1BE = this.IdleActions;
										float arg_1E0_0 = expr_1BE[RuntimeServices.NormalizeArrayIndex(expr_1BE, num5)].min_dur;
										AnimInfo[] expr_1D2 = this.IdleActions;
										this.num_iter = checked((int)Random.Range(arg_1E0_0, expr_1D2[RuntimeServices.NormalizeArrayIndex(expr_1D2, num5)].max_dur));
									}
									else
									{
										this.num_iter = 1;
									}
									float arg_228_0 = (float)this.num_iter;
									Animation arg_219_0 = this.get_animation();
									AnimInfo[] expr_20B = this.IdleActions;
									this.run_length = arg_228_0 * arg_219_0.get_Item(expr_20B[RuntimeServices.NormalizeArrayIndex(expr_20B, num5)].anim).get_clip().get_length();
									if (petState.useTimer)
									{
										flag2 = false;
										num8 = (float)petState.stateStartTime + petState.stateTimeOut - Time.get_time();
										while (!flag2 && num8 > (float)0 && this.run_length > (float)0)
										{
											if (this.run_length <= num8)
											{
												flag2 = true;
											}
											else
											{
												float arg_29A_0;
												AnimInfo[] expr_28C;
												checked
												{
													this.num_iter--;
													arg_29A_0 = (float)this.num_iter;
													expr_28C = this.IdleActions;
												}
												if (arg_29A_0 > expr_28C[RuntimeServices.NormalizeArrayIndex(expr_28C, num5)].min_dur)
												{
													float arg_2D2_0 = (float)this.num_iter;
													Animation arg_2C3_0 = this.get_animation();
													AnimInfo[] expr_2B5 = this.IdleActions;
													this.run_length = arg_2D2_0 * arg_2C3_0.get_Item(expr_2B5[RuntimeServices.NormalizeArrayIndex(expr_2B5, num5)].anim).get_clip().get_length();
												}
												else
												{
													this.run_length = (float)0;
												}
											}
										}
									}
									if (flag2 && this.num_iter >= 1)
									{
										flag = true;
										num3 = num5;
									}
								}
							}
							expr_324 = array;
						}
						if (expr_324[RuntimeServices.NormalizeArrayIndex(expr_324, num5)] == 0)
						{
							int[] expr_334 = array;
							expr_334[RuntimeServices.NormalizeArrayIndex(expr_334, num5)] = 1;
							array2.RemoveAt(num7);
							num2++;
						}
					}
					if (flag)
					{
						IdleLogic.actionIndex = num3;
						Animation arg_38D_0 = this.get_animation();
						AnimInfo[] expr_377 = this.IdleActions;
						arg_38D_0.CrossFade(expr_377[RuntimeServices.NormalizeArrayIndex(expr_377, IdleLogic.actionIndex)].anim, 0.15f);
						float[] expr_398 = this.lastTimeRun;
						expr_398[RuntimeServices.NormalizeArrayIndex(expr_398, IdleLogic.actionIndex)] = Time.get_time();
					}
					else
					{
						petState.stateTimeOut = (float)-1;
					}
				}
			}
		}
	}

	public void Main()
	{
	}
}
