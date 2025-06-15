using System.Collections.Generic;
using System.Linq;

namespace Sandbox;

public sealed class ChatManager : Component
{
	[Property] public ChatPanel ChatPanel { get; set; }
	
	private static ChatManager _instance;
	public static ChatManager Instance => _instance;
	
	private Dictionary<string, ConversationData> conversations = new();
	
	public class ConversationData
	{
		public string NpcId { get; set; }
		public List<ChatPanel.ChatMessage> Messages { get; set; } = new();
		public int CurrentDialogueIndex { get; set; } = 0;
	}
	
	protected override void OnAwake()
	{
		_instance = this;
		
		if ( !ChatPanel.IsValid() )
		{
			var panelGo = Scene.CreateObject();
			panelGo.Name = "ChatPanel";
			ChatPanel = panelGo.Components.Create<ChatPanel>();
		}
	}
	
	protected override void OnUpdate()
	{
		if ( Input.Pressed( "chat" ) )
		{
			if ( ChatPanel.IsVisible )
			{
				ChatPanel.CloseChat();
			}
			else
			{
				var nearestNpc = FindNearestNpc();
				if ( nearestNpc.IsValid() )
				{
					StartConversation( nearestNpc );
				}
			}
		}
	}
	
	public void StartConversation( NpcProfile npc )
	{
		if ( !npc.IsValid() ) return;
		
		var npcId = npc.GameObject.Id.ToString();
		
		if ( !conversations.ContainsKey( npcId ) )
		{
			conversations[npcId] = new ConversationData
			{
				NpcId = npcId
			};
		}
		
		var conversationData = conversations[npcId];
		ChatPanel.OpenChat( npc );
		
		if ( conversationData.Messages.Any() )
		{
			foreach ( var message in conversationData.Messages )
			{
				ChatPanel.Messages.Add( message );
			}
		}
	}
	
	public void SaveMessage( string npcId, string text, bool isPlayer )
	{
		if ( conversations.ContainsKey( npcId ) )
		{
			conversations[npcId].Messages.Add( new ChatPanel.ChatMessage
			{
				Text = text,
				IsPlayer = isPlayer
			} );
		}
	}
	
	private NpcProfile FindNearestNpc()
	{
		var player = Scene.GetAllComponents<Component>()
			.FirstOrDefault( c => c.Tags.Has( "player" ) );
		
		if ( !player.IsValid() ) return null;
		
		var npcs = Scene.GetAllComponents<NpcProfile>();
		NpcProfile nearestNpc = null;
		float nearestDistance = 150f;
		
		foreach ( var npc in npcs )
		{
			if ( !npc.IsValid() ) continue;
			
			var distance = Vector3.DistanceBetween( 
				player.Transform.Position, 
				npc.Transform.Position 
			);
			
			if ( distance < nearestDistance )
			{
				nearestDistance = distance;
				nearestNpc = npc;
			}
		}
		
		return nearestNpc;
	}
}