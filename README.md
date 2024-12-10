# GridNomads

**GridNomads** is a dynamic .NET MAUI application featuring a customizable grid with colorful, interactive nomads that move randomly and leave fading trails. This project is part of an experiment in AI-driven development, with each iteration designed and guided by AI prompts.

## Features
- A **54x96 grid** with dynamic updates and customizable grid size.
- **20 nomads**, evenly divided between red and blue, moving in random directions (N, NE, E, SE, S, SW, W, NW) every 250 milliseconds.
- **Edge wrapping logic** ensures nomads move smoothly across boundaries.
- **Proximity awareness**: Nomads detect and respond to nearby neighbors within 5 cells.
- **Excitement levels**: Nomads change brightness based on their proximity to neighbors.
- **Fading trails**: Nomads leave colorful trails that dim progressively before disappearing.
- **Enhanced visuals**:
  - Vibrant colors for nomads and dynamic brightness based on excitement.
  - Dimmer, fading trails for better contrast.
  - Soft teal background color with thin grid lines for an aesthetically pleasing look.

## AI Experiment
This project is a live demonstration of **AI-driven development**, with all changes guided by iterative AI prompts. The process includes:
1. **Code and README.md generation** by AI.
2. **Commit messages containing the exact prompts** used for changes.
3. **Transparency**: All iterations are documented and available in the git history.

### Iterations
- Initial setup: A **48x27 grid** with basic movement and edge wrapping.
- Enhanced visuals: Added vibrant colors, thin grid lines, and aesthetic adjustments.
- Trail system: Introduced fading trails for nomads.
- Proximity highlighting: Nomads glow when within 5 cells of a neighbor.
- Increased complexity: Grid size doubled, nomad count increased to 20.
- Separation of concerns: Refactored nomad logic to calculate excitement levels and handle proximity awareness.
- Improved rendering: Brightened excited nomads and dimmed trails further for visual distinction.

## Repository URL
Find the repository on GitHub:  
[https://github.com/andrewstellman/GridNomads.git](https://github.com/andrewstellman/GridNomads.git)

## Getting Started
### Prerequisites
- .NET 7.0 SDK or later installed.
- Visual Studio or Visual Studio Code configured for .NET MAUI development.

### Running the App
1. Clone the repository:
   ```bash
   git clone https://github.com/andrewstellman/GridNomads.git
   cd GridNomads
   ```
2. Open the project in your preferred IDE:
* For Visual Studio, open the .sln file.
* For Visual Studio Code, open the project folder.

3. Restore dependencies and build the project:
   ```bash
   dotnet restore
   dotnet build
   ```

4. Run the project:
   ```bash
   dotnet run
   ```

## Contributing
Since this project is an AI experiment, contributions are welcome but should align with the spirit of the project. Feel free to open issues or submit pull requests for improvements or ideas.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.

---

_This README, like the project itself, evolves iteratively through AI-guided development._
