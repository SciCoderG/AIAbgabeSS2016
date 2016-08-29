using System.Collections;
using System;

public class LearningBehaviour
{
	public float m_LearningRate{ get; set; }

	public float m_DiscountRate{ get; set; }

	public float m_RandomAction{ get; set; }

	public float m_RandomState{ get; set; }

	public QTable m_QTable{ get; set; }

	public QActionInterface m_QActionInterface{ get; set; }

	public QualityChangeListener m_QualityChangeListener{ get; set; }

	private Random m_Random;

	public uint m_CurrentState{ get; set; }

	//
	private static readonly int MAX_ACTIONSEARCH_TRYS = 8;

	public LearningBehaviour (QActionInterface qActionInterface, QualityChangeListener qChangeListener)
	{
		m_QActionInterface = qActionInterface;
		m_QualityChangeListener = qChangeListener;
	}

	public void init ()
	{
		m_QTable = new QTable ("first");
		m_CurrentState = m_QActionInterface.getRandomState ();

		m_LearningRate = 0.6f;
		m_DiscountRate = 0.5f;

		m_RandomAction = 1f;
		m_RandomState = 0f;

		m_Random = new Random ();
	}

	public void iterate (int numberOfIterations)
	{
		for (int i = 0; i < numberOfIterations; i++) {
			iterate ();
		}
	}

	public void iterate ()
	{
		int currentAction;

		double doRandomState = m_Random.NextDouble ();
		// continue with a random state, if we randomly choose to,
		// or if the current state is invalid, for example after loading a other board
		if (doRandomState < m_RandomState || !m_QActionInterface.checkIfStateIsValid (m_CurrentState)) {
			m_CurrentState = m_QActionInterface.getRandomState (); // start again from a random state
		}


		bool actionSuccessfull;

		float reward;
		uint newState;

		int numFindActionTries = 0;
		do {
			double doRandomAction = m_Random.NextDouble ();
			if (doRandomAction < m_RandomAction) {
				m_QActionInterface.getRandomPossibleAction (m_CurrentState, out currentAction); // take a random action
			} else {
				m_QTable.getBestAction (m_CurrentState, out currentAction); // take the best action
			}
			// determine the new state and reward from the board
			actionSuccessfull = m_QActionInterface.takeAction (m_CurrentState, currentAction, out reward, out newState);
			numFindActionTries++;
		} while(!actionSuccessfull && numFindActionTries < MAX_ACTIONSEARCH_TRYS);

		// Couldn't find a action, that could be done
		if (!actionSuccessfull) {
			return;
		}

		// determine the quality of the current state from the table
		float quality;
		m_QTable.getActionQuality (m_CurrentState, currentAction, out quality);

		// determine the best quality of the next state
		float maxQuality;
		m_QTable.getBestActionQuality (newState, out maxQuality);

		// calculate the new quality-value
		quality = (1 - m_LearningRate) * quality + m_LearningRate * (reward + m_DiscountRate * maxQuality);
			
		// store the new quality value;
		m_QTable.setActionQuality (m_CurrentState, currentAction, quality);

		// broadcast the new quality value
		m_QualityChangeListener.onQualityToStateChanged (newState, quality);

		// set the new state
		m_CurrentState = newState;
	}

	public void reset ()
	{
		m_QTable.reset ();
	}
}
