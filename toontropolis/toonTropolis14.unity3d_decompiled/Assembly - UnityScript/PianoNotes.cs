using System;
using UnityEngine;

[Serializable]
public class PianoNotes : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (other.get_name() == "Foot Effects")
		{
			this.get_audio().Play();
		}
	}

	public void Main()
	{
	}
}
