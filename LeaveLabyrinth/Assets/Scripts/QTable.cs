using System.Collections;
using System.Collections.Generic;
using System;

public class QTable
{
	public Dictionary<uint , List<AQTuple>> m_DataTable{ get; private set; }

	public int m_NumberOfUpdates{ get; private set; }

	public string m_Name { get; set; }

	private Random m_Random;

	private const float EPSILON = 0.00001f;

	public QTable (string name)
	{
		m_DataTable = new Dictionary<uint,List<AQTuple>> ();
		m_Name = name;

		m_Random = new Random ();
	}


	public bool addState (uint state)
	{
		try {
			m_DataTable.Add (state, new List<AQTuple> ());
			return true;
		} catch (ArgumentException) {
			return false;
		}
	}

	public bool addAction (uint state, int actionID)
	{
		return addAction (state, actionID, 0);
	}

	public bool addAction (uint state, int actionID, float quality)
	{
		List<AQTuple> aqList;
		bool hasState = m_DataTable.TryGetValue (state, out aqList);
		if (hasState) {
			if (!containsAction (actionID, aqList)) {
				aqList.Add (new AQTuple (actionID, quality));
			}
		}
		return hasState;
	}

	public bool getActionQuality (uint state, int actionID, out float quality)
	{
		AQTuple aq;
		bool foundAction = findActionQuality (state, actionID, out aq);
		if (foundAction) {
			quality = aq.m_Quality;
		} else {
			quality = 0f;
		}
		return foundAction;
	}

	public bool setActionQuality (uint state, int actionID, float quality)
	{
		// add the state, if it doesn't exist yet
		bool hasState = m_DataTable.ContainsKey (state);
		if (!hasState) {
			addState (state);
		}

		AQTuple aq;
		bool ableToSetAQ = findActionQuality (state, actionID, out aq);
		// Action was already added to state
		if (ableToSetAQ) {
			aq.m_Quality = quality;
		} else {
			// try to add action to state
			ableToSetAQ = addAction (state, actionID, quality);
		}
		return ableToSetAQ;
	}

	public bool randomState (out uint state)
	{
		state = 0U;

		int stateCount = m_DataTable.Count;
		if (0 == stateCount) {
			return false; // no states to return
		}
		;
		int randomStateNumber = m_Random.Next (stateCount - 1);

		int i = 0;
		foreach (uint s in m_DataTable.Keys) {
			if (i == randomStateNumber) {
				state = s;
				break;
			}
			i++;
		}
		return true; // found a random state
	}

	public bool randomAction (uint state, out int actionID)
	{
		actionID = -1; // init with invalid actionID

		List<AQTuple> aqList;
		bool hasState = m_DataTable.TryGetValue (state, out aqList);
		if (hasState) {
			int actionCount = aqList.Count;

			if (0 == actionCount) {
				return false; // no actions available
			}

			int randomActionNumber = m_Random.Next (actionCount - 1);

			actionID = aqList [randomActionNumber].m_ActionID;
		}
		return hasState;
	}

	public bool getBestAction (uint state, out int actionID)
	{
		float unusedQuality;
		return getBestAction (state, out actionID, out unusedQuality);
	}

	public bool getBestActionQuality (uint state, out float quality)
	{
		int unusedActionID;
		return getBestAction (state, out unusedActionID, out quality);
	}

	public bool getBestAction (uint state, out int actionID, out float quality)
	{
		// init with invalid values
		actionID = -1;
		quality = -2f;

		List<AQTuple> aqList;
		bool hasState = m_DataTable.TryGetValue (state, out aqList);
		if (hasState) {
			int actionCount = aqList.Count;

			if (0 == actionCount) {
				return false; // no actions available
			}
				
			// Get all the AQ Tuples with the best quality
			List<AQTuple> bestQualities = new List<AQTuple> ();
			bestQualities.Add (aqList [0]);

			foreach (AQTuple aq in aqList) {
				// if we found a better quality, clear the current best qualities and add the new best
				if (aq.m_Quality > bestQualities [0].m_Quality) {
					bestQualities.Clear ();
					bestQualities.Add (aq);
				} 
				// if we found an equal good quality, add it to the list
				else {
					float aqDiff = (aq.m_Quality - bestQualities [0].m_Quality);
					if (aqDiff < EPSILON && aqDiff > -EPSILON) {
						bestQualities.Add (aq);
					}
				}
			}

			// Get a random action out of the best qualities
			int randomBestQualityIndex = m_Random.Next (bestQualities.Count - 1);
			actionID = bestQualities [randomBestQualityIndex].m_ActionID;
			quality = bestQualities [randomBestQualityIndex].m_Quality;
		}
		return hasState;
	}


	/** >>> Private convenience-methods <<<  */

	private bool containsAction (int actionID, List<AQTuple> aqList)
	{
		foreach (AQTuple aq in aqList) {
			if (actionID == aq.m_ActionID) {
				return true;
			}
		}
		return false;
	}

	private bool findActionQuality (uint state, int actionID, out AQTuple actionQuality)
	{
		List<AQTuple> aqList;
		bool hasState = m_DataTable.TryGetValue (state, out aqList);
		if (hasState) {
			foreach (AQTuple aq in aqList) {
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
