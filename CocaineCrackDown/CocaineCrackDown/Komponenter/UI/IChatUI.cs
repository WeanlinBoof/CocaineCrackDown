using System;

namespace CocaineCrackDown.Komponenter.UI {
    public interface IChatUI : ISubUI
    {
        void SetChatText(string text);
        event EventHandler<string> OnChatSubmitted;
    }
}