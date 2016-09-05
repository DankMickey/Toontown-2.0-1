using System;

[Serializable]
public class AnimInfo
{
	public string anim;

	public float min_dur;

	public float max_dur;

	public float delta;

	public AnimInfo()
	{
		this.anim = "junk";
		this.min_dur = (float)1;
		this.max_dur = (float)10;
		this.delta = (float)0;
	}

	public void Main()
	{
	}
}
