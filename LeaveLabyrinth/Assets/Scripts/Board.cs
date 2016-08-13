using System.Collections;

public class Board
{


	private Field[] m_FieldArray;
	private int m_RowCount, m_ColumnCount;

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

	public Board () : this (4, 3)
	{
		
	}

	public Board (int rows, int columns)
	{
		m_RowCount = rows;
		m_ColumnCount = columns;

		m_FieldArray = new Field[rows * columns];

		// Initialize the Fields
		for (int i = 0; i < m_FieldArray.GetLength (0); i++) {
			m_FieldArray [i] = new Field ();
		}
	}

	/// <summary>
	/// Gets the requested field.
	/// </summary>
	/// <returns>The requested field.</returns>
	/// <param name="row">Row.</param>
	/// <param name="column">Column.</param>
	public Field getField (int row, int column)
	{
		int index = row * m_ColumnCount + column;
		return m_FieldArray [index];
	}

	/// <summary>
	/// Take the specified action in the specified state
	/// </summary>
	/// <returns><c>true</c>, if action is possible, <c>false</c> otherwise.</returns>
	/// <param name="stateID">Origin State.</param>
	/// <param name="actionID">Action taken.</param>
	/// <param name="reward">Reference to Feedback for specified action, used to return reward</param>
	/// <param name="newState">Reference to the new state-ID, used to return the new state ID.</param>
	public bool takeAction (int stateID, int actionID, ref int reward, ref int newState)
	{
		// Calculate new state index
		int newStateIndex;
		switch (actionID) {
		case ACTION_ID_GO_UP:
			newStateIndex = stateID + m_ColumnCount;
			break;
		case ACTION_ID_GO_RIGHT:
			{
				// are we on the right-most column?
				if (stateID % m_ColumnCount == m_ColumnCount - 1) {
					return false;
				}
				newStateIndex = stateID + 1;
			}
			break;
		case ACTION_ID_GO_DOWN:
			newStateIndex = stateID - m_ColumnCount;
			break;
		case ACTION_ID_GO_LEFT:
			{
				// are we on the left-most column?
				if (stateID % m_ColumnCount == 0) {
					return false;
				}
				newStateIndex = stateID - 1;
			}
			break;
		default:
			return false;
		}


		// check for array out of bounds
		if (newStateIndex > m_FieldArray.GetLength (0) - 1 || newStateIndex < 0) {
			return false;
		}

		Field newField = m_FieldArray [newStateIndex];
		// check, if the new Field is accessible
		if (!newField.m_Accessible) {
			return false;
		}

		reward = newField.m_Reward;
		newState = newStateIndex;
		return true;
	}
		
}
