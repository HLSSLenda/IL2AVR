#if !ARDUINO
namespace Arduino;

public sealed unsafe partial class HardwareSerial
{
    public const byte UBRRL = 0;
    public const byte UCSRB = 0;
    public const byte UCSRA = 0;
    public const byte UDR = 0;
    public const byte UCSRC = 0;
    public const byte UBRRH = 0;
    public const byte URSEL = 0;
    public const byte UCSZ1 = 0;
    public const byte UCSZ0 = 0;
    public const byte TXEN = 0;
    public const byte RXEN = 0;
}
#endif