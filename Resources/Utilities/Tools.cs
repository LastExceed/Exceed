namespace Resources
{
    public class Tools
    {
        public static bool GetBit(int value, int bitNumber)
        {
            return (value & (1 << bitNumber)) != 0;
            //intValue |= 1 << bitPosition;
            //intValue &= ~(1 << bitPosition);
        }
    }
}