namespace BearStarsTest.Messages
{
    public class ChangeLayoutMessage 
    {
        public readonly string PerformedAction;
        public readonly string WindowName;
        public readonly int OpenCount;
        public readonly string PreviousWindowName;

        public ChangeLayoutMessage(string performedAction, string windowHeadline,
            int openCount, string previousWindowName)
        {
            PerformedAction = performedAction;
            WindowName = windowHeadline;
            OpenCount = openCount;
            PreviousWindowName = previousWindowName;
        }
    }
}