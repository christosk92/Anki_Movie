using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnkiMovie_Console_Test {

    public static class AnkiHelpers {
        private static string AnkiBaseUrl = "http://127.0.0.1:8765/";
        private static HttpClient httpClient;

        public static void init() => httpClient = new HttpClient();

        public static async void CreateDeck(string name) {
            try {
                models.Action action = new models.Action {
                    action = "createDeck",
                    version = 6,
                    _params = new models.Params {
                        deck = name
                    }
                };

                await httpClient.PostAsync(AnkiBaseUrl, new StringContent(JsonConvert.SerializeObject(action), Encoding.UTF8, "application/json"));
            }
            catch (HttpRequestException e) {
                throw new Exception("Unable to connect.");
            }
        }

        public static async Task<List<string>> GetDecks() {
            models.Action action = new models.Action {
                action = "deckNames",
                version = 6
            };
            var result = await httpClient.PostAsync(AnkiBaseUrl, new StringContent(JsonConvert.SerializeObject(action), Encoding.UTF8, "application/json"));
            var response = await result.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(response);
            var jarr = obj["result"].Value<JArray>();
            List<String> lst = jarr.ToObject<List<String>>();
            lst.Sort();
            return lst;
        }

        public static String AddToDeck(models.Note note) {
            models.Action action = new models.Action {
                action = "addNote",
                version = 6,
                _params = new models.Params {
                    _notes = note
                }
            };
            var result = httpClient.PostAsync(AnkiBaseUrl, new StringContent(JsonConvert.SerializeObject(action), Encoding.UTF8, "application/json")).Result;
            var contentResult = result.Content.ReadAsStringAsync().Result;
            //duplicate
            var errorContent = JObject.Parse(contentResult)["error"].Value<String>();
            if (!String.IsNullOrEmpty(errorContent))
            {
                if(!errorContent.Contains("deck"))
                    return errorContent;
                else
                {
                    CreateDeck(note.DeckName);
                    AddToDeck(note);
                }
            }
            return "Created card";
        }
    }
}