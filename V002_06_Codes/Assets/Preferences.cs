using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
public class Preferences
{
	public float Volume = -2.0f;
	public bool Music = true;
	
	public bool LoadPreferences()
    {
		
        bool loaded = false;
        string PreferencesFile = Application.persistentDataPath + "/" + "MeltdownMadness.plr";
		if (File.Exists(PreferencesFile))
		{
        	FileStream loadingPrefs = new FileStream(PreferencesFile, FileMode.Open);
        	XmlSerializer serializer = new XmlSerializer(typeof(Preferences));
        	object obj = serializer.Deserialize(loadingPrefs);
        	loadingPrefs.Close();

        	if (obj != null)
        	{
	            Preferences LoadedPrefs = obj as Preferences;

            	Volume = LoadedPrefs.Volume;
            	Music = LoadedPrefs.Music;

            	loaded = true;
        	}
		}
		
		return loaded;
    }

    public bool SavePreferences()
    {
        bool saved = false;
        string PreferencesFile = Application.persistentDataPath + "/" + "MeltdownMadness.plr";

        FileStream stream = new FileStream(PreferencesFile, FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(Preferences));
        if (Volume != -2.0f)
        {
            serializer.Serialize(stream, this);
        }
        else
        {
            Preferences empty = new Preferences();
            empty.Volume = 1.0f;
            empty.Music = true;
            serializer.Serialize(stream, empty);
        }
        stream.Close();
        saved = true;

        return saved;
    }

}

