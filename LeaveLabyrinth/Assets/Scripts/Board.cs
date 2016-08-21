using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, QActionInterface
{
	private List<Field> m_ExistingFields;

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
		m_Random = new System.Random ();

		m_CurrentField = new Field (0, true);
		m_ExistingFields.Add (m_CurrentField);
	}

	public Field getFieldAtPosition (short x, short z)
	{
		Collider[] hits = Physics.OverlapSphere (new Vector3 (x, 0, z), 0.4f);
		foreach (Collider hit in hits) {
			Field field = hit.gameObject.GetComponent<Field> ();
			if (field) {
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




	/** 			>>> 	QActionInterface - Implementation 	<<< 				*/

	public uint getRandomState ()
	{
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
			if (correspondingField.getNeighbour (action)) {
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
		if (!newField) {
			return false;
		}
			
		reward = newField.m_Reward;
		newState = getStateFromField (newField);

		return true;
	}
}
