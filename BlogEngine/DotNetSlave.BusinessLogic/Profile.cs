using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Security;
using System.Xml.Serialization;

namespace BlogEngine.Core
{
    /// <summary>
    /// This business object is to handle the profiles of users
    /// </summary>
    [XmlRoot( "profile" )]
    public class Profile
    {
        private string userName;
        private string name;
        private string age;
        private string interests;
        
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("age")]
        public string Age
        {
            get { return age; }
            set { age = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("interests")]
        public string Interests
        {
            get { return interests; }
            set { interests = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("username")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        ///<summary>
        ///</summary>
        ///<param name="username"></param>
        ///<returns></returns>
        public static Profile GetProfile(string username)
        {
            Profile profile = new Profile();
            try
            {
                // Deserialize the specified file to a Theater object.
                XmlSerializer xs = new XmlSerializer(typeof(Profile));
                FileStream fs = new FileStream(BlogSettings.Instance.StorageLocation + username.ToLowerInvariant() + ".xml", FileMode.Open);
                profile = (Profile)xs.Deserialize(fs);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception: " + x.Message);
            }
            return profile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProfile"></param>
        public static void SaveProfile(Profile userProfile)
        {
            try
            {
                // Serialize the Profile object to an XML file.
                XmlSerializer xs = new XmlSerializer(typeof(Profile));
                FileStream fs = new FileStream(BlogSettings.Instance.StorageLocation + userProfile.userName.ToLowerInvariant() + ".xml", FileMode.Create);
                xs.Serialize(fs, userProfile);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception: " + x.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Profile>  GetProfiles()
        {
            List<Profile> profiles = new List<Profile>();
            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                Profile userProfile = GetProfile(user.UserName);
                profiles.Add(userProfile);
            }
            return profiles;
        }


    }


}
