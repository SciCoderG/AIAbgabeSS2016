using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : QActionInterface
{
	public List<Field> m_ExistingFields{ get; set; }

	// ActionIDs
	public const int ACTION_ID_GO_UP = 0;
	public const int ACTION_ID_GO_RIGHT = 1;
	public const int ACTION_ID_GO_DOWN = 2;
	public const int ACTION_ID_GO_LEFT = 3;

	public static readonly int[] AVAILABLE_ACTION_IDS = {
		ACTION_ID_GO_UP,
		ACTION_ID_GO_RIGHT,
		ACTION_ID_GO_DOWN,
		ACTION_ID_GO_LEFT
	};

	private System.Random m_Random;

	// solely used to speed up everything - we can now use the neighbour references instead of having to raycast neighbours
	public Field m_CurrentField{ get; set; }

	public Board ()
	{
		m_ExistingFields = new List<Field> ();
		m_Random = new System.Random ();

		m_CurrentField = GameObject.CreatePrimitive (PrimitiveType.Cube).AddComponent<Field> ();
		Field temp = GameObject.CreatePrimitive (PrimitiveType.Cube).AddComponent<Field> ();
		temp.transform.position = new Vector3 (-1f, 0f, 0f);
		temp.registerNeighbour (m_CurrentField, ACTION_ID_GO_LEFT);
		m_ExistingFields.Add (temp);

		m_CurrentField.registerNeighbour (temp, ACTION_ID_GO_RIGHT);
		m_ExistingFields.Add (m_CurrentField);
	}

	public Field getFieldAtPosition (short x, short z)
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

	public Field getFieldFromState (uint state)
	{
		short posX;
		short posZ;
		StateConversion.convertFromState (state, out posX, out posZ);

		return getFieldAtPosition (posX, posZ);
	}

	public uint getStateFromField (Field field)
	{
		short posX = (short)field.transform.position.x;
		short posZ = (short)field.transform.position.z;

		return StateConversion.convertToState (posX, posZ);
	}

	/** 		>>> 	QActionInterface - Implementation 	<<< 			*/

	public uint getRandomState ()
	{
		if (m_ExistingFields.Count <= 0) {
			return 0;
		}
		int randomIndex = m_Random.Next (m_ExistingFields.Count);

		Field randomField = m_ExistingFields [randomIndex];

		return getStateFromField (randomField);
	}

	public bool getRandomPossibleAction (uint state, out int actionID)
	{
		actionID = -1;

		Field correspondingField = getFieldFromState (state);

		List<int> possibleActions = new List<int> ();
		foreach (int action in AVAILABLE_ACTION_IDS) {
			if (null != correspondingField.getNeighbour (action)) {
				possibleActions.Add (action);
			}
		}

		int possibleActionCount = possibleActions.Count;
		if (0 < possibleActionCount) {
			int randomPossibleActionIndex = m_Random.Next (possibleActionCount);
			actionID = possibleActions [randomPossibleActionIndex];

			return true;
		} 
		return false;
	}

	public bool takeAction (uint state, int actionID, out float reward, out uint newState)
	{
		reward = 0f;
		newState = 0U;

		Field newField = getFieldFromState (state).getNeighbour (actionID);
		if (null == newField) {
			return false;
		}
			
		reward = newField.m_Reward;
		newState = getStateFromField (newField);

		return true;
	}


	/** 	>>> Save and Load implementation <<< 	*/

	public void save (string boardName)
	{
		BoardSave bs = new BoardSave ();
		bs.m_CurrentFieldIndex = m_ExistingFields.IndexOf (m_CurrentField);

		// Save all fields as FieldSave
		foreach (Field field in m_ExistingFields) {
			FieldSave fs = new FieldSave ();
			fs.m_PosX = field.transform.position.x;
			fs.m_PosZ = field.transform.position.z;
			fs.m_Accessible = field.m_Accessible;
			fs.m_Reward = field.m_Reward;
			bs.m_FieldSaves.Add (fs);
		}
		// Save BoardSave as a File
		SaveLoadManager.SaveBoard (bs, boardName);
	}

	public void load (string boardName)
	{
		BoardSave bs = SaveLoadManager.LoadBoard (boardName);

		if (null == bs) {
			Debug.Log ("Board.load(" + boardName + "): Couldn't load BoardSave.");
			return;
		}
		foreach (FieldSave fs in bs.m_FieldSaves) {
			GameObject fieldObject = GameObject.Instantiate (Resources.Load ("Prefabs/FieldPrefab", typeof(GameObject))) as GameObject;
			fieldObject.name = "Field";
			Field field = fieldObject.GetComponent<Field> ();
			field.m_Accessible = fs.m_Accessible;
			field.m_Reward = fs.m_Reward;
			field.gameObject.transform.position = new Vector3 (fs.m_PosX, 0f, fs.m_PosZ);

			// TODO
			// field.findNeighbours();

			m_ExistingFields.Add (field);
		}
		m_CurrentField = m_ExistingFields [bs.m_CurrentFieldIndex];
	}


}
