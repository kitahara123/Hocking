using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

	public bool Alive { get; private set; } = true;
	
	public void ReactToHit()
	{
		var behavior = GetComponent<WanderingAI>();
		if (behavior != null)
			behavior.SetAlive(false);
		
		StartCoroutine(Die());
	}

	private IEnumerator Die()
	{
		Alive = false;
		transform.Rotate(-75, 0, 0);
		yield return new WaitForSeconds(1.5f);
		
		Destroy(gameObject);
	}
}
