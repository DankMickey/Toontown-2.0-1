using Boo.Lang.Runtime;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(SphereCollider), typeof(Rigidbody))]
[Serializable]
public class Foot : MonoBehaviour
{
	public float baseFootAudioVolume;

	public float soundEffectPitchRandomness;

	public Foot()
	{
		this.baseFootAudioVolume = 1f;
		this.soundEffectPitchRandomness = 0.05f;
	}

	public void OnTriggerEnter(Collider other)
	{
		CollisionParticleEffect collisionParticleEffect = (CollisionParticleEffect)other.GetComponent(typeof(CollisionParticleEffect));
		if (collisionParticleEffect)
		{
			Object.Instantiate(collisionParticleEffect.effect, this.get_transform().get_position(), this.get_transform().get_rotation());
		}
		CollisionSoundEffect collisionSoundEffect = (CollisionSoundEffect)other.GetComponent(typeof(CollisionSoundEffect));
		if (collisionSoundEffect)
		{
			this.get_audio().set_clip(collisionSoundEffect.audioClip);
			this.get_audio().set_volume(collisionSoundEffect.volumeModifier * this.baseFootAudioVolume);
			this.get_audio().set_pitch(Random.Range(1f - this.soundEffectPitchRandomness, 1f + this.soundEffectPitchRandomness));
			this.get_audio().Play();
		}
	}

	public void Reset()
	{
		this.get_rigidbody().set_isKinematic(true);
		RuntimeServices.SetProperty(this.get_collider(), "isTrigger", true);
	}

	public void Main()
	{
	}
}
