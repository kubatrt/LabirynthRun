using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Highscores
{

	public static List<ScoreEntry>	highScores = new List<ScoreEntry>();
	private static string 	FILENAME = "highscores.dat";
	private static int 		MAX_ENTRIES = 10;
	
	public static void DebugStartup () 
	{
		if(!LoadHighscores())
		{
			for(int i=0; i<MAX_ENTRIES; i++)
			{
				string newName = "player " + i.ToString();
				int newScore = 100*i;
				highScores.Add(new ScoreEntry { name = newName, score = newScore });
			}
	
			highScores.Sort(new SortScoreAsc());
		}
	}
	
	public static void Add(string playerName, float timeElapsed, int mazeSize)
	{
		if(highScores.Count >= MAX_ENTRIES) {
			highScores.RemoveAt(MAX_ENTRIES-1);
		}
	
		int score = (int)( mazeSize * 100 - timeElapsed );
		
		Debug.Log ("Add score entry: " + playerName + ", " + score);
		highScores.Add(new ScoreEntry { name = playerName, score = score });
		highScores.Sort(new SortScoreAsc());
		SaveHighscores();
	}
	
	private static void SaveHighscores()
	{
		var bf = new BinaryFormatter();
		bf.Binder = new VersionDeserializationBinder();
		var file = File.Create(Application.persistentDataPath + FILENAME);
		bf.Serialize(file, highScores);
        file.Close ();
		Debug.Log ("Highscores saved: " + Application.persistentDataPath + FILENAME);
	}
	
	public static bool LoadHighscores()
	{
		try
		{
	        var bf = new BinaryFormatter();
			bf.Binder = new VersionDeserializationBinder();
	        var file = File.Open(Application.persistentDataPath + "/highscores.dat", FileMode.Open);
	        highScores = (List<ScoreEntry>)bf.Deserialize(file);
	        file.Close();
		}
		catch(FileNotFoundException)
		{
			Debug.Log("No file. New higscore file will be created!");
			return false;
		}
		
		return true;
	}
}
