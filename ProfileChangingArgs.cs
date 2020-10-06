namespace Core.IO
{
    /// <summary>
    ///   EventArgs class to be passed as the second parameter of a <see cref="Profile.Changing" /> event handler. </summary>
    /// <remarks>
    ///   This class provides all the information relevant to the change about to be made to the Profile.
    ///   Besides the properties of ProfileChangedArgs, it adds the Cancel property which allows the 
    ///   event handler to prevent the change from happening. </remarks>
    /// <seealso cref="ProfileChangedArgs" />
    public class ProfileChangingArgs : ProfileChangedArgs
    {

        /// <summary>
        ///   Initializes a new instance of the ProfileChangingArgs class by initializing all of its properties. </summary>
        /// <param name="changeType">
        ///   The type of change to be made to the profile. </param>
        /// <param name="section">
        ///   The name of the section involved in the change or null. </param>
        /// <param name="entry">
        ///   The name of the entry involved in the change, or if changeType is set to Other, the name of the method/property that was changed. </param>
        /// <param name="value">
        ///   The new value for the entry or method/property, based on the value of changeType. </param>
        /// <seealso cref="ProfileChangeType" />
        public ProfileChangingArgs(ProfileChangeType changeType, string section, string entry, object value) :
            base(changeType, section, entry, value)
        {
        }

        /// <summary>
        ///   Gets or sets whether the change about to the made should be canceled or not. </summary>
        /// <remarks> 
        ///   By default this property is set to false, meaning that the change is allowed.  </remarks>
        public bool Cancel { get; set; }
    }
}
