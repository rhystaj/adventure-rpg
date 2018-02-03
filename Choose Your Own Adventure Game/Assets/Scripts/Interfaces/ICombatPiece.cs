public interface ICombatPiece
{
    int alignment { get; }
    float health { get; set; }
    int position { get; set; }

    bool UseInstrument(ICombatPiece target);

}