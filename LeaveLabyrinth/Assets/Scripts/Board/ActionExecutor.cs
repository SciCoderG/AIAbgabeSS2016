using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionExecutor : QActionInterface
{
	private System.Random m_Random = new System.Random ();

	public uint getRandomState ()
	{
		if (FieldManager.existingFields.Count <= 0) {
			return 0;
		}
		int randomIndex = m_Random.Next (FieldManager.existingFields.Count);

		Field randomField = FieldManager.existingFields [randomIndex];

		return FieldManager.getStateFromField (randomField);
	}

	public bool getRandomPossibleAction (uint state, out int actionID)
	{
		actionID = -1;

		Field correspondingField = FieldManager.getFieldFromState (state);
		if (null == correspondingField) {
			return false;
		}

		List<int> possibleActions = new List<int> ();
		foreach (int action in FieldManager.AVAILABLE_ACTION_IDS) {
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

		Field newField = FieldManager.getFieldFromState (state).getNeighbour (actionID);
		if (null == newField) {
			return false;
		}

		reward = newField.m_Reward;
		newState = FieldManager.getStateFromField (newField);

		return true;
	}

}
