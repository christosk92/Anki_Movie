using AnkiMovie_Console_Test.models;
using Newtonsoft.Json;
using SubtitlesParser.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;
using Xabe.FFmpeg.Model;

namespace AnkiMovie_Console_Test {

    internal class Program {
        private static string videoPath;
        private static string srtPath;
        private static string AnkiMovieData;
        private static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello Weeb!");
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                AnkiMovieData = Path.Combine(localAppData, "AnkiMovie");
                Directory.CreateDirectory(AnkiMovieData);
                if (!System.IO.File.Exists(Path.Combine(AnkiMovieData, "1.json")))
                    PickVideoAndSrt();
                else
                    LoadVideoAndSrt();
                Console.WriteLine("Video: " + videoPath);
                Console.WriteLine("Srt: " + srtPath);
                var results = GetSubtitleText(srtPath);
                //foreach(var result in results)
                //{
                //  int index = results.FindIndex(x=> x.StartTime == result.StartTime);
                //  Console.WriteLine($"{index}: from {result.StartTime} to {result.EndTime} : {result.Lines[0]}");
                //}
                Console.WriteLine(("").PadRight(48, '-'));
                Console.WriteLine("Trying to poke AnkiConnect server... Give me a few..");
                bool result = false;
                var httpClient = new HttpClient();
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync("http://127.0.0.1:8765/");
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    result = true;
                }
                catch (HttpRequestException e)
                {
                    // TODO: Handle exception.
                }
                if (result)
                {
                    Console.WriteLine("Anki-senpai noticed me!");
                    AnkiHelpers.init();
                    //-- Works as of 2/27/2020  --
                    /* var note = new Note
                     {
                         DeckName = "Test Anki Movie",
                         ModelName = "Basic",
                         options = new Options { allowDuplicate = true},
                         Tags = new List<String>{ "AnkiMovie" },
                         Fields = new Fields { Back = "Test1", Front = "Test1" }
                     };
                     var resultAdd = AnkiHelpers.AddToDeck(note);
                     Console.WriteLine(resultAdd); 
                     */
                    try
                    {
                        FFmpegHelper.init(AnkiMovieData);
                        string extracted = await FFmpegHelper.ExtractAudio(videoPath, Path.Combine(AnkiMovieData, "extracted.mp3"));
                        string trimmed = await FFmpegHelper.ExtractAudio(extracted, Path.Combine(AnkiMovieData, "trimmed.mp3"));
                    }
                    catch (Xabe.FFmpeg.Exceptions.FFmpegNotFoundException)
                    { throw new Exception("FFMpeg was not found. Expected output: " + AnkiMovieData); }
                }
                else
                    Console.WriteLine("Anki-senpai didn't notice me...");
            }
            catch (Exception x)
            {
                Console.WriteLine(x.ToString());
            }
        }
        private static void LoadVideoAndSrt() {
            string path = Path.Combine(AnkiMovieData, "1.json");
            ArtItem ObjectResult = null;
            using (var reader = new StreamReader(path))
                ObjectResult = JsonConvert.DeserializeObject<ArtItem>(reader.ReadToEnd());
            videoPath = ObjectResult.MoviePath;
            srtPath = ObjectResult.SrtPath;
        }
        private static void PickVideoAndSrt() {
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            bool confirmedVideo = false;
            while (!confirmedVideo) {
                Console.Write("Enter path of video file: ");
                videoPath = Console.ReadLine();
                if (System.IO.File.Exists(videoPath))
                    confirmedVideo = true;
                else
                    Console.WriteLine("File doesn't exist.");
            }
            bool confirmedSRT = false;
            while (!confirmedSRT) {
                Console.Write("Enter path of subtitle file: ");
                srtPath = Console.ReadLine();
                if (System.IO.File.Exists(srtPath))
                    confirmedSRT = true;
                else
                    Console.WriteLine("File doesn't exist.");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Chosen Video path {videoPath}, and corresponding SRT: {srtPath}");
            Console.WriteLine($"Wanna continue with these settings?");
            bool confirmedEnter = false;
            while (!confirmedEnter) {
                Console.WriteLine("Please enter y/n :");
                string input = Console.ReadLine();
                if (input.ToLower() == "y")
                    confirmedEnter = true;
                else if (input.ToLower() == "n") {
                    PickVideoAndSrt();
                    confirmedEnter = true;
                }
            }
            Console.ResetColor();
            ArtItem newItem = new ArtItem(videoPath, srtPath);
            string json = JsonConvert.SerializeObject(newItem);
            System.IO.File.WriteAllText(Path.Combine(AnkiMovieData, "1.json"), json);
        }
        private static List<SubtitleItem> GetSubtitleText(string FilePath) {
            var parser = new SubtitlesParser.Classes.Parsers.SubParser();
            List<SubtitleItem> items = null;
            using (var fileStream = File.OpenRead(FilePath)) {
                items = parser.ParseStream(fileStream, Encoding.UTF8);
            }
            return items;
        }
    }
}