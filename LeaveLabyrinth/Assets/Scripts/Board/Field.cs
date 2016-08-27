﻿using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Data-class for a Field on a Board.
/// </summary>
public class Field : MonoBehaviour
{
	public float m_Reward{ get; set; }

	public bool m_IsAccessible{ get; set; }

	public Field[] m_Neighbours{ get; set; }

	[SerializeField]
	private Color m_MarkedForDeleteColor = new Color (1f, 0f, 0f);

	[SerializeField]
	private Color m_InitialColor = new Color (1f, 1f, 1f);

	public Field () : this (0, true)
	{
		
	}

	public Field (float reward, bool accessible)
	{
		m_Reward = reward;
		m_IsAccessible = accessible;

		m_Neighbours = new Field[FieldManager.AVAILABLE_ACTION_IDS.Length];
	}

	public Field getNeighbour (int actionID)
	{
		try {
			return m_Neighbours [actionID];
		} catch (IndexOutOfRangeException e) {
			Debug.Log (e.Message);
			return null;
		}
	}

	public bool registerNeighbour (Field newNeighbour, int actionID)
	{
		try {
			// register neighbour
			m_Neighbours [actionID] = newNeighbour;
		} catch (ArgumentOutOfRangeException e) {
			Debug.Log (e.Message);
			return false;
		}

		return true;
	}

	public void OnMarkForDelete ()
	{
		GetComponent<Renderer> ().material.color = m_MarkedForDeleteColor;
	}

	public void OnUnmarkForDelete ()
	{
		GetComponent<Renderer> ().material.color = m_InitialColor;
	}

	void OnMouseUpAsButton ()
	{
		FieldModifier.onClickField (this);
	}

	void OnDestroy ()
	{
		m_Neighbours = null;
	}


		
}