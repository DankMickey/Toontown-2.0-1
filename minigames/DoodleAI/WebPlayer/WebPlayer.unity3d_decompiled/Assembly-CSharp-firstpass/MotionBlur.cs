using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Motion Blur")]
public class MotionBlur : ImageEffectBase
{
	public float blurAmount = 0.8f;

	public bool extraBlur;

	private RenderTexture accumTexture;

	protected void OnDisable()
	{
		base.OnDisable();
		Object.DestroyImmediate(this.accumTexture);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.accumTexture == null || this.accumTexture.get_width() != source.get_width() || this.accumTexture.get_height() != source.get_height())
		{
			Object.DestroyImmediate(this.accumTexture);
			this.accumTexture = new RenderTexture(source.get_width(), source.get_height(), 0);
			this.accumTexture.set_hideFlags(13);
			ImageEffects.Blit(source, this.accumTexture);
		}
		if (this.extraBlur)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.get_width() / 4, source.get_height() / 4, 0);
			ImageEffects.Blit(this.accumTexture, temporary);
			ImageEffects.Blit(temporary, this.accumTexture);
			RenderTexture.ReleaseTemporary(temporary);
		}
		this.blurAmount = Mathf.Clamp(this.blurAmount, 0f, 0.92f);
		base.material.SetTexture("_MainTex", this.accumTexture);
		base.material.SetFloat("_AccumOrig", 1f - this.blurAmount);
		Graphics.Blit(source, this.accumTexture, base.material);
		Graphics.Blit(this.accumTexture, destination);
	}
}
