using System;
using UnityEngine;

[Serializable]
public class coloranim : MonoBehaviour
{
	public Color colorStart;

	public Color colorEnd;

	public float duration;

	public coloranim()
	{
		this.colorStart = Color.get_red();
		this.colorEnd = Color.get_green();
		this.duration = 0.01f;
	}

	public void Update()
	{
		float num = Mathf.PingPong(Time.get_time(), this.duration) / this.duration;
		this.get_renderer().get_material().set_color(Color.Lerp(this.colorStart, this.colorEnd, num));
	}

	public void Main()
	{
	}
}
