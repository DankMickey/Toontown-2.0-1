using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Noise"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class NoiseEffect : MonoBehaviour
{
	public bool monochrome = true;

	private bool rgbFallback;

	public float grainIntensityMin = 0.1f;

	public float grainIntensityMax = 0.2f;

	public float grainSize = 2f;

	public float scratchIntensityMin = 0.05f;

	public float scratchIntensityMax = 0.25f;

	public float scratchFPS = 10f;

	public float scratchJitter = 0.01f;

	public Texture grainTexture;

	public Texture scratchTexture;

	public Shader shaderRGB;

	public Shader shaderYUV;

	private Material m_MaterialRGB;

	private Material m_MaterialYUV;

	private float scratchTimeLeft;

	private float scratchX;

	private float scratchY;

	protected Material material
	{
		get
		{
			if (this.m_MaterialRGB == null)
			{
				this.m_MaterialRGB = new Material(this.shaderRGB);
				this.m_MaterialRGB.set_hideFlags(13);
			}
			if (this.m_MaterialYUV == null && !this.rgbFallback)
			{
				this.m_MaterialYUV = new Material(this.shaderYUV);
				this.m_MaterialYUV.set_hideFlags(13);
			}
			return (this.rgbFallback || this.monochrome) ? this.m_MaterialRGB : this.m_MaterialYUV;
		}
	}

	protected void Start()
	{
		if (!SystemInfo.get_supportsImageEffects())
		{
			base.set_enabled(false);
			return;
		}
		if (this.shaderRGB == null || this.shaderYUV == null)
		{
			Debug.Log("Noise shaders are not set up! Disabling noise effect.");
			base.set_enabled(false);
		}
		else if (!this.shaderRGB.get_isSupported())
		{
			base.set_enabled(false);
		}
		else if (!this.shaderYUV.get_isSupported())
		{
			this.rgbFallback = true;
		}
	}

	protected void OnDisable()
	{
		if (this.m_MaterialRGB)
		{
			Object.DestroyImmediate(this.m_MaterialRGB);
		}
		if (this.m_MaterialYUV)
		{
			Object.DestroyImmediate(this.m_MaterialYUV);
		}
	}

	private void SanitizeParameters()
	{
		this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
		this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
		this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
		this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
		this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
		this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
		this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SanitizeParameters();
		if (this.scratchTimeLeft <= 0f)
		{
			this.scratchTimeLeft = Random.get_value() * 2f / this.scratchFPS;
			this.scratchX = Random.get_value();
			this.scratchY = Random.get_value();
		}
		this.scratchTimeLeft -= Time.get_deltaTime();
		Material material = this.material;
		material.SetTexture("_GrainTex", this.grainTexture);
		material.SetTexture("_ScratchTex", this.scratchTexture);
		float num = 1f / this.grainSize;
		material.SetVector("_GrainOffsetScale", new Vector4(Random.get_value(), Random.get_value(), (float)Screen.get_width() / (float)this.grainTexture.get_width() * num, (float)Screen.get_height() / (float)this.grainTexture.get_height() * num));
		material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + Random.get_value() * this.scratchJitter, this.scratchY + Random.get_value() * this.scratchJitter, (float)Screen.get_width() / (float)this.scratchTexture.get_width(), (float)Screen.get_height() / (float)this.scratchTexture.get_height()));
		material.SetVector("_Intensity", new Vector4(Random.Range(this.grainIntensityMin, this.grainIntensityMax), Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
		Graphics.Blit(source, destination, material);
	}
}
