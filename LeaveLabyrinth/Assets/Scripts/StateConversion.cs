using System.Collections;
using System;

public class StateConversion
{
	public static uint convertToState (ushort row, ushort column)
	{
		byte[] rowBytes = BitConverter.GetBytes (row);
		byte[] columnBytes = BitConverter.GetBytes (column);

		byte[] stateBytes = new byte[4];
		stateBytes [0] = rowBytes [0];
		stateBytes [1] = rowBytes [1];
		stateBytes [2] = columnBytes [0];
		stateBytes [3] = columnBytes [1];
		uint convertedState = BitConverter.ToUInt32 (stateBytes, 0);

		return convertedState;
	}

	public static void convertFromState (uint state, out ushort row, out ushort column)
	{
		byte[] stateBytes = BitConverter.GetBytes (state);

		row = BitConverter.ToUInt16 (stateBytes, 0);
		column = BitConverter.ToUInt16 (stateBytes, 2);
	}
}
