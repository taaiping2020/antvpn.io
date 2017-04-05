using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Windows
{
    /// <summary>
    /// A view model for each chat list item in the overview chat list
    /// </summary>
    public class ServerListItemViewModel : BaseViewModel
    {
        public ServerViewModel ServerViewModel { get; set; }
        /// <summary>
        /// The latest message from this chat
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The initials to show for the profile picture background
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// The RGB values (in hex) for the background color of the profile picture
        /// For example FF00FF for Red and Blue mixed
        /// </summary>
        public string ProfilePictureRGB { get; set; }

        /// <summary>
        /// True if there are unread messages in this chat 
        /// </summary>
        public bool NewContentAvailable { get; set; }

        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string IPv4 { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }

        private string countryName;

        public string CountryName
        {
            get
            {
                if (this.countryName == null)
                {
                    return $"{this.RedirectorServerCountryName} / {this.TrafficServerCountryName}";
                }
                return countryName;
            }
            set { countryName = value; }
        }

        //public string CountryName { get; set; }
        public string CountryFlag { get; set; }
        public string RedirectorServerCountryName { get; set; }

        private string redirectorServerCountryFlag;

        public string RedirectorServerCountryFlag
        {
            get
            {
                if (this.CountryFlag != null)
                {
                    return this.CountryFlag;
                }
                return redirectorServerCountryFlag;
            }
            set { redirectorServerCountryFlag = value; }
        }


        //public string RedirectorServerCountryFlag { get { return  }; set; }
        public string TrafficServerCountryName { get; set; }
        public string TrafficServerCountryFlag { get; set; }
        public bool IsPublic { get; set; }
        public bool IsHybrid { get; set; }
        public bool Off { get; set; }
        public int[] Protocals { get; set; }
        public IEnumerable<string> GetProtocalNames()
        {
            if (this.Protocals == null || this.Protocals.Any())
            {
                yield break;
            }
            foreach (var p in this.Protocals)
            {
                yield return Enum.GetName(typeof(Protocal), (Protocal)p);
            }
        }
        public string ProtocalsDisplay => ToString(GetProtocalNames(), ',');

        private static string ToString(IEnumerable<string> stringArray, char spliter)
        {
            if (stringArray == null || stringArray.Any())
            {
                return null;
            }
            StringBuilder b = new StringBuilder();
            foreach (var item in stringArray)
            {
                b.Append(spliter.ToString()).Append(item);
            }
            return b.ToString().Remove(0, 1);
        }
        public enum Protocal
        {
            SSTP = 1,
            PPTP = 2,
            IKEv2 = 3,
            L2TP = 4
        }
    }
}
