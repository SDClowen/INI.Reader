﻿using System;

namespace Core.IO
{
    /// <summary>
    ///   EventArgs class to be passed as the second parameter of a <see cref="Profile.Changed" /> event handler. </summary>
    /// <remarks>
    ///   This class provides all the information relevant to the change made to the Profile.
    ///   It is also used as a convenient base class for the ProfileChangingArgs class which is passed 
    ///   as the second parameter of the <see cref="Profile.Changing" /> event handler. </remarks>
    /// <seealso cref="ProfileChangingArgs" />
    public class ProfileChangedArgs : EventArgs
    {

        /// <summary>
        ///   Initializes a new instance of the ProfileChangedArgs class by initializing all of its properties. </summary>
        /// <param name="changeType">
        ///   The type of change made to the profile. </param>
        /// <param name="section">
        ///   The name of the section involved in the change or null. </param>
        /// <param name="entry">
        ///   The name of the entry involved in the change, or if changeType is set to Other, the name of the method/property that was changed. </param>
        /// <param name="value">
        ///   The new value for the entry or method/property, based on the value of changeType. </param>
        /// <seealso cref="ProfileChangeType" />
        public ProfileChangedArgs(ProfileChangeType changeType, string section, string entry, object value)
        {
            ChangeType = changeType;
            Section = section;
            Entry = entry;
            Value = value;
        }

        /// <summary>
        ///   Gets the type of change that raised the event. </summary>
        public ProfileChangeType ChangeType { get; }

        /// <summary>
        ///   Gets the name of the section involved in the change, or null if not applicable. </summary>
        public string Section { get; }

        /// <summary>
        ///   Gets the name of the entry involved in the change, or null if not applicable. </summary>
        /// <remarks> 
        ///   If <see cref="ChangeType" /> is set to Other, this property holds the name of the 
        ///   method/property that was changed. </remarks>
        public string Entry { get; }

        /// <summary>
        ///   Gets the new value for the entry or method/property, based on the value of <see cref="ChangeType" />. </summary>
        public object Value { get; }
    }
}
