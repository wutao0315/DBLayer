﻿using System;

namespace DBLayer.Mapping
{
	public abstract class MappingAttribute : Attribute
	{
		public abstract string GetObjectID();
	}
}
