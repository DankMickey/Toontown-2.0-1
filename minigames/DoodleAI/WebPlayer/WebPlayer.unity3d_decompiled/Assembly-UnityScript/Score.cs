using System;
using UnityEngine;

[Serializable]
public class Score : MonoBehaviour
{
	public static int score;

	public GUIText hitCount;

	public override void Update()
	{
		string text = Score.score.ToString();
		this.hitCount.set_text(text);
	}

	public override void Main()
	{
	}
}
