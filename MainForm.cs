using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TwinsGroupInfo
{
    public partial class MainForm : Form
    {
        private ComboBox fontComboBox;
        private Button generateButton;
        private TextBox companyInfoTextBox;
        private Label titleLabel;
        private Panel mainPanel;
        private Label fontLabel;
        private Panel previewPanel;
        private Label previewLabel;

        public MainForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Генератор HTML страницы";
            this.Size = new Size(1000, 700);
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Font = new Font("Segoe UI", 9F);

            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };

            titleLabel = new Label
            {
                Text = "Генератор HTML страницы",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 48),
                Location = new Point(20, 20),
                AutoSize = true
            };

            Label infoLabel = new Label
            {
                Text = "Введите информацию о компании:",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(45, 45, 48),
                Location = new Point(20, 80),
                AutoSize = true
            };

            companyInfoTextBox = new TextBox
            {
                Multiline = true,
                Location = new Point(20, 110),
                Size = new Size(440, 400),
                Font = new Font("Segoe UI", 12),
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            fontLabel = new Label
            {
                Text = "Выберите шрифт:",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(45, 45, 48),
                Location = new Point(20, 530),
                AutoSize = true
            };

            fontComboBox = new ComboBox
            {
                Location = new Point(20, 560),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(250, 250, 250)
            };

            fontComboBox.Items.AddRange(new string[] {
                "Arial",
                "Times New Roman",
                "Calibri",
                "Georgia",
                "Verdana",
                "Roboto"
            });
            fontComboBox.SelectedIndex = 0;

            generateButton = new Button
            {
                Text = "Сгенерировать HTML",
                Location = new Point(20, 600),
                Size = new Size(200, 40),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            generateButton.FlatAppearance.BorderSize = 0;
            generateButton.Click += GenerateButton_Click;

            previewPanel = new Panel
            {
                Location = new Point(500, 110),
                Size = new Size(440, 530),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            previewLabel = new Label
            {
                Text = "Предпросмотр",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(45, 45, 48),
                Location = new Point(500, 80),
                AutoSize = true
            };

            // Добавляем обработчики событий для обновления предпросмотра
            companyInfoTextBox.TextChanged += UpdatePreview;
            fontComboBox.SelectedIndexChanged += UpdatePreview;

            mainPanel.Controls.AddRange(new Control[] { 
                titleLabel,
                infoLabel,
                companyInfoTextBox,
                fontLabel,
                fontComboBox,
                generateButton,
                previewPanel,
                previewLabel
            });

            this.Controls.Add(mainPanel);
        }

        private void UpdatePreview(object sender, EventArgs e)
        {
            previewPanel.Invalidate();
            previewPanel.Paint += PreviewPanel_Paint;
        }

        private void PreviewPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(previewPanel.BackColor);
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            string selectedFont = fontComboBox.SelectedItem.ToString();
            using (Font previewFont = new Font(selectedFont, 12))
            {
                string previewText = string.IsNullOrEmpty(companyInfoTextBox.Text) 
                    ? "Предпросмотр текста будет отображаться здесь" 
                    : companyInfoTextBox.Text;

                Rectangle bounds = new Rectangle(10, 10, previewPanel.Width - 20, previewPanel.Height - 20);
                e.Graphics.DrawString(previewText, previewFont, Brushes.Black, bounds);
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string selectedFont = fontComboBox.SelectedItem.ToString();
            string htmlContent = GenerateHtml(selectedFont);
            
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "HTML файлы|*.html",
                Title = "Сохранить HTML файл",
                FileName = "company_info.html"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveDialog.FileName, htmlContent);
                MessageBox.Show("HTML файл успешно создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GenerateHtml(string fontFamily)
        {
            return $@"<!DOCTYPE html>
<html lang=""ru"">
<head>
    <meta charset=""UTF-8"">
    <title>ООО ""Твинс Групп""</title>
    <style>
        body {{
            font-family: '{fontFamily}', sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }}
        .container {{
            max-width: 800px;
            margin: 0 auto;
            background-color: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 0 20px rgba(0,0,0,0.1);
        }}
        h1 {{
            color: #2c3e50;
            text-align: center;
            margin-bottom: 30px;
            font-size: 2.5em;
        }}
        .info-section {{
            margin-bottom: 20px;
            padding: 20px;
            background-color: #fafafa;
            border-radius: 5px;
        }}
        .info-section h2 {{
            color: #34495e;
            border-bottom: 2px solid #3498db;
            padding-bottom: 5px;
            margin-top: 0;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h1>ООО ""Твинс Групп""</h1>
        <div class=""info-section"">
            {companyInfoTextBox.Text.Replace("\n", "<br>")}
        </div>
    </div>
</body>
</html>";
        }
    }
} 