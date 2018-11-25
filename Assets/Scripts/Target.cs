using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public void ReactToHit()
	{
		var behavior = GetComponent<WanderingAI>();
		if (behavior != null)
			behavior.SetAlive(false);
		
		StartCoroutine(Die());
	}

	private IEnumerator Die()
	{
		transform.Rotate(-75, 0, 0);
		yield return new WaitForSeconds(1.5f);
		
		Destroy(gameObject);
	}
}
