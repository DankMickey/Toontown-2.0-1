using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Contrast Stretch")]
public class ContrastStretchEffect : MonoBehaviour
{
	public float adaptationSpeed = 0.02f;

	public float limitMinimum = 0.2f;

	public float limitMaximum = 0.6f;

	private RenderTexture[] adaptRenderTex = new RenderTexture[2];

	private int curAdaptIndex;

	public Shader shaderLum;

	private Material m_materialLum;

	public Shader shaderReduce;

	private Material m_materialReduce;

	public Shader shaderAdapt;

	private Material m_materialAdapt;

	public Shader shaderApply;

	private Material m_materialApply;

	protected Material materialLum
	{
		get
		{
			if (this.m_materialLum == null)
			{
				this.m_materialLum = new Material(this.shaderLum);
				this.m_materialLum.set_hideFlags(13);
			}
			return this.m_materialLum;
		}
	}

	protected Material materialReduce
	{
		get
		{
			if (this.m_materialReduce == null)
			{
				this.m_materialReduce = new Material(this.shaderReduce);
				this.m_materialReduce.set_hideFlags(13);
			}
			return this.m_materialReduce;
		}
	}

	protected Material materialAdapt
	{
		get
		{
			if (this.m_materialAdapt == null)
			{
				this.m_materialAdapt = new Material(this.shaderAdapt);
				this.m_materialAdapt.set_hideFlags(13);
			}
			return this.m_materialAdapt;
		}
	}

	protected Material materialApply
	{
		get
		{
			if (this.m_materialApply == null)
			{
				this.m_materialApply = new Material(this.shaderApply);
				this.m_materialApply.set_hideFlags(13);
			}
			return this.m_materialApply;
		}
	}

	private void Start()
	{
		if (!SystemInfo.get_supportsImageEffects())
		{
			base.set_enabled(false);
			return;
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < 2; i++)
		{
			if (!this.adaptRenderTex[i])
			{
				this.adaptRenderTex[i] = new RenderTexture(1, 1, 32);
				this.adaptRenderTex[i].set_hideFlags(13);
			}
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < 2; i++)
		{
			Object.DestroyImmediate(this.adaptRenderTex[i]);
			this.adaptRenderTex[i] = null;
		}
		if (this.m_materialLum)
		{
			Object.DestroyImmediate(this.m_materialLum);
		}
		if (this.m_materialReduce)
		{
			Object.DestroyImmediate(this.m_materialReduce);
		}
		if (this.m_materialAdapt)
		{
			Object.DestroyImmediate(this.m_materialAdapt);
		}
		if (this.m_materialApply)
		{
			Object.DestroyImmediate(this.m_materialApply);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture renderTexture = RenderTexture.GetTemporary(source.get_width() / 1, source.get_height() / 1);
		Graphics.Blit(source, renderTexture, this.materialLum);
		while (renderTexture.get_width() > 1 || renderTexture.get_height() > 1)
		{
			int num = renderTexture.get_width() / 2;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = renderTexture.get_height() / 2;
			if (num2 < 1)
			{
				num2 = 1;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
			Graphics.Blit(renderTexture, temporary, this.materialReduce);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		this.CalculateAdaptation(renderTexture);
		this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
		Graphics.Blit(source, destination, this.materialApply);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	private void CalculateAdaptation(Texture curTexture)
	{
		int num = this.curAdaptIndex;
		this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
		float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * Time.get_deltaTime());
		num2 = Mathf.Clamp(num2, 0.01f, 1f);
		this.materialAdapt.SetTexture("_CurTex", curTexture);
		this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
		Graphics.Blit(this.adaptRenderTex[num], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
	}
}
