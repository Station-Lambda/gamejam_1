# GAME.md - Cult Influence Simulator

## Game Overview

A third-person cult management game where players spread ideas and influence NPCs to build a following. Created for a game jam with the theme "Replication".

## Core Concept

- **Genre**: Social manipulation simulator
- **Perspective**: Third-person view
- **Setting**: Colorful, cartoon-style world
- **Theme**: Replication (game jam theme)

## Gameplay Mechanics

### Influence System
- Players interact with NPCs through dialogue using OpenRouter integration
- Conversations gradually influence NPCs to adopt new beliefs and behaviors
- Successfully influenced NPCs become followers of the cult

### NPC System
- Each NPC has:
  - Basic personality traits
  - Memory of past interactions
  - Simple daily routines (work, social, sleep)
- Influenced NPCs develop:
  - Verbal tics and catchphrases
  - Modified behaviors and attitudes
  - Tendency to spread ideas to others

### Progression
- Start with a single player character
- Recruit NPCs one by one through conversation
- Converted NPCs help spread the cult's influence
- No complex hierarchy in V1

## Win Conditions
- **Victory**: Recruit 50 followers to the cult
- **Core Loop**: Find new NPCs → Influence through dialogue → Watch them spread ideas → Build the cult

## Technical Implementation
- **Dialogue**: OpenRouter API for dynamic NPC conversations
- **AI Behavior**: Simple behavior trees for NPC routines
- **Multiplayer**: 1-64 players supported (s&box platform)

## Visual Style
- Fun, colorful cartoon aesthetic
- Accessible and non-threatening appearance
- Clear visual feedback for influenced NPCs

## Version 1 Scope
- Basic influence mechanics through dialogue
- Simple NPC routines
- 50 follower win condition
- No complex hierarchy or resource management
- Focus on replication of ideas and behaviors