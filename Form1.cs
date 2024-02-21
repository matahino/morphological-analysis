using System;
using System.IO;
using System.Windows.Forms;
using System.Data;
namespace test
{
    public partial class Form1 : Form
    {
        private TextBox textBoxFilePath;
        private TextBox textBoxContent;
        private Button buttonLoad;
        private IMeCabtxt meCabtxt;
        private CheckBox checkBoxDuplicate;
        private ComboBox comboBoxLanguage;
        private DataGridView dataGridViewDbView;

        public Form1()
        {
            InitializeComponent();
            InitializeFormComponents();
            meCabtxt = new MeCabtxt();
        }

        private void InitializeFormComponents()
        {
            InitializeTextBoxFilePath();
            InitializeTextBoxContent();
            InitializeButtonLoad();
            InitializeCheckBoxDuplicate();
            InitializeComboBoxLanguage();
            InitializeDataGridView();
            // コントロールをフォームに追加する
             Controls.AddRange(new Control[] { textBoxFilePath, textBoxContent, buttonLoad, checkBoxDuplicate, comboBoxLanguage, dataGridViewDbView });
        }

        private void InitializeTextBoxFilePath()
        {
            textBoxFilePath = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(260, 20)
            };
        }

        private void InitializeTextBoxContent()
        {
            textBoxContent = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(260, 200),
                Multiline = true,
                ReadOnly = true
            };
        }

        private void InitializeButtonLoad()
        {
            buttonLoad = new Button
            {
                Location = new System.Drawing.Point(10, 250),
                Size = new System.Drawing.Size(75, 23),
                Text = "Load"
            };
            buttonLoad.Click += ButtonLoad_Click;
        }

        private void InitializeCheckBoxDuplicate()
        {
            checkBoxDuplicate = new CheckBox
            {
                Location = new System.Drawing.Point(10, 280),
                Size = new System.Drawing.Size(150, 20),
                Text = "Remove duplicate"
            };
        }

        private void InitializeComboBoxLanguage()
        {
            comboBoxLanguage = new ComboBox
            {
                Location = new System.Drawing.Point(10, 310),
                Size = new System.Drawing.Size(150, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBoxLanguage.Items.AddRange(new object[] { "English", "日本語" });
        }


        private void InitializeDataGridView()
        {
            dataGridViewDbView = new DataGridView
            {
                Location = new System.Drawing.Point(280, 10),
                Size = new System.Drawing.Size(500, 320),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ScrollBars = ScrollBars.Both
            };
            Controls.Add(dataGridViewDbView);
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            string filePath = textBoxFilePath.Text;
            var duplicateRemover = new DuplicateRemover();
            

            if (File.Exists(filePath))
            {
                
                meCabtxt.InputText = filePath;
            　　string[] fileContent = File.ReadAllLines(filePath);
                meCabtxt.OutputText = filePath + ".out.txt";

                if (checkBoxDuplicate.Checked)
                {

        
            if (comboBoxLanguage.SelectedItem != null)
            {
                string? selectedLanguage = comboBoxLanguage.SelectedItem.ToString();

                if(selectedLanguage == "日本語")
                {
                    meCabtxt.PerformMecab(meCabtxt.InputText, meCabtxt.OutputText);
                    duplicateRemover.RemoveDuplicateSentences(meCabtxt.OutputText, meCabtxt.OutputText);
   　　　　　　　   LoadDataIntoDataGridView(meCabtxt.OutputText);
                }else if(selectedLanguage == "English")
　　　　　　　　{}
            }else{
                    
                    meCabtxt.PerformMecab(meCabtxt.InputText, meCabtxt.OutputText);
                    duplicateRemover.RemoveDuplicateSentences(meCabtxt.OutputText, meCabtxt.OutputText);
   　　　　　　　   LoadDataIntoDataGridView(meCabtxt.OutputText);

                    }


           }else
                {
                string? selectedLanguage = comboBoxLanguage.SelectedItem as string;
                
                 if (selectedLanguage == "日本語")
                {
                     meCabtxt.PerformMecab(meCabtxt.InputText, meCabtxt.OutputText);
                     LoadDataIntoDataGridView(meCabtxt.OutputText);

                }else if(selectedLanguage == "English")
　　　　　　　　{}
                else if(selectedLanguage != "日本語"&& selectedLanguage != "English"){
                     meCabtxt.PerformMecab(meCabtxt.InputText, meCabtxt.OutputText);
                     LoadDataIntoDataGridView(meCabtxt.OutputText);

               }

                }

            }
            else
            {
                textBoxContent.Text = "File does not exist.";
            }
        }


private void LoadDataIntoDataGridView(string filePath)
{
    DataTable dataTable = new DataTable();
    string[] fileContent = File.ReadAllLines(filePath);

    // 列ヘッダーを追加するためのフラグ
    bool headersAdded = false;

    foreach (string line in fileContent)
    {
        // タブで行を分割
        string[] parts = line.Split('\t');

        // 列ヘッダーを追加していない場合は追加する
        if (!headersAdded)
        {
            foreach (string part in parts)
            {
                // タブの前の列を列名として使用
                dataTable.Columns.Add(part.Trim());
            }
            headersAdded = true; // 列ヘッダーが追加された
        }
        else
        {
            DataRow newRow = dataTable.NewRow();

            // 最初の列は空白として扱うためにスキップ
            for (int i = 1; i < parts.Length; i++)
            {
                // タブの前の列を列の名前として使用
                string columnName = dataTable.Columns[i - 1].ColumnName;
                newRow[columnName] = parts[i].Trim();
            }

            dataTable.Rows.Add(newRow);
        }
    }

    // DataGridViewにデータソースを設定
    dataGridViewDbView.DataSource = dataTable;
}
}
}
