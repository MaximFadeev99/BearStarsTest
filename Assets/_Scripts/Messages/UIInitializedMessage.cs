using System.Collections.Generic;

namespace BearStarsTest.Messages
{
    public class UIInitializedMessage
    {
        public readonly IReadOnlyList<string> CreatedWindows;

        public UIInitializedMessage(IReadOnlyList<string> createdWindows)
        {
            CreatedWindows = createdWindows;
        }
    }
}
