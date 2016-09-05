using System;
using UnityEngine;

[ExecuteInEditMode]
[Serializable]
public class MainMenu : MonoBehaviour
{
	public GUISkin guiSkin;

	public string gameSceneName;

	private string textAreaString;

	private bool isLoading;

	public MainMenu()
	{
		this.textAreaString = string.Empty;
	}

	public override void OnGUI()
	{
		if (this.guiSkin)
		{
			GUI.set_skin(this.guiSkin);
		}
		else
		{
			Debug.Log("Menu: GUI Skin object is missing!");
		}
		checked
		{
			if (GUI.Button(new Rect((float)(Screen.get_width() / 2 - 160), (float)(Screen.get_height() - 110), (float)320, (float)70), "Let's Play"))
			{
				this.isLoading = true;
				PlayerPrefs.SetString("PlayerName", this.textAreaString);
				Application.LoadLevel(this.gameSceneName);
			}
			this.textAreaString = GUI.TextField(new Rect((float)(Screen.get_width() / 2 - 60), (float)(Screen.get_height() - 140), (float)120, (float)120), this.textAreaString);
			if (!((Application.get_platform() == 3) ?? (Application.get_platform() == 5)) && GUI.Button(new Rect((float)(Screen.get_width() / 2 - 160), (float)(Screen.get_height() - 80), (float)320, (float)120), "Quit"))
			{
				Application.Quit();
			}
			if (this.isLoading)
			{
				GUI.Label(new Rect((float)5, (float)(Screen.get_height() - 20), (float)400, (float)70), "Loading Level");
			}
			this.DrawHighScores();
		}
	}

	public override void DrawHighScores()
	{
		checked
		{
			for (int i = 0; i < 10; i++)
			{
				int score;
				string player;
				if (HighScores.instance)
				{
					score = HighScores.instance.GetScore(i);
					player = HighScores.instance.GetPlayer(i);
				}
				GUI.Label(new Rect((float)(Screen.get_width() / 2 - 100), (float)(50 + 25 * i), (float)200, (float)20), i + 1 + " - " + player + " - " + score);
			}
		}
	}

	public override void Main()
	{
	}
}
