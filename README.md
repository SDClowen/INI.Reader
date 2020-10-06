# INI.Reader
Easy to use and fast INI class

*Read Ini File*
```cs
var ini = new Ini(string ini);
```
*Remove Entry*
```cs
ini.RemoveEntry(string section, string entry);
```
*Remove Section*
```cs
ini.RemoveSection(string section);
```
*Set/Get Value*
```cs
ini.SetValue(string section, string entry, object value);
var getValue = ini.GetValue(string section, string entry);
var getValueGeneric = ini.GetValue<T>(string section, string entry); // Generic
```

*Has Entry*
```cs
var hasEntry = ini.HasEntry(string section, string entry);
```
*Has Section*
```cs
var hasSection = ini.HasSection(string section);
```
*Get Entry Names*
```cs
var entryNames = ini.GetEntryNames(string section);
```
*Get Section Names*
```cs
var sectionNames = ini.GetSectionNames();
```
*Get Dataset / Set Dataset*
```cs
var dataSet = ini.GetDataSet();
ini.SetDataSet(DataSet ds);
```




