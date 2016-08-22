using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
{
	public static void SaveBoard (BoardSave toSave, string boardName)
	{
		string fileName = "/" + boardName + ".bs";
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + fileName);
		bf.Serialize (file, toSave);
		file.Close ();
	}

	public static BoardSave LoadBoard (string boardName)
	{
		string fileName = "/" + boardName + ".bs";
		if (File.Exists (Application.persistentDataPath + fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
			BoardSave toLoad = (BoardSave)bf.Deserialize (file);
			file.Close ();
			return toLoad;
		}
		return null;
	}
}
