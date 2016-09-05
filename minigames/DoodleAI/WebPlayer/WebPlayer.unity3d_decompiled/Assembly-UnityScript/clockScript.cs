using System;
using UnityEngine;

[Serializable]
public class clockScript : MonoBehaviour
{
	public bool isPaused;

	public float startTime;

	public float timeRemaining;

	public float percent;

	public Texture2D clockBG;

	public Texture2D clockFG;

	public float clockFGMaxWidth;

	public AudioClip chaseMusic;

	public AudioClip villianMusic;

	public override void Start()
	{
		Debug.Log(Time.get_time());
		this.clockFGMaxWidth = (float)this.clockFG.get_width();
		this.get_audio().set_clip(this.chaseMusic);
		this.get_audio().Play();
	}

	public override void Update()
	{
		if (!this.isPaused)
		{
			this.DoCountdown();
		}
	}

	public override void DoCountdown()
	{
		this.timeRemaining = this.startTime - Time.get_timeSinceLevelLoad();
		this.percent = this.timeRemaining / this.startTime * (float)100;
		if (this.timeRemaining < (float)0)
		{
			this.timeRemaining = (float)0;
			this.isPaused = true;
			this.TimeIsUp();
		}
		this.ShowTime();
	}

	public override void PauseClock()
	{
		this.isPaused = true;
	}

	public override void UnpauseClock()
	{
		this.isPaused = false;
	}

	public override void ShowTime()
	{
		int num = default(int);
		int num2 = default(int);
		checked
		{
			num = (int)(this.timeRemaining / (float)60);
			num2 = (int)(this.timeRemaining % (float)60);
			string text = num.ToString() + ":";
			text += num2.ToString("D2");
			this.get_guiText().set_text(text);
		}
	}

	public override void TimeIsUp()
	{
		this.get_audio().set_clip(this.villianMusic);
		this.get_audio().Play();
		if (Score.score < 23)
		{
			Application.LoadLevel(3);
		}
	}

	public override void OnGUI()
	{
		float num = this.percent / (float)100 * this.clockFGMaxWidth;
		int num2 = 20;
		GUI.BeginGroup(new Rect((float)num2, (float)num2, (float)this.clockBG.get_width(), (float)this.clockBG.get_height()));
		GUI.DrawTexture(new Rect((float)0, (float)0, (float)this.clockBG.get_width(), (float)this.clockBG.get_height()), this.clockBG);
		GUI.BeginGroup(new Rect((float)10, (float)12, num, (float)this.clockFG.get_height()));
		GUI.DrawTexture(new Rect((float)0, (float)0, (float)this.clockFG.get_width(), (float)this.clockFG.get_height()), this.clockFG);
		GUI.EndGroup();
		GUI.EndGroup();
	}

	public override void Main()
	{
	}
}
