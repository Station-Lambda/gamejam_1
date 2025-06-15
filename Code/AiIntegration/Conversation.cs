using System.Threading.Tasks;

namespace Sandbox.AiIntegration;

public class Conversation
{
	private Dictionary<string, List<Message>> conversations = new();
	private HttpBrain httpBrain = new();


	public void StartConversation(string identifier,string context)
	{
		List<Message> contextBase = new();
		contextBase.Add(new Message()
		{
			role = "system",
			content = context
		});
		
		conversations.Add( identifier, contextBase);
		Log.Info(identifier + " initialized");
	}

	public async Task<Message> AddMessage( string identifier, Message message )
	{
		Log.Info("Sending message: " + message.content);
		conversations[identifier].Add(message);
		var response = await httpBrain.RequestToIa( conversations[identifier] );
		var messageResponse = new Message()
		{
			role = "assistant",
			content = response
		};
		conversations[identifier].Add(messageResponse);
		return messageResponse;
	}

	public bool HasConversations(string identifier)
	{
		return conversations.ContainsKey(identifier);
	}

	public List<Message> ListMessages(string identifier)
	{
		return conversations[identifier];
	}
}
