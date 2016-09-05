using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class PetState : MonoBehaviour
{
	public int indexAction;

	public int stateStartTime;

	public float stateTimeOut;

	private int numStates;

	public StateDuration[] stateDur;

	public int calledByPlayer;

	public GameObject owner;

	public float ownerDist;

	public bool useTimer;

	public void Start()
	{
		this.indexAction = 0;
		this.useTimer = true;
	}

	public void InitState(object i_state)
	{
		this.numStates = Extensions.get_length(this.stateDur);
		this.indexAction = RuntimeServices.UnboxInt32(i_state);
		this.stateStartTime = checked((int)Time.get_time());
		StateDuration[] expr_30 = this.stateDur;
		float arg_5A_0 = expr_30[RuntimeServices.NormalizeArrayIndex(expr_30, this.indexAction)].min_dur;
		StateDuration[] expr_48 = this.stateDur;
		this.stateTimeOut = Random.Range(arg_5A_0, expr_48[RuntimeServices.NormalizeArrayIndex(expr_48, this.indexAction)].max_dur);
	}

	public void Update()
	{
	}

	public void AddPetStateTimeout(object additional_time)
	{
		this.stateTimeOut = RuntimeServices.UnboxSingle(RuntimeServices.InvokeBinaryOperator("op_Addition", this.stateTimeOut, additional_time));
	}

	public int GetPetState()
	{
		PetState petState = (PetState)this.GetComponent(typeof(PetState));
		IdleLogic idleLogic = (IdleLogic)this.GetComponent(typeof(IdleLogic));
		FollowLogic followLogic = (FollowLogic)this.GetComponent(typeof(FollowLogic));
		WanderLogic wanderLogic = (WanderLogic)this.GetComponent(typeof(WanderLogic));
		int result = this.indexAction;
		this.useTimer = true;
		if (this.calledByPlayer != 0)
		{
			float num = Vector3.Distance(this.get_transform().get_position(), this.owner.get_transform().get_position());
			if (num <= this.ownerDist && RuntimeServices.ToBool(RuntimeServices.InvokeBinaryOperator("op_LessThan", RuntimeServices.GetProperty(RuntimeServices.GetProperty(this.owner.GetComponent(typeof(CharacterController)), "velocity"), "magnitude"), 1)))
			{
				result = 0;
			}
			else if (num > this.ownerDist)
			{
				result = 2;
			}
			this.useTimer = false;
		}
		else if (this.calledByPlayer == 0 && this.indexAction == 2)
		{
			result = Random.Range(0, 2);
		}
		else if (Time.get_time() - (float)this.stateStartTime > this.stateTimeOut)
		{
			int num2 = this.indexAction;
			if (num2 == 0)
			{
				result = 1;
			}
			else if (num2 == 1)
			{
				result = 0;
			}
			else
			{
				result = this.indexAction;
			}
		}
		return result;
	}

	public void Main()
	{
	}
}
