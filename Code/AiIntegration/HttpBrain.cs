using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Sandbox;

namespace Sandbox.AiIntegration;

/// <summary>
/// Represents a conversation message with role and content
/// </summary>
public struct Message
{
	/// <summary>
	/// Role: "user", "assistant", or "system"
	/// </summary>
	public string Role { get; set; }

	/// <summary>
	/// Message content text
	/// </summary>
	public string Content { get; set; }
}

/// <summary>
/// AI response choice from API
/// </summary>
[SuppressMessage( "ReSharper", "InconsistentNaming" )]
public struct Choice
{
	public string finish_reason { get; set; }
	public string native_finish_reason { get; set; }
	public int Index { get; set; }
	public Message Message { get; set; }
}

/// <summary>
/// Request body for AI API
/// </summary>
public struct HttpBody
{
	public string Model { get; set; }
	public List<Message> Messages { get; set; }
}

/// <summary>
/// Response from AI API
/// </summary>
public struct HttpResponse
{
	public string Id { get; set; }
	public string Provider { get; set; }
	public string Model { get; set; }
	public string Object { get; set; }
	public int Created { get; set; }
	public List<Choice> Choices { get; set; }
}

/// <summary>
/// Handles HTTP communication with AI API
/// </summary>
public sealed class HttpBrain
{
	private const string ApiKey = "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";
	private const string ApiUrl = "https://openrouter.ai/api/v1/chat/completions";
	private const string Model = "deepseek/deepseek-chat-v3-0324:free";

	/// <summary>
	/// Sends messages to AI and returns response
	/// </summary>
	public async Task<string> RequestToIa( List<Message> messages )
	{
		var headers = new Dictionary<string, string>
		{
			{ "Authorization", $"Bearer {ApiKey}" }
		};

		var httpBody = new HttpBody
		{
			Model = Model,
			Messages = messages
		};

		var response = await Http.RequestJsonAsync<HttpResponse>( ApiUrl, "POST", Http.CreateJsonContent( httpBody ), headers );

		if ( response.Choices == null || response.Choices.Count == 0 )
			return string.Empty;

		var message = response.Choices[0].Message;
		Log.Info( $"AI Response: {message.Content}" );

		return message.Content;
	}
}
