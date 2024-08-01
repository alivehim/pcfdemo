using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Read
{
    public interface IBookService
    {
        Book Add(Book book);
        Book Load(string name,string fullName);
        void Delete(Book book);
        Book FindByName(string name);
        void Delete(string bookName);
        Book FindByNameEx(string name);

        void Update(Book book);
    }
}
