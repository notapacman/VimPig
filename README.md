# VimPig
## Description
VimPig is a simple text editor inspired by Vim. It allows text editing using keyboard shortcuts and command-line input, emulating some of Vim's core features. This editor supports text editing operations such as line deletion, copying, pasting, searching, and text replacement, as well as commands for navigating through the text and managing files.

# Features
- Command-Line Mode: Switch between editing mode and command-line mode using Ctrl + O.
- Keyboard Shortcuts: Manage text and navigate lines using keyboard shortcuts.
- Commands: Execute commands for text editing and file management.
- Search and Replace: Search for and replace text in the file.
- File Saving and Opening: Open existing files and save current content.
# Commands
- `:w` — Save the current file. If no file was opened, you will be prompted to choose a location to save.
- `:q` — Close the application.
- `:wq` — Save the current file and close the application.
- `:e <file_path>` — Open the file at the specified path.
- `:dd` — Delete the current line.
- `:yy` — Copy the current line to the clipboard.
- `:gg` — Go to the beginning of the file.
- `:G` — Go to the end of the file.
- `/:<search`> — Start searching for text.
- `:r <search> <replace>` — Replace text in the document.
- `:s <position>` — Split the line at the specified position.
# Hotkeys
- `Ctrl + P` - open Editor Help
- `Ctrl + E (in editor help)` - close editor help
- `Ctrl + O` - open command line
# Usage
1. Running the Application:
   - Compile the project and run the executable file. The text editor will open.
2. Switching to Command-Line Mode:
   - Press Ctrl + O to switch to command-line mode. Enter a command and press Enter to execute it.
3. Working with Text:
   - Use keyboard shortcuts to navigate text and perform editing operations. In command mode, you can use commands to manage text and files.
4. Saving and Opening Files:
   - If a file is already opened, changes will be saved automatically when using the :w command. To open a new file, use the :e <file_path> command.
5. Search and Replace:
   - Use the command /:<search> to start searching for text. The :n and :N commands help navigate between occurrences. Use the :r <search> <replace> command to replace text.
