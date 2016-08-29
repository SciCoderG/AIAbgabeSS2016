using System.Collections;
using System;

/// <summary>
/// Converts information from the environment into a state used by the AI and vice versa
/// </summary>
public class StateConversion
{
	/// <summary>
	/// Converts the position variables into a state
	/// </summary>
	/// <returns>The corresponding state</returns>
	/// <param name="posX">Position x.</param>
	/// <param name="posZ">Position z.</param>
	public static uint convertToState (short posX, short posZ)
	{
		byte[] rowBytes = BitConverter.GetBytes (posX);
		byte[] columnBytes = BitConverter.GetBytes (posZ);

		byte[] stateBytes = new byte[4];
		stateBytes [0] = rowBytes [0];
		stateBytes [1] = rowBytes [1];
		stateBytes [2] = columnBytes [0];
		stateBytes [3] = columnBytes [1];
		uint convertedState = BitConverter.ToUInt32 (stateBytes, 0);

		return convertedState;
	}

	/// <summary>
	/// Converts the state into the position variables and returns them as references
	/// </summary>
	/// <param name="state">State.</param>
	/// <param name="posX">Position x.</param>
	/// <param name="posZ">Position z.</param>
	public static void convertFromState (uint state, out short posX, out short posZ)
	{
		byte[] stateBytes = BitConverter.GetBytes (state);

		posX = BitConverter.ToInt16 (stateBytes, 0);
		posZ = BitConverter.ToInt16 (stateBytes, 2);
	}
}
