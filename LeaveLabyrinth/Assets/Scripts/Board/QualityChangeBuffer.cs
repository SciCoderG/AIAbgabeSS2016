using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Buffers quality changes of actions to a certain state. 
/// Changes are stored in a Dictionary, which maps a state to the
/// changed quality of actions to this state.
/// Gets updated by the Learningbehaviour, if a quality change to a state
/// has occured. 
/// Used to display quality to a state on the corresponding fields in the environment.
/// </summary>
public class QualityChangeBuffer : QualityChangeListener
{

	public Dictionary<uint, float> qualityChangeBuffer{ get; set; }

	public QualityChangeBuffer ()
	{
		qualityChangeBuffer = new Dictionary<uint, float> ();
	}

	/// <summary>
	/// Called, if the quality TO the given state is changed!
	/// </summary>
	/// <param name="state">given state</param>
	/// <param name="quality">Changed quality of a action to the given state</param>
	public void onQualityToStateChanged (uint state, float quality)
	{
		// using the index functions as "add or update"
		qualityChangeBuffer [state] = quality;
	}

	public void clearBuffer ()
	{
		qualityChangeBuffer.Clear ();
	}
}
