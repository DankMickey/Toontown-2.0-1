using System;
using UnityEngine;

[Serializable]
public class MUS_CymbalTrigger : MonoBehaviour
{
	public int which;

	public void OnTriggerEnter(Collider other)
	{
		GameObject gameObject = GameObject.Find("MusicShop");
		gameObject.BroadcastMessage("PlayCymbal", this.which);
	}

	public void Main()
	{
	}
}
