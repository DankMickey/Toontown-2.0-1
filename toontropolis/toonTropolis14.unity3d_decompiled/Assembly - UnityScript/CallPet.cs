using System;
using UnityEngine;

[Serializable]
public class CallPet : MonoBehaviour
{
	public GUIStyle customButton;

	public GameObject myPet;

	public bool toggleBool;

	private float x;

	private float y;

	private float width;

	private float height;

	public CallPet()
	{
		this.toggleBool = false;
		this.x = (float)10;
		this.y = (float)10;
		this.width = (float)120;
		this.height = (float)40;
	}

	public void Update()
	{
		this.CheckMode();
	}

	public void OnGUI()
	{
		if (Status.menuMode)
		{
			GUI.Box(new Rect(this.x + this.width * (float)0, this.y + this.height, this.width, this.height), "[P]:\n heel toggle " + this.toggleBool);
		}
	}

	public void CheckMode()
	{
		if (Input.GetKeyDown(112))
		{
			if (this.toggleBool)
			{
				this.toggleBool = false;
				this.myPet.BroadcastMessage("SetFollowState", 0);
			}
			else
			{
				this.toggleBool = true;
				this.myPet.BroadcastMessage("SetFollowState", 1);
			}
		}
		if (Input.GetKeyDown(104))
		{
			this.myPet.BroadcastMessage("HeelForce");
		}
	}

	public void Main()
	{
	}
}
