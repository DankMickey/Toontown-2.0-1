using System;
using UnityEngine;

[Serializable]
public class billboard : MonoBehaviour
{
	public GameObject target;

	public GameObject billboard;

	public void Update()
	{
		this.get_transform().LookAt(this.target.get_transform());
	}

	public void Main()
	{
	}
}
