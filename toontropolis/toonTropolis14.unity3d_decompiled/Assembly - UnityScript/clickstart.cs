using System;
using UnityEngine;

[Serializable]
public class clickstart : MonoBehaviour
{
	public void OnMouseDown()
	{
		Application.LoadLevel(1);
	}

	public void Main()
	{
	}
}
