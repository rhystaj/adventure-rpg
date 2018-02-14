
using System.Collections.Generic;
/**
* The back-end of combat.
*/
public interface ICombatScenario
{
    Unit.IInstance[,] Board { get; }
    int teamMoving { get; }
    HashSet<Unit.IInstance> unitsAvaliableToMove { get; }

    bool UseInstrument(Unit.IInstance subject, Unit.IInstance target);

    void SetOnWinAction(CombatScenario.TeamEvent onWin);

}