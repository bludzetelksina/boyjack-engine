# BoyJack Engine v0.1.0

## Overview

BoyJack Engine is a simple yet powerful 2D game engine designed for beginner game development enthusiasts. Built on .NET 8.0, it provides a complete game development framework with custom scripting capabilities, 2D graphics rendering, and scene management. The engine is structured around three core modules: BJG (graphics), BJGSH (game shell/core), and BJS (custom scripting language).

## User Preferences

Preferred communication style: Simple, everyday language.

## System Architecture

### Core Engine Structure
The engine follows a modular architecture with three main components:

**BJG (BoyJack Graphics Module)**
- Handles 2D graphics rendering including sprites, textures, animations, and cameras
- Built on top of System.Drawing and Windows Forms Graphics
- Supports transparency, layering, and basic visual effects (fade, scale)
- Provides texture management and sprite rendering capabilities

**BJGSH (BoyJack Game Shell)**
- Core engine module managing the game loop (Update/Draw cycles at 60 FPS)
- Handles event processing, timers, and game state management (menu, game, pause)
- Integrates with Windows Forms for input capture (keyboard, mouse) and window management
- Implements a Scene Manager system for organizing game content

**BJS (BoyJack Script)**
- Custom domain-specific language (DSL) resembling a mix of Lua and BASIC
- Built-in interpreter for executing game scripts
- Provides simple syntax for game logic, event handling, and asset management

### Platform Integration
- Built on Windows Forms for cross-platform compatibility within .NET ecosystem
- Uses System.Media.SoundPlayer for basic WAV audio playback
- Leverages .NET 8.0 runtime for modern performance and features

### Game Development Features
- 60 FPS game loop with Update/Draw cycle separation
- Sprite system with position, scale, and rotation support
- Basic AABB (Axis-Aligned Bounding Box) collision detection
- Asset loading for images (PNG, JPG) and sounds (WAV)
- Input handling for keyboard and mouse interactions
- Scene management for organizing different game states

### Rendering Pipeline
- 2D rendering system built on Windows Forms Graphics
- Support for sprite batching and layered rendering
- Basic visual effects and transformations
- Texture management for efficient asset handling

## External Dependencies

### Core Framework
- **.NET 8.0**: Primary runtime environment providing cross-platform compatibility
- **System.Drawing**: Used for 2D graphics operations and image handling
- **Windows Forms**: Provides windowing system, input handling, and graphics context
- **System.Media**: Handles basic audio playback functionality

### Asset Support
- **Image Formats**: PNG and JPG support through .NET's built-in image handling
- **Audio Formats**: WAV file support via System.Media.SoundPlayer

### Development Tools
- **NuGet Package Manager**: For dependency management (currently no external packages)
- **MSBuild**: For project compilation and build management

The engine is designed to be lightweight with minimal external dependencies, relying primarily on .NET's built-in capabilities to provide a complete game development environment.