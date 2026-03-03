using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Kakeibo.WinForms
{

    internal class SqliteExpenseRepository : IExpenseRepository
    {
        private const string ConnectionString = "Data Source=expenses.db";

        private const string SqlSelectAll = @"
            SELECT id, date, category, price, memo
            FROM expenses;
        ";

        private const string SqlCreateTable = @"
            CREATE TABLE IF NOT EXISTS expenses (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Category TEXT,
                Price INTEGER,
                Memo TEXT
            );
        ";

        private const string SqlInsert = @"
            INSERT INTO expenses(date, category, price, memo)
            VALUES(
                @date,
                @category,
                @price,
                @memo
                );
        ";

        private const string SqlUpdate = @"
            UPDATE expenses
            SET date = @date,
                category = @category,
                price = @price,
                memo = @memo
            WHERE id = @id;
        ";

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
            using (var connection = new SqliteConnection(ConnectionString))
            {
                // 接続開始
                connection.Open();

                // SQL実行のためのコマンドを作成
                using (var command = connection.CreateCommand())
                {
                    // 全件取得するSELECT文
                    command.CommandText = SqlSelectAll;

                    // SQLを実行して結果を読み取る
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Expense
                            {
                                // DBの値をC#の型に変換
                                Id = Convert.ToInt32(reader["id"]),
                                Date = DateTime.Parse(reader["date"].ToString()),
                                Category = reader["category"].ToString(),
                                Price = Convert.ToInt32(reader["price"]),
                                Memo = reader["memo"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 指定したIDの支出データを更新
        /// </summary>
        /// <param name="expense">更新内容を含むデータ</param>
        public void Update(Expense expense)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                // 接続開始
                connection.Open();

                // SQL実行のためのコマンドを作成
                using (var command = connection.CreateCommand())
                {
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
            }
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
