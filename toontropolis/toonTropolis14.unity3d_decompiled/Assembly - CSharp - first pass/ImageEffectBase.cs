using System;
using UnityEngine;

[AddComponentMenu(""), RequireComponent(typeof(Camera))]
public class ImageEffectBase : MonoBehaviour
{
	public Shader shader;

	private Material m_Material;

	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.shader);
				this.m_Material.set_hideFlags(13);
			}
			return this.m_Material;
		}
	}

	protected void Start()
	{
		if (!SystemInfo.get_supportsImageEffects())
		{
			base.set_enabled(false);
			return;
		}
		if (!this.shader.get_isSupported())
		{
			base.set_enabled(false);
		}
	}

	protected void OnDisable()
	{
		if (this.m_Material)
		{
			Object.DestroyImmediate(this.m_Material);
		}
	}
}
