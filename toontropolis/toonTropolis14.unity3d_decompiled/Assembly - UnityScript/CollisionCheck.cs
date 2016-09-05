using System;
using UnityEngine;

[Serializable]
public class CollisionCheck : MonoBehaviour
{
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
