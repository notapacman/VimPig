using System.Drawing.Drawing2D;

namespace VimPig
{
    public partial class Form1 : Form
    {
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
                // Получаем путь к файлу, начиная с 3-го символа, и удаляем лишние пробелы.
                string filePath = command.Substring(4).Trim();

                // Проверка, есть ли аргумент после ":e ".
                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        // Открываем указанный файл.
                        OpenSpecificFile(filePath);
                    }
                    catch (Exception ex)
                    {
                        // Если произошла ошибка при открытии файла, отображаем сообщение с описанием ошибки.
                        MessageBox.Show($"Failed to open file: {ex.Message}");
                    }
                }
                else
                {
                    // Если аргумент не указан, отображаем сообщение о необходимости указания пути к файлу.
                    MessageBox.Show("Error: Missing file path. Please provide a valid file path after the :e command.");
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
                MessageBox.Show("Unknown command or not enought arguments: " + command);
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

            label1.Text = $"Words: {wordCount} | Lines: {lineCount}";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}