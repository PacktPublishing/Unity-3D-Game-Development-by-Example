using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrontEndManager : MonoBehaviour {
	public GUISkin mySkin;
	bool ShowMenu = false;
	bool waiting = false;
	bool ShowingScores = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		if(!ShowingScores)
		{
		if (null != mySkin)
		{
			GUI.skin = mySkin;	
			if (!ShowMenu)
			{
				GUI.Box (new Rect(0, 0, Screen.width, Screen.height), "", "SplashPage");
				if (!waiting)
				{
					StartCoroutine(MenuDelay(2.0f));
				}
			}
			if (ShowMenu)
			{
				// This is where the GUI for the menu would be drawn.
				GUI.Box (new Rect(0, 0, 1024, 410), "Meltdown Madness!", "MenuBox");
				if (GUI.Button(new Rect((Screen.width/2) - (510/2), 184, 510, 51), "Play Meltdown Madness", "ReactorStatusNominal"))
					Application.LoadLevel("ReactorRoom");
				
				if (GUI.Button (new Rect((Screen.width/2) - (510/2), 250, 510, 51), "High Scores", "ReactorStatusRodFailure"))
					ShowingScores = true;
					
					if (GUI.Button (new Rect((Screen.width/2) - (510/2), 320, 510, 51), "Quit Meltdown Madness", "ReactorStatusVenting"))
					Application.Quit();
			}
		}
		}
			else
{
			if (null != mySkin)
			{
				GUI.skin = mySkin;
				GUI.Box (new Rect(2, 0, 1020, 410), "Meltdown Madness!", "MenuBox");
				
				GUI.BeginGroup(new Rect(7, 20, 1010, 390));
					
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
					ShowingScores = false;
				}
			}
			else
			{
				GUI.Box (new Rect(2, 0, 1020, 410), "Meltdown Madness!");
				
				GUI.BeginGroup(new Rect(7, 20, 1010, 390));
					
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
					ShowingScores = false;
				}
			}
		}
		
	}
	
	IEnumerator MenuDelay(float seconds)
	{
		waiting = true;
		yield return new WaitForSeconds(seconds);
		ShowMenu = true;
		waiting = false;
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
