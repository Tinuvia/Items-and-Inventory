namespace Tinuvia.CharacterStats
{ 
    public enum StatModType
    {
        // setting defaults for more flexibility if later more types are added before (or after)
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }

    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Order;
        public readonly object Source; // can hold any type, can be used to tell where the modifier came from (e.g for debug)

        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        // defines other constructors that lets the user only define value and type, 
        // and calls the base constructor to e.g assign order as the int value of the type
        // this works since each enum value automatically is assigned an int value (index)
        // and can be used to sort the modifiers

        public StatModifier(float value, StatModType type) : this(value, type, (int)type, null){ }

        public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }

    }
}
