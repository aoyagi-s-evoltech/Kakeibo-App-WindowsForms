using System;

namespace Kakeibo.WinForms
{
    /// <summary>
    /// 1件分の支出データを表すクラス
    /// </summary>
    internal class Expense
    {
        /// <summary>
        /// 支出データを識別するためのID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 支出が発生した日付
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 支出金額
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 支出のカテゴリ（食費・交通費など）
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 支出に関するメモ（任意）
        /// </summary>
        public string Memo { get; set; }
    }
}
