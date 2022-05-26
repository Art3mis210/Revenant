using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour {

	public float lifeTime = 5.0f;
	Transform Parent;
    private void Awake()
    {
		Parent = transform.parent;
    }
    private void OnEnable()
    {
        StartCoroutine(TurnOff());
    }
    private IEnumerator TurnOff()
	{
		yield return new WaitForSeconds(lifeTime);
		transform.parent = Parent;
		gameObject.SetActive(false);
	}
    private void OnDisable()
    {

    }
}
