using System;
using UnityEngine;

[AddComponentMenu("Detonator/Force"), RequireComponent(typeof(Detonator))]
public class DetonatorForce : DetonatorComponent
{
	private float _baseRadius = 50f;

	private float _basePower = 4000f;

	private float _scaledRange;

	private float _scaledIntensity;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	public float radius;

	public float power;

	public GameObject fireObject;

	public float fireObjectLife;

	private Collider[] _colliders;

	private GameObject _tempFireObject;

	private Vector3 _explosionPosition;

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
	}

	public override void Explode()
	{
		if (!this.on)
		{
			return;
		}
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		if (!this._delayedExplosionStarted)
		{
			this._explodeDelay = this.explodeDelayMin + Random.get_value() * (this.explodeDelayMax - this.explodeDelayMin);
		}
		if (this._explodeDelay <= 0f)
		{
			this._explosionPosition = base.get_transform().get_position();
			this._colliders = Physics.OverlapSphere(this._explosionPosition, this.radius);
			Collider[] colliders = this._colliders;
			int num = colliders.Length;
			for (int i = 0; i < num; i++)
			{
				Collider collider = colliders[i];
				if (collider)
				{
					if (collider.get_rigidbody())
					{
						collider.get_rigidbody().AddExplosionForce(this.power * this.size, this._explosionPosition, this.radius * this.size, 3f * this.size);
						if (this.fireObject)
						{
							this._tempFireObject = (Object.Instantiate(this.fireObject, base.get_transform().get_position(), base.get_transform().get_rotation()) as GameObject);
							this._tempFireObject.get_transform().set_parent(collider.get_transform());
							Transform arg_163_0 = this._tempFireObject.get_transform();
							Vector3 localPosition = new Vector3(0f, 0f, 0f);
							arg_163_0.set_localPosition(localPosition);
							if (this._tempFireObject.get_particleEmitter())
							{
								this._tempFireObject.get_particleEmitter().set_emit(true);
								Object.Destroy(this._tempFireObject, this.fireObjectLife);
							}
						}
					}
				}
			}
			this._delayedExplosionStarted = false;
			this._explodeDelay = 0f;
		}
		else
		{
			this._delayedExplosionStarted = true;
		}
	}

	public void Reset()
	{
		this.radius = this._baseRadius;
		this.power = this._basePower;
	}
}
