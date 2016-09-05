using System;
using UnityEngine;

[Serializable]
public class GlowLookAt : MonoBehaviour
{
	public void Update()
	{
		this.get_transform().LookAt(Camera.get_main().get_transform());
	}

	public void OnBecameVisible()
	{
		this.set_enabled(true);
	}

	public void OnBecameInvisible()
	{
		this.set_enabled(false);
	}

	public void Main()
	{
	}
}
