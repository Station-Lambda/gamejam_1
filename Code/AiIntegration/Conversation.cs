using System;
using System.Threading.Tasks;

namespace Sandbox.AiIntegration;

/// <summary>
/// Manages AI conversations with multiple identifiers and message history
/// </summary>
public sealed class Conversation
{
	/// <summary>
	/// Event fired when any conversation updates
	/// </summary>
	public event Action<string> OnConversationUpdate;

	private readonly Dictionary<string, List<Message>> _conversations = new();
	private readonly HttpBrain _httpBrain = new();


	/// <summary>
	/// Starts a new conversation with system context
	/// </summary>
	public void StartConversation( string identifier, string context )
	{
		var contextBase = new List<Message>
		{
			new()
			{
				Role = "system",
				Content = context
			}
		};

		_conversations.Add( identifier, contextBase );
		OnConversationUpdate?.Invoke( identifier );
	}

	/// <summary>
	/// Adds a message and gets AI response
	/// </summary>
	public async Task<Message> AddMessage( string identifier, Message message )
	{
		if ( !_conversations.TryGetValue( identifier, out var conversation ) )
			return default;

		conversation.Add( message );
		OnConversationUpdate?.Invoke( identifier );

		var response = await _httpBrain.RequestToIa( conversation );
		var messageResponse = new Message
		{
			Role = "assistant",
			Content = response
		};

		conversation.Add( messageResponse );
		OnConversationUpdate?.Invoke( identifier );

		return messageResponse;
	}

	/// <summary>
	/// Checks if conversation exists
	/// </summary>
	public bool HasConversations( string identifier )
	{
		return _conversations.ContainsKey( identifier );
	}

	/// <summary>
	/// Gets all messages for a conversation
	/// </summary>
	public List<Message> ListMessages( string identifier )
	{
		return _conversations.TryGetValue( identifier, out var messages ) ? messages : new List<Message>();
	}

	/// <summary>
	/// Gets all conversation identifiers
	/// </summary>
	public List<string> ListConversations()
	{
		return _conversations.Keys.ToList();
	}
}
