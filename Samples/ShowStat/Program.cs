using System;
using System.IO;
using System.Text;

namespace ShowStat;

using DQD.UnixFileStatus;

class Program
{
	static void Main(string[] args)
	{
		if (args.Length == 0)
			Console.WriteLine("usage: ShowStat <path> [<path> [..]]");
		else
		{
			foreach (string pattern in args)
			{
				var (baseDir, relativePattern) = SplitPattern(pattern);

				foreach (var matchingFile in Globber.GetMatches(baseDir, relativePattern))
					ShowStat(matchingFile.FullName);
			}
		}
	}

	static char[] PathSeparatorChars = [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar];
	static char[] PatternChars = ['*', '?'];

	static (string BaseDirectory, string RelativePattern) SplitPattern(string pattern)
	{
		var baseDirectory = new StringBuilder();
		bool first = true;

		if (Path.IsPathRooted(pattern))
			baseDirectory.Append(Path.GetPathRoot(pattern));

		while (true)
		{
			int separator = pattern.IndexOfAny(PathSeparatorChars);

			if (separator < 0)
				break;

			string token = pattern.Substring(0, separator);

			if (token.IndexOfAny(PatternChars) >= 0)
				break;

			if (first)
				first = false;
			else
				baseDirectory.Append(Path.DirectorySeparatorChar);
			baseDirectory.Append(token);

			pattern = pattern.Substring(separator + 1);
		}

		if (baseDirectory.Length == 0)
			return (".", pattern);
		else
			return (baseDirectory.ToString(), pattern);
	}

	static int s_pathColumnWidth = Console.WindowWidth > 0 ? Console.WindowWidth / 2 : 40;
	static string s_formatString = "{0,-" + s_pathColumnWidth + "}: {1}  {4,15:#,###,###,##0} bytes  node {5}  [{2}, {3}]";

	static void ShowStat(string path)
	{
		try
		{
			var stat = FileEx.GetFileStatus(path);

			if (path.Length > s_pathColumnWidth)
				path = path.Substring(path.Length - s_pathColumnWidth);

			Console.WriteLine(
				s_formatString,
				path,
				stat.Mode.ToModeString(),
				stat.OwnerUserID,
				stat.OwnerGroupID,
				stat.Size,
				stat.INode);
		}
		catch (Exception e)
		{
			Console.Error.WriteLine("{0}: {1}: {2}", path, e.GetType(), e.Message);
		}
	}
}
