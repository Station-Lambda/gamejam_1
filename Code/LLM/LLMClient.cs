using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Sandbox.LLM;

// Simple request/response models for OpenAI-compatible APIs
internal class CompletionRequest
{
	public string model { get; set; }
	public List<Message> messages { get; set; }
	public double temperature { get; set; }
	public int max_tokens { get; set; }
	public double top_p { get; set; }
}

internal class CompletionResponse
{
	public List<Choice> choices { get; set; }
	
	public class Choice
	{
		public Message message { get; set; }
	}
}

/// <summary>
/// Minimal LLM client for sending messages to OpenAI-compatible APIs
/// </summary>
public class LLMClient
{
	private readonly LLMConfig _config;
	
	
	/// <summary>
	/// Creates a new LLM client with default configuration
	/// </summary>
	public LLMClient() : this( LLMConfig.Default )
	{
	}
	
	/// <summary>
	/// Creates a new LLM client with custom configuration
	/// </summary>
	/// <param name="config">Configuration settings for the client</param>
	public LLMClient( LLMConfig config )
	{
		_config = config ?? throw new ArgumentNullException( nameof(config) );
	}
	
	/// <summary>
	/// Sends a list of messages to the LLM and returns the assistant's response
	/// </summary>
	/// <param name="messages">The conversation history</param>
	/// <returns>The assistant's response content</returns>
	/// <exception cref="ArgumentException">Thrown when messages is null or empty</exception>
	/// <exception cref="Exception">Thrown when API communication fails</exception>
	public async Task<string> CompleteAsync( List<Message> messages )
	{
		if ( messages == null || messages.Count == 0 )
		{
			throw new ArgumentException( "Messages cannot be null or empty", nameof(messages) );
		}
		
		
		var headers = new Dictionary<string, string>
		{
			{ "Authorization", $"Bearer {_config.ApiKey}" },
			{ "Content-Type", "application/json" }
		};
		
		var request = new CompletionRequest
		{
			model = _config.Model,
			messages = messages,
			temperature = _config.Temperature,
			max_tokens = _config.MaxTokens ?? 1000,
			top_p = _config.TopP ?? 1.0
		};
		
		try
		{
			var response = await Http.RequestJsonAsync<CompletionResponse>( 
				_config.ApiUrl, 
				"POST", 
				Http.CreateJsonContent( request ), 
				headers 
			);
			
			if ( response.choices == null || response.choices.Count == 0 )
			{
				Log.Warning( "LLMClient: Received empty response from API" );
				return string.Empty;
			}
			
			return response.choices[0].message.Content;
		}
		catch ( Exception ex )
		{
			Log.Error( $"LLMClient: Error during API call - {ex.Message}" );
			throw;
		}
	}
	
	/// <summary>
	/// Sends a single message and returns the response
	/// </summary>
	/// <param name="userMessage">The user's message</param>
	/// <param name="systemPrompt">Optional system prompt to prepend</param>
	/// <returns>The assistant's response</returns>
	public async Task<string> CompleteAsync( string userMessage, string systemPrompt = null )
	{
		var messages = new List<Message>();
		
		if ( !string.IsNullOrEmpty( systemPrompt ) )
		{
			messages.Add( Message.System( systemPrompt ) );
		}
		
		messages.Add( Message.User( userMessage ) );
		
		return await CompleteAsync( messages );
	}
	
}