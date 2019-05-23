using UnityEngine;
using System.Collections;

public class ReactorCore : MonoBehaviour {
	public GameObject ControlRod;
	public Transform UpPosition;
	public Transform DownPosition;

	// Use this for initialization
	void Start () {
	MoveRod (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void MoveRod(bool MoveUp)
	{
		if (null != ControlRod)
		{
			if (MoveUp)
			{
				while(!ControlRod.transform.localPosition.Equals(UpPosition.position))
				{
					ControlRod.transform.position = Vector3.Lerp (ControlRod.transform.position, UpPosition.position, 0.5f);
				}
			}
			else
			{
				while (!ControlRod.transform.localPosition.Equals(DownPosition.position))
				{
					ControlRod.transform.position = Vector3.Lerp(ControlRod.transform.position, DownPosition.position, 0.5f);
				}
			}
		}
	}
}
