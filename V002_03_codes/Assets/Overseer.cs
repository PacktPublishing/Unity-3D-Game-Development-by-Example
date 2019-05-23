using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overseer : MonoBehaviour {
	
	public List<ReactorCore> ReactorCores;
	bool Managing = false;

	// Use this for initialization
	void Start () {
		if (null != ReactorCores)
		{
			foreach(ReactorCore Core in ReactorCores)
			{
				Core.ControlState = ReactorCore.RodStatus.MovingDown;
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
			if (!Managing)
		{
			StartCoroutine(ManageReactors());
		}

	
	}
	
		public ReactorCore SelectCore(List<ReactorCore> Cores)
	{
		ReactorCore SelectedCore = null;
		
		if (null != Cores)
		{
			int ChosenCore = Random.Range(0, Cores.Count);
			
			SelectedCore = Cores[ChosenCore];
		}
		
		if (null != SelectedCore)
		{
			return SelectedCore;
		}
		else
		{
			return null;
		}
	}
	
	IEnumerator ManageReactors()
	{
		Managing = true;
		yield return new WaitForSeconds(Random.Range(1.6f, 2.3f));
		int tries = 3;
		ReactorCore NewUpCore = SelectCore (ReactorCores);
		if (null != NewUpCore)
		{
			if (NewUpCore.ControlState == ReactorCore.RodStatus.Down)
			{
				NewUpCore.ControlState = ReactorCore.RodStatus.Selected;
			}
			else
			{
				while ((tries > 0) && (NewUpCore.ControlState != ReactorCore.RodStatus.Down))
				{
					tries -= 1;
					NewUpCore = SelectCore(ReactorCores);
				}
				
				NewUpCore.ControlState = ReactorCore.RodStatus.Selected;
				
			}
		}
		Managing = false;
	}
}
