using System;

/**
 * A generic proxy class that does not allow its value to be reassigned once it is not null.
 */
public class Once<T> {

    private T _value;
    public T Value
    {
        get { return _value; }
        set
        {
            if (_value == null) _value = value;
            else throw new ValueAlreadyAssignedException();
        }

    }

    public class ValueAlreadyAssignedException : Exception { }

}
