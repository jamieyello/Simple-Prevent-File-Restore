using System.Runtime.InteropServices;

namespace PreventRestore
{
    internal class Program
    {
        static void Main(string[] args) {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) throw new PlatformNotSupportedException();
            if (args.Any()) InitiateFromArgs(args);
            else InitiateFromPrompt();
        }

        static void InitiateFromArgs(string[] args)
        {
            var args_c = ArgHelper.Parse(args);

            bool run_job = false;
            int iterations = 1;
            char? drive = null;

            foreach (var arg in args_c) {
                if (arg.Type == ArgHelper.Arg.ArgType.Help) ArgHelper.PrintHelp();
                else if (arg.Type == ArgHelper.Arg.ArgType.Iterations) {
                    run_job = true;
                    iterations = int.Parse(arg.Parameters[0]);
                }
                else if (arg.Type == ArgHelper.Arg.ArgType.Drive) {
                    run_job = true;
                    drive = arg.Parameters[0][0];
                }
            }

            if (run_job) {
                if (drive == null) throw new Exception("Can't run job with no drive specified. Use --drive or -d to specify a drive.");
                var jobs = new List<PreventRestoreJob>();
                for (int i = 0; i < iterations; i++) {
                    jobs.Add(new(drive.Value));
                }

                foreach (var job in jobs) job.Start();
                Console.ReadLine();
            }

            // We are fully validated, nothing could possibly go wrong

        }



        static void InitiateFromPrompt() {
            Console.WriteLine("Simple prevent restore utility. Detected drives;");
            Console.WriteLine("");
            foreach (var d in DriveInfo.GetDrives()) Console.WriteLine(d.Name);
            Console.WriteLine("");

            bool retry = false;
            string? drive = null;
            do {
                Console.Write("Enter drive letter to overwrite dead data: ");
                drive = Console.ReadLine();
                var validate_error = ArgHelper.ValidateDriveInput(drive);
                if (validate_error != null) Console.WriteLine(validate_error);
                retry = validate_error != null;
                Console.WriteLine("");
            } while (retry);
            drive = drive.Trim().ToUpper();

            var job = new PreventRestoreJob(drive[0]);
            job.Start();
            Console.ReadLine();
        }
    }
}