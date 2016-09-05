using System;
using UnityEngine;

[Serializable]
public class DoodleCount : MonoBehaviour
{
	public GUIText hitCount;

	public int numHits;

	public bool hasLost;

	public int bestScore;

	public int lastBest;

	public override void OnTriggerEnter(Collider col)
	{
		checked
		{
			if (col.get_gameObject().get_tag() == "notTray")
			{
				Debug.Log("yes! a Doodle is Free! !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				this.numHits++;
				string text = this.numHits.ToString();
				this.hitCount.set_text(text);
			}
		}
	}

	public override void Update()
	{
		string text;
		if (!this.hasLost)
		{
			text = this.numHits.ToString();
		}
		else
		{
			text = "Hits:" + this.numHits.ToString() + "\nYour best:" + this.bestScore;
			if (this.bestScore > this.lastBest)
			{
				text += "\nNEW RECORD!";
			}
		}
		this.hitCount.set_text(text);
		if (this.get_transform().get_position().y < (float)-3 && !this.hasLost)
		{
			this.hasLost = true;
			this.lastBest = this.bestScore;
			if (this.numHits > this.bestScore)
			{
				this.bestScore = this.numHits;
			}
		}
	}

	public override void OnGUI()
	{
		if (this.hasLost)
		{
			int num = 100;
			int num2 = 50;
			float num3 = (float)(Screen.get_width() / 2);
			float num4 = (float)(num / 2);
			if (GUI.Button(new Rect(num3 - num4, (float)Screen.get_height() * 0.5f, (float)num, (float)num2), "Play Again"))
			{
				this.numHits = 0;
				this.hasLost = false;
				this.get_transform().set_position(new Vector3(0.5f, (float)2, -0.05f));
				this.get_rigidbody().set_velocity(new Vector3((float)0, (float)0, (float)0));
			}
		}
	}

	public override void Main()
	{
	}
}
