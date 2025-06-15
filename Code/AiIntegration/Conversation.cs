using System.Threading.Tasks;

namespace Sandbox.AiIntegration;

public class Conversation
{
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
		Log.Info(identifier + " initialized");
	}

	public async Task<Message> AddMessage( string identifier, Message message )
	{
		Log.Info("Sending message: " + message.Content);
		_conversations[identifier].Add(message);
		var response = await _httpBrain.RequestToIa( _conversations[identifier] );
		var messageResponse = new Message()
		{
			Role = "assistant",
			Content = response
		};
		_conversations[identifier].Add(messageResponse);
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
}
