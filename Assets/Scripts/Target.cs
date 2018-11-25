using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReactToHit()
	{
		StartCoroutine(Die());
	}

	private IEnumerator Die()
	{
		transform.Rotate(-75, 0, 0);
		yield return new WaitForSeconds(1.5f);
		
		Destroy(gameObject);
	}
}
