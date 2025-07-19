using System;
using System.Runtime.InteropServices;

namespace UnixFileStatus;

[StructLayout(LayoutKind.Sequential)]
internal struct FileStatusInternal
{
	[Flags]
	public enum FileStatusFlags
	{
		None = 0,
		HasBirthTime = 1,
	}

	public FileStatusFlags Flags;
	public int Mode;
	public uint Uid;
	public uint Gid;
	public long Size;
	public long ATime;
	public long ATimeNsec;
	public long MTime;
	public long MTimeNsec;
	public long CTime;
	public long CTimeNsec;
	public long BirthTime;
	public long BirthTimeNsec;
	public long Dev;
	public long RDev;
	public long Ino;
	public uint UserFlags;
}
