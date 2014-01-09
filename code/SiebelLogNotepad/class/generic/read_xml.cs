using System;
using System.Collections.Generic;
using System.Xml;

public class RwFile
{
    static public List<string> ReadXml(string path, string colum)
	{
        List<string> listOfElements = new List<string>();

		XmlTextReader textReader = new XmlTextReader(path);

		int foundName = 0;

		while (textReader.Read())
		{
			if (foundName > 0 && textReader.Name != string.Empty && !String.Equals(textReader.Name, colum, StringComparison.CurrentCultureIgnoreCase))
				foundName = 0;

			if (foundName == 2 && String.Equals(textReader.Name, colum, StringComparison.CurrentCultureIgnoreCase))
			{
				foundName = 0; continue;
			}				

			if (foundName == 0 && String.Equals(textReader.Name, colum, StringComparison.CurrentCultureIgnoreCase))
			{
				foundName = 1; continue;
			}

		    if (foundName != 1 || textReader.Name != string.Empty) continue;

		    listOfElements.Add(textReader.Value);
		}

		textReader.Close();
		return listOfElements;
	}
}
