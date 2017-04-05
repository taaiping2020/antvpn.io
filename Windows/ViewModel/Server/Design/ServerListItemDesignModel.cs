namespace Windows
{
    /// <summary>
    /// The design-time data for a <see cref="ServerListItemViewModel"/>
    /// </summary>
    public class ServerListItemDesignModel : ServerListItemViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static ServerListItemDesignModel Instance => new ServerListItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServerListItemDesignModel()
        {
            Initials = "LM";
            Name = "Luke";
            Message = "This chat app is awesome! I bet it will be fast too";
            ProfilePictureRGB = "3099c5";
            Domain = "hk.antvpn.io";
            this.RedirectorServerCountryName = "China";
            this.TrafficServerCountryName = "United Kingdom";
 
        }

        #endregion
    }
}
