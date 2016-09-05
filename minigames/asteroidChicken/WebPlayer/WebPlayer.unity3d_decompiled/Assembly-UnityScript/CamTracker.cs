using System;
using UnityEngine;

[Serializable]
public class CamTracker : MonoBehaviour
{
	public override void Update()
	{
		Ray ray = this.get_camera().ScreenPointToRay(new Vector3(Input.get_mousePosition().x, Input.get_mousePosition().y, (float)0));
		Debug.DrawRay(ray.get_origin(), ray.get_direction() * (float)100, Color.get_yellow());
	}

	public override void Main()
	{
	}
}
