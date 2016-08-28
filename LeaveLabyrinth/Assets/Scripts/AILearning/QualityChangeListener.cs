using System.Collections;

public interface QualityChangeListener
{
	/// <summary>
	/// Called, if the quality TO the given state is changed!
	/// </summary>
	/// <param name="state">given state</param>
	/// <param name="quality">Changed quality of a action to the given state</param>
	void onQualityToStateChanged (uint state, float quality);
}
