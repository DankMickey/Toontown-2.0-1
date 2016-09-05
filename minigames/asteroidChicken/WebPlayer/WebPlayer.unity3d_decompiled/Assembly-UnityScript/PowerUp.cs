using System;
using UnityEngine;

[Serializable]
public class PowerUp : MonoBehaviour
{
	public override void Awake()
	{
		Radar.lookForPowerUp = true;
	}

	public override void OnTriggerEnter(Collider other)
	{
		if (other.get_tag() == "Player")
		{
			Ship ship = (Ship)other.GetComponent(typeof(Ship));
			ship.Upgrade();
			Radar.lookForPowerUp = false;
			Object.Destroy(this.get_gameObject());
		}
	}

	public override void Main()
	{
	}
}
