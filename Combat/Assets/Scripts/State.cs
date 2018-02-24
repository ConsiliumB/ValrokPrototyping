public abstract class State
{

    //Reference to parent entity
    protected StatefulEntity Entity { get; private set; }

    //Private empty constructor to avoid a State being initialized without a parent entity
    private State() { }

    //Save a reference to the parent entity
    public State(StatefulEntity entity)
    {
        Entity = entity;
    }

    //This method be called when transitioning into this state,
    //and will contain any one-time set-up and initialization needed for this state.
    public virtual void PrepareState() { }

    //This method will execute this states behaviour.
    //Should be called on every update in parent entity
    public abstract void Execute();

    //This method be called when transitioning out of this state,
    //and will contain any cleanup or last actions needed to finish this state
    public virtual void FinishState() { }
}