/**
 * The back-end of combat.
 */
public interface ICombatScenario
{
    IUnit[,] Board { get; }

    bool UseInstrument(IUnit subject, IUnit target);
}