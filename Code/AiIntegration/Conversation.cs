using System;
using System.Threading.Tasks;

namespace Sandbox.AiIntegration;

/// <summary>
/// Manages multiple AI conversations with multiple identifiers
/// </summary>
public class Conversation
{
	/// <summary>
	/// Event fired when any conversation updates
	/// </summary>
	public event Action<string> ConversationUpdated;
	
	
	private Dictionary<string, List<Message>> _conversations = new();
	private HttpBrain _httpBrain = new();


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
		ConversationUpdated?.Invoke(identifier);
	}

	/// <summary>
	/// Adds a message to conversation and gets AI response
	/// </summary>
	public async Task<Message> AddMessage( string identifier, Message message )
	{
		if ( !_conversations.TryGetValue( identifier, out var conversation ) )
			return default;
		
		conversation.Add( message );
		ConversationUpdated?.Invoke( identifier );
		
		var response = await _httpBrain.RequestToAi( conversation );
		var messageResponse = new Message()
		{
			Role = "assistant",
			Content = response
		};
		
		conversation.Add( messageResponse );
		ConversationUpdated?.Invoke(identifier);
		
		return messageResponse;
	}

	/// <summary>
	/// Checks if conversation exists
	/// </summary>
	public bool HasConversations(string identifier)
	{
		return _conversations.ContainsKey(identifier);
	}

	/// <summary>
	/// Gets all messages for a conversation
	/// </summary>
	public List<Message> ListMessages(string identifier)
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
	
	/// <summary>
	/// Closes a conversation and returns a summary of the conversation.
	/// </summary>
	public async Task<string> CloseAndSummarizeConversation(string identifier)
	{
		var conversation = _conversations[identifier];
		_conversations.Remove(identifier);
		ConversationUpdated?.Invoke(identifier);
		
		conversation.Add(new Message()
		{
			Role = "system", 
			Content = """
			          C'est la fin de la discution, fais une synthèse brève sous forme d’un événement important dans la vie du personnage (selon son point de vue).
			          Si aucun événement marquant n’a eu lieu, indique simplement qu’il n’y a rien de notable à retenir.
			          
			          Format attendu :
			          
			          Titre de l’événement : [Quelques mots]
			          
			          Résumé : [2 à 4 phrases, à la première personne, dans le style du personnage, avec ses émotions, ses conclusions, ou ce qu’il en retient]
			          
			          Impact : [Une phrase expliquant si cela a changé son humeur, ses idées, ou son quotidien]
			          
			          Reste dans le personnage, et écris cette synthèse comme si elle venait de lui/elle. Elle peut prendre la forme d’un journal intime, d’une note mentale ou d’un souvenir.
			          """
		});
		return await _httpBrain.RequestToAi( conversation );
	}
}
