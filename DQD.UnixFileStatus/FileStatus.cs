using System;

namespace UnixFileStatus;

public class FileStatus
{
	public long ContainerDeviceINode;
	public long INode;
	public FileMode Mode;
	public uint OwnerUserID;
	public uint OwnerGroupID;
	public long TargetDeviceINode;
	public long Size;
	public DateTime LastAccessTime;
	public DateTime LastWriteTime;
	public DateTime CreationTime;
}
