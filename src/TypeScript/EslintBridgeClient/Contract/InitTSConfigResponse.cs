using Newtonsoft.Json;

namespace SonarLint.VisualStudio.TypeScript.EslintBridgeClient.Contract
{
    internal class InitTSConfigResponse
    {
        [JsonProperty("filename")]
        public string FileName { get; set; }
    }
}
