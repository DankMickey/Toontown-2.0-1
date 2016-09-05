using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ActivateWall : MonoBehaviour
{
	public GameObject doodleWall;

	private float startTime;

	private bool canAnimateMiddle;

	private bool canAnimateInner;

	public float middleRingTimer;

	public float innerRingTimer;

	public ActivateWall()
	{
		this.middleRingTimer = (float)5;
		this.innerRingTimer = (float)10;
	}

	public override void Start()
	{
		this.canAnimateMiddle = true;
		this.canAnimateInner = false;
		this.startTime = Time.get_time();
	}

	public override void Update()
	{
		if (Time.get_time() - this.startTime > this.middleRingTimer && this.canAnimateMiddle)
		{
			this.canAnimateMiddle = false;
			UnityRuntimeServices.Invoke(this.doodleWall.GetComponent("PenWallControl"), "SetMiddleRing", new object[0], typeof(MonoBehaviour));
			this.canAnimateInner = true;
		}
		else if (Time.get_time() - this.startTime > this.innerRingTimer && this.canAnimateInner)
		{
			this.canAnimateInner = false;
			UnityRuntimeServices.Invoke(this.doodleWall.GetComponent("AudioSource"), "Play", new object[0], typeof(MonoBehaviour));
			UnityRuntimeServices.Invoke(this.doodleWall.GetComponent("PenWallControl"), "SetInnerRing", new object[0], typeof(MonoBehaviour));
		}
	}

	public override void Main()
	{
	}
}
