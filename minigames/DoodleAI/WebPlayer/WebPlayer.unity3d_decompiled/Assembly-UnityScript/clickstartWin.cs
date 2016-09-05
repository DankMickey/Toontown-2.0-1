using System;
using UnityEngine;

[Serializable]
public class clickstartWin : MonoBehaviour
{
	public override void OnMouseDown()
	{
		Score.score = 0;
		Application.LoadLevel(1);
	}

	public override void Main()
	{
	}
}
