using System.Collections;

public interface QActionInterface
{
	uint getRandomState ();

	bool getRandomPossibleAction (uint state, out int actionID);

	bool takeAction (uint state, int actionID, out float reward, out uint newState);

	bool checkIfStateIsValid (uint state);
}
