using System;

namespace Sandbox.AiIntegration;

/// <summary>
/// Component that manages AI-powered character conversations and personality
/// </summary>
public sealed class Character : Component
{
	private readonly Conversation _conversation = new();

	/// <summary>
	/// Character's name displayed in conversations
	/// </summary>
	[Property] public string Name { get; set; }

	/// <summary>
	/// World context and situation the character exists in
	/// </summary>
	[Property] public string Context { get; set; }

	/// <summary>
	/// Personality traits, beliefs, fears and motivations
	/// </summary>
	[Property] public string Personality { get; set; }

	/// <summary>
	/// Character's objective in interactions
	/// </summary>
	[Property] public string Goal { get; set; }

	/// <summary>
	/// Language style (formal/casual, serious/ironic, etc.)
	/// </summary>
	[Property] public string LanguageStyle { get; set; }

	/// <summary>
	/// Restrictions on what the character cannot do or say
	/// </summary>
	[Property] public string Restriction { get; set; }
	
	/// <summary>
	/// Debug property for testing new messages in editor
	/// </summary>
	[Property] public string NewMessage { get; set; }

	/// <summary>
	/// Debug trigger to send test messages
	/// </summary>
	[Property] public bool SendMessage { get; set; }

	/// <summary>
	/// Event fired when conversation updates with conversation identifier
	/// </summary>
	public event Action<string> OnConversationUpdate;

	/// <summary>
	/// Builds the AI context prompt from character properties
	/// </summary>
	private string GetContext()
	{
		return $"""
		        Agis comme un personnage fictif. Voici les instructions :
		        
		        Nom du personnage : '{Name}'
		        
		        Contexte : '{Context}'
		        
		        Personnalité : '{Personality}'
		        
		        Objectif : '{Goal}'
		        
		        Style de communication : '{LanguageStyle}'
		        
		        Restrictions : '{Restriction}'
		        
		        """;
	}

	/// <summary>
	/// Starts a new conversation with given identifier
	/// </summary>
	public void StartConversation( string identifier )
	{
		_conversation.StartConversation( identifier, GetContext() );
	}

	protected override void OnStart()
	{
		_conversation.OnConversationUpdate += OnConversationUpdate;
	}

	protected override void OnUpdate()
	{
		if ( !SendMessage )
			return;

		SendMessage = false;

		if ( !_conversation.HasConversations( "test" ) )
			return;

		_ = _conversation.AddMessage( "test", new Message { Role = "user", Content = NewMessage } );
	}
}
