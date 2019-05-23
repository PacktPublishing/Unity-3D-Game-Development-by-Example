using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overseer : MonoBehaviour {
	public GUISkin mySkin;
	public List<ReactorCore> ReactorCores;
	bool Managing = false;
	public AudioSource MusicPlayer;
	public float TimePassed = 0;
	public int RoundLength = 60;
	public int RoundDuration = 60;
	
	public bool CountingTime = true;
	public bool RoundOver = false;
	public int score = 0;
		
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
		
		if (Input.GetKeyDown (KeyCode.P))
		{
			if (Time.timeScale == 1.0f)
				Time.timeScale = 0.0f;
			else
				Time.timeScale = 1.0f;
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
			
			RoundDuration = RoundLength;
			StartCoroutine(RoundTimer ());
			StartCoroutine(CalculateScore());
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
	
	void OnGUI()
	{
		string myStyle  = "ReactorStatusStablizing";
		GUI.Box (new Rect((Screen.width/2) - (375/2), 275, 375, 24), 
			"Click on a Reactor core as it rises to prevent a meltdown!");
		
		if (null != mySkin)
			GUI.skin = mySkin;
		
		GUI.BeginGroup(new Rect((Screen.width/2) - 511, 0, 1024, 102));
		string Reactor1Status = "Offline";
		if (null != ReactorCores[0])
		{
			switch (ReactorCores[0].ControlState)
			{
			case ReactorCore.RodStatus.Up:
				myStyle = "ReactorStatusVenting";
				Reactor1Status = "VENTING RADIATION!";
				break;
			case ReactorCore.RodStatus.MovingUp:
				myStyle = "ReactorStatusRodFailure";
				Reactor1Status = "Control Rod Failure!";
				break;
			case ReactorCore.RodStatus.MovingDown:
				myStyle = "ReactorStatusStabilizing";
				Reactor1Status = "Stabilizing";
				break;
			case ReactorCore.RodStatus.Down:
				myStyle = "ReactorStatusNominal";
				Reactor1Status = "Nominal";
				break;
			case ReactorCore.RodStatus.Selected:
				myStyle = "ReactorStatusUnstable";
				Reactor1Status = "Unstable Activity Detected!";
				break;
			}
		}
		if (null != mySkin)
			GUI.Box(new Rect(2, 2, 509, 48), "Reactor 1 Status", "ReactorConsole");
		else
			GUI.Box(new Rect(2, 2, 509, 48), "Reactor 1 Status");
		if (null != mySkin)
			GUI.Box (new Rect(4, 24, 505, 24), Reactor1Status, myStyle);
		else
			GUI.Box (new Rect(4, 24, 505, 24), Reactor1Status);
		
		string Reactor2Status = "Offline";
		if (null != ReactorCores[1])
		{
			switch (ReactorCores[1].ControlState)
			{
			case ReactorCore.RodStatus.Up:
				myStyle = "ReactorStatusVenting";
				Reactor2Status = "VENTING RADIATION!";
				break;
			case ReactorCore.RodStatus.MovingUp:
				myStyle = "ReactorStatusRodFailure";
				Reactor2Status = "Control Rod Failure!";
				break;
			case ReactorCore.RodStatus.MovingDown:
				myStyle = "ReactorStatusStabilizing";
				Reactor2Status = "Stabilizing";
				break;
			case ReactorCore.RodStatus.Down:
				myStyle = "ReactorStatusNominal";
				Reactor2Status = "Nominal";
				break;
			case ReactorCore.RodStatus.Selected:
				myStyle = "ReactorStatusUnstable";
				Reactor2Status = "Unstable Activity Detected!";
				break;
			}
		}
		if (null != mySkin)
			GUI.Box(new Rect(511, 2, 509, 48), "Reactor 2 Status", "ReactorConsole");
		else
			GUI.Box(new Rect(511, 2, 509, 48), "Reactor 2 Status");
		if (null != mySkin)
			GUI.Box (new Rect(513, 24, 505, 24), Reactor2Status, myStyle);
		else
			GUI.Box (new Rect(513, 24, 505, 24), Reactor2Status);
		
		string Reactor3Status = "Offline";
		if (null != ReactorCores[2])
		{
			switch (ReactorCores[2].ControlState)
			{
			case ReactorCore.RodStatus.Up:
				myStyle = "ReactorStatusVenting";
				Reactor3Status = "VENTING RADIATION!";
				break;
			case ReactorCore.RodStatus.MovingUp:
				myStyle = "ReactorStatusRodFailure";
				Reactor3Status = "Control Rod Failure!";
				break;
			case ReactorCore.RodStatus.MovingDown:
				myStyle = "ReactorStatusStabilizing";
				Reactor3Status = "Stabilizing";
				break;
			case ReactorCore.RodStatus.Down:
				myStyle = "ReactorStatusNominal";
				Reactor3Status = "Nominal";
				break;
			case ReactorCore.RodStatus.Selected:
				myStyle = "ReactorStatusUnstable";
				Reactor3Status = "Unstable Activity Detected!";
				break;
			}
		}
		if (null != mySkin)
			GUI.Box(new Rect(2, 50, 509, 48), "Reactor 3 Status", "ReactorConsole");
		else
			GUI.Box(new Rect(2, 50, 509, 48), "Reactor 3 Status");
		
		if (null != mySkin)
			GUI.Box (new Rect(4, 72, 505, 24), Reactor3Status, myStyle);
		else
			GUI.Box (new Rect(4, 72, 505, 24), Reactor3Status);
		
		string Reactor4Status = "Offline";
		if (null != ReactorCores[3])
		{
			switch (ReactorCores[3].ControlState)
			{
			case ReactorCore.RodStatus.Up:
				myStyle = "ReactorStatusVenting";
				Reactor4Status = "VENTING RADIATION!";
				break;
			case ReactorCore.RodStatus.MovingUp:
				myStyle = "ReactorStatusRodFailure";
				Reactor4Status = "Control Rod Failure!";
				break;
			case ReactorCore.RodStatus.MovingDown:
				myStyle = "ReactorStatusStabilizing";
				Reactor4Status = "Stabilizing";
				break;
			case ReactorCore.RodStatus.Down:
				myStyle = "ReactorStatusNominal";
				Reactor4Status = "Nominal";
				break;
			case ReactorCore.RodStatus.Selected:
				myStyle = "ReactorStatusUnstable";
				Reactor4Status = "Unstable Activity Detected!";
				break;
			}
		}
		if (null != mySkin)
			GUI.Box(new Rect(511, 50, 509, 48), "Reactor 4 Status", "ReactorConsole");
		else
			GUI.Box(new Rect(511, 50, 509, 48), "Reactor 4 Status");

        if (null != mySkin)
		    GUI.Box (new Rect(513, 72, 505, 24), Reactor4Status, myStyle);
        else
            GUI.Box(new Rect(513, 72, 505, 24), Reactor4Status);
		GUI.EndGroup();
		
		if (null != mySkin)
		{
			GUI.Box (new Rect((Screen.width/2) - 100, 100, 200, 48), "Time Till Shutdown", "ReactorConsole");
		}
		else
			GUI.Box (new Rect((Screen.width/2) - 100, 100, 200, 48), "Time Till Shutdown");
		GUI.Box (new Rect((Screen.width/2) - 98, 128, 196, 24), RoundDuration.ToString());
		
		if (null != mySkin)
		{
			GUI.Box (new Rect(2, 300, 180, 48), "Meltdowns Averted", "ReactorConsole");
		}
		else
			GUI.Box (new Rect(2, 300, 180, 48), "Meltdowns Averted");

		
		GUI.Box (new Rect(4, 324, 176, 24), score.ToString ());
		
	}
	
	IEnumerator CalculateScore()
	{
		score = 0;
		foreach (ReactorCore Core in ReactorCores)
		{
			if (null != Core)
			{
				score += Core.TimesFixed;
			}
		}
		
		yield return new WaitForSeconds(0.3f);
		if (RoundDuration > 0)
		{
			StartCoroutine(CalculateScore ());
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
	
	IEnumerator RoundTimer()
	{
		yield return new WaitForSeconds(1.0f);
		if (CountingTime)
		{
			RoundDuration -= 1;
			if ((RoundDuration > 0) && (CountingTime))
			{
				StartCoroutine(RoundTimer());
			}
			else if (RoundDuration <= 0)
			{
				CountingTime = false;
				RoundOver = true;
				Time.timeScale = 0.0f;
			}
		}
		
	}
}
