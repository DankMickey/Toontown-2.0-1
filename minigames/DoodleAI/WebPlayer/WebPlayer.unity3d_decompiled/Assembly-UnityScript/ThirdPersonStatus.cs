using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ThirdPersonStatus : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class $Die$66 : GenericGenerator<WaitForSeconds>
	{
		internal ThirdPersonStatus $self_$68;

		public $Die$66(ThirdPersonStatus self_)
		{
			this.$self_$68 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new ThirdPersonStatus.$Die$66.$(this.$self_$68);
		}
	}

	public int health;

	public int maxHealth;

	public int lives;

	public AudioClip struckSound;

	public AudioClip deathSound;

	private int remainingItems;

	public ThirdPersonStatus()
	{
		this.health = 6;
		this.maxHealth = 6;
		this.lives = 4;
	}

	public override void Awake()
	{
	}

	public override int GetRemainingItems()
	{
		return this.remainingItems;
	}

	public override void ApplyDamage(int damage)
	{
		if (this.struckSound)
		{
			AudioSource.PlayClipAtPoint(this.struckSound, this.get_transform().get_position());
		}
		checked
		{
			this.health -= damage;
			if (this.health <= 0)
			{
				this.SendMessage("Die");
			}
		}
	}

	public override void AddLife(int powerUp)
	{
		checked
		{
			this.lives += powerUp;
			this.health = this.maxHealth;
		}
	}

	public override void AddHealth(int powerUp)
	{
		checked
		{
			this.health += powerUp;
			if (this.health > this.maxHealth)
			{
				this.health = this.maxHealth;
			}
		}
	}

	public override void FoundItem(int numFound)
	{
		checked
		{
			this.remainingItems -= numFound;
		}
	}

	public override void FalloutDeath()
	{
		this.StartCoroutine_Auto(this.Die());
	}

	public override IEnumerator Die()
	{
		return new ThirdPersonStatus.$Die$66(this).GetEnumerator();
	}

	public override void Main()
	{
	}
}
