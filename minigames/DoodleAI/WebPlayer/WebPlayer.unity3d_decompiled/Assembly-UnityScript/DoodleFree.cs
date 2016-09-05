using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class DoodleFree : MonoBehaviour
{
	public int numHits;

	public bool hasLost;

	public int bestScore;

	public int lastBest;

	public int winCount;

	public DoodleFree()
	{
		this.winCount = 23;
	}

	public override void OnCollisionEnter(Collision col)
	{
		checked
		{
			if (RuntimeServices.EqualityOperator(UnityRuntimeServices.GetProperty(col.get_gameObject(), "tag"), "free"))
			{
				Debug.Log("I'm FREE!  Thank You Kind Toon!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				this.numHits++;
				Score.score++;
			}
		}
	}

	public override void Update()
	{
		if (Score.score > this.winCount)
		{
			Application.LoadLevel(4);
		}
		if (!this.hasLost)
		{
			string text = this.numHits.ToString();
		}
		else
		{
			string text2 = "Hits:" + this.numHits.ToString() + "\nYour Best" + this.bestScore;
			if (this.bestScore > this.lastBest)
			{
			}
		}
	}

	public override void Main()
	{
	}
}
