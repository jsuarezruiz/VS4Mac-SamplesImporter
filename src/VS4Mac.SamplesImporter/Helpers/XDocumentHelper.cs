﻿using System;
using System.Xml.Linq;

namespace VS4Mac.SamplesImporter.Helpers
{
	public static class XDocumentHelper
	{
		public static bool CanBeLoaded(string uri)
		{
			try
			{
				XDocument.Load(uri);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}