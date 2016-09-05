using System;
using UnityEngine;

[ExecuteInEditMode]
[Serializable]
public class EdgeDetectEffectNormals : ImageEffectBase
{
	public override void Start()
	{
		this.get_camera().set_depthTextureMode(2);
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		ImageEffects.BlitWithMaterial(this.get_material(), source, destination);
	}

	public void Main()
	{
	}
}
