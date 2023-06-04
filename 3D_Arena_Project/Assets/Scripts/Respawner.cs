using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{

	[SerializeField] private float deltaX;
	[SerializeField] private float deltaZ;
	private static Vector3 center = Vector3.up;

	const int layermaskToCheck =
	(1 << Layers.Enemy) +
		(1 << Layers.Default);
	private void OnTriggerEnter(Collider other)
	{
		var player = other.gameObject.GetComponent<Player>();
		player.PrepareToTeleport();
		other.transform.position = GetRandomTeleportPosition();
	}

	Vector3 GetRandomTeleportPosition()
	{
		int tries = 10;
		while (tries > 0)
		{
			var randomPosition = center
				+ deltaZ * Random.Range(-1f, 1f) * Vector3.forward
				+ deltaX * Random.Range(-1f, 1f) * Vector3.right;
			var colliders = Physics.OverlapSphere(randomPosition, 1, layermaskToCheck);

			if (colliders.Length == 0)
			{
				var g =GameObject.CreatePrimitive(PrimitiveType.Cube);
				g.transform.position = randomPosition;
				g.transform.localScale = Vector3.one * 0.1f;
				Destroy(g.GetComponent<Collider>());
				return randomPosition;
			}
		}
		return center ;
	}

}
