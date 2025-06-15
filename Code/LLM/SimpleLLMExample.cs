using System.Threading.Tasks;

namespace Sandbox.LLM;

/// <summary>
/// Simple example component showing how to use the LLM client
/// </summary>
public sealed class SimpleLLMExample : Component
{
	[Property] public string SystemPrompt { get; set; } = "You are a helpful assistant.";
	[Property] public string TestMessage { get; set; } = "Hello! Can you tell me a joke?";
	
	private LLMClient _client;
	private bool _isProcessing;
	private string _lastResponse;
	
	protected override void OnStart()
	{
		_client = new LLMClient();
	}
	
	protected override void OnUpdate()
	{
		// Press E to send a test message
		if ( Input.Pressed( "use" ) && !_isProcessing )
		{
			_ = SendTestMessage();
		}
	}
	
	private async Task SendTestMessage()
	{
		_isProcessing = true;
		
		try
		{
			Log.Info( $"Sending message: {TestMessage}" );
			_lastResponse = await _client.CompleteAsync( TestMessage, SystemPrompt );
			Log.Info( $"Received response: {_lastResponse}" );
		}
		catch ( System.Exception ex )
		{
			Log.Error( $"Error communicating with LLM: {ex.Message}" );
		}
		finally
		{
			_isProcessing = false;
		}
	}
	
	protected override void DrawGizmos()
	{
		if ( !string.IsNullOrEmpty( _lastResponse ) )
		{
			Gizmo.Draw.ScreenText( $"Last response: {_lastResponse}", new Vector2( 10, 10 ) );
		}
		
		if ( _isProcessing )
		{
			Gizmo.Draw.ScreenText( "Processing...", new Vector2( 10, 30 ) );
		}
	}
}