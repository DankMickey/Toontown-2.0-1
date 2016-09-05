using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class PWR_PinwheelControl : MonoBehaviour
{
	private float[] pin_timers;

	private float allPinsTimer;

	public float allPinsTimerMax;

	public float allPinsTimerMin;

	private float allPinsDelay;

	private bool first_time;

	public PinwheelInfo[] pinwheels;

	private float allPinsSpeed;

	public float basePinwheelSpeed;

	private float[] current_rotation;

	private bool Awake;

	private bool[] spinning_up;

	private float[] spin_start_time;

	private float[] pinwheelRunTimer;

	private float allPinsTime;

	public void Start()
	{
		this.Awake = true;
		this.first_time = true;
		this.pin_timers = new float[8];
		this.current_rotation = new float[8];
		this.spin_start_time = new float[8];
		this.spinning_up = new bool[8];
		this.pinwheelRunTimer = new float[8];
		this.allPinsTimer = Random.Range(this.allPinsTimerMin, this.allPinsTimerMax);
		this.allPinsTime = Time.get_time();
		this.allPinsSpeed = (float)1;
		int i = 0;
		checked
		{
			for (i = 0; i < 8; i++)
			{
				float[] expr_89 = this.pin_timers;
				expr_89[RuntimeServices.NormalizeArrayIndex(expr_89, i)] = (float)0;
				float[] expr_99 = this.current_rotation;
				expr_99[RuntimeServices.NormalizeArrayIndex(expr_99, i)] = (float)0;
				float[] expr_A9 = this.spin_start_time;
				expr_A9[RuntimeServices.NormalizeArrayIndex(expr_A9, i)] = Time.get_time();
				bool[] expr_BC = this.spinning_up;
				expr_BC[RuntimeServices.NormalizeArrayIndex(expr_BC, i)] = true;
				float[] expr_D4 = this.pinwheelRunTimer;
				expr_D4[RuntimeServices.NormalizeArrayIndex(expr_D4, i)] = (float)10;
			}
		}
	}

	public void Update()
	{
		if (this.first_time)
		{
			this.first_time = false;
		}
		checked
		{
			if (unchecked(Time.get_time() - this.allPinsTime) >= this.allPinsTimer)
			{
				if (!this.Awake)
				{
					this.basePinwheelSpeed = 0.2f;
				}
				this.allPinsTimer = Random.Range(this.allPinsTimerMin, this.allPinsTimerMax);
				for (int i = 0; i < Extensions.get_length(this.pinwheels); i++)
				{
					float[] expr_68 = this.pinwheelRunTimer;
					expr_68[RuntimeServices.NormalizeArrayIndex(expr_68, i)] = Random.Range(unchecked(this.allPinsTimer - (float)3), this.allPinsTimer);
				}
				this.allPinsTime = Time.get_time();
			}
			for (int i = 0; i < Extensions.get_length(this.pinwheels); i++)
			{
				this.PinwheelControl(i);
			}
		}
	}

	public void ToggleSleepWake()
	{
		if (this.Awake)
		{
			this.Awake = false;
			this.basePinwheelSpeed = (float)1;
		}
		else
		{
			this.Awake = true;
			this.basePinwheelSpeed = 0.3f;
		}
	}

	public void PinwheelControl(object which)
	{
		float arg_18_0 = Time.get_time();
		float[] expr_0B = this.spin_start_time;
		float arg_2C_0 = arg_18_0 - expr_0B[RuntimeServices.NormalizeArrayIndex(expr_0B, RuntimeServices.UnboxInt32(which))];
		float[] expr_1F = this.pinwheelRunTimer;
		if (arg_2C_0 > expr_1F[RuntimeServices.NormalizeArrayIndex(expr_1F, RuntimeServices.UnboxInt32(which))])
		{
			bool[] expr_39 = this.spinning_up;
			if (expr_39[RuntimeServices.NormalizeArrayIndex(expr_39, RuntimeServices.UnboxInt32(which))])
			{
				bool[] expr_5A = this.spinning_up;
				expr_5A[RuntimeServices.NormalizeArrayIndex(expr_5A, RuntimeServices.UnboxInt32(which))] = false;
			}
			else
			{
				bool[] expr_7C = this.spinning_up;
				if (!expr_7C[RuntimeServices.NormalizeArrayIndex(expr_7C, RuntimeServices.UnboxInt32(which))])
				{
					bool[] expr_9D = this.spinning_up;
					expr_9D[RuntimeServices.NormalizeArrayIndex(expr_9D, RuntimeServices.UnboxInt32(which))] = true;
				}
			}
			float[] expr_BA = this.spin_start_time;
			expr_BA[RuntimeServices.NormalizeArrayIndex(expr_BA, RuntimeServices.UnboxInt32(which))] = Time.get_time();
		}
		PinwheelInfo[] expr_D4 = this.pinwheels;
		Transform pinwheel = expr_D4[RuntimeServices.NormalizeArrayIndex(expr_D4, RuntimeServices.UnboxInt32(which))].pinwheel;
		if (pinwheel)
		{
			Vector3 vector = default(Vector3);
			float[] expr_100 = this.current_rotation;
			vector = new Vector3(expr_100[RuntimeServices.NormalizeArrayIndex(expr_100, RuntimeServices.UnboxInt32(which))] * Time.get_deltaTime(), (float)0, (float)0);
			pinwheel.Rotate(vector);
			bool[] expr_12A = this.spinning_up;
			if (expr_12A[RuntimeServices.NormalizeArrayIndex(expr_12A, RuntimeServices.UnboxInt32(which))])
			{
				float[] expr_14B = this.current_rotation;
				float arg_185_0 = expr_14B[RuntimeServices.NormalizeArrayIndex(expr_14B, RuntimeServices.UnboxInt32(which))];
				float arg_17D_0 = this.basePinwheelSpeed * this.allPinsSpeed;
				PinwheelInfo[] expr_16B = this.pinwheels;
				if (arg_185_0 < arg_17D_0 * expr_16B[RuntimeServices.NormalizeArrayIndex(expr_16B, RuntimeServices.UnboxInt32(which))].rotationSpeed * (float)360)
				{
					float[] expr_192 = this.current_rotation;
					int arg_1CA_1 = RuntimeServices.NormalizeArrayIndex(expr_192, RuntimeServices.UnboxInt32(which));
					float[] expr_1A4 = this.current_rotation;
					float arg_1C9_0 = expr_1A4[RuntimeServices.NormalizeArrayIndex(expr_1A4, RuntimeServices.UnboxInt32(which))];
					PinwheelInfo[] expr_1B7 = this.pinwheels;
					expr_192[arg_1CA_1] = arg_1C9_0 + expr_1B7[RuntimeServices.NormalizeArrayIndex(expr_1B7, RuntimeServices.UnboxInt32(which))].easeInTime;
				}
			}
			else
			{
				float[] expr_1D6 = this.current_rotation;
				if (expr_1D6[RuntimeServices.NormalizeArrayIndex(expr_1D6, RuntimeServices.UnboxInt32(which))] > (float)0)
				{
					float[] expr_1F2 = this.current_rotation;
					int arg_22A_1 = RuntimeServices.NormalizeArrayIndex(expr_1F2, RuntimeServices.UnboxInt32(which));
					float[] expr_204 = this.current_rotation;
					float arg_229_0 = expr_204[RuntimeServices.NormalizeArrayIndex(expr_204, RuntimeServices.UnboxInt32(which))];
					PinwheelInfo[] expr_217 = this.pinwheels;
					expr_1F2[arg_22A_1] = arg_229_0 - expr_217[RuntimeServices.NormalizeArrayIndex(expr_217, RuntimeServices.UnboxInt32(which))].easeOutTime;
				}
				else
				{
					float[] expr_236 = this.current_rotation;
					expr_236[RuntimeServices.NormalizeArrayIndex(expr_236, RuntimeServices.UnboxInt32(which))] = (float)0;
				}
			}
		}
	}

	public void Main()
	{
	}
}
