using System.Collections.Generic;

namespace Windows
{
    /// <summary>
    /// A view model for the overview chat list
    /// </summary>
    public class ServerListViewModel : BaseViewModel
    {
        /// <summary>
        /// The chat list items for the list
        /// </summary>
        public List<ServerListItemViewModel> Items { get; set; }
    }
}
