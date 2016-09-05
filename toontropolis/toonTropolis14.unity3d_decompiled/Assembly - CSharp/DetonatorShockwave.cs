using System;
using UnityEngine;

[AddComponentMenu("Detonator/Shockwave"), RequireComponent(typeof(Detonator))]
public class DetonatorShockwave : DetonatorComponent
{
	private float _baseSize = 1f;

	private float _baseDuration = 0.25f;

	private Vector3 _baseVelocity = new Vector3(0f, 0f, 0f);

	private Color _baseColor;

	private GameObject _shockwave;

	private DetonatorBurstEmitter _shockwaveEmitter;

	public Material shockwaveMaterial;

	public Vector3 velocity;

	public ParticleRenderMode renderMode;

	public DetonatorShockwave()
	{
		this._baseVelocity = this._baseVelocity;
		this._baseColor = Color.get_white();
		base..ctor();
	}

	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildShockwave();
	}

	public void FillMaterials(bool wipe)
	{
		if (!this.shockwaveMaterial || wipe)
		{
			this.shockwaveMaterial = base.MyDetonator().shockwaveMaterial;
		}
	}

	public void BuildShockwave()
	{
		this._shockwave = new GameObject("Shockwave");
		this._shockwaveEmitter = (DetonatorBurstEmitter)this._shockwave.AddComponent("DetonatorBurstEmitter");
		this._shockwave.get_transform().set_parent(base.get_transform());
		this._shockwave.get_transform().set_localPosition(this.localPosition);
		this._shockwaveEmitter.material = this.shockwaveMaterial;
		this._shockwaveEmitter.exponentialGrowth = false;
	}

	public void UpdateShockwave()
	{
		Transform arg_30_0 = this._shockwave.get_transform();
		Vector3 arg_2B_0 = this.localPosition;
		Vector3 vector = new Vector3(this.size, this.size, this.size);
		arg_30_0.set_localPosition(Vector3.Scale(arg_2B_0, vector));
		this._shockwaveEmitter.color = this.color;
		this._shockwaveEmitter.duration = this.duration;
		this._shockwaveEmitter.durationVariation = this.duration * 0.1f;
		this._shockwaveEmitter.count = 1f;
		this._shockwaveEmitter.detail = 1f;
		this._shockwaveEmitter.particleSize = 25f;
		this._shockwaveEmitter.sizeVariation = 0f;
		this._shockwaveEmitter.velocity = new Vector3(0f, 0f, 0f);
		this._shockwaveEmitter.startRadius = 0f;
		this._shockwaveEmitter.sizeGrow = 202f;
		this._shockwaveEmitter.size = this.size;
		this._shockwaveEmitter.explodeDelayMin = this.explodeDelayMin;
		this._shockwaveEmitter.explodeDelayMax = this.explodeDelayMax;
		this._shockwaveEmitter.renderMode = this.renderMode;
	}

	public void Reset()
	{
		this.FillMaterials(true);
		this.on = true;
		this.size = this._baseSize;
		this.duration = this._baseDuration;
		this.explodeDelayMin = 0f;
		this.explodeDelayMax = 0f;
		this.color = this._baseColor;
		this.velocity = this._baseVelocity;
	}

	public override void Explode()
	{
		if (this.on)
		{
			this.UpdateShockwave();
			this._shockwaveEmitter.Explode();
		}
	}
}
