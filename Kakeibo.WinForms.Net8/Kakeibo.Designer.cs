namespace Kakeibo.WinForms.Net8
{
    partial class Kakeibo
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kakeibo));
            kakeiboDataGrid = new DataGridView();
            No = new DataGridViewTextBoxColumn();
            Id = new DataGridViewTextBoxColumn();
            Date = new DataGridViewTextBoxColumn();
            Category = new DataGridViewTextBoxColumn();
            Price = new DataGridViewTextBoxColumn();
            Memo = new DataGridViewTextBoxColumn();
            datePicker = new DateTimePicker();
            categoryText = new ComboBox();
            priceText = new TextBox();
            memoText = new TextBox();
            registerButton = new Button();
            editButton = new Button();
            deleteButton = new Button();
            clearButton = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)kakeiboDataGrid).BeginInit();
            SuspendLayout();
            // 
            // kakeiboDataGrid
            // 
            kakeiboDataGrid.AllowUserToAddRows = false;
            kakeiboDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            kakeiboDataGrid.Columns.AddRange(new DataGridViewColumn[] { No, Id, Date, Category, Price, Memo });
            kakeiboDataGrid.Location = new Point(40, 13);
            kakeiboDataGrid.MultiSelect = false;
            kakeiboDataGrid.Name = "kakeiboDataGrid";
            kakeiboDataGrid.RowHeadersVisible = false;
            kakeiboDataGrid.RowHeadersWidth = 62;
            kakeiboDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            kakeiboDataGrid.Size = new Size(1032, 355);
            kakeiboDataGrid.TabIndex = 0;
            kakeiboDataGrid.CellFormatting += kakeiboDataGrid_CellFormatting;
            // 
            // No
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            No.DefaultCellStyle = dataGridViewCellStyle1;
            No.HeaderText = "No";
            No.MinimumWidth = 8;
            No.Name = "No";
            No.Width = 40;
            // 
            // Id
            // 
            Id.DataPropertyName = "Id";
            Id.HeaderText = "ID";
            Id.MinimumWidth = 8;
            Id.Name = "Id";
            Id.Visible = false;
            Id.Width = 150;
            // 
            // Date
            // 
            Date.DataPropertyName = "Date";
            Date.HeaderText = "日付";
            Date.MinimumWidth = 8;
            Date.Name = "Date";
            Date.Width = 150;
            // 
            // Category
            // 
            Category.DataPropertyName = "Category";
            Category.HeaderText = "カテゴリ";
            Category.MinimumWidth = 8;
            Category.Name = "Category";
            Category.Width = 150;
            // 
            // Price
            // 
            Price.DataPropertyName = "Price";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "C0";
            dataGridViewCellStyle2.NullValue = null;
            Price.DefaultCellStyle = dataGridViewCellStyle2;
            Price.HeaderText = "金額";
            Price.MinimumWidth = 8;
            Price.Name = "Price";
            Price.Width = 150;
            // 
            // Memo
            // 
            Memo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Memo.DataPropertyName = "Memo";
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            Memo.DefaultCellStyle = dataGridViewCellStyle3;
            Memo.HeaderText = "メモ";
            Memo.MinimumWidth = 8;
            Memo.Name = "Memo";
            // 
            // datePicker
            // 
            datePicker.Location = new Point(104, 394);
            datePicker.Name = "datePicker";
            datePicker.Size = new Size(190, 31);
            datePicker.TabIndex = 1;
            // 
            // categoryText
            // 
            categoryText.FormattingEnabled = true;
            categoryText.Items.AddRange(new object[] { "食費", "日用品", "衣服", "交通費", "固定費", "医療", "交際費", "娯楽", "美容", "その他" });
            categoryText.Location = new Point(387, 394);
            categoryText.Name = "categoryText";
            categoryText.Size = new Size(211, 33);
            categoryText.TabIndex = 2;
            // 
            // priceText
            // 
            priceText.Location = new Point(104, 441);
            priceText.Name = "priceText";
            priceText.Size = new Size(494, 31);
            priceText.TabIndex = 3;
            // 
            // memoText
            // 
            memoText.Location = new Point(104, 496);
            memoText.Multiline = true;
            memoText.Name = "memoText";
            memoText.ScrollBars = ScrollBars.Horizontal;
            memoText.Size = new Size(494, 94);
            memoText.TabIndex = 4;
            // 
            // registerButton
            // 
            registerButton.Location = new Point(725, 448);
            registerButton.Name = "registerButton";
            registerButton.Size = new Size(81, 73);
            registerButton.TabIndex = 5;
            registerButton.Text = "追加";
            registerButton.UseVisualStyleBackColor = true;
            registerButton.Click += registerButton_Click;
            // 
            // editButton
            // 
            editButton.Location = new Point(848, 447);
            editButton.Name = "editButton";
            editButton.Size = new Size(89, 74);
            editButton.TabIndex = 6;
            editButton.Text = "編集";
            editButton.UseVisualStyleBackColor = true;
            editButton.Click += editButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(985, 447);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(87, 74);
            deleteButton.TabIndex = 7;
            deleteButton.Text = "削除";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(619, 556);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(85, 34);
            clearButton.TabIndex = 8;
            clearButton.Text = "クリア";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += clearButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(50, 399);
            label1.Name = "label1";
            label1.Size = new Size(48, 25);
            label1.TabIndex = 9;
            label1.Text = "日付";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(317, 399);
            label2.Name = "label2";
            label2.Size = new Size(64, 25);
            label2.TabIndex = 10;
            label2.Text = "カテゴリ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(50, 447);
            label3.Name = "label3";
            label3.Size = new Size(48, 25);
            label3.TabIndex = 11;
            label3.Text = "金額";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(50, 496);
            label4.Name = "label4";
            label4.Size = new Size(38, 25);
            label4.TabIndex = 12;
            label4.Text = "メモ";
            // 
            // Kakeibo
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1120, 624);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(clearButton);
            Controls.Add(deleteButton);
            Controls.Add(editButton);
            Controls.Add(registerButton);
            Controls.Add(memoText);
            Controls.Add(priceText);
            Controls.Add(categoryText);
            Controls.Add(datePicker);
            Controls.Add(kakeiboDataGrid);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Kakeibo";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kakeibo";
            Load += Kakeibo_Load;
            ((System.ComponentModel.ISupportInitialize)kakeiboDataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView kakeiboDataGrid;
        private DateTimePicker datePicker;
        private ComboBox categoryText;
        private TextBox priceText;
        private TextBox memoText;
        private Button registerButton;
        private Button editButton;
        private Button deleteButton;
        private Button clearButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private DataGridViewTextBoxColumn No;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn Category;
        private DataGridViewTextBoxColumn Price;
        private DataGridViewTextBoxColumn Memo;
    }
}
