// MainForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Linq;

namespace CodePasteWizard
{
    public partial class MainForm : Form
    {
        private List<string> codeSnippets = new List<string>();
        private int currentSnippetIndex = 0;
        private string targetDirectory = "";
        private const string DataFileName = "codepaste_data.json";

        public MainForm()
        {
            InitializeComponent();
            InitializeCodeSnippets();
            LoadSavedData();
            SelectButton(button1);
            pbRight.Click += PbRight_Click;
        }

        private void PbRight_Click(object? sender, EventArgs e)
        {
            string processedCode = CodeProcessor.ProcessSnippets(codeSnippets.Where(s => !string.IsNullOrWhiteSpace(s)).ToList());
            textBoxOutput.Text = processedCode;
            UpdateDetectedFilename();
        }

        private void InitializeCodeSnippets()
        {
            codeSnippets = Enumerable.Repeat(string.Empty, 10).ToList();
        }

        private void SelectButton(Button button)
        {
            foreach (Control control in flowLayoutPanelNumbers.Controls)
            {
                if (control is Button btn)
                {
                    btn.BackColor = (btn == button) ? Color.Blue : SystemColors.Control;
                    btn.ForeColor = (btn == button) ? Color.White : SystemColors.ControlText;
                }
            }
            currentSnippetIndex = int.Parse((string)button.Tag) - 1;
            UpdateUI();
        }

        private void UpdateUI()
        {
            textBoxInput.Text = codeSnippets[currentSnippetIndex];
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            string processedCode = CodeProcessor.ProcessSnippets(codeSnippets.Where(s => !string.IsNullOrWhiteSpace(s)).ToList());
            textBoxOutput.Text = processedCode;
            UpdateDetectedFilename();
        }

        private void buttonCopyTop_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxOutput.Text))
            {
                Clipboard.SetText(textBoxOutput.Text);
            }
        }

        private void buttonCopyBottom_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxOutput.Text))
            {
                Clipboard.SetText(textBoxOutput.Text);
            }
        }

   


        private void UpdateDetectedFilename()
        {
            string detectedFilename = FileOperations.DetectFilename(textBoxOutput.Text);
            labelDetectedFilename.Text = $"Detected Filename: {detectedFilename}";
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            InitializeCodeSnippets();
            textBoxInput.Clear();
            textBoxOutput.Clear();
            labelDetectedFilename.Text = "Detected Filename: ";
            SelectButton(button1);
        }

        private void buttonClearSlot_Click(object sender, EventArgs e)
        {
            codeSnippets[currentSnippetIndex] = string.Empty;
            textBoxInput.Clear();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            SaveCurrentData();
        }

        private void SaveCurrentData()
        {
            var dataToSave = new
            {
                Snippets = codeSnippets,
                TargetDirectory = targetDirectory
            };

            string jsonString = JsonSerializer.Serialize(dataToSave);
            File.WriteAllText(DataFileName, jsonString);
        }

        private void LoadSavedData()
        {
            if (File.Exists(DataFileName))
            {
                string jsonString = File.ReadAllText(DataFileName);
                try
                {
                    var loadedData = JsonSerializer.Deserialize<JsonElement>(jsonString);

                    if (loadedData.TryGetProperty("Snippets", out JsonElement snippetsElement))
                    {
                        var loadedSnippets = snippetsElement.EnumerateArray()
                            .Select(element => element.GetString() ?? string.Empty)
                            .ToList();

                        // Ensure we always have 10 slots
                        codeSnippets = loadedSnippets.Concat(Enumerable.Repeat(string.Empty, 10 - loadedSnippets.Count)).Take(10).ToList();
                    }

                    if (loadedData.TryGetProperty("TargetDirectory", out JsonElement targetDirectoryElement))
                    {
                        targetDirectory = targetDirectoryElement.GetString() ?? string.Empty;
                       
                    }

                    UpdateUI();
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error loading saved data: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            codeSnippets[currentSnippetIndex] = textBoxInput.Text;
        }

        private void button1_Click(object sender, EventArgs e) => SelectButton(button1);
        private void button2_Click(object sender, EventArgs e) => SelectButton(button2);
        private void button3_Click(object sender, EventArgs e) => SelectButton(button3);
        private void button4_Click(object sender, EventArgs e) => SelectButton(button4);
        private void button5_Click(object sender, EventArgs e) => SelectButton(button5);
        private void button6_Click(object sender, EventArgs e) => SelectButton(button6);
        private void button7_Click(object sender, EventArgs e) => SelectButton(button7);
        private void button8_Click(object sender, EventArgs e) => SelectButton(button8);
        private void button9_Click(object sender, EventArgs e) => SelectButton(button9);
        private void button10_Click(object sender, EventArgs e) => SelectButton(button10);
    }
}