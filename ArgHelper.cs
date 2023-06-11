using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreventRestore
{
    internal static class ArgHelper
    {
        public class Arg
        {
            public enum ArgType
            {
                Help,
                Iterations,
                Drive,
            }

            public Arg()
            {
                
            }
            public Arg(ArgType type, IEnumerable<string> parameters)
            {
                Type = type;
                Parameters = parameters.ToArray();
            }

            public ArgType Type;
            public string[] Parameters = Array.Empty<string>();
        }

        /// <summary> Parse args. Validates input, throws exception if an argument is invalid. </summary>
        public static Arg[] Parse(IEnumerable<string> args) {
            var args_a = args.ToArray();
            var results = new List<Arg>();

            for (int i = 0; i < args_a.Length; i++) {
                if (args_a[i] == "--help" || args_a[i] == "-h") {
                    results.Add(new Arg() { Type = Arg.ArgType.Help });
                    continue;
                }
                
                if (args_a[i] == "--iterations" || args_a[i] == "-i") {
                    var parameters = GetParameters(args_a, i);
                    if (!parameters.Any()) throw new ArgumentException("Expected iterations value, got none.", args_a[i]);
                    if (parameters.Length > 1) throw new ArgumentException("More than one parameter for iterations.", args_a[i]);
                    if (!int.TryParse(parameters[0], out var iterations) || iterations < 0) throw new ArgumentException("Invalid iterations value. Expected positive integer value.", args_a[i]);
                    results.Add(new Arg() { Type = Arg.ArgType.Iterations, Parameters = parameters.ToArray() });
                    i += parameters.Length;
                    continue;
                }

                if (args_a[i] == "--drive" || args_a[i] == "-d") {
                    var parameters = GetParameters(args_a, i);
                    if (!parameters.Any()) throw new ArgumentException("Expected drive value, got none.", args_a[i]);
                    if (parameters.Length > 1) throw new ArgumentException("More than one parameter for drive target.", args_a[i]);
                    var error = ValidateDriveInput(parameters[0]);
                    if (error != null)  throw new ArgumentException(error, args_a[i]);
                    results.Add(new Arg() { Type = Arg.ArgType.Drive, Parameters = new string[] { parameters[0].ToUpper() } });
                    i += parameters.Length;
                    continue;
                }

                throw new Exception($"Unexpected arg {args_a[i]}, see --help for available commands.");
            }

            return results.ToArray();
        }

        static string[] GetParameters(string[] args, int index)
        {
            var results = new List<string>();
            
            for (int i = index + 1; i < args.Length; i++) {
                if (args[i][0] == '-') break;
                results.Add(args[i]);
            }

            return results.ToArray();
        }

        public static string? ValidateDriveInput(string? drive) {
            if (string.IsNullOrEmpty(drive)) return "Please enter a drive letter.";

            drive = drive.Trim().ToUpper();

            if (drive.Length > 1) return "Please enter a valid drive letter (no symbols).";
            if (!char.IsLetter(drive[0])) return "Please enter a valid drive letter.";
            if (!DriveInfo.GetDrives().Any(x => x.Name.StartsWith(drive))) return "Selected drive does not exist.";

            return null;
        }

        public static void PrintHelp() =>
            Console.WriteLine(
                "Arguments;\n" +
                "   [Command]      [Shorthand] [Parameters]\n" +
                "\n" +
                "   --help         -h          (none)\n" +
                "       Display available commands.\n" +
                "\n" +
                "   --drive        -d          (drive letter)\n" +
                "       Run job on specified drive.\n" +
                "\n" +
                "   --iterations   -i          (positive integer, number of iterations)\n" +
                "       Run job with specified iterations count (how many times the disk should be written over).\n"
                );
    }
}
