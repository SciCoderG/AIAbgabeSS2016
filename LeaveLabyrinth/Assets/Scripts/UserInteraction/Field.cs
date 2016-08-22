using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Data-class for a Field on a Board.
/// </summary>
public class Field : MonoBehaviour
{
	public float m_Reward{ get; set; }

	public bool m_Accessible{ get; set; }

	public Field[] m_Neighbours{ get; private set; }

	public Field () : this (0, true)
	{
		
	}

	public Field (float reward, bool accessible)
	{
		m_Reward = reward;
		m_Accessible = accessible;

		m_Neighbours = new Field[Board.AVAILABLE_ACTION_IDS.Length];
	}

	public Field getNeighbour (int actionID)
	{
		try {
			return m_Neighbours [actionID];
		} catch (ArgumentOutOfRangeException e) {
			Debug.Log (e.Message);
			return null;
		}
	}

	public bool registerNeighbour (Field newNeighbour, int actionID)
	{
		try {
			m_Neighbours [actionID] = newNeighbour;
		} catch (ArgumentOutOfRangeException e) {
			Debug.Log (e.Message);
			return false;
		}
		return true;
	}

}
