using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ConnectRope : MonoBehaviour
{
	[SerializeField] Interaction_Item item;
	[SerializeField] Transform endNode;
	[SerializeField] Transform target;
	private bool init;

	private void OnEnable()
	{
		init = false;
		StartCoroutine(Wait());
	}

	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.5f);
		init = true;
	}

	private void Update()
	{
		if (item.gameObject == null)
			return;

		if (item.gameObject.activeSelf == false)
			return;

		if (target == null)
			return;

		if (item.IsRope_Mount == false)
			return;
		
		if (init)
			endNode.position = target.position;
	}
}