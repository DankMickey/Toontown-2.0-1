using System;
using UnityEngine;

[AddComponentMenu("Detonator/Object Spray"), RequireComponent(typeof(Detonator))]
public class DetonatorSpray : DetonatorComponent
{
	public GameObject sprayObject;

	public int count = 10;

	public float startingRadius;

	public float velocity = 15f;

	public float minScale = 1f;

	public float maxScale = 1f;

	private bool _delayedExplosionStarted;

	private float _explodeDelay;

	private Vector3 _explosionPosition;

	private float _tmpScale;

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
		if (!this._delayedExplosionStarted)
		{
			this._explodeDelay = this.explodeDelayMin + Random.get_value() * (this.explodeDelayMax - this.explodeDelayMin);
		}
		if (this._explodeDelay <= 0f)
		{
			int num = (int)(this.detail * (float)this.count);
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = Random.get_onUnitSphere() * (this.startingRadius * this.size);
				Vector3 vector2 = new Vector3(this.velocity * this.size, this.velocity * this.size, this.velocity * this.size);
				GameObject gameObject = Object.Instantiate(this.sprayObject, base.get_transform().get_position() + vector, base.get_transform().get_rotation()) as GameObject;
				gameObject.get_transform().set_parent(base.get_transform());
				this._tmpScale = this.minScale + Random.get_value() * (this.maxScale - this.minScale);
				this._tmpScale *= this.size;
				gameObject.get_transform().set_localScale(Random.get_value() * new Vector3(this._tmpScale, this._tmpScale, this._tmpScale));
				gameObject.get_rigidbody().set_velocity(Vector3.Scale(vector.get_normalized(), vector2));
				Object.Destroy(gameObject, this.duration * this.timeScale);
				this._delayedExplosionStarted = false;
				this._explodeDelay = 0f;
			}
		}
		else
		{
			this._delayedExplosionStarted = true;
		}
	}

	public void Reset()
	{
	}
}
