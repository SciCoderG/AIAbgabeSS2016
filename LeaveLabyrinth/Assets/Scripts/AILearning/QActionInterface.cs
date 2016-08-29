using System.Collections;

/// <summary>
/// Interface bis functions, the AI needs to call, to be able to
/// learn, without knowing about the specifics of the environment, it is acting in
/// </summary>
public interface QActionInterface
{
	/// <summary>
	/// Get a random possible state in the environment
	/// </summary>
	/// <returns>The random state.</returns>
	uint getRandomState ();

	/// <summary>
	/// Get a random possible action based on the specified state
	/// </summary>
	/// <returns><c>true</c> if a random possible action could be found <c>false</c> otherwise.</returns>
	/// <param name="state">origin-state</param>
	/// <param name="actionID">random action ID</param>
	bool getRandomPossibleAction (uint state, out int actionID);

	/// <summary>
	/// Take the specified action from the specified state. Return the reward and the 
	/// resulting new state by Reference.
	/// </summary>
	/// <returns><c>true</c>if the action was taken successfully <c>false</c> otherwise.</returns>
	/// <param name="state">origin state</param>
	/// <param name="actionID">action to be taken.</param>
	/// <param name="reward">resulting reward</param>
	/// <param name="newState">resulting new state</param>
	bool takeAction (uint state, int actionID, out float reward, out uint newState);

	/// <summary>
	/// Checks if the specified state still exists / is still valid.
	/// </summary>
	/// <returns><c>true</c>, if specified state is valid, <c>false</c> otherwise.</returns>
	/// <param name="state">specified State.</param>
	bool checkIfStateIsValid (uint state);
}
