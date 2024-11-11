namespace BearStarsTest.Messages
{
    public class CloseWindowMessage
    {
        public readonly string PerformedAction;
        public readonly string WindowName;

        public CloseWindowMessage(string performedAction, string windowName)
        {
            PerformedAction = performedAction;
            WindowName = windowName;
        }
    }
}