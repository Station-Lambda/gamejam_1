using System;
using Sandbox.BehaviourTree;

namespace Sandbox;

public class AiCharacter : Component
{
	[Property] public bool ShowDebug { get; set; } = true;
	[Property] public float PatrolRadius { get; set; } = 500f;
	[Property] public float MoveSpeed { get; set; } = 100f;
	[Property] public float TargetThreshold { get; set; } = 5f;
	[Property] public float MinWaitTime { get; set; } = 1f;
	[Property] public float MaxWaitTime { get; set; } = 5f;

	private Node _behaviourTree;
	private BehaviourTreeContext _context;
	private Vector3 _targetPosition;
	private bool _hasTarget;

	protected override void OnStart()
	{
		_behaviourTree = BuildBehaviourTree();
		_context = new BehaviourTreeContext();
	}

	protected override void OnUpdate()
	{
		_behaviourTree.Execute( _context );

		if ( ShowDebug && _context.LastExecutedNode != null )
		{
			ShowDebugInfo();
		}
	}

	private void ShowDebugInfo()
	{
		var debugText = $"{_context.CurrentPath}\nStatus: {_context.LastNodeStatus}";
		var transform = Transform.World;
		transform.Position += Vector3.Up * 100;
		transform.Rotation = transform.Rotation.RotateAroundAxis( Vector3.Forward, 90 );
		transform.Rotation = transform.Rotation.RotateAroundAxis( Vector3.Left, 90 );
		Gizmo.Draw.WorldText( debugText, transform );
	}

	private Node BuildBehaviourTree()
	{
		var root = new SelectorNode();

		var findNewTargetSequence = new SequenceNode();
		findNewTargetSequence.AddChild( new ConditionNode( ShouldFindNewTarget, "NeedNewTarget?" ) );
		findNewTargetSequence.AddChild( new ActionNode( FindNewTargetPosition, "FindNewTarget" ) );
		findNewTargetSequence.AddChild( new TimerNode( GetRandomWaitTime ) );

		var moveToTargetSequence = new SequenceNode();
		moveToTargetSequence.AddChild( new ConditionNode( HasTarget, "HasTarget?" ) );
		moveToTargetSequence.AddChild( new ActionNode( MoveToTarget, "MoveToTarget" ) );

		root.AddChild( findNewTargetSequence );
		root.AddChild( moveToTargetSequence );

		return root;
	}

	private bool ShouldFindNewTarget()
	{
		return !_hasTarget || IsAtTarget();
	}

	private bool IsAtTarget()
	{
		if ( !_hasTarget )
			return false;

		return _targetPosition.Distance( WorldPosition ) <= TargetThreshold;
	}

	private bool HasTarget()
	{
		return _hasTarget;
	}

	private NodeStatus FindNewTargetPosition()
	{
		var randomOffset = new Vector3(
			Random.Shared.Float( -PatrolRadius, PatrolRadius ),
			Random.Shared.Float( -PatrolRadius, PatrolRadius ),
			0
		);

		_targetPosition = WorldPosition + randomOffset;
		_hasTarget = true;

		return NodeStatus.Success;
	}

	private NodeStatus MoveToTarget()
	{
		var direction = (_targetPosition - WorldPosition).Normal;
		var movement = direction * MoveSpeed * Time.Delta;
		WorldPosition += movement;

		if ( IsAtTarget() )
		{
			_hasTarget = false;
			return NodeStatus.Success;
		}

		return NodeStatus.Running;
	}

	private float GetRandomWaitTime()
	{
		return Random.Shared.Float( MinWaitTime, MaxWaitTime );
	}
}
