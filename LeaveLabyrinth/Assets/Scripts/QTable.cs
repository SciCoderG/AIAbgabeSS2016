using System.Collections;
using System.Collections.Generic;
using System;


public class QTable
{
	public Dictionary<State , List<ActionQuality>> m_DataTable{ get; private set; }

	public int m_NumberOfUpdates{ get; private set; }

	public string m_Name { get; set; }

	public QTable (string name)
	{
		m_DataTable = new Dictionary<State,List<ActionQuality>> ();
		m_Name = name;
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

	public bool addAction (State state, int actionID, int quality)
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

	private bool containsAction (int actionID, List<ActionQuality> aqList)
	{
		foreach (ActionQuality aq in aqList) {
			if (actionID == aq.m_ActionID) {
				return true;
			}
		}
		return false;
	}
}
