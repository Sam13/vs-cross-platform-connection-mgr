using System;
using System.Security;
using liblinux;
using liblinux.Persistence;

namespace CrossPlatformConnectionMgr
{
	internal sealed class Program
	{
		private enum Result
		{
			Success = 0,
			InvalidCommandLine = 1,
			UnsupportedCommand = 2
		}

		public static int Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Invalid command line");
				return (int) Result.InvalidCommandLine;
			}

			var command = args[0];

			if (string.Compare(command, "add", StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (args.Length < 5)
				{
					Console.WriteLine("Invalid command line");
					return (int) Result.InvalidCommandLine;
				}
				
				var host = args[1];
				var port = int.Parse(args[2]);
				var user = args[3];
				var password = args[4];

				Console.WriteLine($"Adding credentials for {host}");

				var cis = new ConnectionInfoStore();
				var passwordSecure = new SecureString();
				foreach (var c in password)
				{
					passwordSecure.AppendChar(c);
				}

				cis.Add(new PasswordConnectionInfo(host, port, TimeSpan.MaxValue, user, passwordSecure));
				
				Console.WriteLine($"Saving credentials");
				cis.Save();
				Console.WriteLine($"Credentials saved successfully");

				return (int) Result.Success;
			}
			return (int) Result.UnsupportedCommand;
		}
	}
}