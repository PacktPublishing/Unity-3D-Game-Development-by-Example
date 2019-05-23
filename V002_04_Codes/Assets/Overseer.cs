using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overseer : MonoBehaviour {
	
	public List<ReactorCore> ReactorCores;
	bool Managing = false;
	public AudioSource MusicPlayer;
	public float TimePassed = 0;
		
	void volume(bool up)
	{
		float volume = AudioListener.volume;
		if (up)
		{
			volume += 0.05f;
			
			if (volume > 1.0f)
				volume = 1.0f;
		}
		else
		{
			volume -= 0.05f;
			
			if (volume < 0)
				volume = 0;
		}
		
		AudioListener.volume = volume;
	}
	
	void ProcessKeyInput()
	{
		if ((Input.GetKeyDown(KeyCode.Equals)) || (Input.GetKeyDown(KeyCode.Plus)) || (Input.GetKeyDown(KeyCode.KeypadPlus)))
		{
			volume(true);
		}
		
		if ((Input.GetKeyDown(KeyCode.Minus)) || (Input.GetKeyDown(KeyCode.KeypadMinus)))
		{
			volume(false);
		}
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			MusicManager();
		}
		
		TimePassed += Time.deltaTime;
		if (TimePassed > 10.0f)
		{
			Preferences SavePrefs = new Preferences();
			if (null != MusicPlayer)
			{
				SavePrefs.Music = MusicPlayer.isPlaying;
			}
			
			SavePrefs.Volume = AudioListener.volume;
			SavePrefs.SavePreferences();
			TimePassed = 0;
		}
	}
	
	void MusicManager()
	{
		if (null != MusicPlayer)
		{
			if (MusicPlayer.isPlaying)
			{
				MusicPlayer.Stop ();
			}
			else
			{
				MusicPlayer.Play ();
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		if (null != ReactorCores)
		{
			foreach(ReactorCore Core in ReactorCores)
			{
				Core.ControlState = ReactorCore.RodStatus.MovingDown;
			}
		}
		
		Preferences Playerpref = new Preferences();
		if (Playerpref.LoadPreferences())
		{
			if (Playerpref.Volume != -2.0f)
			{
				AudioListener.volume = Playerpref.Volume;
			}
			
			if (!Playerpref.Music)
			{
				if (null != MusicPlayer)
				{
					MusicPlayer.Stop ();
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
			if (!Managing)
		{
			StartCoroutine(ManageReactors());
		}

		ProcessKeyInput();
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
