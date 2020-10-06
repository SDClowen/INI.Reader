namespace Core.IO
{
	public enum ProfileChangeType
	{
		/// <summary> 
		///   The change refers to the <see cref="Profile.Name" /> property. </summary>		
		/// <remarks> 
		///   <see cref="ProfileChangedArgs.Value" /> will contain the new name. </remarks>
		Name,

		/// <summary> 
		///   The change refers to the <see cref="Profile.ReadOnly" /> property. </summary>		
		/// <remarks> 
		///   <see cref="ProfileChangedArgs.Value" /> will be true. </remarks>
		ReadOnly,

		/// <summary> 
		///   The change refers to the <see cref="Profile.SetValue" /> method. </summary>		
		/// <remarks> 
		///   <see cref="ProfileChangedArgs.Section" />,  <see cref="ProfileChangedArgs.Entry" />, 
		///   and <see cref="ProfileChangedArgs.Value" /> will be set to the same values passed 
		///   to the SetValue method. </remarks>
		SetValue,

		/// <summary> 
		///   The change refers to the <see cref="Profile.RemoveEntry" /> method. </summary>		
		/// <remarks> 
		///   <see cref="ProfileChangedArgs.Section" /> and <see cref="ProfileChangedArgs.Entry" /> 
		///   will be set to the same values passed to the RemoveEntry method. </remarks>
		RemoveEntry,

		/// <summary> 
		///   The change refers to the <see cref="Profile.RemoveSection" /> method. </summary>		
		/// <remarks> 
		///   <see cref="ProfileChangedArgs.Section" /> will contain the name of the section passed to the RemoveSection method. </remarks>
		RemoveSection,

		/// <summary> 
		///   The change refers to method or property specific to the Profile class. </summary>		
		/// <remarks> 
		///   <see cref="ProfileChangedArgs.Entry" /> will contain the name of the  method or property.
		///   <see cref="ProfileChangedArgs.Value" /> will contain the new value. </remarks>
		Other
	}
}
