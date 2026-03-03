using System.Collections.Generic;

namespace Kakeibo.WinForms
{
    internal interface IExpenseRepository
    {
        List<Expense> GetAll();
        Expense GetById(int id);
        void Insert(Expense expense);
        void Update(Expense expense);
        void Delete(int id);
    }
}
