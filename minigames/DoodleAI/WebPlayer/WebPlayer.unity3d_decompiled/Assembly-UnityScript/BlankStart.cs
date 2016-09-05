using System;
using UnityEngine;

[Serializable]
public class BlankStart : MonoBehaviour
{
	public override void Update()
	{
		Application.LoadLevel(2);
	}

	public override void Main()
	{
	}
}
