
using System.Collections;
/**
* An action performed on the combat scenario.
*/
public interface ICombatAction {

    /**
     * Performt the action.
     */ 
    void Perform(ICombatScenario scenario);

    /**
     * Visulise the action with the given animator.
     */ 
    IEnumerator Animate(ICombatAnimator animator);

}
