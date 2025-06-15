using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sandbox.LLM;

/// <summary>
/// Client for interacting with Large Language Model APIs.
/// Provides a clean interface for sending messages and receiving AI-generated responses.
/// 
/// This client is designed to work with OpenAI-compatible APIs (OpenRouter, OpenAI, etc.)
/// and handles the HTTP communication, serialization, and error handling.
/// 
/// Example usage:
/// <code>
/// var client = new LLMClient();
/// var response = await client.CompleteAsync("Hello, how are you?");
/// </code>
/// </summary>
public class LLMClient
{
	private readonly LLMConfig _config;
	
	/// <summary>
	/// Event fired before a request is sent to the API
	/// </summary>
	public event Action<List<Message>> BeforeRequest;
	
	/// <summary>
	/// Event fired after a response is received from the API
	/// </summary>
	public event Action<string> AfterResponse;
	
	/// <summary>
	/// Event fired when an error occurs during API communication
	/// </summary>
	public event Action<Exception> OnError;
	
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
		
		BeforeRequest?.Invoke( messages );
		
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
			max_tokens = _config.MaxTokens,
			top_p = _config.TopP
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
			
			var content = response.choices[0].message.Content;
			AfterResponse?.Invoke( content );
			
			return content;
		}
		catch ( Exception ex )
		{
			Log.Error( $"LLMClient: Error during API call - {ex.Message}" );
			OnError?.Invoke( ex );
			throw new Exception( "Failed to get response from LLM", ex );
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
	
	/// <summary>
	/// Estimates the number of tokens in a text (rough approximation)
	/// </summary>
	/// <param name="text">The text to estimate</param>
	/// <returns>Approximate token count</returns>
	public static int EstimateTokens( string text )
	{
		if ( string.IsNullOrEmpty( text ) ) return 0;
		
		// Rough estimate: ~4 characters per token for English text
		// For more accurate estimation, consider using a proper tokenizer
		return (int)Math.Ceiling( text.Length / 4.0 );
	}
	
	/// <summary>
	/// Estimates the total token count for a list of messages
	/// </summary>
	/// <param name="messages">Messages to count tokens for</param>
	/// <returns>Total approximate token count</returns>
	public static int EstimateTokens( List<Message> messages )
	{
		var totalTokens = 0;
		foreach ( var message in messages )
		{
			// Add role tokens (typically 1-2 tokens)
			totalTokens += 2;
			// Add content tokens
			totalTokens += EstimateTokens( message.Content );
		}
		return totalTokens;
	}
	
	/// <summary>
	/// Validates if the messages list will fit within token limits
	/// </summary>
	/// <param name="messages">Messages to validate</param>
	/// <param name="maxTokens">Maximum allowed tokens (default: 4096)</param>
	/// <returns>True if within limits, false otherwise</returns>
	public static bool ValidateTokenLimit( List<Message> messages, int maxTokens = 4096 )
	{
		return EstimateTokens( messages ) <= maxTokens;
	}
	
	/// <summary>
	/// Creates a new client with a different model while keeping other settings
	/// </summary>
	/// <param name="model">The model to use</param>
	/// <returns>A new LLMClient instance</returns>
	public LLMClient WithModel( string model )
	{
		var newConfig = new LLMConfig
		{
			Model = model,
			Temperature = _config.Temperature,
			MaxTokens = _config.MaxTokens,
			TopP = _config.TopP,
			ApiUrl = _config.ApiUrl,
			ApiKey = _config.ApiKey,
			TimeoutMs = _config.TimeoutMs
		};
		
		return new LLMClient( newConfig );
	}
}