using UnityEngine;
using System.Collections;

public class ReactorCore : MonoBehaviour {
	public GameObject ControlRod;
	public Transform UpPosition;
	public Transform DownPosition;

	// Use this for initialization
	void Start () {
		StartCoroutine(MoveRod(false));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public IEnumerator MoveRod(bool MoveUp)
	{
		bool done = false;
		if (null != ControlRod)
		{
			if (MoveUp)
			{
				while(!ControlRod.transform.localPosition.Equals(UpPosition.position))
				{
					ControlRod.transform.position = Vector3.Lerp (ControlRod.transform.position, UpPosition.position, Time.deltaTime);
					yield return new WaitForEndOfFrame();
				}
				
				
			}
			else
			{
				while (!ControlRod.transform.localPosition.Equals(DownPosition.position))
				{
					ControlRod.transform.position = Vector3.Lerp(ControlRod.transform.position, DownPosition.position, Time.deltaTime);
					yield return new WaitForEndOfFrame();
				}
			}
		}
		
		
		yield return new WaitForEndOfFrame();
		if (!done)
		{
			StartCoroutine(MoveRod(MoveUp));
		}
	}
}
