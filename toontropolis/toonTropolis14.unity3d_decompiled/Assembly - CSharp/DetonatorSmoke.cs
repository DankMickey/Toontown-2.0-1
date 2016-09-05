using System;
using UnityEngine;

[AddComponentMenu("Detonator/Smoke"), RequireComponent(typeof(Detonator))]
public class DetonatorSmoke : DetonatorComponent
{
	private const float _baseSize = 1f;

	private const float _baseDuration = 8f;

	private const float _baseDamping = 0.1300004f;

	private Color _baseColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	private float _scaledDuration;

	private GameObject _smokeA;

	private DetonatorBurstEmitter _smokeAEmitter;

	public Material smokeAMaterial;

	private GameObject _smokeB;

	private DetonatorBurstEmitter _smokeBEmitter;

	public Material smokeBMaterial;

	public bool drawSmokeA;

	public bool drawSmokeB;

	public DetonatorSmoke()
	{
		this._baseColor = this._baseColor;
		this.drawSmokeA = true;
		this.drawSmokeB = true;
		base..ctor();
	}

	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildSmokeA();
		this.BuildSmokeB();
	}

	public void FillMaterials(bool wipe)
	{
		if (!this.smokeAMaterial || wipe)
		{
			this.smokeAMaterial = base.MyDetonator().smokeAMaterial;
		}
		if (!this.smokeBMaterial || wipe)
		{
			this.smokeBMaterial = base.MyDetonator().smokeBMaterial;
		}
	}

	public void BuildSmokeA()
	{
		this._smokeA = new GameObject("SmokeA");
		this._smokeAEmitter = (DetonatorBurstEmitter)this._smokeA.AddComponent("DetonatorBurstEmitter");
		this._smokeA.get_transform().set_parent(base.get_transform());
		this._smokeA.get_transform().set_localPosition(this.localPosition);
		this._smokeAEmitter.material = this.smokeAMaterial;
		this._smokeAEmitter.exponentialGrowth = false;
		this._smokeAEmitter.sizeGrow = 0.095f;
	}

	public void UpdateSmokeA()
	{
		Transform arg_31_0 = this._smokeA.get_transform();
		Vector3 arg_2C_0 = this.localPosition;
		Vector3 vector = new Vector3(this.size, this.size, this.size);
		arg_31_0.set_localPosition(Vector3.Scale(arg_2C_0, vector));
		this._smokeA.get_transform().LookAt(Camera.get_main().get_transform());
		this._smokeA.get_transform().set_localPosition(-(Vector3.get_forward() * -1.5f));
		this._smokeAEmitter.color = this.color;
		this._smokeAEmitter.duration = this.duration * 0.5f;
		this._smokeAEmitter.durationVariation = 0f;
		this._smokeAEmitter.timeScale = this.timeScale;
		this._smokeAEmitter.count = 2f;
		this._smokeAEmitter.particleSize = 25f;
		this._smokeAEmitter.sizeVariation = 3f;
		this._smokeAEmitter.velocity = new Vector3(3f, 4f, 4f);
		this._smokeAEmitter.startRadius = 10f;
		this._smokeAEmitter.size = this.size;
		this._smokeAEmitter.useExplicitColorAnimation = true;
		this._smokeAEmitter.explodeDelayMin = this.explodeDelayMin;
		this._smokeAEmitter.explodeDelayMax = this.explodeDelayMax;
		Color color = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color2 = new Color(0.2f, 0.2f, 0.2f, 0.7f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0f);
		this._smokeAEmitter.colorAnimation[0] = color;
		this._smokeAEmitter.colorAnimation[1] = color2;
		this._smokeAEmitter.colorAnimation[2] = color2;
		this._smokeAEmitter.colorAnimation[3] = color3;
		this._smokeAEmitter.colorAnimation[4] = color4;
	}

	public void BuildSmokeB()
	{
		this._smokeB = new GameObject("SmokeB");
		this._smokeBEmitter = (DetonatorBurstEmitter)this._smokeB.AddComponent("DetonatorBurstEmitter");
		this._smokeB.get_transform().set_parent(base.get_transform());
		this._smokeB.get_transform().set_localPosition(this.localPosition);
		this._smokeBEmitter.material = this.smokeBMaterial;
		this._smokeBEmitter.exponentialGrowth = false;
		this._smokeBEmitter.sizeGrow = 0.095f;
	}

	public void UpdateSmokeB()
	{
		Transform arg_31_0 = this._smokeB.get_transform();
		Vector3 arg_2C_0 = this.localPosition;
		Vector3 vector = new Vector3(this.size, this.size, this.size);
		arg_31_0.set_localPosition(Vector3.Scale(arg_2C_0, vector));
		this._smokeB.get_transform().LookAt(Camera.get_main().get_transform());
		this._smokeB.get_transform().set_localPosition(-(Vector3.get_forward() * -1f));
		this._smokeBEmitter.color = this.color;
		this._smokeBEmitter.duration = this.duration * 0.5f;
		this._smokeBEmitter.durationVariation = 0f;
		this._smokeBEmitter.count = 2f;
		this._smokeBEmitter.particleSize = 25f;
		this._smokeBEmitter.sizeVariation = 3f;
		this._smokeBEmitter.velocity = new Vector3(7f, 7f, 7f);
		this._smokeBEmitter.startRadius = 10f;
		this._smokeBEmitter.size = this.size;
		this._smokeBEmitter.useExplicitColorAnimation = true;
		this._smokeBEmitter.explodeDelayMin = this.explodeDelayMin;
		this._smokeBEmitter.explodeDelayMax = this.explodeDelayMax;
		Color color = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color2 = new Color(0.2f, 0.2f, 0.2f, 0.7f);
		Color color3 = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		Color color4 = new Color(0.2f, 0.2f, 0.2f, 0f);
		this._smokeBEmitter.colorAnimation[0] = color;
		this._smokeBEmitter.colorAnimation[1] = color2;
		this._smokeBEmitter.colorAnimation[2] = color2;
		this._smokeBEmitter.colorAnimation[3] = color3;
		this._smokeBEmitter.colorAnimation[4] = color4;
	}

	public void Reset()
	{
		this.FillMaterials(true);
		this.on = true;
		this.size = 1f;
		this.duration = 8f;
		this.explodeDelayMin = 0f;
		this.explodeDelayMax = 0f;
		this.color = this._baseColor;
	}

	public override void Explode()
	{
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		if (this.on)
		{
			this.UpdateSmokeA();
			this.UpdateSmokeB();
			if (this.drawSmokeA)
			{
				this._smokeAEmitter.Explode();
			}
			if (this.drawSmokeB)
			{
				this._smokeBEmitter.Explode();
			}
		}
	}
}
