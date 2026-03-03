using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Kakeibo.WinForms
{
    class XmlExpenseRepository : IExpenseRepository
    {
        // 保存先のXMLファイル
        private const string FilePath = "expence.xml";

        /// <summary>
        /// XMLを読み込むためのDataSet(スキーマ付き)を作成する(SQLiteのCREATE TABLE)
        /// </summary>
        /// <returns>Expensesテーブルを含むDataSet</returns>
        private DataSet CreateDataSet()
        {
            var dataSet = new DataSet("ExpensesDataSet");
            var table = new DataTable("Expenses");

            // カラムを定義
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("date", typeof(DateTime));
            table.Columns.Add("price", typeof(int));
            table.Columns.Add("category", typeof(string));
            table.Columns.Add("memo", typeof(string));

            // DataSetに表を反映
            dataSet.Tables.Add(table);

            return dataSet;
        }

        /// <summary>
        /// XMLが存在するするなら読み込み、存在しない場合は新しいDataSetを作成する
        /// </summary>
        /// <returns>XMLを読み込んだDataSet、XMLがない場合は新しい空のDataSet</returns>
        private DataSet LoadOrCreateDataSet()
        {
            // 空のDataSetを作成する
            var dataSet = CreateDataSet();

            // XMLが存在するなら読み込む
            if (File.Exists(FilePath))
            {
                dataSet.ReadXml(FilePath, XmlReadMode.ReadSchema);
            }
            return dataSet;
        }

        /// <summary>
        /// XMLに登録されている全支出データを取得し、List<Expense>に変換して返す
        /// </summary>
        /// <returns>XMLに保存されている全ての支出データのリスト</returns>
        public List<Expense> GetAll()
        {
            // XMLを読み込む(なければ新しく作成したDataSetを返す)
            var dataSet = LoadOrCreateDataSet();
            var table = dataSet.Tables["Expenses"];

            var list = new List<Expense>();

            // DataTableを1行ずつExpenseに変換
            foreach (DataRow row in table.Rows)
            {
                list.Add(new Expense
                {
                    // DBの値をC#の型に変換
                    Id = Convert.ToInt32(row["id"]),
                    Date = DateTime.Parse(row["date"].ToString()),
                    Category = row["category"].ToString(),
                    Price = Convert.ToInt32(row["price"]),
                    Memo = row["memo"].ToString()
                });
            }
            return list;
        }

        /// <summary>
        /// 支出データをXMLに追加する
        /// </summary>
        /// <param name="expense">追加する支出データ</param>
        public void Insert(Expense expense)
        {
            // XMLを読み込む(なければ新しく作成したDataSetを返す)
            var dataSet = LoadOrCreateDataSet();
            var table = dataSet.Tables["Expenses"];

            int nextId = 1;
            if (table.Rows.Count > 0)
            {
                // 最後の行のidを取得して+1する
                nextId = Convert.ToInt32(table.Compute("MAX(id)", "")) + 1;
            }

            var row = table.NewRow();
            row["id"] = nextId;
            row["date"] = expense.Date;
            row["price"] = expense.Price;
            row["category"] = expense.Category;
            row["memo"] = expense.Memo;

            // DataTableに行を追加
            table.Rows.Add(row);

            // XMLに保存
            dataSet.WriteXml(FilePath, XmlWriteMode.WriteSchema);
        }

        /// <summary>
        /// 指定したIDの支出データを更新
        /// </summary>
        /// <param name="expense">更新後の内容の支出データ</param>
        public void Update(Expense expense)
        {
            // XMLを読み込む(なければ新しく作成したDataSetを返す)
            var dataSet = LoadOrCreateDataSet();
            var table = dataSet.Tables["Expenses"];

            // idが一致する行を探す
            foreach (DataRow row in table.Rows)
            {
                if (Convert.ToInt32(row["id"]) == expense.Id)
                {
                    // 行の内容を上書き
                    row["date"] = expense.Date;
                    row["price"] = expense.Price;
                    row["category"] = expense.Category;
                    row["memo"] = expense.Memo;

                    // XMLに保存
                    dataSet.WriteXml(FilePath, XmlWriteMode.WriteSchema);
                    return;
                }
            }
        }

        /// <summary>
        /// 指定したIDのデータを削除
        /// </summary>
        /// <param name="id">削除対象の支出データ</param>
        public void Delete(int id)
        {
            // XMLを読み込む(なければ新しく作成したDataSetを返す)
            var dataSet = LoadOrCreateDataSet();
            var table = dataSet.Tables["Expenses"];

            // idが一致する行を探す
            foreach (DataRow row in table.Rows)
            {
                if (Convert.ToInt32(row["id"]) == id)
                {
                    // 行の内容を削除
                    table.Rows.Remove(row);

                    // XMLに保存
                    dataSet.WriteXml(FilePath, XmlWriteMode.WriteSchema);
                    return;
                }
            }
        }

        /// <summary>
        /// 指定したIDの支出データを取得
        /// </summary>
        /// <param name="id">取得したい支出のデータID</param>
        /// <returns>IDに一致する支出データ。見つからなかった場合はnull</returns>
        public Expense GetById(int id)
        {
            var dataSet = LoadOrCreateDataSet();
            var table = dataSet.Tables["Expenses"];

            // idが一致する行を探す
            foreach (DataRow row in table.Rows)
            {
                // DataRowをExpenseに変換して返す
                if (Convert.ToInt32(row["id"]) == id)
                {
                    return new Expense
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Date = Convert.ToDateTime(row["date"]),
                        Price = Convert.ToInt32(row["price"]),
                        Category = row["category"].ToString(),
                        Memo = row["memo"]?.ToString()
                    };
                }
            }
            // 見つからなかった場合
            return null;
        }
    }
}
