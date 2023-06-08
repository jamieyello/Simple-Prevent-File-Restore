using System.Runtime.InteropServices;

namespace PreventRestore
{
    internal class Program
    {
        static void Main(string[] args) {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) throw new PlatformNotSupportedException();
            InitiateFromPrompt();
        }

        static void InitiateFromPrompt() {
            Console.WriteLine("Prevent restore utility. Detected drives;");
            Console.WriteLine("");
            foreach (var d in DriveInfo.GetDrives()) Console.WriteLine(d.Name);
            Console.WriteLine("");

            bool retry = false;
            string? drive = null;
            do {
                Console.Write("Enter drive letter to overwrite dead data: ");
                drive = Console.ReadLine();
                retry = !ValidateDriveInput(drive);
                Console.WriteLine("");
            } while (retry);
            drive = drive.Trim().ToUpper();

            var job = new PreventRestoreJob(drive[0]);
            job.Start();
        }

        static bool ValidateDriveInput(string? drive) {
            if (string.IsNullOrEmpty(drive)) {
                Console.WriteLine("Please enter a drive letter.");
                return false;
            }

            drive = drive.Trim().ToUpper();

            if (drive.Length > 1) {
                Console.WriteLine("Please enter a valid drive letter (no symbols).");
                return false;
            }

            if (!char.IsLetter(drive[0])) {
                Console.WriteLine("Please enter a valid drive letter.");
                return false;
            }

            if (!DriveInfo.GetDrives().Any(x => x.Name.StartsWith(drive))) {
                Console.WriteLine("Selected drive does not exist.");
                return false;
            }

            return true;
        }
    }
}