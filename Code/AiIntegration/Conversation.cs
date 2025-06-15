using System;
using System.Threading.Tasks;

namespace Sandbox.AiIntegration;

public class Conversation
{
	public event Action<string> OnConversationUpdate;
	
	
	private Dictionary<string, List<Message>> _conversations = new();
	private HttpBrain _httpBrain = new();


	public void StartConversation(string identifier,string context)
	{
		List<Message> contextBase = new();
		contextBase.Add(new Message()
		{
			Role = "system",
			Content = context
		});
		
		_conversations.Add( identifier, contextBase);
		OnConversationUpdate?.Invoke(identifier);
	}

	public async Task<Message> AddMessage( string identifier, Message message )
	{
		_conversations[identifier].Add(message);
		OnConversationUpdate?.Invoke(identifier);
		var response = await _httpBrain.RequestToIa( _conversations[identifier] );
		var messageResponse = new Message()
		{
			Role = "assistant",
			Content = response
		};
		_conversations[identifier].Add(messageResponse);
		OnConversationUpdate?.Invoke(identifier);
		return messageResponse;
	}

	public bool HasConversations(string identifier)
	{
		return _conversations.ContainsKey(identifier);
	}

	public List<Message> ListMessages(string identifier)
	{
		return _conversations[identifier];
	}

	public List<string> ListConversations()
	{
		return _conversations.Keys.ToList();
	}
	
	public async Task<string> CloseAndSummarizeConversation(string identifier)
	{
		var conversation = _conversations[identifier];
		_conversations.Remove(identifier);
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
		return await _httpBrain.RequestToIa( conversation );
	}
}
