using System.Collections;
using System;

public class StateConversion
{
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

	public static void convertFromState (uint state, out short posX, out short posZ)
	{
		byte[] stateBytes = BitConverter.GetBytes (state);

		posX = BitConverter.ToInt16 (stateBytes, 0);
		posZ = BitConverter.ToInt16 (stateBytes, 2);
	}
}
