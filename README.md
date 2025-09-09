
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

For any questions or issues, please use the GitHub Issues tab.
