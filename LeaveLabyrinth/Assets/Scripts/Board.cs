using System.Collections;
using System;
using System.Collections.Generic;

public class Board
{


	private Field[,] m_FieldMatrix;
	private ushort m_RowCount, m_ColumnCount;

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

	private Random m_Random;

	public Board () : this (3, 4)
	{
		m_FieldMatrix [0, 3].m_Reward = -1f;
		m_FieldMatrix [1, 1].m_Accessible = false;
		m_FieldMatrix [2, 3].m_Reward = 1f;
	}

	public Board (ushort rows, ushort columns)
	{
		m_Random = new Random ();

		m_RowCount = rows;
		m_ColumnCount = columns;

		m_FieldMatrix = new Field[rows, columns];

		// Initialize the Field Matrix
		for (ushort i = 0; i < m_FieldMatrix.GetLength (0); i++) {
			for (ushort j = 0; j < m_FieldMatrix.GetLength (1); j++) {
				m_FieldMatrix [i, j] = new Field (0f, true);
			}
		}


	}

	public Field getField (ushort row, ushort column)
	{
		return m_FieldMatrix [row, column];
	}

	public uint getRandomBoardState ()
	{
		ushort randomRow = (ushort)m_Random.Next (m_RowCount);
		ushort randomColumn = (ushort)m_Random.Next (m_ColumnCount);

		return StateConversion.convertToState (randomRow, randomColumn);
	}

	public bool getRandomPossibleBoardAction (uint state, out int actionID)
	{
		actionID = -1;

		ushort row;
		ushort column;
		StateConversion.convertFromState (state, out row, out column);

		List<int> possibleActions = new List<int> ();
		foreach (int action in AVAILABLE_ACTION_IDS) {
			// check if action is possible
			if (tryAction (state, action, out row, out column)) {
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

	/// <summary>
	/// Validate the action. If valid, returns the reward for the action and the new state we're in
	/// </summary>
	/// <returns><c>true</c>, if action was valid, <c>false</c> otherwise.</returns>
	/// <param name="state">Origin state.</param>
	/// <param name="actionID">Action to be taken on state.</param>
	/// <param name="reward">Reward.</param>
	/// <param name="newState">New state.</param>
	public bool takeAction (uint state, int actionID, out float reward, out uint newState)
	{
		ushort newRow;
		ushort newColumn;

		reward = 0f;
		newState = 0U;

		// switch through actions and check, if we're hitting a field on the board
		bool actionWasValid = tryAction (state, actionID, out newRow, out newColumn);
		if (!actionWasValid) {
			return false;
		}

		Field newField = getField (newRow, newColumn);

		// check, if the new Field is accessible
		if (!newField.m_Accessible) {
			return false;
		}

		// If Action is valid, return reward and new state
		reward = newField.m_Reward;
		newState = StateConversion.convertToState (newRow, newColumn);

		return true;
	}

	private bool tryAction (uint state, int actionID, out ushort row, out ushort column)
	{
		ushort tryRow;
		ushort tryColumn;
		StateConversion.convertFromState (state, out tryRow, out tryColumn);

		row = 0;
		column = 0;

		switch (actionID) {
		case ACTION_ID_GO_UP:
			{
				tryRow++;
				if (tryRow >= m_RowCount) {
					return false;
				}
			}
			break;
		case ACTION_ID_GO_RIGHT:
			{
				tryColumn++;
				if (tryColumn >= m_ColumnCount) {
					return false;
				}
			}
			break;
		case ACTION_ID_GO_DOWN:
			{
				if (tryRow == 0) {
					return false;
				} else {
					tryRow--;
				}
			}
			break;
		case ACTION_ID_GO_LEFT:
			{
				if (tryColumn == 0) {
					return false;
				} else {
					tryColumn--;
				}
			}
			break;
		default:
			return false;
		}

		// if action was ok, set our values to the new values
		row = tryRow;
		column = tryColumn;

		return true;
	}
}
