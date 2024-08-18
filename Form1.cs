namespace VimPig
{
    public partial class Form1 : Form
    {
        private string currentFilePath = string.Empty; // Путь к открытому файлу
        private bool isCommandMode = false; // Режим командной строки
        private string lastSearchTerm = string.Empty; // Последний поисковый запрос
        private int currentClipboardLine = -1; // Хранит последнюю скопированную строку
        private int lastLineIndex = 0; // Хранит индекс строки перед открытием командной строки

        public Form1()
        {
            InitializeComponent();
            UpdateStatus();
        }

        private void Scintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                ToggleCommandMode();
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

        private void ToggleCommandMode()
        {
            if (isCommandMode)
            {
                // Save the current line before hiding command box
                lastLineIndex = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
            }

            isCommandMode = !isCommandMode;
            commandTextBox.Visible = isCommandMode;

            if (isCommandMode)
            {
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
            else if (command.StartsWith(":e "))
            {
                OpenSpecificFile(command.Substring(3));
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
            else if (command.StartsWith("/:"))
            {
                StartSearch(command.Substring(2));
            }
            else if (command.StartsWith(":r "))
            {
                ReplaceText(command.Substring(3));
            }
            else
            {
                MessageBox.Show("Unknown command: " + command);
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

        private void OpenSpecificFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                currentFilePath = filePath;
                textBox1.Text = File.ReadAllText(currentFilePath);
            }
            else
            {
                MessageBox.Show("File not found: " + filePath);
            }
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

            statusLabel.Text = $"Words: {wordCount} | Lines: {lineCount}";
        }
    }
}