namespace UnixFileStatus;

public static class FileModeExtensions
{
	public static bool IsExecutable(this FileMode mode) => (mode & (FileMode.OtherExecute | FileMode.GroupExecute | FileMode.UserExecute)) != 0;
	public static bool IsWriteable(this FileMode mode) => (mode & (FileMode.OtherWrite | FileMode.GroupWrite | FileMode.UserWrite)) != 0;
	public static bool IsReadable(this FileMode mode) => (mode & (FileMode.OtherRead | FileMode.GroupRead | FileMode.UserRead)) != 0;

	public static bool IsSticky(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.StickyBit;
	public static bool IsSetGroup(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.SetGroup;
	public static bool IsSetUser(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.SetUser;
	public static bool IsFIFO(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.IsFIFO;
	public static bool IsCharacterDevice(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.IsCharacterDevice;
	public static bool IsBlockDevice(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.IsBlockDevice;
	public static bool IsDirectory(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.IsDirectory;
	public static bool IsLink(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.IsLink;
	public static bool IsSocket(this FileMode mode) => (mode & FileMode.FileTypeMask) == FileMode.IsSocket;

	public static string ToModeString(this FileMode mode)
	{
		char[] ret = ['-', '-', '-', '-', '-', '-', '-', '-', '-', '-'];

		if (mode.IsBlockDevice())
			ret[0] = 'b';
		else if (mode.IsCharacterDevice())
			ret[0] = 'c';
		else if (mode.IsDirectory())
			ret[0] = 'd';
		else if (mode.IsLink())
			ret[0] = 'l';
		else if (mode.IsFIFO())
			ret[0] = 'p';
		else if (mode.IsSocket())
			ret[0] = 's';

		FormatPermission(1, FileMode.UserRead, FileMode.UserWrite, FileMode.UserExecute, FileMode.SetUser, 0);
		FormatPermission(4, FileMode.GroupRead, FileMode.GroupWrite, FileMode.GroupExecute, FileMode.SetGroup, 0);
		FormatPermission(7, FileMode.OtherRead, FileMode.OtherWrite, FileMode.OtherExecute, 0, 0);

		return new string(ret);

		void FormatPermission(int offset, FileMode readable, FileMode writeable, FileMode executable, FileMode setID, FileMode sticky)
		{
			if ((mode & readable) != 0)
				ret[offset] = 'r';
			offset++;
			if ((mode & writeable) != 0)
				ret[offset] = 'w';
			offset++;
			if ((mode & setID) != 0)
				ret[offset] = ((mode & executable) != 0) ? 's' : 'S';
			else if ((mode & sticky) != 0)
				ret[offset] = ((mode & executable) != 0) ? 't' : 'T';
			else if ((mode & executable) != 0)
				ret[offset] = 'x';
		}
	}
}
