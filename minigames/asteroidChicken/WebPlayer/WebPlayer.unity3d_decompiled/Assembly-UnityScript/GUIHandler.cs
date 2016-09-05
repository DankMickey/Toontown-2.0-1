using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class GUIHandler : MonoBehaviour
{
	public float scoreTickTime;

	public GameObject scoreObject;

	public GameObject enemiesObject;

	public GameObject[] livesArray;

	public GameObject gameOverObject;

	public GameObject readyToPlayObject;

	public GameObject readyToPlayTextObject;

	private float prevTick;

	public AudioClip tickClip;

	private TextMesh enemiesTextMesh;

	private TextMesh textMesh;

	private TextMesh scoreTextMesh;

	private TextMesh countdownTextMesh;

	public GUIHandler()
	{
		this.scoreTickTime = 0.05f;
	}

	public override void Start()
	{
		this.enemiesTextMesh = (TextMesh)this.enemiesObject.GetComponent(typeof(TextMesh));
		this.scoreTextMesh = (TextMesh)this.scoreObject.GetComponent(typeof(TextMesh));
		this.countdownTextMesh = (TextMesh)this.readyToPlayTextObject.GetComponent(typeof(TextMesh));
	}

	public override void Update()
	{
		this.UpdateTimeLeft(GameController.instance.timeLeft);
		float time = Time.get_time();
		if (time - this.prevTick > this.scoreTickTime)
		{
			int num = this.CheckGUIScore();
			if (num < GameController.instance.gameScore)
			{
				this.AddGUIScore();
			}
			this.prevTick = time;
		}
		this.UpdateGameOver(GameController.instance.gameOver);
		this.UpdateReadyToPlay(GameController.instance.readyToPlay, GameController.instance.countdown);
	}

	public override void UpdateReadyToPlay(bool readyToPlay, int countdown)
	{
		if (readyToPlay)
		{
			this.readyToPlayObject.get_renderer().set_enabled(false);
			this.readyToPlayTextObject.get_renderer().set_enabled(false);
		}
		else
		{
			this.readyToPlayObject.get_renderer().set_enabled(true);
			this.countdownTextMesh.set_text(countdown.ToString());
		}
	}

	public override void UpdateGameOver(bool gameOver)
	{
		if (gameOver)
		{
			this.gameOverObject.get_renderer().set_enabled(true);
		}
	}

	public override void UpdateLives(int nLives)
	{
		checked
		{
			for (int i = 0; i < Extensions.get_length(this.livesArray) - nLives; i++)
			{
				this.livesArray[i].get_renderer().set_enabled(false);
			}
		}
	}

	public override void UpdateTimeLeft(float timeLeft)
	{
		this.enemiesTextMesh.set_text(timeLeft.ToString());
	}

	public override int CheckGUIScore()
	{
		return (!this.scoreObject) ? 0 : UnityBuiltins.parseInt(this.scoreTextMesh.get_text());
	}

	public override void AddGUIScore()
	{
		checked
		{
			if (this.scoreObject)
			{
				int num = UnityBuiltins.parseInt(this.scoreTextMesh.get_text());
				num++;
				this.scoreTextMesh.set_text(num.ToString());
			}
			AudioSource.PlayClipAtPoint(this.tickClip, this.get_transform().get_position(), 0.1f);
		}
	}

	public override void Main()
	{
	}
}
