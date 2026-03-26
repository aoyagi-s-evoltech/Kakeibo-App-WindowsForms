using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace Kakeibo.WinForms.Net8
{
    /// <summary>
    /// メイン画面
    /// 入力フォーム（項目・日付・金額・メモ）と一覧表示(DataGridView)の管理、
    /// データの追加・編集・削除・保存処理を担当する
    /// </summary>
    public partial class Kakeibo : Form
    {
        /// <summary>
        /// エラーメッセージ表示時のタイトル
        /// </summary>
        private const string ErrorTitle = "入力エラー";

        /// <summary>
        /// 情報メッセージ表示時のタイトル
        /// </summary>
        private const string InfoTitle = "通知";

        /// <summary>
        /// 確認メッセージ表示時のタイトル
        /// </summary>
        private const string ConfirmTitle = "確認";

        /// <summary>
        /// データベースのファイル名
        /// </summary>
        private const string DbFileName = "expenses.db";

        /// <summary>
        /// 金額入力で許可する最大桁数(記号なし)
        /// </summary>
        private const int MaxPriceDigits = 11;

        /// <summary>
        /// DataTableの列を表す列番号の定数
        /// </summary>
        private enum Columns
        {
            /// <summary>
            /// ID列(0列目)
            /// </summary>
            Id = 0,

            /// <summary>
            /// 日付列(1列目)
            /// </summary>
            Date = 1,

            /// <summary>
            /// カテゴリ列(2列目)
            /// </summary>
            Category = 2,

            /// <summary>
            /// 金額列(3列目)
            /// </summary>
            Price = 3,

            /// <summary>
            /// メモ列(4列目)
            /// </summary>
            Memo = 4
        }

        /// <summary>
        /// 家計簿データの保存と読み込みを担当するリポジトリ
        /// </summary>
        private IExpenseRepository repository;

        /// <summary>
        /// 一覧表示用のDataTable
        /// </summary>
        private DataTable table;

        /// <summary>
        /// フォームの初期化処理
        /// </summary>
        public Kakeibo()
        {
            // フォームの初期化
            InitializeComponent();

            // SQLiteの初期化
            SQLitePCL.Batteries_V2.Init();
        }

        /// <summary>
        /// フォーム起動時にデータを読み込み、一覧に反映する
        /// SQLiteまたはXMLからの読み込みを行う
        /// </summary>
        /// <param name="sender">イベントの送信元</param>
        /// <param name="e">イベントデータ</param>
        private void Kakeibo_Load(object sender, EventArgs e)
        {
            // 保存用ファイルの有無を確認し、使用するリポジトリを選択
            if (File.Exists(DbFileName))
            {
                repository = new SqliteExpenseRepository();
            }
            else
            {
                repository = new XmlExpenseRepository();
            }

            // DataGridViewに紐付けるDataTableの列を定義する
            if (table.Columns.Count == 0)
            {
                table.Columns.Add("Id", typeof(int));
                table.Columns.Add("Date", typeof(DateTime));
                table.Columns.Add("Category", typeof(string));
                table.Columns.Add("Price", typeof(int));
                table.Columns.Add("Memo", typeof(string));
            }

            // 列をDataGridViewに手動で追加するため、AutoGenerateColumnsをfalseに設定する
            kakeiboDataGrid.AutoGenerateColumns = false;

            // DataGridViewとDataTableを紐付ける
            kakeiboDataGrid.DataSource = table;

            // データの読み込み
            Reload();
        }

        /// <summary>
        /// データをリポジトリから読み込み、DataGridViewに反映する
        /// </summary>
        /// <remarks> 
        /// DataTableを一度クリアしてから再構築する
        /// 最新のデータを1行ずつDataTableに追加していく
        /// 追加後、No列に連番を振る
        /// </remarks>
        private void Reload()
        {
            // リポジトリから全てのデータを取得する
            var items = repository.GetAll();
            // DataTableを一度クリアする
            table.Rows.Clear();

            // 取得したデータを1行ずつDataTableに追加する
            foreach (var expense in items)
            {
                table.Rows.Add(
                    expense.Id,
                    expense.Date,
                    expense.Category,
                    expense.Price,
                    expense.Memo
                );
            }
        }

        /// <summary>
        /// 入力された内容を新規登録し、一覧を更新する
        /// </summary>
        /// <param name="sender">登録ボタン</param>
        /// <param name="e">イベントデータ</param>
        private void registerButton_Click(object sender, EventArgs e)
        {
            // 入力内容のチェック
            if (!CheckInput(out int price)) return;

            // 問題ないとわかったデータでリポジトリに登録処理を行う
            var expense = new Expense
            {
                Date = datePicker.Value,
                Category = categoryText.Text,
                Price = price,
                Memo = memoText.Text
            };

            // 登録処理を行った後、一覧を更新する
            repository.Insert(expense);
            Reload();

            // 登録後は一番下の行を選択状態にする
            if (kakeiboDataGrid.Rows.Count > 0)
            {
                // 最後の行のインデックスを取得する
                int lastRowIndex = kakeiboDataGrid.Rows.Count - 1;

                // 最後の行の「日付(Date)」のセルを選択状態にする
                kakeiboDataGrid.CurrentCell = kakeiboDataGrid.Rows[lastRowIndex].Cells["Date"];
            }
        }

        /// <summary>
        /// 選択された行の内容を編集し、保存後に一覧を更新する
        /// </summary>
        /// <param name="sender">編集ボタン</param>
        /// <param name="e">イベントデータ</param>
        private void editButton_Click(object sender, EventArgs e)
        {
            // 行が選択されているかチェック
            if (!HasData())
            {
                MessageBox.Show(this, "編集できるデータがありません。", InfoTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 入力内容の妥当性をチェック
            if (!CheckInput(out int price)) return;

            // 選択された行のIDを取得する
            int id = (int)kakeiboDataGrid.CurrentRow.Cells["Id"].Value;

            // 編集後の内容をもとにリポジトリの更新処理を行う
            var expense = new Expense
            {
                Id = id,
                Date = datePicker.Value,
                Category = categoryText.Text,
                Price = price,
                Memo = memoText.Text
            };

            // 更新処理を行った後、一覧を更新する
            repository.Update(expense);
            Reload();
        }

        /// <summary>
        /// 選択された行を削除し、一覧を更新する
        /// </summary>
        /// <param name="sender">削除ボタン</param>
        /// <param name="e">イベントデータ</param>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            // 行が選択されているかチェック
            if (!HasData())
            {
                MessageBox.Show(this, "削除できるデータがありません。", InfoTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // 現在選択されている行のインデックスを取得する
            int currentIndex = kakeiboDataGrid.CurrentRow.Index;

            // 選択された行のIDを取得する
            int id = (int)kakeiboDataGrid.CurrentRow.Cells["Id"].Value;

            // 削除処理を行った後、一覧を更新する
            repository.Delete(id);
            Reload();

            // 削除後は、削除された行の前の行を選択状態にする
            if (kakeiboDataGrid.Rows.Count > 0)
            {
                // 一番下を消した時でもエラーにならないよう調整
                int nextIndex = Math.Max(0, currentIndex - 1);
                kakeiboDataGrid.CurrentCell = kakeiboDataGrid.Rows[nextIndex].Cells["Date"];
            }
        }

        /// <summary>
        /// 確認画面を表示し、承諾された場合に入力欄をリセットする
        /// </summary>
        /// <param name="sender">クリアボタン</param>
        /// <param name="e">イベントデータ</param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            // 確認画面を表示する
            DialogResult result = MessageBox.Show(this, "入力内容をクリアしますか？", ConfirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // クリアする場合は入力欄を初期状態に戻す
            if (result == DialogResult.Yes)
            {
                datePicker.Value = DateTime.Today;
                categoryText.SelectedIndex = -1;
                categoryText.Text = "";
                priceText.Text = "";
                memoText.Text = "";
            }
        }

        /// <summary>
        /// 金額のセルの形式を設定する
        /// </summary>
        /// <param name="sender">DataGridView</param>
        /// <param name="e">セルの書式設定イベントデータ</param>
        private void kakeiboDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 処理しているセルが「金額(Price)」の列でない場合は何もしない
            DataGridViewColumn targetColumn = kakeiboDataGrid.Columns[e.ColumnIndex];
            if (targetColumn.Name != "Price") return;

            // セルの値を文字列として取得する
            string text = e.Value.ToString();
            // 「\」「,」「-」などの記号が含まれる場合も数値として正しく解析できるようにする
            var style = System.Globalization.NumberStyles.AllowCurrencySymbol | System.Globalization.NumberStyles.Number;

            // 金額がマイナスの値の場合は赤色、正の値の場合は黒色で表示する
            if (decimal.TryParse(text,style,null,out decimal price))
            {
                // セルのスタイルを取得する
                DataGridViewCellStyle penColor = e.CellStyle;

                if (price < 0)
                {
                    penColor.ForeColor = Color.Red;
                    penColor.SelectionForeColor = Color.Red;
                }
                else
                {
                    penColor.ForeColor = Color.Black;
                    penColor.SelectionForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// 常に行のヘッダーに上からの連番を表示する
        /// </summary>
        /// <param name="sender">イベントの送信元</param>
        /// <param name="e">イベントデータ</param>
        private void kakeiboDataGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // 上からの連番を表示する
            kakeiboDataGrid.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
        }

        /// <summary>
        /// 入力エラーが発生した際にエラーメッセージを表示し、入力を元に戻す
        /// </summary>
        /// <param name="sender">イベントの送信元</param>
        /// <param name="e">イベントデータ</param>
        private void kakeiboDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 現在の入力を破棄して、セルの値を元に戻す
            kakeiboDataGrid.CancelEdit();

            // エラーが発生した列の名前を取得する
            var targetColumn = (Columns)e.ColumnIndex;
            string message = "入力された値の形式が正しくありません。";

            // 列の名前に応じて、エラーメッセージを表示する
            if (targetColumn == Columns.Date)
            {
                MessageBox.Show($"{message}\n正しい日付（yyyy/mm/dd）を入力してください。", ErrorTitle);
            } 
            else if(targetColumn == Columns.Price) 
            {
                MessageBox.Show($"{message}\n金額は{MaxPriceDigits - 1}桁以内の整数で入力してください。", ErrorTitle);
            }

            // アプリの強制終了を防ぐ
            e.ThrowException = false;
        }

        /// <summary>
        /// セルの値が確定する前に、入力内容が正しいか検証する
        /// </summary>
        private void kakeiboDataGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // 金額列(Price)のチェック。文字列ではなくEnumで判定
            if (e.ColumnIndex == (int)Columns.Price)
            {
                string input = CleanPriceText(e.FormattedValue.ToString());

                if (input.Length >= MaxPriceDigits)
                {
                    MessageBox.Show(this, $"金額は{MaxPriceDigits - 1}桁以内の整数で入力してください。", ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 入力された内容が有効かどうかを確認する
        /// </summary>
        /// <param name="price">入力された金額</param>
        /// <returns>入力が有効な値の場合はtrue、無効な値の場合はfalse</returns>
        private bool CheckInput(out int price)
        {
            // 初期化
            price = 0;

            // カテゴリが空でないか確認
            if (string.IsNullOrWhiteSpace(categoryText.Text))
            {
                MessageBox.Show(this, "カテゴリを入力してください。", ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string pureText = CleanPriceText(priceText.Text);

            if (pureText.Length >= MaxPriceDigits)
            {
                MessageBox.Show(this, $"金額は{MaxPriceDigits - 1}桁以内で入力してください。", ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 金額が数値として正しいか確認
            if (!int.TryParse(pureText, out price))
            {
                MessageBox.Show(this, "金額は整数で入力してください。", ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // OKならtrueを返す
            return true;
        }

        /// <summary>
        /// 一覧に表示可能なデータが存在するかどうかを確認する
        /// </summary>
        /// <returns>データが存在する場合はtrue、存在しない場合はfalse</returns>
        private bool HasData()
        {
            return kakeiboDataGrid.Rows.Count > 0;
        }

        /// <summary>
        /// 金額文字列から記号を除去する
        /// </summary>
        /// <param name="target">対象の文字列</param>
        /// <returns>数字のみの文字列</returns>
        private string CleanPriceText(string target)
        {
            if (string.IsNullOrEmpty(target)) return "";
            return target.Replace(",", "").Replace("￥", "").Replace("\\", "").Trim();
        }
    }
}