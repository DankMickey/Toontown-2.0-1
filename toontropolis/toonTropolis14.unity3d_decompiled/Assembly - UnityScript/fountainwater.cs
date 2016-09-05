using System;
using UnityEngine;

[Serializable]
public class fountainwater : MonoBehaviour
{
	public void Update()
	{
		float num = Mathf.Sin(Time.get_time()) * 0.05f + (float)1;
		float num2 = Mathf.Sin(Time.get_time()) * 0.07f + (float)1;
		this.get_renderer().get_material().SetTextureScale("_BumpMap", new Vector2(num, num2));
	}

	public void Main()
	{
	}
}
