namespace Sandbox.LLM;

/// <summary>
/// Configuration settings for the LLM client.
/// Allows customization of model, parameters, and API endpoints.
/// </summary>
public class LLMConfig
{
	/// <summary>
	/// The model identifier to use for completions.
	/// Examples: "gpt-4", "claude-3", "deepseek/deepseek-chat"
	/// </summary>
	public string Model { get; set; } = "deepseek/deepseek-chat-v3-0324:free";
	
	/// <summary>
	/// Temperature for response randomness (0.0 - 2.0).
	/// Lower values (0.0-0.5) = more deterministic and focused
	/// Medium values (0.6-0.9) = balanced creativity
	/// Higher values (1.0-2.0) = more creative and unpredictable
	/// </summary>
	public float Temperature { get; set; } = 0.7f;
	
	/// <summary>
	/// Maximum number of tokens to generate in the response.
	/// null = use model's default limit
	/// </summary>
	public int? MaxTokens { get; set; }
	
	/// <summary>
	/// Top-p (nucleus) sampling parameter.
	/// Alternative to temperature, controls diversity via cumulative probability.
	/// Values between 0.0 and 1.0, where lower values = less randomness
	/// </summary>
	public float? TopP { get; set; }
	
	/// <summary>
	/// The API endpoint URL for LLM requests.
	/// Must be OpenAI-compatible chat completions endpoint.
	/// </summary>
	public string ApiUrl { get; set; } = "https://openrouter.ai/api/v1/chat/completions";
	
	/// <summary>
	/// API key for authentication with the LLM service
	/// </summary>
	public string ApiKey { get; set; } = "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";
	
	/// <summary>
	/// Request timeout in milliseconds
	/// </summary>
	public int TimeoutMs { get; set; } = 30000;
	
	/// <summary>
	/// Creates a default configuration
	/// </summary>
	public static LLMConfig Default => new();
	
	/// <summary>
	/// Creates a configuration optimized for deterministic responses
	/// </summary>
	public static LLMConfig Deterministic => new()
	{
		Temperature = 0.1f,
		TopP = 0.1f
	};
	
	/// <summary>
	/// Creates a configuration optimized for creative responses
	/// </summary>
	public static LLMConfig Creative => new()
	{
		Temperature = 1.2f,
		TopP = 0.95f
	};
}