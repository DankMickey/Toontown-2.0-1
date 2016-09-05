using System;
using UnityEngine;

[Serializable]
public class ObjectRotator : MonoBehaviour
{
	public void Update()
	{
		this.get_transform().Rotate((float)0, (float)45 * Time.get_deltaTime(), (float)0);
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
