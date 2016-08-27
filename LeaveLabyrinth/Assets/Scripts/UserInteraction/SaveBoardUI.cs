using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
