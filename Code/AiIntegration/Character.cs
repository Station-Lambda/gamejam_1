using System;

namespace Sandbox.AiIntegration;

public class Character : Component
{
	private Conversation _conversation = new Conversation();
	
	[Property] public string Name { get; set; } // [Nom du personnage]
	[Property] public string Context { get; set; } // [Brève description du monde ou de la situation dans laquelle il/elle évolue]
	[Property] public string Personality { get; set; } // [Traits dominants, manière de parler, attitude, croyances, peurs, motivations]
	[Property] public string Goal { get; set; } // [Ce que le personnage veut accomplir dans cette interaction]
	[Property] public string LanguageStyle { get; set; } // [Langage familier/soutenu, ironique/sérieux, rapide/lent, etc.]
	[Property] public string Restriction { get; set; } // [Ce que le personnage ne doit pas faire ou dire]
	
	[Property] public string NewMessage { get; set; }
	[Property] public bool SendMessage { get; set; }
	
	public event Action<string> OnConversationUpdate;

	public string GetContext()
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

	public void StartConversation(string identifier)
	{
		_conversation.StartConversation(identifier, GetContext());
	}

	public List<string> GetConversation( List<string> identifiers )
	{
		return _conversation.ListConversations();
	}
	
	protected override void OnStart()
	{
		// _conversation.StartConversation("test", GetContext());
		_conversation.OnConversationUpdate += OnConversationUpdate;
	}

	protected override void OnUpdate()
	{
		if ( SendMessage )
		{
			SendMessage = false;
			if ( _conversation.HasConversations("test"))
				_conversation.AddMessage( "test", new Message() { Role = "user", Content = NewMessage } );
		}
	}
}
	
