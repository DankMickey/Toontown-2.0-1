using System;
using UnityEngine;

[AddComponentMenu("Detonator/Sparks"), RequireComponent(typeof(Detonator))]
public class DetonatorSparks : DetonatorComponent
{
	private float _baseSize = 1f;

	private float _baseDuration = 4f;

	private Vector3 _baseVelocity = new Vector3(155f, 155f, 155f);

	private Color _baseColor;

	private float _baseDamping;

	private Vector3 _baseForce;

	private float _scaledDuration;

	private GameObject _sparks;

	private DetonatorBurstEmitter _sparksEmitter;

	public Material sparksMaterial;

	public Vector3 velocity;

	public DetonatorSparks()
	{
		this._baseVelocity = this._baseVelocity;
		this._baseColor = Color.get_white();
		this._baseDamping = 0.185f;
		this._baseForce = Physics.get_gravity();
		base..ctor();
	}

	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildSparks();
	}

	public void FillMaterials(bool wipe)
	{
		if (!this.sparksMaterial || wipe)
		{
			this.sparksMaterial = base.MyDetonator().sparksMaterial;
		}
	}

	public void BuildSparks()
	{
		this._sparks = new GameObject("Sparks");
		this._sparksEmitter = (DetonatorBurstEmitter)this._sparks.AddComponent("DetonatorBurstEmitter");
		this._sparks.get_transform().set_parent(base.get_transform());
		this._sparks.get_transform().set_localPosition(this.localPosition);
		this._sparksEmitter.material = this.sparksMaterial;
		this._sparksEmitter.force = Physics.get_gravity();
		this._sparksEmitter.useExplicitColorAnimation = false;
	}

	public void UpdateSparks()
	{
		this._scaledDuration = this.duration * this.timeScale;
		this._sparksEmitter.color = this.color;
		this._sparksEmitter.duration = this._scaledDuration / 2f;
		this._sparksEmitter.durationVariation = this._scaledDuration;
		this._sparksEmitter.count = (float)((int)(this.detail * 50f));
		this._sparksEmitter.particleSize = 0.5f;
		this._sparksEmitter.sizeVariation = 0.25f;
		this._sparksEmitter.velocity = this.velocity;
		this._sparksEmitter.startRadius = 0f;
		this._sparksEmitter.size = this.size;
		this._sparksEmitter.explodeDelayMin = this.explodeDelayMin;
		this._sparksEmitter.explodeDelayMax = this.explodeDelayMax;
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
		this.force = this._baseForce;
	}

	public override void Explode()
	{
		if (this.on)
		{
			this.UpdateSparks();
			this._sparksEmitter.Explode();
		}
	}
}
