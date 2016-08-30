using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Implements the QActionInterface. Connects the Learningbehaviour to the environment.
/// </summary>
public class ActionExecutor : QActionInterface
{
	private System.Random m_Random = new System.Random ();

	/// <summary>
	/// Checks if the specified state still exists / is still valid.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="state">specified State.</param>
	public bool checkIfStateIsValid (uint state)
	{
		return (null != FieldManager.getFieldFromState (state));
	}

	/// <summary>
	/// Get a random possible state in the environment
	/// </summary>
	/// <returns>The random state.</returns>
	public uint getRandomState ()
	{
		if (FieldManager.existingFields.Count <= 0) {
			return 0;
		}
		int randomIndex = m_Random.Next (FieldManager.existingFields.Count);

		Field randomField = FieldManager.existingFields [randomIndex];

		return FieldManager.getStateFromField (randomField);
	}

	/// <summary>
	/// Get a random possible action based on the specified state
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="state">origin-state</param>
	/// <param name="actionID">random action ID</param>
	public bool getRandomPossibleAction (uint state, out int actionID)
	{
		actionID = -1;

		Field correspondingField = FieldManager.getFieldFromState (state);
		if (null == correspondingField) {
			return false;
		}

		List<int> possibleActions = new List<int> ();
		foreach (int action in FieldManager.AVAILABLE_ACTION_IDS) {
			Field neighbour = correspondingField.getNeighbour (action);
			if (null != neighbour && neighbour.M_IsAccessible) {
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
	/// Take the specified action from the specified state. Return the reward and the 
	/// resulting new state by Reference.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="state">origin state</param>
	/// <param name="actionID">action to be taken.</param>
	/// <param name="reward">resulting reward</param>
	/// <param name="newState">resulting new state</param>
	public bool takeAction (uint state, int actionID, out float reward, out uint newState)
	{
		reward = 0f;
		newState = 0U;

		Field origin = FieldManager.getFieldFromState (state);
		if (null == origin) {
			return false;
		}
		Field newField = origin.getNeighbour (actionID);
		if (null == newField) {
			return false;
		}

		reward = newField.M_Reward;
		newState = FieldManager.getStateFromField (newField);

		return true;
	}

}
