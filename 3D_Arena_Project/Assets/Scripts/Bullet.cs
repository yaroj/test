using Assets.Scripts;
using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Player Player;
	public Rigidbody Rb;
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out IEnemy enemy))
		{
			if (enemy.GetHit(BulletData.Damage))
			{
				if (Random.Range(0, PlayerData.MaxHealth) > Player.data.Health)
				{
					Ricochet();
				}
				Player.OnEnemyKill(enemy.PowerPerKill);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}

	private void Ricochet( )
	{
		var enemies = Physics.OverlapSphere(transform.position, 5, Layers.Enemy);
		if(enemies.Length > 0)
		{
			Rb.velocity = enemies[0].transform.position - transform.position;
			return;
		}
	}
}
