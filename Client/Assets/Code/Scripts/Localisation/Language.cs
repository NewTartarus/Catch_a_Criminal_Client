using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScotlandYard.Scripts.Localisation
{
    public class Language
    {
        protected int id;
        protected string name;

        public int ID
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public Language(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
