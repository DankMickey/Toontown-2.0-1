using System;
using UnityEngine;

[Serializable]
public class AudioController : MonoBehaviour
{
	public AudioClip originalClip;

	public AudioClip oldClip;

	public AudioClip nightClip;

	private AudioClip currentClip;

	public void Update()
	{
		if (triggerCogBuilding.fear)
		{
			this.get_audio().set_volume(0.1f);
		}
		else
		{
			this.get_audio().set_volume(0.3f);
		}
		if (PlayerCollisions.dayMode == "night")
		{
			if (this.currentClip == this.originalClip)
			{
				this.get_audio().Stop();
				this.currentClip = this.nightClip;
			}
		}
		else if (this.currentClip == this.nightClip)
		{
			this.get_audio().Stop();
			this.currentClip = this.originalClip;
		}
		if (!this.get_audio().get_isPlaying())
		{
			this.get_audio().set_clip(this.currentClip);
			this.get_audio().Play();
		}
	}

	public void Main()
	{
		this.currentClip = this.originalClip;
	}
}
