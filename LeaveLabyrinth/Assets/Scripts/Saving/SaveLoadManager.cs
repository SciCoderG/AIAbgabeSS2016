using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Manages saving and loading of the "board" (which means saving and loading of a Boardsave,
/// which contains all FieldSaves needed. There is no actual "Board" class anymore 
/// </summary>
public static class SaveLoadManager
{
	private static string boardSaveDirectory = "/BoardSaves/";
	private static string boardSaveFileEnding = ".bs";

	public static void saveBoard (BoardSave toSave, string boardName)
	{
		string fileName = boardSaveDirectory + boardName + boardSaveFileEnding;
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + fileName);
		bf.Serialize (file, toSave);
		file.Close ();
	}

	public static BoardSave loadBoard (string boardName)
	{
		string fileName = boardSaveDirectory + boardName + boardSaveFileEnding;
		if (File.Exists (Application.persistentDataPath + fileName)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
			BoardSave toLoad = (BoardSave)bf.Deserialize (file);
			file.Close ();
			return toLoad;
		}
		return null;
	}

	public static string[] getBoardFileNames ()
	{
		checkBoardSaveDirectory ();
		string[] boardFileNames = Directory.GetFiles (Application.persistentDataPath + boardSaveDirectory, "*" + boardSaveFileEnding);
		for (int i = 0; i < boardFileNames.Length; i++) {
			// truncate data path beginning
			int dataPathBeginningLength = (Application.persistentDataPath + boardSaveDirectory).Length;
			boardFileNames [i] = boardFileNames [i].Substring (dataPathBeginningLength);
			// truncate filename ending
			int fileNameLength = boardFileNames [i].Length - boardSaveFileEnding.Length;
			boardFileNames [i] = boardFileNames [i].Substring (0, fileNameLength);
		}
		return boardFileNames;
	}

	/// <summary>
	/// Checks, if the boardsave directory exists, and, if not, creates it.
	/// </summary>
	private static void checkBoardSaveDirectory ()
	{
		if (!Directory.Exists (Application.persistentDataPath + boardSaveDirectory)) {
			Directory.CreateDirectory (Application.persistentDataPath + boardSaveDirectory);
		}
	}
}
