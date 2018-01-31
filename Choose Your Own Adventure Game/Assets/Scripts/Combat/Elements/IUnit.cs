public interface IUnit
{
    int Alignment { get; }
    float health { get; set; }

    bool UseInstrument(Unit target);
}