using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PreventRestore
{
    public class PreventRestoreJob
    {
        readonly DriveInfo drive;
        FancyCmdOutput? output;
        readonly string? custom_path;
        bool started;

        string OutputFile => 
            Path.Combine(custom_path ?? drive.Name, "PREVENT_RESTORE_FILLER");

        public PreventRestoreJob(char drive) {
            drive = char.ToUpper(drive);
            if (!char.IsLetter(drive)) throw new ArgumentException("Drive is invalid.", nameof(drive));
            if (!DriveInfo.GetDrives().Any(x => x.Name[0] == drive)) throw new ArgumentException("Drive does not exist.", nameof(drive));
            this.drive = DriveInfo.GetDrives().Where(x => x.Name[0] == drive).First();
            if (this.drive.Name[0] == 'C') custom_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public void Start() {
            if (started) throw new Exception("Jobs cannot be re-used.");
            started = true;

            if (File.Exists(OutputFile)) File.Delete(OutputFile);
            var write_buffer = new byte[1024 * 32];
            output = new(drive.TotalFreeSpace);

            long i = 0;
            using var task = Task.Run(() => { 
                using var fs = new FileStream(Path.Combine(OutputFile), FileMode.OpenOrCreate);

                while (drive.TotalFreeSpace > 0) {
                    if (drive.TotalFreeSpace < write_buffer.Length) {
                        var remainder_write_buffer = RandomNumberGenerator.GetBytes((int)drive.TotalFreeSpace);
                        fs.Write(remainder_write_buffer);
                        i += remainder_write_buffer.LongLength;
                    }
                    else {
                        RandomNumberGenerator.Fill(write_buffer);
                        fs.Write(write_buffer);
                        i += write_buffer.LongLength;
                    }
                }
            });

            while (!task.IsCompleted) {
                output.Update(i);
                Thread.Sleep(2000);
            }
            output.Update(i);

            Console.WriteLine();
            if (task.IsFaulted && task.Exception != null) Console.WriteLine($"Error thrown: {task.Exception.Message}");
            else Console.WriteLine("Done.");
            if (File.Exists(OutputFile)) File.Delete(OutputFile);
        }
    }
}
