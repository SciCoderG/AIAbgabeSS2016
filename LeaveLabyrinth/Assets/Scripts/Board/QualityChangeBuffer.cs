using System.Collections;
using System.Collections.Generic;

public class QualityChangeBuffer : QualityChangeListener
{

	public Dictionary<uint, float> qualityChangeBuffer{ get; set; }

	public QualityChangeBuffer ()
	{
		qualityChangeBuffer = new Dictionary<uint, float> ();
	}

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
