using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Blur"), ExecuteInEditMode]
public class BlurEffect : MonoBehaviour
{
	public int iterations = 3;

	public float blurSpread = 0.6f;

	private static string blurMatString = "Shader \"BlurConeTap\" {\n\tProperties { _MainTex (\"\", any) = \"\" {} }\n\tSubShader {\n\t\tPass {\n\t\t\tZTest Always Cull Off ZWrite Off Fog { Mode Off }\n\t\t\tSetTexture [_MainTex] {constantColor (0,0,0,0.25) combine texture * constant alpha}\n\t\t\tSetTexture [_MainTex] {constantColor (0,0,0,0.25) combine texture * constant + previous}\n\t\t\tSetTexture [_MainTex] {constantColor (0,0,0,0.25) combine texture * constant + previous}\n\t\t\tSetTexture [_MainTex] {constantColor (0,0,0,0.25) combine texture * constant + previous}\n\t\t}\n\t}\n\tFallback off\n}";

	private static Material m_Material;

	protected static Material material
	{
		get
		{
			if (BlurEffect.m_Material == null)
			{
				BlurEffect.m_Material = new Material(BlurEffect.blurMatString);
				BlurEffect.m_Material.set_hideFlags(13);
				BlurEffect.m_Material.get_shader().set_hideFlags(13);
			}
			return BlurEffect.m_Material;
		}
	}

	protected void OnDisable()
	{
		if (BlurEffect.m_Material)
		{
			Object.DestroyImmediate(BlurEffect.m_Material.get_shader());
			Object.DestroyImmediate(BlurEffect.m_Material);
		}
	}

	protected void Start()
	{
		if (!SystemInfo.get_supportsImageEffects())
		{
			base.set_enabled(false);
			return;
		}
		if (!BlurEffect.material.get_shader().get_isSupported())
		{
			base.set_enabled(false);
			return;
		}
	}

	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, BlurEffect.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, BlurEffect.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
		this.DownSample4x(source, temporary);
		bool flag = true;
		for (int i = 0; i < this.iterations; i++)
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
		if (flag)
		{
			ImageEffects.Blit(temporary, destination);
		}
		else
		{
			ImageEffects.Blit(temporary2, destination);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}
}
