using System;
using UnityEngine;

[Serializable]
public class Detonator Spray Helper : MonoBehaviour
{
	public float startTimeMin;

	public float startTimeMax;

	public float stopTimeMin;

	public float stopTimeMax;

	public Material firstMaterial;

	public Material secondMaterial;

	private float startTime;

	private float stopTime;

	private float spawnTime;

	private bool isReallyOn;

	public Detonator Spray Helper()
	{
		this.startTimeMin = (float)0;
		this.startTimeMax = (float)0;
		this.stopTimeMin = (float)10;
		this.stopTimeMax = (float)10;
	}

	public void Start()
	{
		this.isReallyOn = this.get_particleEmitter().get_emit();
		this.get_particleEmitter().set_emit(false);
		this.spawnTime = Time.get_time();
		this.startTime = Random.get_value() * (this.startTimeMax - this.startTimeMin) + this.startTimeMin + Time.get_time();
		this.stopTime = Random.get_value() * (this.stopTimeMax - this.stopTimeMin) + this.stopTimeMin + Time.get_time();
		if (Random.get_value() > 0.5f)
		{
			this.get_renderer().set_material(this.firstMaterial);
		}
		else
		{
			this.get_renderer().set_material(this.secondMaterial);
		}
	}

	public void FixedUpdate()
	{
		if (Time.get_time() > this.startTime)
		{
			this.get_particleEmitter().set_emit(this.isReallyOn);
		}
		if (Time.get_time() > this.stopTime)
		{
			this.get_particleEmitter().set_emit(false);
		}
	}

	public void Main()
	{
	}
}
