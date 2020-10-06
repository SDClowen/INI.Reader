using System.Data;

namespace Core.IO
{
    /// <summary>
    ///   Definition of the <see cref="Profile.Changing" /> event handler. </summary>
    /// <remarks>
    ///   This definition complies with the .NET Framework's standard for event handlers.
    ///   The sender is always set to the Profile object that raised the event. </remarks>
    public delegate void ProfileChangingHandler(object sender, ProfileChangingArgs e);

    /// <summary>
    ///   Definition of the <see cref="Profile.Changed" /> event handler. </summary>
    /// <remarks>
    ///   This definition complies with the .NET Framework's standard for event handlers.
    ///   The sender is always set to the Profile object that raised the event. </remarks>
    public delegate void ProfileChangedHandler(object sender, ProfileChangedArgs e);

    /// <summary>
    ///   Interface implemented by all profile classes in this namespace.
    ///   It represents a normal profile. </summary>
    /// <remarks>
    ///   This interface takes the members of IReadOnlyProfile (its base interface) and adds
    ///   to it the rest of the members, which allow modifications to the profile.  
    ///   Altogether, this represents a complete profile object. </remarks>
    /// <seealso cref="IReadOnlyProfile" />
    /// <seealso cref="Profile" />

    public interface IProfile : IReadOnlyProfile
    {
        /// <summary>
        ///   Gets or sets the name associated with the profile. </summary>
        /// <remarks>
        ///   This should be the name of the file where the data is stored, or something equivalent.
        ///   When setting this property, the <see cref="ReadOnly" /> property should be checked and if true, an InvalidOperationException should be raised.
        ///   The <see cref="Changing" /> and <see cref="Changed" /> events should be raised before and after this property is changed. </remarks>
        /// <seealso cref="DefaultName" />
        new string Name
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the name associated with the profile by default. </summary>
        /// <remarks>
        ///   This is used to set the default Name of the profile and it is typically based on 
        ///   the name of the executable plus some extension. </remarks>
        /// <seealso cref="Name" />
        string DefaultName
        {
            get;
        }

        /// <summary>
        ///   Gets or sets whether the profile is read-only or not. </summary>
        /// <remarks>
        ///   A read-only profile should not allow any operations that alter sections,
        ///   entries, or values, such as <see cref="SetValue" /> or <see cref="RemoveEntry" />.  
        ///   Once a profile has been marked read-only, it should be allowed to go back; 
        ///   attempting to do so should cause an InvalidOperationException to be raised.
        ///   The <see cref="Changing" /> and <see cref="Changed" /> events should be raised before 
        ///   and after this property is changed. </remarks>
        /// <seealso cref="CloneReadOnly" />
        /// <seealso cref="IReadOnlyProfile" />
        bool ReadOnly
        {
            get;
            set;
        }

        /// <summary>
        ///   Sets the value for an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry where the value will be set. </param>
        /// <param name="value">
        ///   The value to set. If it's null, the entry should be removed. </param>
        /// <remarks>
        ///   This method should check the <see cref="ReadOnly" /> property and throw an InvalidOperationException if it's true.
        ///   It should also raise the <see cref="Changing" /> and <see cref="Changed" /> events before and after the value is set. </remarks>
        /// <seealso cref="IReadOnlyProfile.GetValue" />
        void SetValue(string section, string entry, object value);

        /// <summary>
        ///   Removes an entry from a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to remove. </param>
        /// <remarks>
        ///   This method should check the <see cref="ReadOnly" /> property and throw an InvalidOperationException if it's true.
        ///   It should also raise the <see cref="Changing" /> and <see cref="Changed" /> events before and after the entry is removed. </remarks>
        /// <seealso cref="RemoveSection" />
        void RemoveEntry(string section, string entry);

        /// <summary>
        ///   Removes a section. </summary>
        /// <param name="section">
        ///   The name of the section to remove. </param>
        /// <remarks>
        ///   This method should check the <see cref="ReadOnly" /> property and throw an InvalidOperationException if it's true.
        ///   It should also raise the <see cref="Changing" /> and <see cref="Changed" /> events before and after the section is removed. </remarks>
        /// <seealso cref="RemoveEntry" />
        void RemoveSection(string section);

        /// <summary>
        ///   Writes the data of every table from a DataSet into this profile. </summary>
        /// <param name="ds">
        ///   The DataSet object containing the data to be set. </param>
        /// <remarks>
        ///   Each table in the DataSet should be used to represent a section of the profile.  
        ///   Each column of each table should represent an entry.  And for each column, the corresponding value
        ///   of the first row is the value that should be passed to <see cref="SetValue" />.  
        ///   <para>
        ///   This method serves as a convenient way to take any data inside a generic DataSet and 
        ///   write it to any of the available profiles. </para></remarks>
        /// <seealso cref="IReadOnlyProfile.GetDataSet" />
        void SetDataSet(DataSet ds);

        /// <summary>
        ///   Creates a copy of itself and makes it read-only. </summary>
        /// <returns>
        ///   The return value should be a copy of itself as an IReadOnlyProfile object. </returns>
        /// <remarks>
        ///   This method is meant as a convenient way to pass a read-only copy of the profile to methods 
        ///   that are not allowed to modify it. </remarks>
        /// <seealso cref="ReadOnly" />
        IReadOnlyProfile CloneReadOnly();

        /// <summary>
        ///   Event that should be raised just before the profile is to be changed to allow the change to be canceled. </summary>
        /// <seealso cref="Changed" />
        event ProfileChangingHandler Changing;

        /// <summary>
        ///   Event that should be raised right after the profile has been changed. </summary>
        /// <seealso cref="Changing" />
        event ProfileChangedHandler Changed;
    }
}
