using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Overseer : MonoBehaviour {
	public List<ReactorCore> ReactorCores;	
	bool Managing = false;
	public AudioSource MusicPlayer;
	public int RoundLength = 60;
	public int RoundDuration = 60;
	
	bool CountingTime = true;
	bool RoundOver = false;
	
	float TimePassed = 0;
	
	public GUISkin mySkin;
	string PlayerInitials = "TST";
	int score = 0;
	int tempScore = 0;
	int bonusMultiplier = 0;
	
	
	bool paused = false;
	bool Combo = false;
	
	// Use this for initialization
	void Start () {
		if (null != ReactorCores)
		{
			foreach(ReactorCore Core in ReactorCores)
			{
				Core.ControlState = ReactorCore.RodStatus.MovingDown;
			}
		}
		
		Preferences PlayerPrefs = new Preferences();
		if (PlayerPrefs.LoadPreferences ())
		{
			if (PlayerPrefs.Volume != -2.0f)
			{
				AudioListener.volume = PlayerPrefs.Volume;
			}
			
			if (!PlayerPrefs.Music)
			{
				if (null != MusicPlayer)
				{
					MusicPlayer.Stop();
				}
			}
		}

		RoundDuration = RoundLength;
		StartCoroutine(RoundTimer());
		StartCoroutine(CalculateScore ());
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!Managing)
		{
			StartCoroutine(ManageReactors());
		}
		
		if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            RaycastHit hit = new RaycastHit();
			foreach (Touch touch in Input.touches)
			{
	            Ray ray = Camera.main.ScreenPointToRay (touch.position);
	            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) 
				{
					if (null != hit.collider)
					{
                   GameObject Core = hit.transform.gameObject;
						ReactorCore core = Core.GetComponent<ReactorCore>();
						if (null == core)
						{
							core = Core.transform.parent.gameObject.GetComponent<ReactorCore>();
						}
						if (null != core)
						{
							Debug.Log ("Touch processed.");
							core.OnMouseUp();
						}	
						break;
					}
	            }
			}
		}
		
		ProcessKeyInput();
	
	}
	
	void OnDestroy()
	{
		if (null != ReactorCores)
		{
			for(int i = ReactorCores.Count - 1; i >= 0; i--)
			{
				ReactorCores.TrimExcess();
				if (null != ReactorCores[i])
				{
					Destroy(ReactorCores[i].gameObject);
				}
			}
		}
	}
	
	void OnGUI()
	{
if (!RoundOver)
		{
			GUI.Box (new Rect((Screen.width/2) - (375/2), 400, 375, 24), "Click on a Reactor core as it rises to prevent a meltdown!");
			if (null != mySkin)
				{
					GUI.skin = mySkin;
				}
			
				GUI.BeginGroup(new Rect((Screen.width/2) - 511, 0, 1020, 102), "ReactorConsole");
				string Reactor1Status = "Offline";
				string myStyle  = "ReactorStatusStablizing";
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
							myStyle = "ReactorStatusStablizing";
							Reactor1Status = "Stablizing";
							break;
						case ReactorCore.RodStatus.Down:
							myStyle = "ReactorStatusNominal";
							Reactor1Status = "Nominal";
							break;
						case ReactorCore.RodStatus.Selected:
							myStyle = "ReactorStatusUnstable";
							Reactor1Status = "Unstable Activity Detected!";
							break;
						default:
							myStyle = "ReactorStatusNominal";
							Reactor1Status = "Nominal";
							break;
					}
				}
			if (null != mySkin)			
				GUI.Box(new Rect(2, 2, 509, 48), "Reactor 1 Status", "ReactorConsole");
			else
				GUI.Box(new Rect(2, 2, 509, 48), "Reactor 1 Status");
			if (null != mySkin)
				GUI.Box(new Rect(4, 24, 505, 24), Reactor1Status, myStyle);
			else
				GUI.Box(new Rect(4, 24, 505, 24), Reactor1Status);
			
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
							myStyle = "ReactorStatusStablizing";
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
						default:
							Reactor2Status = "Nominal";
							break;
					}
				}
			if (null != mySkin)
			{
				GUI.Box(new Rect(511, 2, 509, 48), "Reactor 2 Status", "ReactorConsole");
				GUI.Box (new Rect(513, 24, 505, 24), Reactor2Status, myStyle);
			}
			else
			{
				GUI.Box(new Rect(511, 2, 509, 48), "Reactor 2 Status");
				GUI.Box (new Rect(513, 24, 505, 24), Reactor2Status);
			}
			
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
							myStyle = "ReactorStatusStablizing";
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
						default:
							myStyle = "ReactorStatusNominal";
							Reactor3Status = "Nominal";
							break;
					}
				}		
				
				if (null != mySkin)
				{
					GUI.Box(new Rect(2, 50, 509, 48), "Reactor 3 Status", "ReactorConsole");
					GUI.Box (new Rect(4, 72, 505, 24), Reactor3Status, myStyle);
				}
				else
				{
					GUI.Box(new Rect(2, 50, 509, 48), "Reactor 3 Status");
					GUI.Box (new Rect(4, 72, 505, 24), Reactor3Status);
				}
			
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
							myStyle = "ReactorStatusStablizing";
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
						default:
							myStyle = "ReactorStatusNominal";
							Reactor4Status = "Nominal";
							break;
					}
				}
			if (null != mySkin)
			{
				GUI.Box(new Rect(511, 50, 509, 48), "Reactor 4 Status", "ReactorConsole");
				GUI.Box (new Rect(513, 72, 505, 24), Reactor4Status, myStyle);
			}
			else
			{
				GUI.Box(new Rect(511, 50, 509, 48), "Reactor 4 Status");
				GUI.Box (new Rect(513, 72, 505, 24), Reactor4Status);
			}
			GUI.EndGroup();
			
			if (null != mySkin)
			{
				GUI.Box (new Rect((Screen.width/2) - 100, 100, 200, 48),"Time Till Shutdown", "ReactorConsole");
			}
			else
				GUI.Box (new Rect((Screen.width/2) - 100, 108, 200, 48), "Time Till Shutdown");
			GUI.Box (new Rect((Screen.width/2) - 98, 130, 196, 24), RoundDuration.ToString());
			
			if (null != mySkin)
			{
				GUI.Box (new Rect(2, 300, 180, 48), "Meltdowns Averted", "ReactorConsole");
			}
			else
				GUI.Box (new Rect(2, 300, 180, 48), "Meltdowns Averted");
			
			GUI.Box (new Rect(4, 324, 176, 24), score.ToString ());
			
			if (Combo)
			{
				if (null != mySkin)
				{
					GUI.Box (new Rect(Screen.width - 182, 300, 180, 48), "Bonus Multiplier", "ReactorConsole");
				}
			
				GUI.Box (new Rect(Screen.width - 180, 324, 176, 24), "x" + bonusMultiplier.ToString());
			}
			
			if (paused)
			{
				if (null != mySkin)
				{
					GUI.Box (new Rect((Screen.width/2) - 254, (Screen.height/2) - 24, 510, 48), "Game Paused", "ReactorConsole");
					if (GUI.Button (new Rect((Screen.width/2) - 250, (Screen.height/2) - 4, 505, 25), "Resume Game", "ReactorStatusNominal"))
					{
						paused = false;
						Time.timeScale = 1.0f;
					}
				}
				else
				{
					GUI.Box (new Rect((Screen.width/2) - 254, (Screen.height/2) - 24, 510, 48), "Game Paused");
					if (GUI.Button (new Rect((Screen.width/2) - 250, (Screen.height/2) - 4, 505, 25), "Resume Game"))
					{
						paused = false;
						Time.timeScale = 1.0f;
					}
				}
			}
		}
		else
		{
			if (null != mySkin)
			{
				GUI.skin = mySkin;
				GUI.Box (new Rect(2, 0, 1020, 410), "Game Over", "MenuBox");
				string scoreString = "Your Score: ";
				GUI.BeginGroup(new Rect(7, 20, 1010, 390));
					GUI.Label (new Rect (5, 50, 1010, 32), scoreString + score.ToString ());
					GUI.Label (new Rect(5, 86, 1010, 32), "Enter Your initials:");
					
					PlayerInitials = GUI.TextField (new Rect(5, 122, 50, 20),PlayerInitials, 3);
					ScoreEntry PlayerScore = new ScoreEntry();
					PlayerScore.Initials = PlayerInitials;
					PlayerScore.Score = score;
					if (GUI.Button(new Rect(60, 122, 100, 20), "Submit Score"))
					{
						SubmitScore(PlayerScore);
					}
					GUI.Box (new Rect(0, 146, 1010, 230), "High Scores");
					HighScores CurrentScores = ReturnScoresList();
					if (null != CurrentScores)
					{
						if ((null != CurrentScores.Scores) && (CurrentScores.Scores.Count > 0))
						{
							for (int i = 0; ((i < CurrentScores.Scores.Count) && (i < 10)) ; i++)
							{
								GUI.Label(new Rect(10, 168 + (i * 24), 1000, 20), CurrentScores.Scores[i].Initials + "    " + CurrentScores.Scores[i].Score.ToString ());
							}
						}
					}
				GUI.EndGroup();
				GUI.Box (new Rect((Screen.width/2) - 502, 425, 1004, 48),"", "ReactorConsole");
				if (GUI.Button (new Rect((Screen.width/2) - 500, 430, 1000, 40), "Main Menu", "ReactorStatusVenting"))
				{
					Application.LoadLevel("MainMenu");				}
			}
			else
			{
				GUI.Box (new Rect(2, 0, 1020, 410), "Game Over");
				string scoreString = "Your Score: ";
				GUI.BeginGroup(new Rect(7, 20, 1010, 390));
					GUI.Label (new Rect (5, 50, 1010, 32), scoreString + score.ToString ());
					GUI.Label (new Rect(5, 86, 1010, 32), "Enter Your initials:");
					string PlayerInitials = "TST";
					PlayerInitials = GUI.TextField (new Rect(5, 122, 50, 20),PlayerInitials, 3);
					ScoreEntry PlayerScore = new ScoreEntry();
					PlayerScore.Initials = PlayerInitials;
					PlayerScore.Score = score;
					if (GUI.Button(new Rect(60, 122, 100, 20), "Submit Score"))
					{
						SubmitScore(PlayerScore);
					}
					GUI.Box (new Rect(0, 146, 1010, 230), "High Scores");
					HighScores CurrentScores = ReturnScoresList();
					if (null != CurrentScores)
					{
						if ((null != CurrentScores.Scores) && (CurrentScores.Scores.Count > 0))
						{
							for (int i = 0; ((i < CurrentScores.Scores.Count) && (i < 10)) ; i++)
							{
								GUI.Label(new Rect(10, 168 + (i * 24), 1000, 20), CurrentScores.Scores[i].Initials + "    " + CurrentScores.Scores[i].Score.ToString ());
							}
						}
					}
				GUI.EndGroup();
				GUI.Box (new Rect((Screen.width/2) - 502, 425, 1004, 48), "");
				if (GUI.Button (new Rect((Screen.width/2) - 500, 430, 1000, 40), "Main Menu"))
				{
					Application.LoadLevel("MainMenu");
				}
			}
		}
	}
	
	IEnumerator CalculateScore()
	{
		tempScore = 0;

		foreach (ReactorCore Core in ReactorCores)
		{
			if (null != Core)
			{
				if (Core.LastClickGood)
					bonusMultiplier += 1;
				else
					bonusMultiplier -= 1;
				
				if (bonusMultiplier > 4)
					bonusMultiplier = 4;
				else if (bonusMultiplier <= 1)
					bonusMultiplier = 1;
				
				tempScore += Core.TimesFixed;
				Core.TimesFixed = 0;
			}
		}
		
		if (bonusMultiplier > 1)
			Combo = true;
		else
			Combo = false;
		tempScore *= bonusMultiplier;
		score += tempScore;
		
		yield return new WaitForSeconds(0.3f);
		if (RoundDuration > 0)
		{
			StartCoroutine(CalculateScore());
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
	
	void Volume(bool up)
	{
		float volume = AudioListener.volume;
		if (up)
		{
			volume += 0.05f;
			
			if (volume > 1.0f)
			{
				volume = 1.0f;
			}
		}
		else
		{
			volume -= 0.05f;
			
			if (volume < 0)
			{
				volume = 0;
			}
		}
		
		AudioListener.volume = volume;
	}
	void ProcessKeyInput()
	{
		if ((Input.GetKeyDown(KeyCode.Equals)) || (Input.GetKeyDown(KeyCode.Plus)) || (Input.GetKeyDown(KeyCode.KeypadPlus)))
		{
			Volume (true);
		}
		
		if ((Input.GetKeyDown(KeyCode.Minus)) || (Input.GetKeyDown(KeyCode.KeypadMinus)))
		{
			Volume (false);
		}
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			MusicManager ();
		}
		
				if (Input.GetKeyDown (KeyCode.P))
		{
			if (Time.timeScale == 1.0f)
				Time.timeScale = 0.0f;
			else
				Time.timeScale = 1.0f;
			if (!paused)
				paused = true;
			else
				paused = false;
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
				ScoreEntry PlayerScore = new ScoreEntry();
				PlayerScore.Initials = "TST";
				PlayerScore.Score = score;
				SubmitScore(PlayerScore);
				RoundOver = true;
				Time.timeScale = 0.0f;
			}
		}
	}
	
	public HighScores SubmitScore(ScoreEntry playerScore)
	{
		HighScores TopTenScores = new HighScores();
		TopTenScores.Scores = new List<ScoreEntry>();
		if (TopTenScores.LoadHighScores())
		{
			int lowestScore = 2000000;
			for (int i = 0; i < TopTenScores.Scores.Count; i++)
			{
				if (TopTenScores.Scores[i].Score < lowestScore)
					lowestScore = TopTenScores.Scores[i].Score;
			}
			if (lowestScore == 2000000)
				lowestScore = 0;
			if (playerScore.Score > lowestScore)
			{
				for (int i = 0; i < TopTenScores.Scores.Count; i++)
				{
					if (TopTenScores.Scores[i].Score == lowestScore)
					{
						TopTenScores.Scores.Remove (TopTenScores.Scores[i]);
						break;
					}
				}
				bool found = false;
				for (int i = 0; i < TopTenScores.Scores.Count; i++)
				{
					if (TopTenScores.Scores[i].Score <= score)
					{
						TopTenScores.Scores.Insert(i, playerScore);
						TopTenScores.SaveHighScores();
						found = true;
						break;
					}
				}
				if (!found)
				{
					TopTenScores.Scores.Add (playerScore);
					TopTenScores.SaveHighScores();
				}
			}
         else if (TopTenScores.Scores.Count < 10)
			{
				TopTenScores.Scores.Add (playerScore);
				TopTenScores.SaveHighScores();
			}
		}
		else
		{
			TopTenScores.Scores.Add (playerScore);
			TopTenScores.SaveHighScores();
		}
		return TopTenScores;
	}
	
	public HighScores ReturnScoresList()
	{
		HighScores ScoresList = new HighScores();
		ScoresList.Scores = new List<ScoreEntry>();
		if (ScoresList.LoadHighScores())
		{
			return ScoresList;
		}
		else
			return null;
	}
}
