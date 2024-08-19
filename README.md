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
`:w` - Save the current file. If no file was opened, you will be prompted to choose a location to save.\n
`:q` - Close the application.
`:wq` - Save the current file and then close the application.
`:e` - Open a file dialog for selecting a file to open.
`:delete <lineNumber>` - Delete the specified line. Replace <lineNumber> with the line number you wish to delete.
`:script` - Open a file dialog to select a script file and execute its commands sequentially.
`:history` - Display the command history in a message box.
`:shell <command>` - Execute the specified shell command and display the output.
`:uppercase <lineNumber>` - Convert the specified line to uppercase. Replace <lineNumber> with the line number.
`:clear` - Clear all text in the editor.
`:lowercase <lineNumber>` - Convert the specified line to lowercase. Replace <lineNumber> with the line number.
`:copy <lineNumber>` - Copy the specified line to the clipboard. Replace <lineNumber> with the line number.
`:dd` - Delete the current line.
`:yy` - Copy the current line to the clipboard.
`:gg` - Move the cursor to the beginning of the text.
`:G` - Move the cursor to the end of the text.
`:uncomment <lineNumber>` - Remove comments from the specified line. Replace <lineNumber> with the line number.
`:comment <lineNumber>` - Add comments to the specified line. Replace <lineNumber> with the line number.
`:move <sourceLine> <destLine>` - Move a line from sourceLine to destLine. Replace <sourceLine> and <destLine> with the appropriate line numbers.
`/:<search>` - Start searching for <search> text in the document.
`:r <search> <replace>` - Replace occurrences of <search> with <replace> text in the document.
`:clean` - Remove all empty lines from the text.
`:insert <lineNumber> <text>` - Insert a new line with the specified text at lineNumber. Replace <lineNumber> with the line number and <text> with the content.
`:remove <lineNumber>` - Remove the line at lineNumber. Replace <lineNumber> with the line number.
`:timestamp` - Insert the current timestamp at the cursor's position.
`:outdent <spaces>` - Outdent lines by the specified number of spaces. Replace <spaces> with the number of spaces.

:paste <lineNumber> - Paste the clipboard content at the specified lineNumber. Replace <lineNumber> with the line number.
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
