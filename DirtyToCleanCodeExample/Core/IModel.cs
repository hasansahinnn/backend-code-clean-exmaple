using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IModel // hem marker hemde önemli ortak kolonları tutar. burada ayrıca bir Model class'ı açarak Id leri arka planda eklerdim ama yine iş çok büyüyecek 
    {
        public int Id { get; set; }
    }
}
