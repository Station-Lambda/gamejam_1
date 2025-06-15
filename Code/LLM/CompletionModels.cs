using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sandbox.LLM;

/// <summary>
/// Internal request payload structure for LLM API calls.
/// Follows OpenAI API format for compatibility.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal struct CompletionRequest
{
	public string model { get; set; }
	public List<Message> messages { get; set; }
	public float? temperature { get; set; }
	public int? max_tokens { get; set; }
	public float? top_p { get; set; }
}

/// <summary>
/// Internal response structure from LLM API.
/// Follows OpenAI API format for compatibility.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal struct CompletionResponse
{
	public string id { get; set; }
	public string @object { get; set; }
	public long created { get; set; }
	public string model { get; set; }
	public List<Choice> choices { get; set; }
	public Usage usage { get; set; }
}

/// <summary>
/// Represents a single choice/completion from the LLM API response
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal struct Choice
{
	public string finish_reason { get; set; }
	public int index { get; set; }
	public Message message { get; set; }
}

/// <summary>
/// Token usage information from the API response
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
internal struct Usage
{
	public int prompt_tokens { get; set; }
	public int completion_tokens { get; set; }
	public int total_tokens { get; set; }
}