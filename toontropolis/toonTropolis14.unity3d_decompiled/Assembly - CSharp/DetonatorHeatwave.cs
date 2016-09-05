using System;
using UnityEngine;

[AddComponentMenu("Detonator/Heatwave (Pro Only)"), RequireComponent(typeof(Detonator))]
public class DetonatorHeatwave : DetonatorComponent
{
	private GameObject _heatwave;

	private float s;

	private float _startSize;

	private float _maxSize;

	private float _baseDuration = 0.25f;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	public float zOffset = 0.5f;

	public float distortion = 64f;

	private float _elapsedTime;

	private float _normalizedTime;

	public Material heatwaveMaterial;

	private Material _material;

	public override void Init()
	{
	}

	private void Update()
	{
		if (this._delayedExplosionStarted)
		{
			this._explodeDelay -= Time.get_deltaTime();
			if (this._explodeDelay <= 0f)
			{
				this.Explode();
			}
		}
		if (this._heatwave)
		{
			this._heatwave.get_transform().set_rotation(Quaternion.FromToRotation(Vector3.get_up(), Camera.get_main().get_transform().get_position() - this._heatwave.get_transform().get_position()));
			this._heatwave.get_transform().set_localPosition(this.localPosition + Vector3.get_forward() * this.zOffset);
			this._elapsedTime += Time.get_deltaTime();
			this._normalizedTime = this._elapsedTime / this.duration;
			this.s = Mathf.Lerp(this._startSize, this._maxSize, this._normalizedTime);
			this._heatwave.get_renderer().get_material().SetFloat("_BumpAmt", (1f - this._normalizedTime) * this.distortion);
			Transform arg_145_0 = this._heatwave.get_gameObject().get_transform();
			Vector3 localScale = new Vector3(this.s, this.s, this.s);
			arg_145_0.set_localScale(localScale);
			if (this._elapsedTime > this.duration)
			{
				Object.Destroy(this._heatwave.get_gameObject());
			}
		}
	}

	public override void Explode()
	{
		if (SystemInfo.get_supportsImageEffects())
		{
			if (this.detailThreshold > this.detail || !this.on)
			{
				return;
			}
			if (!this._delayedExplosionStarted)
			{
				this._explodeDelay = this.explodeDelayMin + Random.get_value() * (this.explodeDelayMax - this.explodeDelayMin);
			}
			if (this._explodeDelay <= 0f)
			{
				this._startSize = 0f;
				this._maxSize = this.size * 10f;
				this._material = new Material(Shader.Find("HeatDistort"));
				this._heatwave = GameObject.CreatePrimitive(4);
				if (!this.heatwaveMaterial)
				{
					this.heatwaveMaterial = base.MyDetonator().heatwaveMaterial;
				}
				this._material.CopyPropertiesFromMaterial(this.heatwaveMaterial);
				this._heatwave.get_renderer().set_material(this._material);
				this._heatwave.get_transform().set_parent(base.get_transform());
				this._delayedExplosionStarted = false;
				this._explodeDelay = 0f;
			}
			else
			{
				this._delayedExplosionStarted = true;
			}
		}
	}

	public void Reset()
	{
		this.duration = this._baseDuration;
	}
}
