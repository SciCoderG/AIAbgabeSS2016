using System.Collections;

public interface ActionTakenListener
{
	void onActionTaken (uint fromState, uint toState, int action, float quality);
}
