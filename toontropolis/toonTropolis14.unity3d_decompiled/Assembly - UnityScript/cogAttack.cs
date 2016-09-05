using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class cogAttack : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class CogTaunt$42 : GenericGenerator<WaitForSeconds>
	{
		internal cogAttack $self_168;

		public CogTaunt$42(cogAttack self_)
		{
			this.$self_168 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogAttack.CogTaunt$42.$(this.$self_168);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class CogDeath$43 : GenericGenerator<WaitForSeconds>
	{
		internal cogAttack $self_192;

		public CogDeath$43(cogAttack self_)
		{
			this.$self_192 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogAttack.CogDeath$43.$(this.$self_192);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class CogDamageA$44 : GenericGenerator<WaitForSeconds>
	{
		internal object $inputDamage172;

		internal cogAttack $self_173;

		public CogDamageA$44(object inputDamage, cogAttack self_)
		{
			this.$inputDamage172 = inputDamage;
			this.$self_173 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogAttack.CogDamageA$44.$(this.$inputDamage172, this.$self_173);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class CogDamageB$45 : GenericGenerator<WaitForSeconds>
	{
		internal object $inputDamage177;

		internal cogAttack $self_178;

		public CogDamageB$45(object inputDamage, cogAttack self_)
		{
			this.$inputDamage177 = inputDamage;
			this.$self_178 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogAttack.CogDamageB$45.$(this.$inputDamage177, this.$self_178);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class CogDamageC$46 : GenericGenerator<WaitForSeconds>
	{
		internal object $inputDamage183;

		internal cogAttack $self_184;

		public CogDamageC$46(object inputDamage, cogAttack self_)
		{
			this.$inputDamage183 = inputDamage;
			this.$self_184 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogAttack.CogDamageC$46.$(this.$inputDamage183, this.$self_184);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class CogAttackA$47 : GenericGenerator<WaitForSeconds>
	{
		internal cogAttack $self_186;

		public CogAttackA$47(cogAttack self_)
		{
			this.$self_186 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogAttack.CogAttackA$47.$(this.$self_186);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class CogAttackB$48 : GenericGenerator<WaitForSeconds>
	{
		internal cogAttack $self_188;

		public CogAttackB$48(cogAttack self_)
		{
			this.$self_188 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogAttack.CogAttackB$48.$(this.$self_188);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class CogAttackC$49 : GenericGenerator<WaitForSeconds>
	{
		internal cogAttack $self_190;

		public CogAttackC$49(cogAttack self_)
		{
			this.$self_190 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new cogAttack.CogAttackC$49.$(this.$self_190);
		}
	}

	public int hitpoints;

	public int strengthCogAttackA;

	public int strengthCogAttackB;

	public int strengthCogAttackC;

	public GameObject playerChar;

	public GameObject cameraMain;

	public GameObject prefabDamage;

	public GameObject lookAtTarget;

	public AnimationClip animTaunt;

	public AnimationClip animIdleBattle;

	public AnimationClip animCogAttackA;

	public AnimationClip animCogAttackB;

	public AnimationClip animCogAttackC;

	public AnimationClip animCogDamageA;

	public AnimationClip animCogDamageB;

	public AnimationClip animCogDamageC;

	public GameObject piano;

	public Transform launchPos;

	public AnimationClip animCogDeath;

	public cogAttack()
	{
		this.hitpoints = 30;
		this.strengthCogAttackA = 5;
		this.strengthCogAttackB = 10;
		this.strengthCogAttackC = 15;
	}

	public void Start()
	{
		if (!this.playerChar)
		{
			this.playerChar = GameObject.FindWithTag("Player");
		}
		if (!this.cameraMain)
		{
			this.cameraMain = GameObject.FindWithTag("MainCamera");
		}
	}

	public IEnumerator CogTaunt()
	{
		return new cogAttack.CogTaunt$42(this).GetEnumerator();
	}

	public IEnumerator CogDamageA(object inputDamage)
	{
		return new cogAttack.CogDamageA$44(inputDamage, this).GetEnumerator();
	}

	public IEnumerator CogDamageB(object inputDamage)
	{
		return new cogAttack.CogDamageB$45(inputDamage, this).GetEnumerator();
	}

	public IEnumerator CogDamageC(object inputDamage)
	{
		return new cogAttack.CogDamageC$46(inputDamage, this).GetEnumerator();
	}

	public void CogAttackRandom()
	{
		int num = Random.Range(0, 100);
		if (num < 33)
		{
			this.StartCoroutine_Auto(this.CogAttackA());
		}
		if (num >= 33 && num < 66)
		{
			this.StartCoroutine_Auto(this.CogAttackB());
		}
		if (num >= 66)
		{
			this.StartCoroutine_Auto(this.CogAttackC());
		}
	}

	public IEnumerator CogAttackA()
	{
		return new cogAttack.CogAttackA$47(this).GetEnumerator();
	}

	public IEnumerator CogAttackB()
	{
		return new cogAttack.CogAttackB$48(this).GetEnumerator();
	}

	public IEnumerator CogAttackC()
	{
		return new cogAttack.CogAttackC$49(this).GetEnumerator();
	}

	public IEnumerator CogDeath()
	{
		return new cogAttack.CogDeath$43(this).GetEnumerator();
	}

	public void CameraLookAtMe()
	{
		this.cameraMain.SendMessage("SetTarget", this.lookAtTarget.get_transform(), 1);
	}

	public void Main()
	{
	}
}
