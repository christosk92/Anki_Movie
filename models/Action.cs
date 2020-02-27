using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnkiMovie_Console_Test.models
{
    public class Action
    {

        [JsonProperty("action")]
        public string action { get; set; }
        [JsonProperty("version")]
        public int version { get; set; }
        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public Params _params { get; set; }

    }

    public partial class Params
    {
        [JsonProperty("deck", NullValueHandling = NullValueHandling.Ignore)]
        public string deck { get; set; }

        [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
        public Note _notes { get; set; }
    }

    public partial class Note
    {
        [JsonProperty("deckName", NullValueHandling = NullValueHandling.Ignore)]
        public string DeckName { get; set; }

        [JsonProperty("modelName", NullValueHandling = NullValueHandling.Ignore)]
        public string ModelName { get; set; }

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public Fields Fields { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }

        [JsonProperty("audio", NullValueHandling = NullValueHandling.Ignore)]
        public Audio Audio { get; set; }
    }

    public partial class Audio
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("filename", NullValueHandling = NullValueHandling.Ignore)]
        public string Filename { get; set; }

        [JsonProperty("skipHash", NullValueHandling = NullValueHandling.Ignore)]
        public string SkipHash { get; set; }

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Fields { get; set; }
    }

    public partial class Fields
    {
        [JsonProperty("Front", NullValueHandling = NullValueHandling.Ignore)]
        public string Front { get; set; }

        [JsonProperty("Back", NullValueHandling = NullValueHandling.Ignore)]
        public string Back { get; set; }
    }

}