using System;
using UnityEngine;

[Serializable]
public class UVScroller : MonoBehaviour
{
	public float scrollSpeed;

	public UVScroller()
	{
		this.scrollSpeed = 0.1f;
	}

	public void Update()
	{
		float num = Time.get_time() * this.scrollSpeed;
		this.get_renderer().get_material().SetTextureOffset("_BumpMap", new Vector2(num / (7f * (float)-1), num));
		this.get_renderer().get_material().SetTextureOffset("_MainTex", new Vector2(num / 10f, num));
	}

	public void Main()
	{
	}
}
