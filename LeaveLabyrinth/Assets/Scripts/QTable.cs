using System.Collections;
using System.Collections.Generic;
using System;

public class QTable
{
	public Dictionary<State , List<ActionQuality>> m_DataTable{ get; private set; }

	public int m_NumberOfUpdates{ get; private set; }

	public string m_Name { get; set; }

	private Random m_Random;

	public QTable (string name)
	{
		m_DataTable = new Dictionary<State,List<ActionQuality>> ();
		m_Name = name;

		m_Random = new Random ();
	}


	public bool addState (State state)
	{
		try {
			m_DataTable.Add (state, new List<ActionQuality> ());
			return true;
		} catch (ArgumentException) {
			return false;
		}
	}

	public bool addAction (State State, int actionID)
	{
		return addAction (State, actionID, 0);
	}

	public bool addAction (State state, int actionID, float quality)
	{
		List<ActionQuality> aqList;
		bool hasState = m_DataTable.TryGetValue (state, out aqList);
		if (hasState) {
			if (!containsAction (actionID, aqList)) {
				aqList.Add (new ActionQuality (actionID, quality));
			}
		}
		return hasState;
	}

	public bool getActionQuality (State State, int actionID, out float quality)
	{
		ActionQuality aq;
		bool foundAction = findActionQuality (State, actionID, out aq);
		if (foundAction) {
			quality = aq.m_Quality;
		} else {
			quality = 0f;
		}
		return foundAction;
	}

	public bool setActionQuality (State state, int actionID, float quality)
	{
		ActionQuality aq;
		bool foundActionInState = findActionQuality (state, actionID, out aq);
		if (foundActionInState) {
			aq.m_Quality = quality;
		}
		return foundActionInState;
	}

	public bool randomState (out State state)
	{
		state = null;

		int stateCount = m_DataTable.Count;
		if (0 == stateCount) {
			return false; // no states to return
		}
		;
		int randomStateNumber = m_Random.Next (stateCount - 1);

		int i = 0;
		foreach (State s in m_DataTable.Keys) {
			if (i == randomStateNumber) {
				state = s;
				break;
			}
			i++;
		}
		return true; // found a random state
	}

	public bool randomAction (State state, out int actionID)
	{
		actionID = -1; // invalid actionID

		List<ActionQuality> aqList;
		bool hasState = m_DataTable.TryGetValue (state, out aqList);
		if (hasState) {
			int actionCount = aqList.Count;

			if (0 == actionCount) {
				return false; // no actions available
			}

			int randomActionNumber = m_Random.Next (actionCount - 1);

			int i = 0;
			foreach (ActionQuality aq in aqList) {
				if (i == randomActionNumber) {
					actionID = aq.m_ActionID;
					break;
				}
				i++;
			}
			return false; // for some reason we did not find an action in the random range. shouldn't happen...
		}
		return hasState;
	}

	public bool bestAction (State State, out int actionID)
	{
		// TODO

	}


	/** >>> Private convenience-methods <<<  */

	private bool containsAction (int actionID, List<ActionQuality> aqList)
	{
		foreach (ActionQuality aq in aqList) {
			if (actionID == aq.m_ActionID) {
				return true;
			}
		}
		return false;
	}

	private bool findActionQuality (State state, int actionID, out ActionQuality actionQuality)
	{
		List<ActionQuality> aqList;
		bool hasState = m_DataTable.TryGetValue (state, out aqList);
		if (hasState) {
			foreach (ActionQuality aq in aqList) {
				if (actionID == aq.m_ActionID) {
					actionQuality = aq;
					return true; // found action with specified id
				}
			}
		}
		actionQuality = null;
		return hasState; // didn't find state or the specified action
	}
}
