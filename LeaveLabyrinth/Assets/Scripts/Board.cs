using System.Collections;

public class Board
{


	private Field[,] m_FieldMatrix;
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

	public Board () : this (3, 4)
	{
		
	}

	public Board (int rows, int columns)
	{
		m_RowCount = rows;
		m_ColumnCount = columns;

		m_FieldMatrix = new Field[rows, columns];

		// Initialize the Field Matrix
		for (int i = 0; i < m_FieldMatrix.GetLength (0); i++) {
			for (int j = 0; i < m_FieldMatrix.GetLength (1); j++) {
				m_FieldMatrix [i, j] = new Field (0f, true);
			}
		}
	}

	public Field getField (int row, int column)
	{
		return m_FieldMatrix [row, column];
	}


	/// <summary>
	/// Validate the action. If valid, returns the reward for the action and the new state we're in
	/// </summary>
	/// <returns><c>true</c>, if action was valid, <c>false</c> otherwise.</returns>
	/// <param name="state">Origin state.</param>
	/// <param name="actionID">Action to be taken on state.</param>
	/// <param name="reward">Reward.</param>
	/// <param name="newState">New state.</param>
	public bool takeAction (State state, int actionID, out float reward, out State newState)
	{
		int newRow = state.m_Row;
		int newColumn = state.m_Column;

		reward = 0f;
		newState = null;

		// switch through actions and check, if we're hitting a field on the board
		switch (actionID) {
		case ACTION_ID_GO_UP:
			{
				newRow++;
				if (newRow >= m_RowCount) {
					return false;
				}
			}
			break;
		case ACTION_ID_GO_RIGHT:
			{
				newColumn++;
				if (newColumn >= m_ColumnCount) {
					return false;
				}
			}
			break;
		case ACTION_ID_GO_DOWN:
			{
				newRow--;
				if (newRow < 0) {
					return false;
				}
			}
			break;
		case ACTION_ID_GO_LEFT:
			{
				newColumn--;
				if (newColumn < 0) {
					return false;
				}
			}
			break;
		default:
			return false;
		}

		Field newField = getField (newRow, newColumn);

		// check, if the new Field is accessible
		if (!newField.m_Accessible) {
			return false;
		}

		// If Action is valid, return reward and new state
		reward = newField.m_Reward;
		newState = new State (newRow, newColumn);

		return true;
	}
		
}
