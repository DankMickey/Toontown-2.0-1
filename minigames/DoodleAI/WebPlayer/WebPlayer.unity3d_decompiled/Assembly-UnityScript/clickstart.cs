using System;
using UnityEngine;

[Serializable]
public class clickstart : MonoBehaviour
{
	public override void OnMouseDown()
	{
		Application.LoadLevel(1);
	}

	public override void Main()
	{
	}
}
