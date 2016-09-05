using System;
using UnityEngine;

[AddComponentMenu("Image Effects/SSAO"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class SSAOEffect : MonoBehaviour
{
	public enum SSAOSamples
	{
		Low,
		Medium,
		High
	}

	public float m_Radius = 0.4f;

	public SSAOEffect.SSAOSamples m_SampleCount = SSAOEffect.SSAOSamples.Medium;

	public float m_OcclusionIntensity = 1.5f;

	public int m_Blur = 2;

	public int m_Downsampling = 2;

	public float m_OcclusionAttenuation = 1f;

	public float m_MinZ = 0.01f;

	public Shader m_SSAOShader;

	private Material m_SSAOMaterial;

	public Texture2D m_RandomTexture;

	private bool m_Supported;

	private bool m_IsOpenGL;

	private static Material CreateMaterial(Shader shader)
	{
		if (!shader)
		{
			return null;
		}
		Material material = new Material(shader);
		material.set_hideFlags(13);
		return material;
	}

	private static void DestroyMaterial(Material mat)
	{
		if (mat)
		{
			Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	private void OnDisable()
	{
		SSAOEffect.DestroyMaterial(this.m_SSAOMaterial);
	}

	private void Start()
	{
		if (!SystemInfo.get_supportsImageEffects() || !SystemInfo.SupportsRenderTextureFormat(1))
		{
			this.m_Supported = false;
			base.set_enabled(false);
			return;
		}
		this.CreateMaterials();
		if (!this.m_SSAOMaterial || this.m_SSAOMaterial.get_passCount() != 5)
		{
			this.m_Supported = false;
			base.set_enabled(false);
			return;
		}
		base.get_camera().set_depthTextureMode(2);
		this.m_Supported = true;
		this.m_IsOpenGL = SystemInfo.get_graphicsDeviceVersion().StartsWith("OpenGL");
	}

	private void CreateMaterials()
	{
		if (!this.m_SSAOMaterial && this.m_SSAOShader.get_isSupported())
		{
			this.m_SSAOMaterial = SSAOEffect.CreateMaterial(this.m_SSAOShader);
			this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.m_Supported || !this.m_SSAOShader.get_isSupported())
		{
			base.set_enabled(false);
			return;
		}
		this.CreateMaterials();
		this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
		this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
		this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
		this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
		this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
		this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
		RenderTexture renderTexture = RenderTexture.GetTemporary(source.get_width() / this.m_Downsampling, source.get_height() / this.m_Downsampling, 0);
		float fieldOfView = base.get_camera().get_fieldOfView();
		float farClipPlane = base.get_camera().get_farClipPlane();
		float num = Mathf.Tan(fieldOfView * 0.0174532924f * 0.5f) * farClipPlane;
		float num2 = num * base.get_camera().get_aspect();
		this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(num2, num, farClipPlane));
		int num3;
		int num4;
		if (this.m_RandomTexture)
		{
			num3 = this.m_RandomTexture.get_width();
			num4 = this.m_RandomTexture.get_height();
		}
		else
		{
			num3 = 1;
			num4 = 1;
		}
		this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)renderTexture.get_width() / (float)num3, (float)renderTexture.get_height() / (float)num4, 0f));
		this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
		bool flag = this.m_Blur > 0;
		Graphics.Blit((!flag) ? source : null, renderTexture, this.m_SSAOMaterial, (int)this.m_SampleCount);
		if (flag)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.get_width(), source.get_height(), 0);
			this.m_SSAOMaterial.SetVector("_TexelOffsetScale", (!this.m_IsOpenGL) ? new Vector4((float)this.m_Blur / (float)source.get_width(), 0f, 0f, 0f) : new Vector4((float)this.m_Blur, 0f, 1f / (float)this.m_Downsampling, 0f));
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(null, temporary, this.m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.get_width(), source.get_height(), 0);
			this.m_SSAOMaterial.SetVector("_TexelOffsetScale", (!this.m_IsOpenGL) ? new Vector4(0f, (float)this.m_Blur / (float)source.get_height(), 0f, 0f) : new Vector4(0f, (float)this.m_Blur, 1f, 0f));
			this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
			Graphics.Blit(source, temporary2, this.m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(temporary);
			renderTexture = temporary2;
		}
		this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
		Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
		RenderTexture.ReleaseTemporary(renderTexture);
	}
}
