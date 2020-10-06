using System;

namespace Core.IO
{
    public interface IReadOnlyProfile : ICloneable
    {
        /// <summary>
        ///   Gets the name associated with the profile. </summary>
        /// <remarks>
        ///   This should be the name of the file where the data is stored, or something equivalent. </remarks>
        string Name
        {
            get;
        }

        /// <summary>
        ///   Retrieves the value of an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <returns>
        ///   The return value should be the entry's value, or null if the entry does not exist. </returns>
        /// <seealso cref="HasEntry" />
        object GetValue(string section, string entry);

        /// <summary>
        ///   Determines if an entry exists inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to be checked for existence. </param>
        /// <returns>
        ///   If the entry exists inside the section, the return value should be true; otherwise false. </returns>
        /// <seealso cref="HasSection" />
        /// <seealso cref="GetEntryNames" />
        bool HasEntry(string section, string entry);

        /// <summary>
        ///   Determines if a section exists. </summary>
        /// <param name="section">
        ///   The name of the section to be checked for existence. </param>
        /// <returns>
        ///   If the section exists, the return value should be true; otherwise false. </returns>
        /// <seealso cref="HasEntry" />
        /// <seealso cref="GetSectionNames" />
        bool HasSection(string section);

        /// <summary>
        ///   Retrieves the names of all the entries inside a section. </summary>
        /// <param name="section">
        ///   The name of the section holding the entries. </param>
        /// <returns>
        ///   If the section exists, the return value should be an array with the names of its entries; 
        ///   otherwise it should be null. </returns>
        /// <seealso cref="HasEntry" />
        /// <seealso cref="GetSectionNames" />
        string[] GetEntryNames(string section);

        /// <summary>
        ///   Retrieves the names of all the sections. </summary>
        /// <returns>
        ///   The return value should be an array with the names of all the sections. </returns>
        /// <seealso cref="HasSection" />
        /// <seealso cref="GetEntryNames" />
        string[] GetSectionNames();

        /// <summary>
        ///   Retrieves a DataSet object containing every section, entry, and value in the profile. </summary>
        /// <returns>
        ///   If the profile exists, the return value should be a DataSet object representing the profile; otherwise it's null. </returns>
        /// <remarks>
        ///   The returned DataSet should be named using the <see cref="Name" /> property.  
        ///   It should contain one table for each section, and each entry should be represented by a column inside the table.
        ///   Each table should contain only one row where the values will be stored corresponding to each column (entry). 
        ///   <para>
        ///   This method serves as a convenient way to extract the profile's data to this generic medium known as the DataSet.  
        ///   This allows it to be moved to many different places, including a different type of profile object 
        ///   (eg., INI to XML conversion). </para>
        /// </remarks>
        System.Data.DataSet GetDataSet();
    }
}
