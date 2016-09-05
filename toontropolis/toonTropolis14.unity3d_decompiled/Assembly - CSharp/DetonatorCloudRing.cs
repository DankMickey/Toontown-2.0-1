using System;
using UnityEngine;

[RequireComponent(typeof(Detonator))]
public class DetonatorCloudRing : DetonatorComponent
{
	private float _baseSize = 1f;

	private float _baseDuration = 5f;

	private Vector3 _baseVelocity = new Vector3(155f, 5f, 155f);

	private Color _baseColor;

	private Vector3 _baseForce;

	private GameObject _cloudRing;

	private DetonatorBurstEmitter _cloudRingEmitter;

	public Material cloudRingMaterial;

	public Vector3 velocity;

	public DetonatorCloudRing()
	{
		this._baseVelocity = this._baseVelocity;
		this._baseColor = Color.get_white();
		this._baseForce = new Vector3(0.162f, 2.56f, 0f);
		this._baseForce = this._baseForce;
		base..ctor();
	}

	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildCloudRing();
	}

	public void FillMaterials(bool wipe)
	{
		if (!this.cloudRingMaterial || wipe)
		{
			this.cloudRingMaterial = base.MyDetonator().smokeBMaterial;
		}
	}

	public void BuildCloudRing()
	{
		this._cloudRing = new GameObject("CloudRing");
		this._cloudRingEmitter = (DetonatorBurstEmitter)this._cloudRing.AddComponent("DetonatorBurstEmitter");
		this._cloudRing.get_transform().set_parent(base.get_transform());
		this._cloudRing.get_transform().set_localPosition(this.localPosition);
		this._cloudRingEmitter.material = this.cloudRingMaterial;
		this._cloudRingEmitter.useExplicitColorAnimation = true;
	}

	public void UpdateCloudRing()
	{
		Transform arg_31_0 = this._cloudRing.get_transform();
		Vector3 arg_2C_0 = this.localPosition;
		Vector3 vector = new Vector3(this.size, this.size, this.size);
		arg_31_0.set_localPosition(Vector3.Scale(arg_2C_0, vector));
		this._cloudRingEmitter.color = this.color;
		this._cloudRingEmitter.duration = this.duration;
		this._cloudRingEmitter.durationVariation = this.duration / 4f;
		this._cloudRingEmitter.count = (float)((int)(this.detail * 50f));
		this._cloudRingEmitter.particleSize = 10f;
		this._cloudRingEmitter.sizeVariation = 2f;
		this._cloudRingEmitter.velocity = this.velocity;
		this._cloudRingEmitter.startRadius = 3f;
		this._cloudRingEmitter.size = this.size;
		this._cloudRingEmitter.force = this.force;
		this._cloudRingEmitter.explodeDelayMin = this.explodeDelayMin;
		this._cloudRingEmitter.explodeDelayMax = this.explodeDelayMax;
		Color arg_135_0 = this.color;
		Color color = new Color(0.2f, 0.2f, 0.2f, 0.6f);
		Color color2 = Color.Lerp(arg_135_0, color, 0.5f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.5f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0.3f);
		Color color5 = new Color(0.2f, 0.2f, 0.2f, 0f);
		this._cloudRingEmitter.colorAnimation[0] = color2;
		this._cloudRingEmitter.colorAnimation[1] = color3;
		this._cloudRingEmitter.colorAnimation[2] = color3;
		this._cloudRingEmitter.colorAnimation[3] = color4;
		this._cloudRingEmitter.colorAnimation[4] = color5;
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
			this.UpdateCloudRing();
			this._cloudRingEmitter.Explode();
		}
	}
}
