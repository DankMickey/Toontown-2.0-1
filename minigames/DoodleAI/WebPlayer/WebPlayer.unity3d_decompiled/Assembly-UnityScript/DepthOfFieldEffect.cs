using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Depth Of Field"), ExecuteInEditMode, RequireComponent(typeof(Camera))]
[Serializable]
public class DepthOfFieldEffect : MonoBehaviour
{
	public float focalDistance;

	public float focalRange;

	public float iterations;

	public float iterations2;

	public float blurSpread;

	public Shader renderSceneShader;

	public Shader compositeShader;

	private static string blurMatString = "Shader \"BlurConeTap\" { SubShader { Pass { " + "ZTest Always Cull Off ZWrite Off Fog { Mode Off } " + "\tSetTexture [__RenderTex] {constantColor (0,0,0,0.25) combine texture * constant alpha} " + "\tSetTexture [__RenderTex] {constantColor (0,0,0,0.25) combine texture * constant + previous} " + "\tSetTexture [__RenderTex] {constantColor (0,0,0,0.25) combine texture * constant + previous} " + "\tSetTexture [__RenderTex] {constantColor (0,0,0,0.25) combine texture * constant + previous} " + "} } Fallback off }";

	private Material m_Material;

	private Material m_CompositeMaterial;

	private RenderTexture renderTexture;

	private GameObject shaderCamera;

	public DepthOfFieldEffect()
	{
		this.focalDistance = 10f;
		this.focalRange = 300f;
		this.iterations = 0.01f;
		this.iterations2 = 0.02f;
		this.blurSpread = 0.6f;
	}

	private Material GetMaterial()
	{
		if (this.m_Material == null)
		{
			this.m_Material = new Material(DepthOfFieldEffect.blurMatString);
			this.m_Material.set_hideFlags(13);
			this.m_Material.get_shader().set_hideFlags(13);
		}
		return this.m_Material;
	}

	private Material GetCompositeMaterial()
	{
		if (this.m_CompositeMaterial == null)
		{
			this.m_CompositeMaterial = new Material(this.compositeShader);
			this.m_CompositeMaterial.set_hideFlags(13);
		}
		return this.m_CompositeMaterial;
	}

	public override void OnDisable()
	{
		if (this.m_Material)
		{
			Object.DestroyImmediate(this.m_Material.get_shader());
			Object.DestroyImmediate(this.m_Material);
		}
		Object.DestroyImmediate(this.m_CompositeMaterial);
		Object.DestroyImmediate(this.shaderCamera);
		if (this.renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(this.renderTexture);
			this.renderTexture = null;
		}
	}

	public override void Start()
	{
		if (!SystemInfo.get_supportsImageEffects())
		{
			this.set_enabled(false);
		}
		else if (!this.GetMaterial().get_shader().get_isSupported() || !this.GetCompositeMaterial().get_shader().get_isSupported())
		{
			this.set_enabled(false);
		}
		else if (!this.renderSceneShader || !this.renderSceneShader.get_isSupported())
		{
			this.set_enabled(false);
		}
	}

	public override void OnPreRender()
	{
		if (this.get_enabled() && this.get_gameObject().get_active())
		{
			if (this.renderTexture != null)
			{
				RenderTexture.ReleaseTemporary(this.renderTexture);
				this.renderTexture = null;
			}
			this.renderTexture = checked(RenderTexture.GetTemporary((int)this.get_camera().get_pixelWidth(), (int)this.get_camera().get_pixelHeight(), 16));
			if (!this.shaderCamera)
			{
				this.shaderCamera = new GameObject("ShaderCamera", new Type[]
				{
					typeof(Camera)
				});
				this.shaderCamera.get_camera().set_enabled(false);
				this.shaderCamera.set_hideFlags(13);
			}
			Camera camera = this.shaderCamera.get_camera();
			camera.CopyFrom(this.get_camera());
			camera.set_backgroundColor(new Color((float)1, (float)1, (float)1, (float)1));
			camera.set_clearFlags(2);
			camera.set_targetTexture(this.renderTexture);
			camera.RenderWithShader(this.renderSceneShader, "RenderType");
		}
	}

	private void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		RenderTexture.set_active(dest);
		source.SetGlobalShaderProperty("__RenderTex");
		float offsetX = (0.5f + (float)iteration * this.blurSpread) / (float)source.get_width();
		float offsetY = (0.5f + (float)iteration * this.blurSpread) / (float)source.get_height();
		GL.PushMatrix();
		GL.LoadOrtho();
		Material material = this.GetMaterial();
		checked
		{
			for (int i = 0; i < material.get_passCount(); i++)
			{
				material.SetPass(i);
				DepthOfFieldEffect.Render4TapQuad(dest, offsetX, offsetY);
			}
			GL.PopMatrix();
		}
	}

	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		RenderTexture.set_active(dest);
		source.SetGlobalShaderProperty("__RenderTex");
		float offsetX = 1f / (float)source.get_width();
		float offsetY = 1f / (float)source.get_height();
		GL.PushMatrix();
		GL.LoadOrtho();
		Material material = this.GetMaterial();
		checked
		{
			for (int i = 0; i < material.get_passCount(); i++)
			{
				material.SetPass(i);
				DepthOfFieldEffect.Render4TapQuad(dest, offsetX, offsetY);
			}
			GL.PopMatrix();
		}
	}

	public override void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
		RenderTexture temporary3 = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
		this.DownSample4x(source, temporary);
		bool flag = true;
		int num = 0;
		while ((float)num < this.iterations + this.iterations2)
		{
			if (flag)
			{
				this.FourTapCone(temporary, temporary2, num);
			}
			else
			{
				this.FourTapCone(temporary2, temporary, num);
			}
			flag = !flag;
			if ((float)num == this.iterations - (float)1)
			{
				ImageEffects.Blit((!flag) ? temporary2 : temporary, temporary3);
			}
			checked
			{
				num++;
			}
		}
		Material compositeMaterial = this.GetCompositeMaterial();
		compositeMaterial.SetTexture("_BlurTex1", temporary3);
		compositeMaterial.SetTexture("_BlurTex2", (!flag) ? temporary2 : temporary);
		compositeMaterial.SetTexture("_DepthTex", this.renderTexture);
		Shader.SetGlobalVector("_FocalParams", new Vector4(this.focalDistance, 1f / this.focalDistance, this.focalRange, 1f / this.focalRange));
		ImageEffects.BlitWithMaterial(compositeMaterial, source, destination);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
		RenderTexture.ReleaseTemporary(temporary3);
		if (this.renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(this.renderTexture);
			this.renderTexture = null;
		}
	}

	private static void Render4TapQuad(RenderTexture dest, float offsetX, float offsetY)
	{
		GL.Begin(7);
		Vector2 vector = Vector2.get_zero();
		if (dest != null)
		{
			vector = dest.GetTexelOffset() * 0.75f;
		}
		DepthOfFieldEffect.Set4TexCoords(vector.x, vector.y, offsetX, offsetY);
		GL.Vertex3((float)0, (float)0, 0.1f);
		DepthOfFieldEffect.Set4TexCoords(1f + vector.x, vector.y, offsetX, offsetY);
		GL.Vertex3((float)1, (float)0, 0.1f);
		DepthOfFieldEffect.Set4TexCoords(1f + vector.x, 1f + vector.y, offsetX, offsetY);
		GL.Vertex3((float)1, (float)1, 0.1f);
		DepthOfFieldEffect.Set4TexCoords(vector.x, 1f + vector.y, offsetX, offsetY);
		GL.Vertex3((float)0, (float)1, 0.1f);
		GL.End();
	}

	private static void Set4TexCoords(float x, float y, float offsetX, float offsetY)
	{
		GL.MultiTexCoord2(0, x - offsetX, y - offsetY);
		GL.MultiTexCoord2(1, x + offsetX, y - offsetY);
		GL.MultiTexCoord2(2, x + offsetX, y + offsetY);
		GL.MultiTexCoord2(3, x - offsetX, y + offsetY);
	}

	public override void Main()
	{
	}
}
