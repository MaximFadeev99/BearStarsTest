namespace BearStarsTest.Messages
{
    public class WindowPressedMessage
    {
        public readonly string PressedWindowName;

        public WindowPressedMessage(string pressedWindowName)
        {
            PressedWindowName = pressedWindowName;
        }
    }
}