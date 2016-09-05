using System;
using UnityEngine;

[Serializable]
public class PowerSwitch : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("switch");
	}

	public void Main()
	{
	}
}
