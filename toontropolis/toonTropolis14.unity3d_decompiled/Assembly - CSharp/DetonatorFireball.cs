using System;
using UnityEngine;

[AddComponentMenu("Detonator/Fireball"), RequireComponent(typeof(Detonator))]
public class DetonatorFireball : DetonatorComponent
{
	private float _baseSize = 1f;

	private float _baseDuration = 3f;

	private Color _baseColor = new Color(1f, 0.423f, 0f, 0.5f);

	private float _baseDamping;

	private float _scaledDuration;

	private GameObject _fireballA;

	private DetonatorBurstEmitter _fireballAEmitter;

	public Material fireballAMaterial;

	private GameObject _fireballB;

	private DetonatorBurstEmitter _fireballBEmitter;

	public Material fireballBMaterial;

	private GameObject _fireShadow;

	private DetonatorBurstEmitter _fireShadowEmitter;

	public Material fireShadowMaterial;

	public bool drawFireballA;

	public bool drawFireballB;

	public bool drawFireShadow;

	private Color _detailAdjustedColor;

	public DetonatorFireball()
	{
		this._baseColor = this._baseColor;
		this._baseDamping = 0.1300004f;
		this.drawFireballA = true;
		this.drawFireballB = true;
		this.drawFireShadow = true;
		base..ctor();
	}

	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildFireballA();
		this.BuildFireballB();
		this.BuildFireShadow();
	}

	public void FillMaterials(bool wipe)
	{
		if (!this.fireballAMaterial || wipe)
		{
			this.fireballAMaterial = base.MyDetonator().fireballAMaterial;
		}
		if (!this.fireballBMaterial || wipe)
		{
			this.fireballBMaterial = base.MyDetonator().fireballBMaterial;
		}
		if (!this.fireShadowMaterial || wipe)
		{
			if ((double)Random.get_value() > 0.5)
			{
				this.fireShadowMaterial = base.MyDetonator().smokeAMaterial;
			}
			else
			{
				this.fireShadowMaterial = base.MyDetonator().smokeBMaterial;
			}
		}
	}

	public void BuildFireballA()
	{
		this._fireballA = new GameObject("FireballA");
		this._fireballAEmitter = (DetonatorBurstEmitter)this._fireballA.AddComponent("DetonatorBurstEmitter");
		this._fireballA.get_transform().set_parent(base.get_transform());
		this._fireballAEmitter.material = this.fireballAMaterial;
	}

	public void UpdateFireballA()
	{
		Transform arg_30_0 = this._fireballA.get_transform();
		Vector3 arg_2B_0 = this.localPosition;
		Vector3 vector = new Vector3(this.size, this.size, this.size);
		arg_30_0.set_localPosition(Vector3.Scale(arg_2B_0, vector));
		this._fireballAEmitter.color = this.color;
		this._fireballAEmitter.duration = this.duration * 0.5f;
		this._fireballAEmitter.durationVariation = this.duration * 0.5f;
		this._fireballAEmitter.count = 3f;
		this._fireballAEmitter.timeScale = this.timeScale;
		this._fireballAEmitter.detail = this.detail;
		this._fireballAEmitter.particleSize = 14f;
		this._fireballAEmitter.sizeVariation = 3f;
		this._fireballAEmitter.velocity = new Vector3(5f, 5f, 5f);
		this._fireballAEmitter.startRadius = 4f;
		this._fireballAEmitter.size = this.size;
		this._fireballAEmitter.useExplicitColorAnimation = true;
		Color color = new Color(1f, 1f, 1f, 0.5f);
		Color color2 = new Color(0.6f, 0.15f, 0.15f, 0.3f);
		Color color3 = new Color(0.1f, 0.2f, 0.45f, 0f);
		this._fireballAEmitter.colorAnimation[0] = Color.Lerp(this.color, color, 0.8f);
		this._fireballAEmitter.colorAnimation[1] = Color.Lerp(this.color, color, 0.5f);
		this._fireballAEmitter.colorAnimation[2] = this.color;
		this._fireballAEmitter.colorAnimation[3] = Color.Lerp(this.color, color2, 0.7f);
		this._fireballAEmitter.colorAnimation[4] = color3;
		this._fireballAEmitter.explodeDelayMin = this.explodeDelayMin;
		this._fireballAEmitter.explodeDelayMax = this.explodeDelayMax;
	}

	public void BuildFireballB()
	{
		this._fireballB = new GameObject("FireballB");
		this._fireballBEmitter = (DetonatorBurstEmitter)this._fireballB.AddComponent("DetonatorBurstEmitter");
		this._fireballB.get_transform().set_parent(base.get_transform());
		this._fireballBEmitter.material = this.fireballBMaterial;
	}

	public void UpdateFireballB()
	{
		Transform arg_30_0 = this._fireballB.get_transform();
		Vector3 arg_2B_0 = this.localPosition;
		Vector3 vector = new Vector3(this.size, this.size, this.size);
		arg_30_0.set_localPosition(Vector3.Scale(arg_2B_0, vector));
		this._fireballBEmitter.color = this.color;
		this._fireballBEmitter.duration = this.duration * 0.5f;
		this._fireballBEmitter.durationVariation = this.duration * 0.5f;
		this._fireballBEmitter.count = 3f;
		this._fireballBEmitter.timeScale = this.timeScale;
		this._fireballBEmitter.detail = this.detail;
		this._fireballBEmitter.particleSize = 10f;
		this._fireballBEmitter.sizeVariation = 6f;
		this._fireballBEmitter.velocity = new Vector3(19f, 19f, 19f);
		this._fireballBEmitter.startRadius = 4f;
		this._fireballBEmitter.size = this.size;
		this._fireballBEmitter.useExplicitColorAnimation = true;
		Color color = new Color(1f, 1f, 1f, 0.5f);
		Color color2 = new Color(0.6f, 0.15f, 0.15f, 0.3f);
		Color color3 = new Color(0.1f, 0.2f, 0.45f, 0f);
		this._fireballBEmitter.colorAnimation[0] = Color.Lerp(this.color, color, 0.8f);
		this._fireballBEmitter.colorAnimation[1] = Color.Lerp(this.color, color, 0.5f);
		this._fireballBEmitter.colorAnimation[2] = this.color;
		this._fireballBEmitter.colorAnimation[3] = Color.Lerp(this.color, color2, 0.7f);
		this._fireballBEmitter.colorAnimation[4] = color3;
		this._fireballBEmitter.explodeDelayMin = this.explodeDelayMin;
		this._fireballBEmitter.explodeDelayMax = this.explodeDelayMax;
	}

	public void BuildFireShadow()
	{
		this._fireShadow = new GameObject("FireShadow");
		this._fireShadowEmitter = (DetonatorBurstEmitter)this._fireShadow.AddComponent("DetonatorBurstEmitter");
		this._fireShadow.get_transform().set_parent(base.get_transform());
		this._fireShadowEmitter.material = this.fireShadowMaterial;
	}

	public void UpdateFireShadow()
	{
		Transform arg_30_0 = this._fireShadow.get_transform();
		Vector3 arg_2B_0 = this.localPosition;
		Vector3 vector = new Vector3(this.size, this.size, this.size);
		arg_30_0.set_localPosition(Vector3.Scale(arg_2B_0, vector));
		this._fireShadow.get_transform().LookAt(Camera.get_main().get_transform());
		this._fireShadow.get_transform().set_localPosition(-(Vector3.get_forward() * 1f));
		this._fireShadowEmitter.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
		this._fireShadowEmitter.duration = this.duration * 0.5f;
		this._fireShadowEmitter.durationVariation = this.duration * 0.5f;
		this._fireShadowEmitter.timeScale = this.timeScale;
		this._fireShadowEmitter.detail = 1f;
		this._fireShadowEmitter.particleSize = 13f;
		this._fireShadowEmitter.velocity = new Vector3(3f, 3f, 3f);
		this._fireShadowEmitter.sizeVariation = 1f;
		this._fireShadowEmitter.count = 2f;
		this._fireShadowEmitter.startRadius = 6f;
		this._fireShadowEmitter.size = this.size;
		this._fireShadowEmitter.explodeDelayMin = this.explodeDelayMin;
		this._fireShadowEmitter.explodeDelayMax = this.explodeDelayMax;
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
	}

	public override void Explode()
	{
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		if (this.on)
		{
			this.UpdateFireballA();
			this.UpdateFireballB();
			this.UpdateFireShadow();
			if (this.drawFireballA)
			{
				this._fireballAEmitter.Explode();
			}
			if (this.drawFireballB)
			{
				this._fireballBEmitter.Explode();
			}
			if (this.drawFireShadow)
			{
				this._fireShadowEmitter.Explode();
			}
		}
	}
}
