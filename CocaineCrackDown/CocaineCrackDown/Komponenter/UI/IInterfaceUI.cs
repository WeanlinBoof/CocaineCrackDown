using System;

namespace CocaineCrackDown.Komponenter.UI {
    public interface IInventoryUI
    {
        event EventHandler<object> OnItemAdded;
        event EventHandler<object> OnItemRemoved;
    }
}