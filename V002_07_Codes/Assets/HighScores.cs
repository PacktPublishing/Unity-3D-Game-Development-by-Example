using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class HighScores {
	public List<ScoreEntry> Scores;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
		public bool LoadHighScores()
    {
		
        bool loaded = false;
        string HighScoresFile = Application.persistentDataPath + "/" + "MeltdownMadness.scr";
		if (File.Exists(HighScoresFile))
		{
        	FileStream LoadedScores = new FileStream(HighScoresFile, FileMode.Open);
        	XmlSerializer serializer = new XmlSerializer(typeof(HighScores));
        	object obj = serializer.Deserialize(LoadedScores);
        	LoadedScores.Close();

        	if (obj != null)
        	{
	            HighScores LoadedHighScores = obj as HighScores;
				
				if (null != LoadedHighScores.Scores)
					Scores = LoadedHighScores.Scores;
				else
					Scores = new List<ScoreEntry>();
						
				
				loaded = true;
        	}
		}
		
		return loaded;
    }

    public bool SaveHighScores()
    {
        bool saved = false;
        string HighScoresFile = Application.persistentDataPath + "/" + "MeltdownMadness.scr";

        FileStream stream = new FileStream(HighScoresFile, FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(HighScores));
        if (null != Scores)
        {
            serializer.Serialize(stream, this);
        }
        else
        {
            HighScores empty = new HighScores();
            empty.Scores = new List<ScoreEntry>();
            serializer.Serialize(stream, empty);
        }
        stream.Close();
        saved = true;

        return saved;
    }
}

[Serializable]
public struct ScoreEntry
{
	public string Initials;
	public int Score;
}
