using System;

[Serializable]
public class StateDuration
{
	public string state_name;

	public float min_dur;

	public float max_dur;

	public StateDuration()
	{
		this.state_name = "idle";
		this.min_dur = (float)1;
		this.max_dur = (float)10;
	}

	public void Main()
	{
	}
}
