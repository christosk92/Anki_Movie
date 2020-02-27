using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace AnkiMovie_Console_Test
{
    public static class FFmpegHelper
    {
        public static void init(string ffmpegpath) => FFmpeg.ExecutablesPath = ffmpegpath;
        public static async Task<string> ExtractAudio(string input, string output)
        {
            Console.WriteLine("Extracting audio from: " + input);
            if (System.IO.File.Exists(output))
                System.IO.File.Delete(output);
            var action = Conversion.ExtractAudio(input, output);
            action.OnProgress += (sender, args) =>
            {
                var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                Console.WriteLine($"[{args.Duration} / {args.TotalLength}] {percent}%");
            };
            await action.Start();
            Console.WriteLine("Extracted audio to: " + output);
            return output;
        }
        public static async Task<string> Trim(string input, string output, TimeSpan from, TimeSpan to)
        {
            Console.WriteLine("Trimming audio from: " + input);
            if (System.IO.File.Exists(output))
                System.IO.File.Delete(output);
            var action = Conversion.Split(output, output, from, to);
            action.OnProgress += (sender, args) =>
            {
                var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                Console.WriteLine($"[{args.Duration} / {args.TotalLength}] {percent}%");
            };
            await action.Start();
            Console.WriteLine("Trimmed audio to: " + output);
            return output;
        }
    }
}
