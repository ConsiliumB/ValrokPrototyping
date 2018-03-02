public abstract class State
{
    //This method be called when transitioning into this state,
    //and will contain any one-time set-up and initialization needed for this state.
    public virtual void PrepareState() { }

    //This method will execute this states behaviour.
    //Should be called on every update in parent entity
    public virtual void Execute() { }

    //This method be called when transitioning out of this state,
    //and will contain any cleanup or last actions needed to finish this state
    public virtual void FinishState() { }
}