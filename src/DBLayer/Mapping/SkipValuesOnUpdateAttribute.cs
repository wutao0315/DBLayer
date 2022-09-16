﻿namespace DBLayer.Mapping;

/// <summary>
/// Attribute for skipping specific values on update.
/// </summary>

public class SkipValuesOnUpdateAttribute : SkipValuesByListAttribute
{
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="values">
	/// Values to skip on update operations.
	/// </param>
	public SkipValuesOnUpdateAttribute(params object?[]? values) : base(values ?? new object?[] { null }) { }

	/// <summary>
	/// Operations, affected by value skipping.
	/// </summary>
	public override SkipModification Affects => SkipModification.Update;

	public override string GetObjectID()
	{
		return $".{(int)Affects}.";
	}
}