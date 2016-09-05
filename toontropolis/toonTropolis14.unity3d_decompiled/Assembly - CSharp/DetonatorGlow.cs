using System;
using UnityEngine;

[AddComponentMenu("Detonator/Glow"), RequireComponent(typeof(Detonator))]
public class DetonatorGlow : DetonatorComponent
{
	private float _baseSize = 1f;

	private float _baseDuration = 3f;

	private Vector3 _baseVelocity = new Vector3(0f, 0f, 0f);

	private Color _baseColor;

	private float _scaledDuration;

	private GameObject _glow;

	private DetonatorBurstEmitter _glowEmitter;

	public Material glowMaterial;

	public Vector3 velocity;

	public DetonatorGlow()
	{
		this._baseVelocity = this._baseVelocity;
		this._baseColor = Color.get_black();
		base..ctor();
	}

	public override void Init()
	{
		this.FillMaterials(false);
		this.BuildGlow();
	}

	public void FillMaterials(bool wipe)
	{
		if (!this.glowMaterial || wipe)
		{
			this.glowMaterial = base.MyDetonator().glowMaterial;
		}
	}

	public void BuildGlow()
	{
		this._glow = new GameObject("Glow");
		this._glowEmitter = (DetonatorBurstEmitter)this._glow.AddComponent("DetonatorBurstEmitter");
		this._glow.get_transform().set_parent(base.get_transform());
		this._glow.get_transform().set_localPosition(this.localPosition);
		this._glowEmitter.material = this.glowMaterial;
		this._glowEmitter.exponentialGrowth = false;
		this._glowEmitter.useExplicitColorAnimation = true;
	}

	public void UpdateGlow()
	{
		Transform arg_31_0 = this._glow.get_transform();
		Vector3 arg_2C_0 = this.localPosition;
		Vector3 vector = new Vector3(this.size, this.size, this.size);
		arg_31_0.set_localPosition(Vector3.Scale(arg_2C_0, vector));
		this._glowEmitter.color = this.color;
		this._glowEmitter.duration = this.duration;
		this._glowEmitter.timeScale = this.timeScale;
		this._glowEmitter.count = 1f;
		this._glowEmitter.particleSize = 65f;
		this._glowEmitter.sizeVariation = 0f;
		this._glowEmitter.velocity = new Vector3(0f, 0f, 0f);
		this._glowEmitter.startRadius = 0f;
		this._glowEmitter.sizeGrow = 0f;
		this._glowEmitter.size = this.size;
		this._glowEmitter.explodeDelayMin = this.explodeDelayMin;
		this._glowEmitter.explodeDelayMax = this.explodeDelayMax;
		Color arg_133_0 = this.color;
		Color color = new Color(0.5f, 0.1f, 0.1f, 1f);
		Color color2 = Color.Lerp(arg_133_0, color, 0.5f);
		color2.a = 0.9f;
		Color arg_16D_0 = this.color;
		Color color3 = new Color(0.6f, 0.3f, 0.3f, 1f);
		Color color4 = Color.Lerp(arg_16D_0, color3, 0.5f);
		color4.a = 0.8f;
		Color arg_1A7_0 = this.color;
		Color color5 = new Color(0.7f, 0.3f, 0.3f, 1f);
		Color color6 = Color.Lerp(arg_1A7_0, color5, 0.5f);
		color6.a = 0.5f;
		Color arg_1E1_0 = this.color;
		Color color7 = new Color(0.4f, 0.3f, 0.4f, 1f);
		Color color8 = Color.Lerp(arg_1E1_0, color7, 0.5f);
		color8.a = 0.2f;
		Color color9 = new Color(0.1f, 0.1f, 0.4f, 0f);
		this._glowEmitter.colorAnimation[0] = color2;
		this._glowEmitter.colorAnimation[1] = color4;
		this._glowEmitter.colorAnimation[2] = color6;
		this._glowEmitter.colorAnimation[3] = color8;
		this._glowEmitter.colorAnimation[4] = color9;
	}

	private void Update()
	{
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
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		if (this.on)
		{
			this.UpdateGlow();
			this._glowEmitter.Explode();
		}
	}
}
