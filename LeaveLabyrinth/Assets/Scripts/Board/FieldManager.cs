using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class FieldManager
{
	public static List<Field> existingFields = new List<Field> ();

	// ActionIDs
	public const int ACTION_ID_GO_FORWARD = 0;
	public const int ACTION_ID_GO_RIGHT = 1;
	public const int ACTION_ID_GO_BACKWARD = 2;
	public const int ACTION_ID_GO_LEFT = 3;

	public static readonly int[] AVAILABLE_ACTION_IDS = {
		ACTION_ID_GO_FORWARD,
		ACTION_ID_GO_RIGHT,
		ACTION_ID_GO_BACKWARD,
		ACTION_ID_GO_LEFT
	};

	public static readonly Vector3[] AVAILABLE_ACTION_DIRECTIONS = {
		new Vector3 (0f, 0f, 1f), // forward
		new Vector3 (1f, 0f, 0f), // right
		new Vector3 (0f, 0f, -1f), // backward
		new Vector3 (-1f, 0f, 0f) // left
	};

	public static SaveBoardUI saveBoardUI{ get; set; }

	public static AIController aiController{ get; set; }

	public static Field getFieldAtPosition (short x, short z)
	{
		Collider[] hits = Physics.OverlapSphere (new Vector3 (x, 0, z), 0.4f);
		foreach (Collider hit in hits) {
			Field field = hit.gameObject.GetComponent<Field> ();
			if (null != field) {
				return field;
			}
		}
		return null;
	}

	public static Field getFieldFromState (uint state)
	{
		short posX;
		short posZ;
		StateConversion.convertFromState (state, out posX, out posZ);

		return getFieldAtPosition (posX, posZ);
	}

	public static uint getStateFromField (Field field)
	{
		short posX = (short)field.transform.position.x;
		short posZ = (short)field.transform.position.z;

		return StateConversion.convertToState (posX, posZ);
	}

	public static Field[] findNeighbours (Field field)
	{
		Field[] neighbours = new Field[4];
		RaycastHit hit;
		// dis some crazy shit. we're just going through all actions, getting corresponding directions,
		// raycasting in that direction and if we're hitting a field --> putting it into the return array
		foreach (int actionID in AVAILABLE_ACTION_IDS) {
			if (Physics.Raycast (field.transform.position, AVAILABLE_ACTION_DIRECTIONS [actionID], out hit, 1f)) {
				Field fieldHit = hit.collider.gameObject.GetComponent<Field> ();
				if (null != fieldHit) {
					neighbours [actionID] = fieldHit;
				}
			} 
		}
		return neighbours;
	}

	/** 	>>> Save and Load implementation <<< 	*/

	public static void save ()
	{
		String fileName = saveBoardUI.saveFileInput.text;
		if (0 == fileName.Length) {
			Debug.Log ("Invalid Savefile name.");
			fileName = "default";
		}
		save (fileName);
	}

	public static void save (string boardName)
	{
		BoardSave bs = new BoardSave ();

		// Save all fields as FieldSave
		foreach (Field field in existingFields) {
			if (null == field) {
				continue; // this happens sometimes. Perhabs Garbage Collector is too slow?
			}
			FieldSave fs = new FieldSave ();
			fs.m_PosX = field.transform.position.x;
			fs.m_PosZ = field.transform.position.z;
			fs.m_Accessible = field.M_IsAccessible;
			fs.m_Reward = field.M_Reward;
			bs.m_FieldSaves.Add (fs);
		}
		// Save BoardSave as a File
		SaveLoadManager.saveBoard (bs, boardName);

		// update saveBoardUI
		saveBoardUI.updateBoardLoadOptions ();
	}

	public static void load ()
	{
		load (saveBoardUI.loadFileDropdown.captionText.text);
	}

	public static void load (string boardName)
	{
		aiController.reset ();

		foreach (Field field in existingFields) {
			if (null == field) {
				continue;
			}
			GameObject.Destroy (field.gameObject);
		}
		existingFields.Clear ();

		BoardSave bs = SaveLoadManager.loadBoard (boardName);

		if (null == bs) {
			Debug.Log ("Board.load(" + boardName + "): Couldn't load BoardSave.");
			return;
		}
		foreach (FieldSave fs in bs.m_FieldSaves) {
			
			Field field = FieldModifier.createField (fs.m_PosX, fs.m_PosZ, fs.m_Accessible, fs.m_Reward);

			existingFields.Add (field);
		}
	}
}
