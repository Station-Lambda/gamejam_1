using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Sandbox;

namespace Sandbox.AiIntegration;

public struct Message
{
	public string role { get; set; }
	public string content { get; set; }
	
	// refusal: null,
	// reasoning: null
}

public struct Choice
{
	// logprobs: null,
	public string finish_reason { get; set; }
	public string native_finish_reason { get; set; }
	public int Index { get; set; }
	
	public Message Message { get; set; }
}

public struct HttpBody
{
	public string Model { get; set; }
	public List<Message> Messages { get; set; }
}

public struct HttpResponse
{
	public string Id { get; set; }
	public string Provider { get; set; }
	public string Model { get; set; }
	
	public string Object { get; set; }
	public int Created { get; set; }
	public List<Choice> Choices { get; set; }
}

public class HttpBrain
{
	const string API_KEY = "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";

	public async Task<string> RequestToIa(List<Message> messages )
	{
		var headers = new Dictionary<string, string>();
		headers.Add("Authorization", "Bearer " + API_KEY);
		
		var httpBody = new HttpBody();
		httpBody.Model = "deepseek/deepseek-chat-v3-0324:free";
		httpBody.Messages = messages;
		
		var response = await Http.RequestJsonAsync<HttpResponse>( "https://openrouter.ai/api/v1/chat/completions", "POST", Http.CreateJsonContent( httpBody ), headers);
		
		var choice =  response.Choices[0];
		var message = choice.Message;
		
		Log.Info(" Message : " + message.content);
		
		return message.content;
	}
}
