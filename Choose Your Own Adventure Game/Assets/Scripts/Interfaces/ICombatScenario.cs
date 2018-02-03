/**
 * The back-end of combat.
 */
public interface ICombatScenario
{
    Unit.IInstance[,] Board { get; }

    bool UseInstrument(Unit.IInstance subject, Unit.IInstance target);
}