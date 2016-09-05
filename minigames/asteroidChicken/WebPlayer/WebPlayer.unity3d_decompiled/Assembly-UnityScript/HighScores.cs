using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class HighScores : MonoBehaviour
{
	public static HighScores instance;

	public string defaultName;

	private Array scoresArray;

	public HighScores()
	{
		this.defaultName = "NoName";
	}

	public override void Awake()
	{
		HighScores.instance = (HighScores)Object.FindObjectOfType(typeof(HighScores));
		if (HighScores.instance == null)
		{
			Debug.Log("Could not find the HighScores class.");
		}
		checked
		{
			if (PlayerPrefs.GetInt("HighscoreSet") == 0)
			{
				Debug.Log("Resetting Highscores..");
				for (int i = 0; i < 10; i++)
				{
					string text = this.defaultName + ", " + Random.Range(0, 50);
					PlayerPrefs.SetString("Highscore" + i, text);
				}
				PlayerPrefs.SetInt("HighscoreSet", 1);
			}
		}
	}

	public override void Start()
	{
		this.scoresArray = new Array();
		checked
		{
			for (int i = 0; i < 10; i++)
			{
				this.scoresArray.Push(PlayerPrefs.GetString("Highscore" + i));
			}
		}
	}

	public override void SortScores()
	{
		int length = this.scoresArray.length;
		checked
		{
			for (int i = 0; i < length - 1; i++)
			{
				for (int j = i + 1; j < length; j++)
				{
					object lhs = this.GetScoreFromString(this.scoresArray[i]);
					object rhs = this.GetScoreFromString(this.scoresArray[j]);
					if (RuntimeServices.ToBool(RuntimeServices.InvokeBinaryOperator("op_LessThan", lhs, rhs)))
					{
						this.Swap(i, j);
					}
				}
			}
			this.UpdatePlayerPrefs();
		}
	}

	public override int GetScoreFromString(object theString)
	{
		object target = UnityRuntimeServices.Invoke(theString, "Split", new object[]
		{
			", ".get_Chars(0)
		}, typeof(MonoBehaviour));
		return RuntimeServices.UnboxInt32(RuntimeServices.Invoke(typeof(UnityBuiltins), "parseInt", new object[]
		{
			RuntimeServices.GetSlice(target, string.Empty, new object[]
			{
				1
			})
		}));
	}

	public override string GetNameFromString(object theString)
	{
		object target = UnityRuntimeServices.Invoke(theString, "Split", new object[]
		{
			", ".get_Chars(0)
		}, typeof(MonoBehaviour));
		return (string)RuntimeServices.Coerce(RuntimeServices.GetSlice(target, string.Empty, new object[]
		{
			0
		}), typeof(string));
	}

	public override void AddScore(string name, int score)
	{
		string value = name + ", " + score;
		this.scoresArray.Push(value);
		this.SortScores();
		this.scoresArray.Pop();
	}

	public override void UpdatePlayerPrefs()
	{
		checked
		{
			for (int i = 0; i < 10; i++)
			{
				PlayerPrefs.SetString("Highscore" + i, (string)RuntimeServices.Coerce(this.scoresArray[i], typeof(string)));
				Debug.Log(this.scoresArray[i]);
			}
		}
	}

	public override void Swap(object a, object b)
	{
		object value = this.scoresArray[RuntimeServices.UnboxInt32(a)];
		this.scoresArray[RuntimeServices.UnboxInt32(a)] = this.scoresArray[RuntimeServices.UnboxInt32(b)];
		this.scoresArray[RuntimeServices.UnboxInt32(b)] = value;
	}

	public override int GetScore(object id)
	{
		return this.GetScoreFromString(this.scoresArray[RuntimeServices.UnboxInt32(id)]);
	}

	public override string GetPlayer(object id)
	{
		return this.GetNameFromString(this.scoresArray[RuntimeServices.UnboxInt32(id)]);
	}

	public override void Main()
	{
	}
}
