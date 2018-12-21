
using System.Collections;
/**
* An action performed on the combat scenario.
*/
public interface ICombatAction {

    /**
     * Performt the action.
     */ 
    IEnumerator Perform(CombatAnimator animator, ICombatScenario scenario);

}
