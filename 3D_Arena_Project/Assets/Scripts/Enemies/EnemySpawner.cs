using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
	public class EnemySpawner : MonoBehaviour
	{

		private List<IEnemy> enemies = new List<IEnemy>();
		private static EnemySpawner instance;

		// Use this for initialization
		[SerializeField] int timeToSpawn = 5;
		[SerializeField] int minTimeToSpawn = 2;
		[SerializeField] float bossSpawnRatio = 0.2f;
		[SerializeField] float distanceFromCenter = 4.8f;
		[SerializeField] float DefaultHeight = 0.32f;
		[SerializeField] EnemyBoss bossEnemy;
		[SerializeField] Enemy enemy;

		private int countOfEnemiesToSpawn = 1;

		public static EnemySpawner Instance { get => instance; set => instance = value; }

		void Start()
		{
			instance = this;
			StartCoroutine(StartSpawning());
		}

		IEnumerator StartSpawning()
		{
			while (true)
			{
				for (int i = 0; i < countOfEnemiesToSpawn; i++)
				{
					SpawnRandomEnemy();
				}
				yield return new WaitForSeconds(timeToSpawn);
				if (timeToSpawn > minTimeToSpawn)
				{
					timeToSpawn--;
				}
				else
				{
					countOfEnemiesToSpawn++;
				}
			}
		}

		private void SpawnRandomEnemy()
		{
			if (Random.value < bossSpawnRatio)
			{
				var e = Instantiate(bossEnemy, GetRandomSpawnLocation(), Quaternion.identity);
				e.Player = Player.Instance;
				enemies.Add(e);
			}
			else
			{
				var e = Instantiate(enemy, GetRandomSpawnLocation(), Quaternion.identity);
				e.Player = Player.Instance;
				enemies.Add(e);
			}
		}

		private Vector3 GetRandomSpawnLocation()
		{
			while (true)
			{
				Vector3 spawnLocation = new Vector3(
					Random.Range(-distanceFromCenter, distanceFromCenter),
					DefaultHeight,
					Random.Range(-distanceFromCenter, distanceFromCenter));
				if (spawnLocation.magnitude < distanceFromCenter)
				{
					return spawnLocation;
				}
			}
		}

		public void KillAllEnemies()
		{
			const int damageToKillThemAll = 999999;
			foreach (var e in enemies)
			{
				if (e != null)
				{
					try { e.GetHit(damageToKillThemAll);
					}
					catch(System.Exception)
					{

					}
					}
			}
			enemies = new List<IEnemy>();
		}

	}
}