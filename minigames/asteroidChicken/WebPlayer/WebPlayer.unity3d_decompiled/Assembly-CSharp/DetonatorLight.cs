using System;
using UnityEngine;

[AddComponentMenu("Detonator/Light"), RequireComponent(typeof(Detonator))]
public class DetonatorLight : DetonatorComponent
{
	private float _baseIntensity = 1f;

	private Color _baseColor = Color.get_white();

	private float _scaledDuration;

	private float _explodeTime = -1000f;

	private GameObject _light;

	private Light _lightComponent;

	public float intensity;

	private float _reduceAmount;

	public override void Init()
	{
		this._light = new GameObject("Light");
		this._light.get_transform().set_parent(base.get_transform());
		this._light.get_transform().set_localPosition(this.localPosition);
		this._lightComponent = (Light)this._light.AddComponent("Light");
		this._lightComponent.set_type(2);
	}

	private void Update()
	{
		if (this._explodeTime + this._scaledDuration > Time.get_time() && this._lightComponent.get_intensity() > 0f)
		{
			this._reduceAmount = this.intensity * (Time.get_deltaTime() / this._scaledDuration);
			Light expr_4B = this._lightComponent;
			expr_4B.set_intensity(expr_4B.get_intensity() - this._reduceAmount);
		}
		else if (this._lightComponent)
		{
			this._lightComponent.set_enabled(false);
		}
	}

	public override void Explode()
	{
		if (this.detailThreshold > this.detail)
		{
			return;
		}
		this._lightComponent.set_color(this.color);
		this._lightComponent.set_range(this.size * 50f);
		this._scaledDuration = this.duration * this.timeScale;
		this._lightComponent.set_enabled(true);
		this._lightComponent.set_intensity(this.intensity);
		this._explodeTime = Time.get_time();
	}

	public void Reset()
	{
		this.color = this._baseColor;
		this.intensity = this._baseIntensity;
	}
}
