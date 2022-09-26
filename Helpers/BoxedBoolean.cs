namespace Scribble.Helpers;

public static class BooleanBox
{
	public static readonly object True = true;
	public static readonly object False = false;

	public static object Box(this bool bln) => bln ? True : False;

	public static object InverseBox(this bool bln) => bln ? False : True;
}
