using System;
using UnityEngine;

[Serializable]
public class BlankStart : MonoBehaviour
{
	public void Update()
	{
		Application.LoadLevel(2);
	}

	public void Main()
	{
	}
}
