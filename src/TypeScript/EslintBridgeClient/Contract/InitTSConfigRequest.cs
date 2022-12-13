using Newtonsoft.Json;

namespace SonarLint.VisualStudio.TypeScript.EslintBridgeClient.Contract
{
    internal class InitTSConfigRequest
    {
        [JsonProperty("baseDir")]
        public string BaseDirectory { get; set; }
    }
}
