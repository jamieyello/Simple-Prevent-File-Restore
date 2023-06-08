using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreventRestore
{
    class FancyCmdOutput
    {
        bool initiated;
        long target_size_bytes;
        DateTime? last_updated;
        long? last_progress;
        const char BLOCK = '■';

        public FancyCmdOutput(long target_size_bytes) {
            this.target_size_bytes = target_size_bytes;
        }

        public void Update(long progress_bytes) {
            var elapsed = last_updated == null ? null : DateTime.Now - last_updated;
            var elapsed_bytes = last_progress == null ? null : progress_bytes - last_progress;
            var bps = elapsed_bytes == null || elapsed == null ? null : elapsed_bytes / elapsed.Value.TotalSeconds;
            var remaining_bytes = target_size_bytes - progress_bytes;
            TimeSpan? eta = !bps.HasValue || bps.Value == 0 ? null : TimeSpan.FromSeconds(remaining_bytes / bps.Value);

            if (initiated) Console.CursorTop -= 4;

            Console.WriteLine($"{(bps == null ? "-" : (long)BytesToMegabytes((long)bps.Value))}MB/s".PadRight(60, ' '));
            Console.WriteLine($"{(long)BytesToMegabytes(progress_bytes)}MB / {(long)BytesToMegabytes(target_size_bytes)}MB".PadRight(60, ' '));
            Console.WriteLine($"{$"{progress_bytes * 100 / target_size_bytes}%",-4}{GetProgressBar(progress_bytes, target_size_bytes, 45)}".PadRight(60, ' '));
            Console.WriteLine($"Estimated time remaining: {(eta == null ? "-" : $"{(long)eta.Value.TotalHours} hours, {eta.Value.Minutes} minutes, {eta.Value.Seconds} seconds")}".PadRight(60, ' '));

            last_updated = DateTime.Now;
            last_progress = progress_bytes;
            initiated = true;
        }

        static string GetProgressBar(long count, long target, int length) {
            var progress = (decimal)count / target;
            var blocks = (int)Math.Round(progress * length);
            var result = string.Join("", Enumerable.Range(0, blocks).Select(x => BLOCK)).PadRight(length, '-');
            return result;
        }

        static double BytesToMegabytes(long bytes) => 
            (bytes / 1024f) / 1024f;
    }
}
