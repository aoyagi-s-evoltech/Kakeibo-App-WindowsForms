using System.Collections.Generic;

namespace Kakeibo.WinForms
{
    /// <summary>
    /// データを読み書きするためのインターフェース
    /// Sqliteの場合でもXMLの場合でも、同じ方法でデータの操作が可能になる
    /// </summary>
    internal interface IExpenseRepository
    {
        // 登録されている全ての支出を取得する
        List<Expense> GetAll();

        // 指定したIDの支出データを取得する
        Expense GetById(int id);

        // 新しい支出データを追加する
        void Insert(Expense expense);

        // 指定した支出データを更新する
        void Update(Expense expense);

        // 指定したIDの支出データを削除する
        void Delete(int id);
    }
}
