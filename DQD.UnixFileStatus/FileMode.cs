using System;

namespace UnixFileStatus;

[Flags]
public enum FileMode
{
	None = 0,
	OtherExecute = 0b000_000_001,
	OtherWrite   = 0b000_000_010,
	OtherRead    = 0b000_000_100,
	GroupExecute = 0b000_001_000,
	GroupWrite   = 0b000_010_000,
	GroupRead    = 0b000_100_000,
	UserExecute  = 0b001_000_000,
	UserWrite    = 0b010_000_000,
	UserRead     = 0b100_000_000,

	StickyBit         = 0b0000000001_000000000,
	SetGroup          = 0b0000000010_000000000,
	SetUser           = 0b0000000100_000000000,
	IsFIFO            = 0b0000001000_000000000,
	IsCharacterDevice = 0b0000010000_000000000,
	IsDirectory       = 0b0000100000_000000000,
	IsBlockDevice     = 0b0000110000_000000000,
	IsRegularFile     = 0b0001000000_000000000,
	IsLink            = 0b0010000000_000000000,
	IsSocket          = 0b0100000000_000000000,

	PermissionsMask = 0b00000000000_111111111,
	FileTypeMask    = 0b11111111111_000000000,
}
