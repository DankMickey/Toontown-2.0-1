using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class GameController : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class $TimedGameRestart$142 : GenericGenerator<WaitForSeconds>
	{
		internal GameController $self_$146;

		public $TimedGameRestart$142(GameController self_)
		{
			this.$self_$146 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GameController.$TimedGameRestart$142.$(this.$self_$146);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class $NewLevel$147 : GenericGenerator<WaitForSeconds>
	{
		internal GameController $self_$149;

		public $NewLevel$147(GameController self_)
		{
			this.$self_$149 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GameController.$NewLevel$147.$(this.$self_$149);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class $GameOver$150 : GenericGenerator<WaitForSeconds>
	{
		internal GameController $self_$154;

		public $GameOver$150(GameController self_)
		{
			this.$self_$154 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new GameController.$GameOver$150.$(this.$self_$154);
		}
	}

	public int gameLevel;

	public int gameEnemies;

	public int gameLives;

	public int gameScore;

	public bool gameOver;

	private float startTime;

	public float timeLeft;

	public float gameLength;

	private float gameLengthFrames;

	public AsteroidField donutHolePrefab;

	public static GameController instance;

	public bool readyToPlay;

	public GameObject shipPrefab;

	public StarField starFieldPrefab;

	private AsteroidField donutHole;

	public int countdown;

	public float startDelayTime;

	public float startDelay;

	private bool secondPhase;

	private bool starFieldExists;

	public AudioClip gameOverSound;

	public AudioClip gameOverWinSound;

	public GameObject astronaut;

	public GameController()
	{
		this.gameEnemies = -100;
		this.gameLives = 3;
		this.gameLength = (float)10;
		this.startDelay = 5f;
	}

	public override void Start()
	{
		MonoBehaviour.print("GAMECONTROLLER : Start ");
		this.starFieldExists = false;
		this.secondPhase = false;
		this.timeLeft = this.gameLength;
		this.readyToPlay = false;
		this.gameLengthFrames = this.gameLength * (float)60;
		this.startDelayTime = Time.get_time();
		this.startTime = Time.get_time();
		this.donutHole = (AsteroidField)Object.Instantiate(this.donutHolePrefab);
		this.countdown = checked((int)this.startDelay);
	}

	public override void Update()
	{
		this.countdown = checked((int)(unchecked(this.startDelay - (Time.get_time() - this.startDelayTime))));
		if (this.donutHole && !this.starFieldExists)
		{
			Object.Instantiate(this.starFieldPrefab);
			this.starFieldExists = true;
		}
		if (!this.readyToPlay && this.countdown <= 0)
		{
			this.readyToPlay = true;
			this.startTime = Time.get_time();
		}
	}

	public override bool GetReadyToPlay()
	{
		return this.readyToPlay;
	}

	public override void SetGameOver(bool gameState)
	{
		this.gameOver = gameState;
		this.readyToPlay = false;
		this.timeLeft = (float)0;
	}

	public override bool GetGameOver()
	{
		return this.gameOver;
	}

	public override void Awake()
	{
		GameController.instance = (GameController)Object.FindObjectOfType(typeof(GameController));
		if (GameController.instance == null)
		{
			Debug.Log("Could not locate the Game Controller");
		}
		this.gameOver = false;
	}

	public override IEnumerator TimedGameRestart(float timer, Vector3 position)
	{
		return new GameController.$TimedGameRestart$142(this).GetEnumerator();
	}

	public override void UpdateGameTime()
	{
		if (!this.readyToPlay)
		{
			this.timeLeft = this.gameLength;
		}
		else
		{
			this.timeLeft = this.gameLength - (Time.get_time() - this.startTime);
			if (this.timeLeft <= (float)0)
			{
				this.timeLeft = (float)0;
				this.gameOver = true;
				this.StartCoroutine_Auto(this.GameOver());
			}
		}
	}

	public override void FixedUpdate()
	{
		if (this.gameEnemies == 0)
		{
			this.gameOver = true;
			MonoBehaviour.print("GAMECONTROLLER: no, more enemies game Over ");
			GameController.instance.gameLevel = checked(GameController.instance.gameLevel + 1);
			this.StartCoroutine_Auto(this.NewLevel());
		}
		if (!this.gameOver)
		{
			this.UpdateGameTime();
		}
	}

	public override void StartDelay()
	{
		if (Time.get_time() - this.startDelayTime > this.startDelay)
		{
			this.readyToPlay = true;
			this.startTime = Time.get_time();
		}
	}

	public override IEnumerator NewLevel()
	{
		return new GameController.$NewLevel$147(this).GetEnumerator();
	}

	public override IEnumerator GameOver()
	{
		return new GameController.$GameOver$150(this).GetEnumerator();
	}

	public override void AddScore()
	{
		HighScores.instance.AddScore(PlayerPrefs.GetString("PlayerName"), this.gameScore);
	}

	public override void Main()
	{
	}
}
