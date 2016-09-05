using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Glow"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class GlowEffect : MonoBehaviour
{
	public float glowIntensity = 1.5f;

	public int blurIterations = 3;

	public float blurSpread = 0.7f;

	public Color glowTint = new Color(1f, 1f, 1f, 0f);

	private static string compositeMatString = "Shader \"GlowCompose\" {\n\tProperties {\n\t\t_Color (\"Glow Amount\", Color) = (1,1,1,1)\n\t\t_MainTex (\"\", RECT) = \"white\" {}\n\t}\n\tSubShader {\n\t\tPass {\n\t\t\tZTest Always Cull Off ZWrite Off Fog { Mode Off }\n\t\t\tBlend One One\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine constant * texture DOUBLE}\n\t\t}\n\t}\n\tFallback off\n}";

	private static Material m_CompositeMaterial;

	private static string blurMatString = "Shader \"GlowConeTap\" {\n\tProperties {\n\t\t_Color (\"Blur Boost\", Color) = (0,0,0,0.25)\n\t\t_MainTex (\"\", RECT) = \"white\" {}\n\t}\n\tSubShader {\n\t\tPass {\n\t\t\tZTest Always Cull Off ZWrite Off Fog { Mode Off }\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine texture * constant alpha}\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}\n\t\t}\n\t}\n\tFallback off\n}";

	private static Material m_BlurMaterial;

	public Shader downsampleShader;

	private Material m_DownsampleMaterial;

	protected static Material compositeMaterial
	{
		get
		{
			if (GlowEffect.m_CompositeMaterial == null)
			{
				GlowEffect.m_CompositeMaterial = new Material(GlowEffect.compositeMatString);
				GlowEffect.m_CompositeMaterial.set_hideFlags(13);
				GlowEffect.m_CompositeMaterial.get_shader().set_hideFlags(13);
			}
			return GlowEffect.m_CompositeMaterial;
		}
	}

	protected static Material blurMaterial
	{
		get
		{
			if (GlowEffect.m_BlurMaterial == null)
			{
				GlowEffect.m_BlurMaterial = new Material(GlowEffect.blurMatString);
				GlowEffect.m_BlurMaterial.set_hideFlags(13);
				GlowEffect.m_BlurMaterial.get_shader().set_hideFlags(13);
			}
			return GlowEffect.m_BlurMaterial;
		}
	}

	protected Material downsampleMaterial
	{
		get
		{
			if (this.m_DownsampleMaterial == null)
			{
				this.m_DownsampleMaterial = new Material(this.downsampleShader);
				this.m_DownsampleMaterial.set_hideFlags(13);
			}
			return this.m_DownsampleMaterial;
		}
	}

	protected void OnDisable()
	{
		if (GlowEffect.m_CompositeMaterial)
		{
			Object.DestroyImmediate(GlowEffect.m_CompositeMaterial.get_shader());
			Object.DestroyImmediate(GlowEffect.m_CompositeMaterial);
		}
		if (GlowEffect.m_BlurMaterial)
		{
			Object.DestroyImmediate(GlowEffect.m_BlurMaterial.get_shader());
			Object.DestroyImmediate(GlowEffect.m_BlurMaterial);
		}
		if (this.m_DownsampleMaterial)
		{
			Object.DestroyImmediate(this.m_DownsampleMaterial);
		}
	}

	protected void Start()
	{
		if (!SystemInfo.get_supportsImageEffects())
		{
			base.set_enabled(false);
			return;
		}
		if (this.downsampleShader == null)
		{
			Debug.Log("No downsample shader assigned! Disabling glow.");
			base.set_enabled(false);
		}
		else
		{
			if (!GlowEffect.blurMaterial.get_shader().get_isSupported())
			{
				base.set_enabled(false);
			}
			if (!GlowEffect.compositeMaterial.get_shader().get_isSupported())
			{
				base.set_enabled(false);
			}
			if (!this.downsampleMaterial.get_shader().get_isSupported())
			{
				base.set_enabled(false);
			}
		}
	}

	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, GlowEffect.blurMaterial, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		this.downsampleMaterial.set_color(new Color(this.glowTint.r, this.glowTint.g, this.glowTint.b, this.glowTint.a / 4f));
		Graphics.Blit(source, dest, this.downsampleMaterial);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.glowIntensity = Mathf.Clamp(this.glowIntensity, 0f, 10f);
		this.blurIterations = Mathf.Clamp(this.blurIterations, 0, 30);
		this.blurSpread = Mathf.Clamp(this.blurSpread, 0.5f, 1f);
		RenderTexture temporary = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
		this.DownSample4x(source, temporary);
		float num = Mathf.Clamp01((this.glowIntensity - 1f) / 4f);
		GlowEffect.blurMaterial.set_color(new Color(1f, 1f, 1f, 0.25f + num));
		bool flag = true;
		for (int i = 0; i < this.blurIterations; i++)
		{
			if (flag)
			{
				this.FourTapCone(temporary, temporary2, i);
			}
			else
			{
				this.FourTapCone(temporary2, temporary, i);
			}
			flag = !flag;
		}
		ImageEffects.Blit(source, destination);
		if (flag)
		{
			this.BlitGlow(temporary, destination);
		}
		else
		{
			this.BlitGlow(temporary2, destination);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}

	public void BlitGlow(RenderTexture source, RenderTexture dest)
	{
		GlowEffect.compositeMaterial.set_color(new Color(1f, 1f, 1f, Mathf.Clamp01(this.glowIntensity)));
		Graphics.Blit(source, dest, GlowEffect.compositeMaterial);
	}
}
