using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public float ClosedRotation = 180f;
	public float OpenRotation = 90f;

	[ContextMenu("Open")]
    public void Open()
	{
		Vector3 newRotation = transform.eulerAngles;
		newRotation.y = OpenRotation;
		transform.eulerAngles = newRotation;
	}

	[ContextMenu("Close")]
	public void Close()
	{
		Vector3 newRotation = transform.eulerAngles;
		newRotation.y = ClosedRotation;
		transform.eulerAngles = newRotation;
	}
}
