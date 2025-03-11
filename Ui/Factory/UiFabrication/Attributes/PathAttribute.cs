using System;

namespace GameBridge.Ui.Factory.UiFabrication.Attributes;

public class PathAttribute(PathType pathType) : Attribute
{
	public PathType PathType { get; } = pathType;
}