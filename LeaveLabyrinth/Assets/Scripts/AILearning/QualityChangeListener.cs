using System.Collections;

/// <summary>
/// Implement a function, that's called, if the quality of a state changed
/// </summary>
public interface QualityChangeListener
{
	/// <summary>
	/// Called, if the quality TO the given state is changed!
	/// </summary>
	/// <param name="state">given state</param>
	/// <param name="quality">Changed quality of a action to the given state</param>
	void onQualityToStateChanged (uint state, float quality);
}
