
# Spatial_ClassRoom

A Unity-based virtual reality classroom project. This repository contains all assets, scripts, and configuration files needed to build and run the VR classroom experience.

## Features

- Immersive classroom environment
- Customizable scenes and prefabs
- Networked attendee management
- Teleportation and interaction scripts
- WebGL export support

## Getting Started

### Prerequisites

- Unity Hub (recommended)
- Unity Editor (version 2021.3 LTS or newer)
- Git (for cloning the repository)

### Installation

1. Clone the repository:
	```sh
	git clone https://github.com/Mohib997/Spatial_ClassRoom.git
	```
2. Open Unity Hub and add the cloned folder (`Spatial_ClassRoom`).
3. Open the project in Unity Editor.
4. Let Unity import all assets and packages (may take a few minutes).

### Running the Project

1. In Unity Editor, open the main scene from `Assets/Scenes/`.
2. Press the Play button to run the VR classroom locally.
3. For WebGL build, go to `File > Build Settings`, select WebGL, and click Build.

## Connecting to Spatial.io

To connect your Unity project to Spatial.io:

1. Go to [Spatial.io](https://spatial.io/) and create an account (if you don't have one).
2. In Unity, open the Spatial SDK documentation and follow the setup instructions:
	- Download and import the Spatial SDK Unity package from the official website or Unity Asset Store.
	- Add the required Spatial components to your scene (see SDK docs for details).
	- Configure your Spatial project settings (API keys, environment, etc.) as described in the SDK documentation.
3. Build and run your project. You should now be able to connect and interact with Spatial.io spaces from your Unity app.
4. For more advanced features (avatars, networking, etc.), refer to the official [Spatial Unity SDK documentation](https://docs.spatial.io/unity-sdk/).

## Folder Structure

- `Assets/` - Main project assets, scripts, scenes, prefabs
- `Packages/` - Unity package manifest
- `ProjectSettings/` - Unity project settings
- `Exports/` - Exported builds and packages

## Technologies Used

- Unity (C#)
- ShaderLab
- HLSL

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License.

## Author

- Mohib997


## Working & Responsibilities of Scripts
**AttendeesManager.cs**
-	Manages the presence and state of users (attendees) in a virtual environment.
-	Listens for user join/leave events and updates local data structures and UI accordingly.
-	Maintains a dictionary of active actors and prevents duplicate attendee entries.
-	Handles network events to synchronize attendee information across all clients.
-	Integrates with other manager scripts for coordinated attendee and post data management.
-	Acts as a central controller for all attendee-related logic.
---
**Room.cs**
-	Represents a virtual room where users can join, leave, and interact.
-	Manages posts and attendees specific to the room.
-	Handles UI logic for post creation, attendee entry, and input field management.
-	Provides methods for adding/removing attendees and posts in response to network or user events.
-	Integrates with attendee and post entry components for dynamic UI updates.
---
**TeleportHandler.cs**
-	Handles user teleportation (joining and leaving rooms) within the virtual environment.
-	Listens for join/leave button events and triggers room entry/exit logic.
-	Notifies other systems when a user enters or leaves a room.
-	Updates the user's position and orientation in the virtual space accordingly.
---
**AttendeesNetworkManager.cs**
-	Manages network communication related to attendees.
-	Sends and receives network events for attendee join/leave actions.
-	Notifies all or specific clients about attendee state changes.
-	Ensures attendee data is synchronized across the network.
---
**PostNetworkManager.cs**
-	Handles network communication for post-related actions.
-	Sends and receives events for post creation and updates.
-	Synchronizes post data across all clients or specific users.
-	Provides methods to broadcast post information to the network.
---
**LeaveRoom.cs**
-	Represents the logic for a user leaving a room.
-	Provides an interface for UI interaction to trigger room exit.
-	Notifies other systems when a user requests to leave a room.
---
**JoinRoom.cs**
-	Represents the logic for a user joining a room.
-	Handles UI interaction for room entry, including visual feedback (e.g., button blinking).
-	Notifies other systems when a user requests to join a room.
---
**PostViewerEntry.cs**
-	Represents a single post entry in the UI.
-	Displays post title, content, author, and room number.
-	Handles UI interaction for viewing or editing post details.
-	Integrates with the room and post management systems.
---
**ActorData.cs**
-	Stores and manages data related to a user (actor) in the environment.
-	Handles assignment of actor properties and downloading of profile pictures.
-	Provides event handling for avatar changes and data updates.
---
**AttendeesEntry.cs**
-	Represents a single attendee entry in the UI.
-	Displays attendee name, profile picture, room number, and join time.
-	Handles UI interaction for attendee-related actions.
---
**RaycastHandler.cs**
-	Handles raycast-based UI interaction in the virtual environment.
-	Detects user clicks and triggers actions on UI elements implementing the IUIRaycastable interface.
-	Provides a generic interface for making UI elements interactable via raycasting.

