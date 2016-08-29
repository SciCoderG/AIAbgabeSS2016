using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// UI class managing the Inputs and Outputs corresponding to the Save-and-Load-Board-UI
/// </summary>
public class SaveBoardUI : MonoBehaviour
{
	public InputField saveFileInput;

	public Dropdown loadFileDropdown;

	public SaveBoardUI ()
	{
		FieldManager.saveBoardUI = this;
	}

	void Start ()
	{
		updateBoardLoadOptions ();
	}

	public void updateBoardLoadOptions ()
	{
		loadFileDropdown.ClearOptions ();
		List<string> boardLoadOptionList = new List<string> (SaveLoadManager.getBoardFileNames ());
		loadFileDropdown.AddOptions (boardLoadOptionList);
	}
}
