# Save System Wrapper

## Overview
Save System Wrapper (SSW) is a robust and flexible save system for Unity game development. It provides an easy-to-use API for saving and loading game data, ensuring data integrity and consistency across different save systems.

## Features
- **Cross-Platform Support**: Works seamlessly on Windows, macOS, Linux, and mobile platforms (availbale functionality depends on specific save module you are using).
- **Customizable**: Easily extendable to fit the specific needs of your game.

## Installation
After installing into project this solutions is ready to go.

## Usage
From scratch SSW uses PlayerPrefs to save data, supported types: string, int, bool;
Here is a basic example of how to use SSW in your project(currently used save system platform should support custom classes serialization):

### Saving Data
```csharp
using Game.SaveSystem.Core;

var saveData = new SaveData();
saveData.PlayerName = "Player1";
saveData.Score = 1000;

SaveManager.Save("saveDataKey", saveData);
```

### Loading Data
```csharp
using Game.SaveSystem.Core;

var saveData = SaveSystem.Load<SaveData>("saveDataKey", defaultValue: new SaveData());
Console.WriteLine($"Player Name: {saveData.PlayerName}, Score: {saveData.Score}");
```

## Documentation
For detailed documentation and advanced usage, please refer to this repository wiki.

## Contributing
We welcome contributions to improve SSW.

## License
SSW is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

## Contact
For any questions or support, please contact us at lol80672@gmail.com.
