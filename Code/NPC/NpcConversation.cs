using System.Collections.Generic;
using System.Threading.Tasks;
using Sandbox.LLM;

namespace Sandbox;

/// <summary>
/// Component that manages LLM-powered conversations for NPCs.
/// Handles thread creation, message management, and player interactions.
/// </summary>
public sealed class NpcConversation : Component
{
	[Property] public NpcProfile Profile { get; set; }
	
	[Property] public bool EnableConversation { get; set; } = true;
	
	[Property] public float ConversationRange { get; set; } = 200f;
	
	[Property, Group( "Advanced" )] public int MaxMessageHistory { get; set; } = 20;
	
	[Property, Group( "Advanced" )] public float Temperature { get; set; } = 0.8f;
	
	private Thread _thread;
	private bool _isConversing;
	
	/// <summary>
	/// Event fired when a conversation starts
	/// </summary>
	public event System.Action<GameObject> ConversationStarted;
	
	/// <summary>
	/// Event fired when a conversation ends
	/// </summary>
	public event System.Action<string> ConversationEnded;
	
	protected override void OnStart()
	{
		base.OnStart();
		
		if ( Profile == null )
		{
			Profile = Components.Get<NpcProfile>();
		}
		
		if ( Profile != null && EnableConversation )
		{
			InitializeThread();
		}
	}
	
	private void InitializeThread()
	{
		var metadata = new Dictionary<string, string>
		{
			{ "npc_id", GameObject.Id.ToString() },
			{ "npc_name", Profile.Name },
			{ "npc_gender", Profile.Gender.ToString() }
		};
		
		_thread = new Thread( Profile.Context, metadata );
		
		_thread.MessageAdded += OnMessageAdded;
		_thread.ThreadUpdated += OnThreadUpdated;
	}
	
	/// <summary>
	/// Sends a message to the NPC and gets a response
	/// </summary>
	/// <param name="message">The message to send</param>
	/// <returns>The NPC's response</returns>
	public async Task<string> SendMessage( string message )
	{
		if ( _thread == null || !EnableConversation )
			return "Je ne peux pas parler maintenant.";
		
		if ( !_isConversing )
		{
			_isConversing = true;
			ConversationStarted?.Invoke( GameObject );
		}
		
		// Manage message history to prevent token overflow
		if ( _thread.Messages.Count > MaxMessageHistory )
		{
			_thread.TruncateHistory( MaxMessageHistory );
		}
		
		try
		{
			var response = await _thread.SendUserMessage( message );
			return response.Content;
		}
		catch ( System.Exception ex )
		{
			Log.Error( $"NpcConversation error: {ex.Message}" );
			return "Désolé, je suis un peu confus en ce moment...";
		}
	}
	
	/// <summary>
	/// Ends the current conversation and generates a summary
	/// </summary>
	/// <returns>A summary of the conversation from the NPC's perspective</returns>
	public async Task<string> EndConversation()
	{
		if ( _thread == null || !_isConversing )
			return null;
		
		_isConversing = false;
		
		var summary = await _thread.GenerateSummary();
		
		// Add the summary to the NPC's memory if it's meaningful
		if ( !string.IsNullOrEmpty( summary ) && Profile != null )
		{
			Profile.Memory.Add( summary );
			
			// Keep only the last 10 memories
			if ( Profile.Memory.Count > 10 )
			{
				Profile.Memory.RemoveAt( 0 );
			}
		}
		
		ConversationEnded?.Invoke( summary );
		
		// Clear conversation history but keep system prompt
		_thread.ClearHistory();
		
		return summary;
	}
	
	/// <summary>
	/// Checks if a player is within conversation range
	/// </summary>
	/// <param name="player">The player GameObject to check</param>
	/// <returns>True if the player is in range</returns>
	public bool IsPlayerInRange( GameObject player )
	{
		if ( player == null ) return false;
		
		var distance = Vector3.DistanceBetween( GameObject.WorldPosition, player.WorldPosition );
		return distance <= ConversationRange;
	}
	
	/// <summary>
	/// Gets the current conversation thread
	/// </summary>
	public Thread GetThread() => _thread;
	
	/// <summary>
	/// Updates the NPC's emotional state based on conversation
	/// </summary>
	/// <param name="emotion">The new emotion</param>
	public void UpdateEmotion( string emotion )
	{
		if ( Profile != null )
		{
			Profile.CurrentEmotion = emotion;
		}
	}
	
	private void OnMessageAdded( Thread thread, Message message )
	{
		if ( message.Role == "assistant" )
		{
			Log.Info( $"{Profile?.Name ?? "NPC"}: {message.Content}" );
		}
	}
	
	private void OnThreadUpdated( Thread thread )
	{
		// Could be used for autosave or other thread management
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
		
		if ( _thread != null )
		{
			_thread.MessageAdded -= OnMessageAdded;
			_thread.ThreadUpdated -= OnThreadUpdated;
		}
	}
	
	protected override void DrawGizmos()
	{
		base.DrawGizmos();
		
		if ( !EnableConversation ) return;
		
		// Draw conversation range
		using ( Gizmo.Scope( "conversation_range" ) )
		{
			Gizmo.Draw.Color = _isConversing ? Color.Green.WithAlpha( 0.3f ) : Color.Cyan.WithAlpha( 0.2f );
			Gizmo.Draw.LineSphere( Vector3.Zero, ConversationRange );
		}
	}
}