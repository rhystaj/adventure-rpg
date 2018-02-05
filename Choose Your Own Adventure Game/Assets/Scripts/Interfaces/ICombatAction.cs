/**
    * An action performed on the combat scenario.
    */
public interface ICombatAction {

    /**
     * Performt the action.
     */ 
    void Perform(CombatScenario scenario);

    /**
     * Visulise the action with the given animator.
     */ 
    void Animate(ICombatAnimator animator);

}
