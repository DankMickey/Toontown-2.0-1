using System;
using UnityEngine;

[Serializable]
public class GS_BoxTriggerActions : MonoBehaviour
{
	public GameObject gagShop;

	public void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.get_tag() == "Player")
		{
			MonoBehaviour.print("BOX TRIGGER: trigger box actions");
		}
	}

	public void Main()
	{
	}
}
