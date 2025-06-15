using System;

namespace Sandbox.LLM;

/// <summary>
/// Represents a message in an LLM conversation.
/// Following the standard role-based message format used by most LLM APIs.
/// </summary>
public struct Message
{
	/// <summary>
	/// The role of the message sender.
	/// Standard values: "system", "user", "assistant"
	/// - system: Sets behavior and context for the AI
	/// - user: Input from the user/player
	/// - assistant: Response from the AI
	/// </summary>
	public string Role { get; set; }
	
	/// <summary>
	/// The actual content of the message
	/// </summary>
	public string Content { get; set; }
	
	/// <summary>
	/// Optional timestamp for when the message was created
	/// </summary>
	public DateTime? Timestamp { get; set; }
	
	/// <summary>
	/// Creates a new message with the current timestamp
	/// </summary>
	public static Message Create( string role, string content )
	{
		return new Message
		{
			Role = role,
			Content = content,
			Timestamp = DateTime.UtcNow
		};
	}
	
	/// <summary>
	/// Creates a system message
	/// </summary>
	public static Message System( string content ) => Create( "system", content );
	
	/// <summary>
	/// Creates a user message
	/// </summary>
	public static Message User( string content ) => Create( "user", content );
	
	/// <summary>
	/// Creates an assistant message
	/// </summary>
	public static Message Assistant( string content ) => Create( "assistant", content );
}