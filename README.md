# Discord Activity Controller

This C# program intakes Discord tokens and 'fakes' an online Discord status by using headless Selenium Chrome web drivers running in the background. Additionally, you can optionally launch a single token without headless mode to control it. The tokens are provided through a `tokens.txt` file, with one token per line.

## Features

- Fake online Discord status using headless Selenium Chrome web drivers.
- Option to control a specific account without headless mode.
- Easy token management through a `tokens.txt` file.

## Requirements

- .NET Framework
- Selenium WebDriver
- Chrome WebDriver

## Usage

1. **Add your Discord tokens to `tokens.txt`:**
    - Ensure each token is on a new line.
    
    ```
    token1
    token2
    token3
    ...
    ```

2. **Build and run the project:**
    - Open the solution in Visual Studio.
    - Build the solution.
    - Run the project.

3. **Optional: Control a specific account without headless mode:**
    - Modify the relevant section in the code to launch a single token without headless mode for direct control.

## Contact

For any inquiries or issues, please open an issue on the repository.
