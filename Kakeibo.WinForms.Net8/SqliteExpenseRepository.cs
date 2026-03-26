using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Kakeibo.WinForms
{
    /// <summary>
    /// SQLiteを使用して支出データを保存するクラス
    /// </summary>
    internal class SqliteExpenseRepository : IExpenseRepository
    {
        /// <summary>
        /// SQLiteの接続文字列
        /// </summary>
        private const string ConnectionString = "Data Source=expenses.db";

        /// <summary>
        /// 支出データを全件取得するためのSQL文
        /// </summary>
        private const string SqlSelectAll = @"
            SELECT id, date, category, price, memo
            FROM expenses;
        ";

        /// <summary>
        /// expensesテーブルが存在しない場合に新しくテーブル作成するためのSQL文
        /// </summary>
        private const string SqlCreateTable = @"
            CREATE TABLE IF NOT EXISTS expenses (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Category TEXT,
                Price INTEGER,
                Memo TEXT
            );
        ";

        /// <summary>
        /// 新しく支出データを追加するためのSQL文
        /// </summary>
        private const string SqlInsert = @"
            INSERT INTO expenses(date, category, price, memo)
            VALUES(
                @date,
                @category,
                @price,
                @memo
                );
        ";

        /// <summary>
        /// 既存の支出データを更新するためのSQL文
        /// Idを指定して、日付・カテゴリ・金額・メモを更新する
        /// </summary>
        private const string SqlUpdate = @"
            UPDATE expenses
            SET date = @date,
                category = @category,
                price = @price,
                memo = @memo
            WHERE id = @id;
        ";

        /// <summary>
        /// 指定したIDの支出データを削除するためのSQL文
        /// </summary>
        private const string SqlDelete = @"
            DELETE FROM expenses
            WHERE Id = @id;
        ";

        /// <summary>
        /// DBへ接続
        /// テーブルが存在しない場合は新規作成する
        /// </summary>
        public SqliteExpenseRepository()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                // 接続開始
                connection.Open();
                // SQL実行のためのコマンドを作成
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = SqlCreateTable;

                    // SQL文の実行
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 支出データをDBに追加する
        /// </summary>
        /// <param name="expense">登録する支出データ</param>
        public void Insert(Expense expense)
        {
            // DBへ接続
            using (var connection = new SqliteConnection(ConnectionString))
            {
                // 接続開始
                connection.Open();

                // SQL実行のためのコマンドを作成
                using (var command = connection.CreateCommand())
                {
                    // INSERT文で値を入れる
                    command.CommandText = SqlInsert;

                    // SQL文内の@dateにdateを渡す
                    command.Parameters.AddWithValue("@date", expense.Date.ToString("yyyy-MM-dd"));
                    // SQL文内の@categoryにcategoryを渡す
                    command.Parameters.AddWithValue("@category", expense.Category);
                    // SQL文内の@priceにpriceを渡す
                    command.Parameters.AddWithValue("@price", expense.Price);
                    // SQL文内の@memoにmemoを渡す
                    command.Parameters.AddWithValue("@memo", expense.Memo);

                    // INSERT文の実行
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// DBに登録されている全支出データを取得する
        /// </summary>
        /// <returns>データベースに保存されている全ての支出データのリスト</returns>
        public List<Expense> GetAll()
        {
            var list = new List<Expense>();

            // DBへ接続
            using var connection = new SqliteConnection(ConnectionString);
            // 接続開始
            connection.Open();

            // SQL実行のためのコマンドを作成
            using var command = connection.CreateCommand();
            // 全件取得するSELECT文
            command.CommandText = SqlSelectAll;

            // SQLを実行して結果を読み取る
            using var reader = command.ExecuteReader();

            // カラム名から番号を先に取得しておく(GetOrdinalはカラム名から番号を取得するメソッド)
            int idIndex = reader.GetOrdinal("id");
            int dateIndex = reader.GetOrdinal("date");
            int categoryIndex = reader.GetOrdinal("category");
            int priceIndex = reader.GetOrdinal("price");
            int memoIndex = reader.GetOrdinal("memo");

            while (reader.Read())
            {
                // IsDBNullでnull判定
                int id = reader.IsDBNull(idIndex) ? 0 : reader.GetInt32(idIndex);
                string dateText = reader.IsDBNull(dateIndex) ? "" : reader.GetString(dateIndex);
                string category = reader.IsDBNull(categoryIndex) ? "" : reader.GetString(categoryIndex);
                int price = reader.IsDBNull(priceIndex) ? 0 : reader.GetInt32(priceIndex);
                string memo = reader.IsDBNull(memoIndex) ? "" : reader.GetString(memoIndex);

                // DBから取得した値をExpenseに詰めてリストへ追加
                list.Add(new Expense
                {
                    Id = id,
                    Date = string.IsNullOrEmpty(dateText) ? DateTime.MinValue : DateTime.Parse(dateText),
                    Category = category,
                    Price = price,
                    Memo = memo
                });
            }
            return list;
        }

        /// <summary>
        /// 指定したIDの支出データを更新
        /// </summary>
        /// <param name="expense">更新内容を含むデータ</param>
        public void Update(Expense expense)
        {
            using var connection = new SqliteConnection(ConnectionString);
            // 接続開始
            connection.Open();

            // SQL実行のためのコマンドを作成
            using var command = connection.CreateCommand();

            // 指定したIDのデータを更新
            command.CommandText = SqlUpdate;

            // パラメータをSQLに渡す
            command.Parameters.AddWithValue("@date", expense.Date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@category", expense.Category);
            command.Parameters.AddWithValue("@price", expense.Price);
            command.Parameters.AddWithValue("@memo", expense.Memo);
            command.Parameters.AddWithValue("@id", expense.Id);

            // UPDATE文の実行
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 指定したIDのデータを削除
        /// </summary>
        /// <param name="id">削除対象のID</param>
        public void Delete(int id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                // 接続開始
                connection.Open();
                // SQL実行のためのコマンドを作成
                using (var command = connection.CreateCommand())
                {
                    // 指定したIDのデータを削除
                    command.CommandText = SqlDelete;

                    // SQL文内の@idにidを渡す
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 指定したIDの支出データを1件取得する
        /// </summary>
        /// <param name="id">取得したいデータのID</param>
        /// <returns>指定したIDに一致する支出データ（存在しない場合は null）</returns>
        public Expense GetById(int id)
        {
            return GetAll().Find(x => x.Id == id);
        }
    }
}
