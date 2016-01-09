using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowTargets : MonoBehaviour {

	[SerializeField]
	NavMeshAgent agent;

	GameObject player;

	void Start()
	{
		player = GameObject.FindWithTag ("Player");
		Destroy (gameObject, 5);
	}

	void Reset()
	{
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update()
	{
		agent.SetDestination (player.transform.position);
	}
}