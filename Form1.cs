using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace VimPig
{
    public partial class Form1 : Form
    {
        private List<string> commandHistory = new List<string>();
        private string currentFilePath = string.Empty; // Путь к открытому файлу
        public static bool isCommandMode = false; // Режим командной строки
        private string lastSearchTerm = string.Empty; // Последний поисковый запрос
        private int currentClipboardLine = -1; // Хранит последнюю скопированную строку
        private int lastLineIndex = 0; // Хранит индекс строки перед открытием командной строки

        public Form1()
        {
            InitializeComponent();
            UpdateStatus();
            textBox1.Resize += (sender, e) =>
            {
                SetRoundedShape(textBox1, 15);
            };

            // Устанавливаем начальную форму при создании
            SetRoundedShape(textBox1, 15);
            panel1.Paint += (sender, e) =>
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    // Çàäàíèå ðàäèóñà çàêðóãëåíèÿ
                    int radius = 20;

                    // Ñîçäàíèå ïðÿìîóãîëüíèêà ñ çàêðóãëåííûìè óãëàìè
                    path.StartFigure();
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(panel1.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(panel1.Width - radius, panel1.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, panel1.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    // Ïðèìåíåíèå îáðåçêè
                    panel1.Region = new Region(path);
                }
            };
            panel3.Paint += (sender, e) =>
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    // Çàäàíèå ðàäèóñà çàêðóãëåíèÿ
                    int radius = 20;

                    // Ñîçäàíèå ïðÿìîóãîëüíèêà ñ çàêðóãëåííûìè óãëàìè
                    path.StartFigure();
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(panel3.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(panel3.Width - radius, panel3.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, panel3.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    // Ïðèìåíåíèå îáðåçêè
                    panel3.Region = new Region(path);
                }
            };
            panel2.Paint += (sender, e) =>
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    // Çàäàíèå ðàäèóñà çàêðóãëåíèÿ
                    int radius = 20;

                    // Ñîçäàíèå ïðÿìîóãîëüíèêà ñ çàêðóãëåííûìè óãëàìè
                    path.StartFigure();
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(panel2.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(panel2.Width - radius, panel2.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, panel2.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();

                    // Ïðèìåíåíèå îáðåçêè
                    panel2.Region = new Region(path);
                }
            };
            commandTextBox.Resize += (sender, e) =>
            {
                SetRoundedShape(commandTextBox, 12);
            };

            // Устанавливаем начальную форму при создании
            SetRoundedShape(commandTextBox, 12);
        }
        private void SetRoundedShape(Control control, int radius)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                control.Region = new Region(path);
            }
        }

        private void Scintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                ToggleCommandMode();

                e.SuppressKeyPress = true;
                return;
            }
            if (e.Control && e.KeyCode == Keys.P)
            {
                Help help = new Help();
                help.Show();
                UpdateStattohelp();
                e.SuppressKeyPress = true;
                return;
            }

            if (isCommandMode && e.KeyCode == Keys.Enter)
            {
                ExecuteCommand(commandTextBox.Text);
                commandTextBox.Text = string.Empty;
                e.SuppressKeyPress = true;
                return;
            }


            UpdateStatus();

        }

        private void UpdateStattocomm()
        {
            RPC.SetState("Writing Commands..", true);
        }
        public void UpdateStattodef()
        {
            RPC.SetState("Editing..", true);
        }
        private void UpdateStattohelp()
        {
            RPC.SetState("Looking for help by commands", true);
        }

        private void ToggleCommandMode()
        {
            if (isCommandMode)
            {
                UpdateStattodef();
                // Save the current line before hiding command box
                lastLineIndex = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
            }

            isCommandMode = !isCommandMode;
            commandTextBox.Visible = isCommandMode;

            if (isCommandMode)
            {
                UpdateStattocomm();
                commandTextBox.Focus();

                // Проверка на существование строки и корректность индекса
                if (lastLineIndex >= 0 && lastLineIndex < textBox1.Lines.Length)
                {
                    int lineStart = textBox1.GetFirstCharIndexFromLine(lastLineIndex);
                    if (lineStart != -1)
                    {
                        textBox1.SelectionStart = lineStart;
                        textBox1.SelectionLength = textBox1.Lines[lastLineIndex].Length;
                    }
                }
            }
            else
            {
                textBox1.Focus();

                // Проверка на существование строки и корректность индекса
                if (lastLineIndex >= 0 && lastLineIndex < textBox1.Lines.Length)
                {
                    int lineStart = textBox1.GetFirstCharIndexFromLine(lastLineIndex);
                    if (lineStart != -1)
                    {
                        textBox1.SelectionStart = lineStart;
                    }
                }
            }
        }
        private void ExecuteCommand(string command)
        {
            commandHistory.Add(command);
            command = command.Trim();
            if (command == ":w")
            {
                SaveFile();
            }
            else if (command == ":q")
            {
                Application.Exit();
            }
            else if (command == ":wq")
            {
                SaveFile();
                Application.Exit();
            }
            else if (command == ":e")
            {
                OpenSpecificFile();
            }

            else if (command.StartsWith(":delete "))
            {
                if (int.TryParse(command.Substring(8).Trim(), out int lineNumber))
                {
                    DeleteSpecificLine(lineNumber);
                }
                else
                {
                    MessageBox.Show("Invalid line number.");
                }
            }
            else if (command.StartsWith(":script"))
            {
                ExecuteScript();
            }
            else if (command == ":history")
            {
                MessageBox.Show(string.Join("\n", commandHistory), "Command History");
            }

            else if (command.StartsWith(":shell "))
            {
                string shellCommand = command.Substring(7).Trim();
                ExecuteShellCommand(shellCommand);
            }
            else if (command.StartsWith(":uppercase "))
            {
                if (int.TryParse(command.Substring(11).Trim(), out int lineNumber))
                {
                    UppercaseLine(lineNumber);
                }
                else
                {
                    MessageBox.Show("Invalid line number.");
                }
            }

            else if (command == ":clear")
            {
                textBox1.Clear();
                MessageBox.Show("All text cleared.");
            }
            else if (command.StartsWith(":lowercase "))
            {
                if (int.TryParse(command.Substring(11).Trim(), out int lineNumber))
                {
                    LowercaseLine(lineNumber);
                }
                else
                {
                    MessageBox.Show("Invalid line number.");
                }
            }
            else if (command.StartsWith(":copy "))
            {
                if (int.TryParse(command.Substring(6).Trim(), out int lineNumber))
                {
                    CopyLineToClipboard(lineNumber);
                }
                else
                {
                    MessageBox.Show("Invalid line number.");
                }
            }

            else if (command == ":dd")
            {
                DeleteCurrentLine();
            }
            else if (command == ":yy")
            {
                CopyCurrentLine();
            }
            else if (command == ":gg")
            {
                MoveToBeginning();
            }
            else if (command == ":G")
            {
                MoveToEnd();
            }
            else if (command.StartsWith(":uncomment "))
            {
                if (int.TryParse(command.Substring(11).Trim(), out int lineNumber))
                {
                    UncommentLine(lineNumber);
                }
                else
                {
                    MessageBox.Show("Invalid line number.");
                }
            }
            else if (command.StartsWith(":comment "))
            {
                if (int.TryParse(command.Substring(9).Trim(), out int lineNumber))
                {
                    CommentLine(lineNumber);
                }
                else
                {
                    MessageBox.Show("Invalid line number.");
                }
            }
            else if (command.StartsWith(":move "))
            {
                var parts = command.Split(' ', 3);
                if (parts.Length == 3 && int.TryParse(parts[1], out int sourceLine) && int.TryParse(parts[2], out int destLine))
                {
                    MoveLine(sourceLine, destLine);
                }
                else
                {
                    MessageBox.Show("Invalid command syntax.");
                }
            }
            else if (command.StartsWith("/:"))
            {
                StartSearch(command.Substring(2));
            }
            else if (command.StartsWith(":r "))
            {
                ReplaceText(command.Substring(3));
            }
            else if (command == ":clean")
            {
                CleanEmptyLines();
            }
            else if (command.StartsWith(":insert "))
            {
                var parts = command.Split(' ', 3);
                if (parts.Length == 3 && int.TryParse(parts[1], out int lineNumber))
                {
                    InsertLine(lineNumber, parts[2]);
                }
                else
                {
                    MessageBox.Show("Invalid command syntax.");
                }
            }

            else if (command.StartsWith(":remove "))
            {
                if (int.TryParse(command.Substring(8).Trim(), out int lineNumber))
                {
                    RemoveLine(lineNumber);
                }
                else
                {
                    MessageBox.Show("Invalid line number.");
                }
            }
            else if (command == ":timestamp")
            {
                InsertTimestamp();
            }
            else if (command.StartsWith(":outdent "))
            {
                if (int.TryParse(command.Substring(9).Trim(), out int spaces))
                {
                    OutdentLines(spaces);
                }
                else
                {
                    MessageBox.Show("Invalid number of spaces.");
                }
            }
            else if (command.StartsWith(":paste "))
            {
                if (int.TryParse(command.Substring(7).Trim(), out int lineNumber))
                {
                    PasteClipboardToLine(lineNumber);
                }
                else
                {
                    MessageBox.Show("Invalid line number.");
                }
            }
            else
            {
                MessageBox.Show("Unknown command or not enought arguments: " + command);
            }
        }
        private void OutdentLines(int spaces)
        {
            var lines = textBox1.Lines;
            var indent = new string(' ', spaces);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(indent))
                {
                    lines[i] = lines[i].Substring(indent.Length);
                }
            }
            textBox1.Lines = lines;
            MessageBox.Show($"Outdented lines by {spaces} spaces.");
        }

        private void CleanEmptyLines()
        {
            var lines = textBox1.Lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            textBox1.Lines = lines;
            MessageBox.Show("Removed empty lines.");
        }


        private void InsertTimestamp()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int cursorPosition = textBox1.SelectionStart;
            textBox1.Text = textBox1.Text.Insert(cursorPosition, timestamp);
            textBox1.SelectionStart = cursorPosition + timestamp.Length;
        }
        private void InsertLine(int lineNumber, string text)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length + 1)
            {
                var lines = textBox1.Lines.ToList();
                lines.Insert(lineNumber - 1, text);
                textBox1.Lines = lines.ToArray();
                MessageBox.Show("Text inserted.");
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void MoveLine(int sourceLine, int destLine)
        {
            if (sourceLine >= 1 && sourceLine <= textBox1.Lines.Length && destLine >= 1 && destLine <= textBox1.Lines.Length + 1)
            {
                var lines = textBox1.Lines.ToList();
                var line = lines[sourceLine - 1];
                lines.RemoveAt(sourceLine - 1);
                lines.Insert(destLine - 1, line);
                textBox1.Lines = lines.ToArray();
                MessageBox.Show("Line moved.");
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void RemoveLine(int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length)
            {
                var lines = textBox1.Lines.ToList();
                lines.RemoveAt(lineNumber - 1);
                textBox1.Lines = lines.ToArray();
                MessageBox.Show("Line removed.");
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void UncommentLine(int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length)
            {
                var lines = textBox1.Lines.ToList();
                if (lines[lineNumber - 1].StartsWith("// "))
                {
                    lines[lineNumber - 1] = lines[lineNumber - 1].Substring(3);
                    textBox1.Lines = lines.ToArray();
                    MessageBox.Show("Line uncommented.");
                }
                else
                {
                    MessageBox.Show("Line is not commented.");
                }
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void CommentLine(int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length)
            {
                var lines = textBox1.Lines.ToList();
                lines[lineNumber - 1] = "// " + lines[lineNumber - 1];
                textBox1.Lines = lines.ToArray();
                MessageBox.Show("Line commented.");
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void PasteClipboardToLine(int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length)
            {
                var lines = textBox1.Lines.ToList();
                lines.Insert(lineNumber - 1, Clipboard.GetText());
                textBox1.Lines = lines.ToArray();
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void UppercaseLine(int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length)
            {
                var lines = textBox1.Lines.ToList();
                lines[lineNumber - 1] = lines[lineNumber - 1].ToUpper();
                textBox1.Lines = lines.ToArray();
                MessageBox.Show("Line converted to uppercase.");
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void CopyLineToClipboard(int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length)
            {
                Clipboard.SetText(textBox1.Lines[lineNumber - 1]);
                MessageBox.Show("Line copied to clipboard.");
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void LowercaseLine(int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length)
            {
                var lines = textBox1.Lines.ToList();
                lines[lineNumber - 1] = lines[lineNumber - 1].ToLower();
                textBox1.Lines = lines.ToArray();
                MessageBox.Show("Line converted to lowercase.");
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void DeleteSpecificLine(int lineNumber)
        {
            if (lineNumber >= 1 && lineNumber <= textBox1.Lines.Length)
            {
                var lines = textBox1.Lines.ToList();
                lines.RemoveAt(lineNumber - 1);
                textBox1.Lines = lines.ToArray();
            }
            else
            {
                MessageBox.Show("Line number out of range.");
            }
        }
        private void ExecuteShellCommand(string shellCommand)
        {
            try
            {
                var processInfo = new ProcessStartInfo("cmd.exe", "/c " + shellCommand)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                var process = Process.Start(processInfo);
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(output))
                    MessageBox.Show(output);
                if (!string.IsNullOrEmpty(error))
                    MessageBox.Show("Error: " + error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Shell command execution failed: " + ex.Message);
            }
        }

        private void SaveFile()
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                File.WriteAllText(currentFilePath, textBox1.Text);
            }
            else
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Text Files|*.txt|All Files|*.*";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        currentFilePath = sfd.FileName;
                        File.WriteAllText(currentFilePath, textBox1.Text);
                    }
                }
            }
        }
        private void ExecuteScript()
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    openFileDialog.Title = "Select a Script File";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string scriptPath = openFileDialog.FileName;

                        // Чтение и выполнение команд из выбранного файла
                        string[] commands = File.ReadAllLines(scriptPath);

                        foreach (string cmd in commands)
                        {
                            ExecuteCommand(cmd.Trim());
                        }

                        MessageBox.Show("Script executed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing script: " + ex.Message);
            }
        }

        private void OpenSpecificFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Устанавливаем фильтр, чтобы показывать только текстовые файлы.
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = "Open Text File";
                UpdateStatt();

                // Если пользователь выбрал файл и нажал "Открыть"
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    UpdateStattocomm();
                    UpdateStatus();
                    currentFilePath = openFileDialog.FileName;
                    textBox1.Text = File.ReadAllText(currentFilePath);
                }
                else
                {
                    UpdateStattocomm();
                    UpdateStatus();
                    MessageBox.Show("No file selected.");
                }
            }
        }
        public void UpdateStatt()
        {
            RPC.SetState("Choosing File..", true);
        }


        private void DeleteCurrentLine()
        {
            int lineIndex = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
            int start = textBox1.GetFirstCharIndexFromLine(lineIndex);
            int length = textBox1.Lines[lineIndex].Length;
            int end = start + length + Environment.NewLine.Length;
            textBox1.Text = textBox1.Text.Remove(start, end - start);
        }

        private void CopyCurrentLine()
        {
            int lineIndex = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
            currentClipboardLine = lineIndex;
        }

        private void PasteLineBelow()
        {
            if (currentClipboardLine >= 0)
            {
                int lineIndex = textBox1.GetLineFromCharIndex(textBox1.SelectionStart) + 1;
                int start = textBox1.GetFirstCharIndexFromLine(lineIndex);
                string lineToPaste = textBox1.Lines[currentClipboardLine];
                textBox1.Text = textBox1.Text.Insert(start, lineToPaste + Environment.NewLine);
            }
        }

        private void MoveToBeginning()
        {
            textBox1.SelectionStart = 0;
            textBox1.ScrollToCaret();
        }

        private void MoveToEnd()
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void StartSearch(string term)
        {
            lastSearchTerm = term;
            FindNext();
        }

        private void FindNext()
        {
            if (!string.IsNullOrEmpty(lastSearchTerm))
            {
                int start = textBox1.SelectionStart + textBox1.SelectionLength;
                int index = textBox1.Text.IndexOf(lastSearchTerm, start);
                if (index != -1)
                {
                    textBox1.SelectionStart = index;
                    textBox1.SelectionLength = lastSearchTerm.Length;
                    textBox1.ScrollToCaret();
                }
            }
        }

        private void FindPrevious()
        {
            if (!string.IsNullOrEmpty(lastSearchTerm))
            {
                int start = textBox1.SelectionStart - 1;
                int index = textBox1.Text.LastIndexOf(lastSearchTerm, start);
                if (index != -1)
                {
                    textBox1.SelectionStart = index;
                    textBox1.SelectionLength = lastSearchTerm.Length;
                    textBox1.ScrollToCaret();
                }
            }
        }

        private void ReplaceText(string replaceCommand)
        {
            var parts = replaceCommand.Split(new[] { ' ' }, 2);
            if (parts.Length == 2)
            {
                string searchText = parts[0];
                string replacementText = parts[1];
                textBox1.Text = textBox1.Text.Replace(searchText, replacementText);
            }
        }

        private void UpdateStatus()
        {
            int wordCount = textBox1.Text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int lineCount = textBox1.Lines.Length;

            label1.Text = $"Words: {wordCount} | Lines: {lineCount}";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}