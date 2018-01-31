public interface IUnit
{
    int Alignment { get; }
    float health { get; set; }
    int position { get; set; }

    bool UseInstrument(IUnit target);

    /**
     * Returns an instantiated clone of the current object.
     */ 
    IUnit InstantiateClone();

}