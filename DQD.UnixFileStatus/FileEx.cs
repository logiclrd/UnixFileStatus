using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DQD.UnixFileStatus;

public static class FileEx
{
	[DllImport("libSystem.Native", EntryPoint = "SystemNative_Stat", CharSet = CharSet.Ansi, SetLastError = true)]
	static extern int Stat([MarshalAs(UnmanagedType.LPStr)] string path, out FileStatusInternal output);

	public static bool TryGetFileStatus(string path, out FileStatus status)
	{
		FileStatus ret = default!;
		bool success = false;

		GetFileStatusImpl(
			path,
			produce: result => ret = result,
			except: e => success = false);

		status = ret;

		return success;
	}

	public static FileStatus GetFileStatus(string path)
	{
		FileStatus ret = default!;

		GetFileStatusImpl(
			path,
			produce: result => ret = result,
			except: e => throw e);

		return ret;
	}

	static void GetFileStatusImpl(string path, Action<FileStatus> produce, Action<Exception> except)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			except(new NotSupportedException());
			return;
		}

		int result = Stat(path, out var status);

		if (result < 0)
		{
			except(new Win32Exception());
			return;
		}

		produce(
			new FileStatus()
			{
				ContainerDeviceINode = status.Dev,
				INode = status.Ino,
				Mode = (FileMode)status.Mode,
				OwnerUserID = status.Uid,
				OwnerGroupID = status.Gid,
				TargetDeviceINode = status.RDev,
				Size = status.Size,
				LastAccessTime = ConvertDateTime(status.ATime, status.ATimeNsec),
				LastWriteTime = ConvertDateTime(status.MTime, status.MTimeNsec),
				CreationTime = ConvertDateTime(status.CTime, status.CTimeNsec),
			});
	}

	public static bool IsExecutable(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsExecutable();
	public static bool IsWriteable(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsWriteable();
	public static bool IsReadable(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsReadable();

	public static bool IsSticky(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsSticky();
	public static bool IsSetGroup(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsSetGroup();
	public static bool IsSetUser(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsSetUser();
	public static bool IsFIFO(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsFIFO();
	public static bool IsCharacterDevice(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsCharacterDevice();
	public static bool IsBlockDevice(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsBlockDevice();
	public static bool IsDirectory(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsDirectory();
	public static bool IsLink(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsLink();
	public static bool IsSocket(string path) => TryGetFileStatus(path, out var status) && status.Mode.IsSocket();

	public static bool IsDevice(string path)
	{
		if (TryGetFileStatus(path, out var status))
		{
			var fileType = status.Mode & FileMode.FileTypeMask;

			if ((fileType & (FileMode.IsCharacterDevice | FileMode.IsBlockDevice)) != 0)
				return true;
		}

		return false;
	}

	static readonly DateTime Epoch = new DateTime(1970, 1, 1);

	static DateTime ConvertDateTime(long seconds, long nanoseconds)
		=> Epoch.AddSeconds(seconds).AddMicroseconds(nanoseconds / 1000.0);
}
