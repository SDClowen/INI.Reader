﻿using System;
using System.Data;

namespace Core.IO
{
    public abstract class Profile : IProfile
    {
        // Fields
        private string m_name;
        private bool m_readOnly;

        /// <summary>
        ///   Event used to notify that the profile is about to be changed. </summary>
        /// <seealso cref="Changed" />
        public event ProfileChangingHandler Changing;

        /// <summary>
        ///   Event used to notify that the profile has been changed. </summary>
        /// <seealso cref="Changing" />
        public event ProfileChangedHandler Changed;

        /// <summary>
        ///   Initializes a new instance of the Profile class by setting the <see cref="Name" /> to <see cref="DefaultName" />. </summary>
        protected Profile()
        {
            m_name = DefaultName;
        }

        /// <summary>
        ///   Initializes a new instance of the Profile class by setting the <see cref="Name" /> to a value. </summary>
        /// <param name="name">
        ///   The name to initialize the <see cref="Name" /> property with. </param>
        protected Profile(string name)
        {
            m_name = name;
        }

        /// <summary>
        ///   Initializes a new instance of the Profile class based on another Profile object. </summary>
        /// <param name="profile">
        ///   The Profile object whose properties and events are used to initialize the object being constructed. </param>
        protected Profile(Profile profil)
        {
            m_name = profil.m_name;
            m_readOnly = profil.m_readOnly;
            Changing = profil.Changing;
            Changed = profil.Changed;
        }

        /// <summary>
        ///   Gets or sets the name associated with the profile. </summary>
        /// <exception cref="NullReferenceException">
        ///   Setting this property to null. </exception>
        /// <exception cref="InvalidOperationException">
        ///   Setting this property if ReadOnly is true. </exception>
        /// <remarks>
        ///   This is usually the name of the file where the data is stored. 
        ///   The <see cref="Changing" /> event is raised before changing this property.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this property 
        ///   returns immediately without being changed.  After the property is changed, 
        ///   the <see cref="Changed" /> event is raised. </remarks>
        /// <seealso cref="DefaultName" />
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                VerifyNotReadOnly();
                if (m_name == value.Trim())
                    return;

                if (!RaiseChangeEvent(true, ProfileChangeType.Name, null, null, value))
                    return;

                m_name = value.Trim();
                RaiseChangeEvent(false, ProfileChangeType.Name, null, null, value);
            }
        }

        /// <summary>
        ///   Gets or sets whether the profile is read-only or not. </summary>
        /// <exception cref="InvalidOperationException">
        ///   Setting this property if it's already true. </exception>
        /// <remarks>
        ///   A read-only profile does not allow any operations that alter sections,
        ///   entries, or values, such as <see cref="SetValue" /> or <see cref="RemoveEntry" />.  
        ///   Once a profile has been marked read-only, it may no longer go back; 
        ///   attempting to do so causes an InvalidOperationException to be raised.
        ///   The <see cref="Changing" /> event is raised before changing this property.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this property 
        ///   returns immediately without being changed.  After the property is changed, 
        ///   the <see cref="Changed" /> event is raised. </remarks>
        /// <seealso cref="CloneReadOnly" />
        /// <seealso cref="IReadOnlyProfile" />
        public bool ReadOnly
        {
            get
            {
                return m_readOnly;
            }

            set
            {
                VerifyNotReadOnly();
                if (m_readOnly == value)
                    return;

                if (!RaiseChangeEvent(true, ProfileChangeType.ReadOnly, null, null, value))
                    return;

                m_readOnly = value;
                RaiseChangeEvent(false, ProfileChangeType.ReadOnly, null, null, value);
            }
        }

        /// <summary>
        ///   Gets the name associated with the profile by default. </summary>
        /// <remarks>
        ///   This property needs to be implemented by derived classes.  
        ///   See <see cref="IProfile.DefaultName">IProfile.DefaultName</see> for additional remarks. </remarks>
        /// <seealso cref="Name" />
        public abstract string DefaultName
        {
            get;
        }

        /// <summary>
        ///   Retrieves a copy of itself. </summary>
        /// <returns>
        ///   The return value is a copy of itself as an object. </returns>
        /// <remarks>
        ///   This method needs to be implemented by derived classes. </remarks>
        /// <seealso cref="CloneReadOnly" />
        public abstract object Clone();

        /// <summary>
        ///   Sets the value for an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry where the value will be set. </param>
        /// <param name="value">
        ///   The value to set. If it's null, the entry should be removed. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true or
        ///   <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise.
        ///   See <see cref="IProfile.SetValue">IProfile.SetValue</see> for additional remarks. </remarks>
        /// <seealso cref="GetValue" />
        public abstract void SetValue(string section, string entry, object value);

        /// <summary>
        ///   Retrieves the value of an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <returns>
        ///   The return value is the entry's value, or null if the entry does not exist. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise. </remarks>
        /// <seealso cref="SetValue" />
        /// <seealso cref="HasEntry" />
        public abstract object GetValue(string section, string entry);
        /// <summary>
        ///   Retrieves the integer value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <returns>
        ///   The return value is the entry's value converted to an integer.  If the value
        ///   cannot be converted, the return value is 0.  If the entry does not exist, the
        ///   given default value is returned. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method calls <c>GetValue<typeparam name="T"></typeparam>(section, entry)</c> of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </remarks>
        /// <seealso cref="SetValue" />
        /// <seealso cref="HasEntry" />
        public virtual T GetValue<T>(string section, string entry)
        {
            object value = GetValue(section, entry);
            T result = default(T);

            if (typeof(T) == typeof(bool))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(ushort))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(short))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(uint))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(int))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(byte))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(sbyte))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(long))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(ulong))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(float))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            if (typeof(T) == typeof(double))
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            return result;
        }

        /// <summary>
        ///   Determines if an entry exists inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to be checked for existence. </param>
        /// <returns>
        ///   If the entry exists inside the section, the return value is true; otherwise false. </returns>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method calls GetEntryNames of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </remarks>
        /// <seealso cref="HasSection" />
        /// <seealso cref="GetEntryNames" />
        public virtual bool HasEntry(string section, string entry)
        {
            string[] entries = GetEntryNames(section);

            if (entries == null)
                return false;

            VerifyAndAdjustEntry(ref entry);
            return Array.IndexOf(entries, entry) >= 0;
        }

        /// <summary>
        ///   Determines if a section exists. </summary>
        /// <param name="section">
        ///   The name of the section to be checked for existence. </param>
        /// <returns>
        ///   If the section exists, the return value is true; otherwise false. </returns>
        /// <seealso cref="HasEntry" />
        /// <seealso cref="GetSectionNames" />
        public virtual bool HasSection(string section)
        {
            string[] sections = GetSectionNames();

            if (sections == null)
                return false;

            VerifyAndAdjustSection(ref section);
            return Array.IndexOf(sections, section) >= 0;
        }

        /// <summary>
        ///   Removes an entry from a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to remove. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise.
        ///   See <see cref="IProfile.RemoveEntry">IProfile.RemoveEntry</see> for additional remarks. </remarks>
        /// <seealso cref="RemoveSection" />
        public abstract void RemoveEntry(string section, string entry);

        /// <summary>
        ///   Removes a section. </summary>
        /// <param name="section">
        ///   The name of the section to remove. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise.
        ///   See <see cref="IProfile.RemoveSection">IProfile.RemoveSection</see> for additional remarks. </remarks>
        /// <seealso cref="RemoveEntry" />
        public abstract void RemoveSection(string section);

        /// <summary>
        ///   Retrieves the names of all the entries inside a section. </summary>
        /// <param name="section">
        ///   The name of the section holding the entries. </param>
        /// <returns>
        ///   If the section exists, the return value should be an array with the names of its entries; 
        ///   otherwise null. </returns>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise. </remarks>
        /// <seealso cref="HasEntry" />
        /// <seealso cref="GetSectionNames" />
        public abstract string[] GetEntryNames(string section);

        /// <summary>
        ///   Retrieves the names of all the sections. </summary>
        /// <returns>
        ///   The return value should be an array with the names of all the sections. </returns>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what exceptions derived versions may raise. </remarks>
        /// <seealso cref="HasSection" />
        /// <seealso cref="GetEntryNames" />
        public abstract string[] GetSectionNames();

        /// <summary>
        ///   Retrieves a copy of itself and makes it read-only. </summary>
        /// <returns>
        ///   The return value is a copy of itself as a IReadOnlyProfile object. </returns>
        /// <remarks>
        ///   This method serves as a convenient way to pass a read-only copy of the profile to methods 
        ///   that are not allowed to modify it. </remarks>
        /// <seealso cref="ReadOnly" />
        public virtual IReadOnlyProfile CloneReadOnly()
        {
            Profile profile = (Profile)Clone();
            profile.m_readOnly = true;

            return profile;
        }

        /// <summary>
        ///   Retrieves a DataSet object containing every section, entry, and value in the profile. </summary>
        /// <returns>
        ///   If the profile exists, the return value is a DataSet object representing the profile; otherwise it's null. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <remarks>
        ///   The returned DataSet will be named using the <see cref="Name" /> property.  
        ///   It will contain one table for each section, and each entry will be represented by a column inside the table.
        ///   Each table will contain only one row where the values will stored corresponding to each column (entry). 
        ///   <para>
        ///   This method serves as a convenient way to extract the profile's data to this generic medium known as the DataSet.  
        ///   This allows it to be moved to many different places, including a different type of profile object 
        ///   (eg., INI to XML conversion). </para>
        ///   <para>
        ///   This method calls GetSectionNames, GetEntryNames, and GetValue of the derived class, so check the 
        ///   documentation to see what other exceptions may be raised. </para></remarks>
        /// <seealso cref="SetDataSet" />
        public virtual DataSet GetDataSet()
        {
            VerifyName();

            string[] sections = GetSectionNames();
            if (sections == null)
                return null;

            DataSet ds = new DataSet(Name);

            // Add a table for each section
            foreach (string section in sections)
            {
                DataTable table = ds.Tables.Add(section);

                // Retrieve the column names and values
                string[] entries = GetEntryNames(section);
                DataColumn[] columns = new DataColumn[entries.Length];
                object[] values = new object[entries.Length];

                int i = 0;
                foreach (string entry in entries)
                {
                    object value = GetValue(section, entry);

                    columns[i] = new DataColumn(entry, value.GetType());
                    values[i++] = value;
                }

                // Add the columns and values to the table
                table.Columns.AddRange(columns);
                table.Rows.Add(values);
            }

            return ds;
        }

        /// <summary>
        ///   Writes the data of every table from a DataSet into this profile. </summary>
        /// <param name="ds">
        ///   The DataSet object containing the data to be set. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true or
        ///   <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   ds is null. </exception>
        /// <remarks>
        ///   Each table in the DataSet represents a section of the profile.  
        ///   Each column of each table represents an entry.  And for each column, the corresponding value
        ///   of the first row is the value to be passed to <see cref="SetValue" />.  
        ///   Note that only the first row is imported; additional rows are ignored.
        ///   <para>
        ///   This method serves as a convenient way to take any data inside a generic DataSet and 
        ///   write it to any of the available profiles. </para>
        ///   <para>
        ///   This method calls SetValue of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </para></remarks>
        /// <seealso cref="GetDataSet" />
        public virtual void SetDataSet(DataSet ds)
        {
            if (ds == null)
                throw new ArgumentNullException("ds");

            // Create a section for each table
            foreach (DataTable table in ds.Tables)
            {
                string section = table.TableName;
                DataRowCollection rows = table.Rows;
                if (rows.Count == 0)
                    continue;

                // Loop through each column and add it as entry with value of the first row				
                foreach (DataColumn column in table.Columns)
                {
                    string entry = column.ColumnName;
                    object value = rows[0][column];

                    SetValue(section, entry, value);
                }
            }
        }

        /// <summary>
        ///   Gets the name of the file to be used as the default, without the profile-specific extension. </summary>
        /// <remarks>
        ///   This property is used by file-based Profile implementations 
        ///   when composing the DefaultName.  These implementations take the value returned by this
        ///   property and add their own specific extension (.ini, .xml, .config, etc.).
        ///   <para>
        ///   For Windows applications, this property returns the full path of the executable.  
        ///   For Web applications, this returns the full path of the web.config file without 
        ///   the .config extension.  </para></remarks>
        /// <seealso cref="DefaultName" />
        protected string DefaultNameWithoutExtension => "profile";

        /// <summary>
        ///   Verifies the given section name is not null and trims it. </summary>
        /// <param name="section">
        ///   The section name to verify and adjust. </param>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method may be used by derived classes to make sure that a valid
        ///   section name has been passed, and to make any necessary adjustments to it
        ///   before passing it to the corresponding APIs. </remarks>
        /// <seealso cref="VerifyAndAdjustEntry" />
        protected virtual void VerifyAndAdjustSection(ref string section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            section = section.Trim();
        }

        /// <summary>
        ///   Verifies the given entry name is not null and trims it. </summary>
        /// <param name="entry">
        ///   The entry name to verify and adjust. </param>
        /// <remarks>
        ///   This method may be used by derived classes to make sure that a valid
        ///   entry name has been passed, and to make any necessary adjustments to it
        ///   before passing it to the corresponding APIs. </remarks>
        /// <exception cref="ArgumentNullException">
        ///   entry is null. </exception>
        /// <seealso cref="VerifyAndAdjustSection" />
        protected virtual void VerifyAndAdjustEntry(ref string entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            entry = entry.Trim();
        }

        /// <summary>
        ///   Verifies the Name property is not empty or null. </summary>
        /// <remarks>
        ///   This method may be used by derived classes to make sure that the 
        ///   APIs are working with a valid Name (file name) </remarks>
        /// <exception cref="InvalidOperationException">
        ///   name is empty or null. </exception>
        /// <seealso cref="Name" />
        protected internal virtual void VerifyName()
        {
            if (m_name == null || m_name == "")
                throw new InvalidOperationException("Operation not allowed because Name property is null or empty.");
        }

        /// <summary>
        ///   Verifies the ReadOnly property is not true. </summary>
        /// <remarks>
        ///   This method may be used by derived classes as a convenient way to 
        ///   validate that modifications to the profile can be made. </remarks>
        /// <exception cref="InvalidOperationException">
        ///   ReadOnly is true. </exception>
        /// <seealso cref="ReadOnly" />
        protected internal virtual void VerifyNotReadOnly()
        {
            if (m_readOnly)
                throw new InvalidOperationException("Operation not allowed because ReadOnly property is true.");
        }

        /// <summary>
        ///   Raises either the Changing or Changed event. </summary>
        /// <param name="changing">
        ///   If true, the Changing event is raised otherwise it's Changed. </param>
        /// <param name="changeType">
        ///   The type of change being made. </param>
        /// <param name="section">
        ///   The name of the section that was involved in the change or null if not applicable. </param>
        /// <param name="entry">
        ///   The name of the entry that was involved in the change or null if not applicable. 
        ///   If changeType is equal to Other, entry is the name of the property involved in the change.</param>
        /// <param name="value">
        ///   The value that was changed or null if not applicable. </param>
        /// <returns>
        ///   The return value is based on the event raised.  If the Changing event was raised, 
        ///   the return value is the opposite of ProfileChangingArgs.Cancel; otherwise it's true.</returns>
        /// <remarks>
        ///   This method may be used by derived classes as a convenient alternative to calling 
        ///   OnChanging and OnChanged.  For example, a typical call to OnChanging would require
        ///   four lines of code, which this method reduces to two. </remarks>
        /// <seealso cref="Changing" />
        /// <seealso cref="Changed" />
        /// <seealso cref="OnChanging" />
        /// <seealso cref="OnChanged" />
        protected bool RaiseChangeEvent(bool changing, ProfileChangeType changeType, string section, string entry, object value)
        {
            if (changing)
            {
                // Don't even bother if there are no handlers.
                if (Changing == null)
                    return true;

                ProfileChangingArgs e = new ProfileChangingArgs(changeType, section, entry, value);
                OnChanging(e);
                return !e.Cancel;
            }

            // Don't even bother if there are no handlers.
            if (Changed != null)
                OnChanged(new ProfileChangedArgs(changeType, section, entry, value));
            return true;
        }

        /// <summary>
        ///   Raises the Changing event. </summary>
        /// <param name="e">
        ///   The arguments object associated with the Changing event. </param>
        /// <remarks>
        ///   This method should be invoked prior to making a change to the profile so that the
        ///   Changing event is raised, giving a chance to the handlers to prevent the change from
        ///   happening (by setting e.Cancel to true). This method calls each individual handler 
        ///   associated with the Changing event and checks the resulting e.Cancel flag.  
        ///   If it's true, it stops and does not call of any remaining handlers since the change 
        ///   needs to be prevented anyway. </remarks>
        /// <seealso cref="Changing" />
        /// <seealso cref="OnChanged" />
        protected virtual void OnChanging(ProfileChangingArgs e)
        {
            if (Changing == null)
                return;

            foreach (ProfileChangingHandler handler in Changing.GetInvocationList())
            {
                handler(this, e);

                // If a particular handler cancels the event, stop
                if (e.Cancel)
                    break;
            }
        }

        /// <summary>
        ///   Raises the Changed event. </summary>
        /// <param name="e">
        ///   The arguments object associated with the Changed event. </param>
        /// <remarks>
        ///   This method should be invoked after a change to the profile has been made so that the
        ///   Changed event is raised, giving a chance to the handlers to be notified of the change. </remarks>
        /// <seealso cref="Changed" />
        /// <seealso cref="OnChanging" />
        protected virtual void OnChanged(ProfileChangedArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }


    }
}
