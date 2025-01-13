using System;

namespace GameBridge.Ui.Attributes;

public class PathAttribute(PathType pathType) : Attribute
{
	public PathType PathType { get; } = pathType;
}