namespace BearStarsTest.Messages
{
    public class ButtonPressedMessage
    {
        public readonly string ButtonLabel;

        public ButtonPressedMessage(string buttonLabel)
        {
            ButtonLabel = buttonLabel;
        }
    }
}