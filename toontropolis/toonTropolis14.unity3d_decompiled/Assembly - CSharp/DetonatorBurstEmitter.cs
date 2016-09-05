using System;
using UnityEngine;

public class DetonatorBurstEmitter : DetonatorComponent
{
	private ParticleEmitter _particleEmitter;

	private ParticleRenderer _particleRenderer;

	private ParticleAnimator _particleAnimator;

	private float _baseDamping = 0.1300004f;

	private float _baseSize = 1f;

	private Color _baseColor = Color.get_white();

	public Vector3 velocity = new Vector3(1f, 1f, 1f);

	public float damping;

	public float startRadius;

	public float maxScreenSize;

	public bool explodeOnAwake;

	public bool oneShot;

	public float sizeVariation;

	public float particleSize;

	public float count;

	public float sizeGrow;

	public bool exponentialGrowth;

	public float durationVariation;

	public ParticleRenderMode renderMode;

	public bool useExplicitColorAnimation;

	public Color[] colorAnimation;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	public Material material;

	private float _emitTime;

	private float speed;

	private float initFraction;

	private static float epsilon = 0.01f;

	private float _tmpParticleSize;

	private Vector3 _tmpPos;

	private Vector3 _tmpDir;

	private Vector3 _thisPos;

	private float _tmpDuration;

	private float _tmpCount;

	private float _scaledDuration;

	private float _scaledDurationVariation;

	private float _scaledStartRadius;

	private float _scaledColor;

	public DetonatorBurstEmitter()
	{
		this.velocity = this.velocity;
		this.damping = 1f;
		this.startRadius = 1f;
		this.maxScreenSize = 2f;
		this.oneShot = true;
		this.particleSize = 1f;
		this.count = 1f;
		this.sizeGrow = 20f;
		this.exponentialGrowth = true;
		this.colorAnimation = new Color[5];
		this.speed = 3f;
		this.initFraction = 0.1f;
		base..ctor();
	}

	public override void Init()
	{
		MonoBehaviour.print("UNUSED");
	}

	public void Awake()
	{
		this._particleEmitter = (base.get_gameObject().AddComponent("EllipsoidParticleEmitter") as ParticleEmitter);
		this._particleRenderer = (base.get_gameObject().AddComponent("ParticleRenderer") as ParticleRenderer);
		this._particleAnimator = (base.get_gameObject().AddComponent("ParticleAnimator") as ParticleAnimator);
		this._particleEmitter.set_hideFlags(13);
		this._particleRenderer.set_hideFlags(13);
		this._particleAnimator.set_hideFlags(13);
		this._particleAnimator.set_damping(this._baseDamping);
		this._particleEmitter.set_emit(false);
		this._particleRenderer.set_maxParticleSize(this.maxScreenSize);
		this._particleRenderer.set_material(this.material);
		this._particleRenderer.get_material().set_color(Color.get_white());
		this._particleAnimator.set_sizeGrow(this.sizeGrow);
		if (this.explodeOnAwake)
		{
			this.Explode();
		}
	}

	private void Update()
	{
		if (this.exponentialGrowth)
		{
			float num = Time.get_time() - this._emitTime;
			float num2 = this.SizeFunction(num - DetonatorBurstEmitter.epsilon);
			float num3 = this.SizeFunction(num);
			float num4 = (num3 / num2 - 1f) / DetonatorBurstEmitter.epsilon;
			this._particleAnimator.set_sizeGrow(num4);
		}
		else
		{
			this._particleAnimator.set_sizeGrow(this.sizeGrow);
		}
		if (this._delayedExplosionStarted)
		{
			this._explodeDelay -= Time.get_deltaTime();
			if (this._explodeDelay <= 0f)
			{
				this.Explode();
			}
		}
	}

	private float SizeFunction(float elapsedTime)
	{
		float num = 1f - 1f / (1f + elapsedTime * this.speed);
		return this.initFraction + (1f - this.initFraction) * num;
	}

	public void Reset()
	{
		this.size = this._baseSize;
		this.color = this._baseColor;
		this.damping = this._baseDamping;
	}

	public override void Explode()
	{
		if (this.on)
		{
			this._scaledDuration = this.timeScale * this.duration;
			this._scaledDurationVariation = this.timeScale * this.durationVariation;
			this._scaledStartRadius = this.size * this.startRadius;
			this._particleRenderer.set_particleRenderMode(this.renderMode);
			if (!this._delayedExplosionStarted)
			{
				this._explodeDelay = this.explodeDelayMin + Random.get_value() * (this.explodeDelayMax - this.explodeDelayMin);
			}
			if (this._explodeDelay <= 0f)
			{
				Color[] array = this._particleAnimator.get_colorAnimation();
				if (this.useExplicitColorAnimation)
				{
					array[0] = this.colorAnimation[0];
					array[1] = this.colorAnimation[1];
					array[2] = this.colorAnimation[2];
					array[3] = this.colorAnimation[3];
					array[4] = this.colorAnimation[4];
				}
				else
				{
					array[0] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 0.7f);
					array[1] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 1f);
					array[2] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 0.5f);
					array[3] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 0.3f);
					array[4] = new Color(this.color.r, this.color.g, this.color.b, this.color.a * 0f);
				}
				this._particleAnimator.set_colorAnimation(array);
				this._particleRenderer.set_material(this.material);
				this._particleAnimator.set_force(this.force);
				this._tmpCount = this.count * this.detail;
				if (this._tmpCount < 1f)
				{
					this._tmpCount = 1f;
				}
				this._thisPos = base.get_gameObject().get_transform().get_position();
				int num = 1;
				while ((float)num <= this._tmpCount)
				{
					Vector3 arg_30C_0 = Random.get_insideUnitSphere();
					Vector3 vector = new Vector3(this._scaledStartRadius, this._scaledStartRadius, this._scaledStartRadius);
					this._tmpPos = Vector3.Scale(arg_30C_0, vector);
					this._tmpPos = this._thisPos + this._tmpPos;
					Vector3 arg_35C_0 = Random.get_insideUnitSphere();
					Vector3 vector2 = new Vector3(this.velocity.x, this.velocity.y, this.velocity.z);
					this._tmpDir = Vector3.Scale(arg_35C_0, vector2);
					Vector3 arg_388_0 = this._tmpDir;
					Vector3 vector3 = new Vector3(this.size, this.size, this.size);
					this._tmpDir = Vector3.Scale(arg_388_0, vector3);
					this._tmpParticleSize = this.size * (this.particleSize + Random.get_value() * this.sizeVariation);
					this._tmpDuration = this._scaledDuration + Random.get_value() * this._scaledDurationVariation;
					this._particleEmitter.Emit(this._tmpPos, this._tmpDir, this._tmpParticleSize, this._tmpDuration, this.color);
					num++;
				}
				this._emitTime = Time.get_time();
				this._delayedExplosionStarted = false;
				this._explodeDelay = 0f;
			}
			else
			{
				this._delayedExplosionStarted = true;
			}
		}
	}
}
