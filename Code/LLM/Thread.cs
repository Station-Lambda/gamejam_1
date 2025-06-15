using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sandbox.LLM;

/// <summary>
/// Represents a conversation thread following modern LLM conventions.
/// Manages message history, context, and interactions with the LLM API.
/// 
/// This class provides a high-level interface for managing conversational AI threads,
/// similar to OpenAI's thread API structure, allowing for multiple independent conversations
/// with maintained context and history.
/// </summary>
public class Thread
{
	/// <summary>
	/// Unique identifier for this thread
	/// </summary>
	public string Id { get; private set; }
	
	/// <summary>
	/// Collection of messages in this thread, ordered chronologically
	/// </summary>
	public List<Message> Messages { get; private set; } = new();
	
	/// <summary>
	/// Metadata associated with this thread (e.g., NPC name, context)
	/// </summary>
	public Dictionary<string, string> Metadata { get; set; } = new();
	
	/// <summary>
	/// Timestamp when the thread was created
	/// </summary>
	public DateTime CreatedAt { get; private set; }
	
	/// <summary>
	/// Timestamp of the last message in the thread
	/// </summary>
	public DateTime LastMessageAt { get; private set; }
	
	/// <summary>
	/// Event fired when a new message is added to the thread
	/// </summary>
	public event Action<Thread, Message> MessageAdded;
	
	/// <summary>
	/// Event fired when the thread is updated (metadata, etc.)
	/// </summary>
	public event Action<Thread> ThreadUpdated;
	
	private readonly LLMClient _llmClient;
	
	/// <summary>
	/// Creates a new thread with an optional system message
	/// </summary>
	/// <param name="systemPrompt">Initial system prompt to set the context</param>
	/// <param name="metadata">Optional metadata for the thread</param>
	public Thread( string systemPrompt = null, Dictionary<string, string> metadata = null )
	{
		Id = Guid.NewGuid().ToString();
		CreatedAt = DateTime.UtcNow;
		LastMessageAt = CreatedAt;
		_llmClient = new LLMClient();
		
		if ( metadata != null )
		{
			Metadata = metadata;
		}
		
		if ( !string.IsNullOrEmpty( systemPrompt ) )
		{
			AddSystemMessage( systemPrompt );
		}
	}
	
	/// <summary>
	/// Adds a system message to the thread
	/// </summary>
	/// <param name="content">The system message content</param>
	public void AddSystemMessage( string content )
	{
		var message = new Message
		{
			Role = "system",
			Content = content
		};
		
		AddMessage( message );
	}
	
	/// <summary>
	/// Adds a user message to the thread and gets an AI response
	/// </summary>
	/// <param name="content">The user's message</param>
	/// <returns>The AI assistant's response message</returns>
	public async Task<Message> SendUserMessage( string content )
	{
		var userMessage = new Message
		{
			Role = "user",
			Content = content
		};
		
		AddMessage( userMessage );
		
		var response = await _llmClient.CompleteAsync( Messages );
		
		var assistantMessage = new Message
		{
			Role = "assistant",
			Content = response
		};
		
		AddMessage( assistantMessage );
		
		return assistantMessage;
	}
	
	/// <summary>
	/// Adds an assistant message directly (useful for loading history)
	/// </summary>
	/// <param name="content">The assistant's message</param>
	public void AddAssistantMessage( string content )
	{
		var message = new Message
		{
			Role = "assistant",
			Content = content
		};
		
		AddMessage( message );
	}
	
	/// <summary>
	/// Adds a message to the thread
	/// </summary>
	/// <param name="message">The message to add</param>
	private void AddMessage( Message message )
	{
		Messages.Add( message );
		LastMessageAt = DateTime.UtcNow;
		
		MessageAdded?.Invoke( this, message );
		ThreadUpdated?.Invoke( this );
	}
	
	/// <summary>
	/// Gets the last N messages from the thread
	/// </summary>
	/// <param name="count">Number of messages to retrieve</param>
	/// <returns>List of recent messages</returns>
	public List<Message> GetRecentMessages( int count )
	{
		return Messages.TakeLast( count ).ToList();
	}
	
	/// <summary>
	/// Clears all messages except the system message (if any)
	/// </summary>
	public void ClearHistory()
	{
		var systemMessage = Messages.FirstOrDefault( m => m.Role == "system" );
		Messages.Clear();
		
		if ( systemMessage.Role == "system" )
		{
			Messages.Add( systemMessage );
		}
		
		ThreadUpdated?.Invoke( this );
	}
	
	/// <summary>
	/// Truncates the thread to keep only the last N messages (preserving system message)
	/// </summary>
	/// <param name="maxMessages">Maximum number of messages to keep</param>
	public void TruncateHistory( int maxMessages )
	{
		var systemMessage = Messages.FirstOrDefault( m => m.Role == "system" );
		var nonSystemMessages = Messages.Where( m => m.Role != "system" ).ToList();
		
		if ( nonSystemMessages.Count > maxMessages )
		{
			Messages.Clear();
			
			if ( systemMessage.Role == "system" )
			{
				Messages.Add( systemMessage );
			}
			
			Messages.AddRange( nonSystemMessages.TakeLast( maxMessages ) );
			ThreadUpdated?.Invoke( this );
		}
	}
	
	/// <summary>
	/// Generates a summary of the conversation using the LLM
	/// </summary>
	/// <param name="summaryPrompt">Optional custom prompt for summarization</param>
	/// <returns>A summary of the conversation</returns>
	public async Task<string> GenerateSummary( string summaryPrompt = null )
	{
		var prompt = summaryPrompt ?? 
			"""
			Résume cette conversation en quelques phrases du point de vue du personnage.
			Concentre-toi sur les éléments importants, les émotions ressenties et les décisions prises.
			Écris à la première personne comme un souvenir ou une entrée de journal.
			""";
		
		var tempMessages = new List<Message>( Messages )
		{
			new Message
			{
				Role = "user",
				Content = prompt
			}
		};
		
		return await _llmClient.CompleteAsync( tempMessages );
	}
	
	/// <summary>
	/// Exports the thread as a formatted string
	/// </summary>
	/// <returns>Formatted thread content</returns>
	public string Export()
	{
		var export = $"Thread ID: {Id}\n";
		export += $"Created: {CreatedAt:yyyy-MM-dd HH:mm:ss}\n";
		export += $"Last Message: {LastMessageAt:yyyy-MM-dd HH:mm:ss}\n";
		
		if ( Metadata.Count > 0 )
		{
			export += "Metadata:\n";
			foreach ( var kvp in Metadata )
			{
				export += $"  {kvp.Key}: {kvp.Value}\n";
			}
		}
		
		export += "\nMessages:\n";
		foreach ( var msg in Messages )
		{
			export += $"[{msg.Role}]: {msg.Content}\n\n";
		}
		
		return export;
	}
}