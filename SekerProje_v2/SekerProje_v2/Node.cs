using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekerProje_v2
{
    public class Node
    {
        private string PrsnlCardId;
        public string prsnlCardId
        {
            get { return PrsnlCardId; }
            set { PrsnlCardId = value; }
        }


        private string PrsnlName;

        public string prsnlName
        {
            get { return PrsnlName; }
            set { PrsnlName = value; }
        }
        private string PrsnlSurname;

        public string prsnlSurname
        {
            get { return PrsnlSurname; }
            set { PrsnlSurname = value; }
        }
        private string PrsnlId;
        public string prsnlId
        {
            get { return PrsnlId; }
            set { PrsnlId = value; }
        }
        private string PrsnlDepartment;

        public string prsnlDepartment
        {
            get { return PrsnlDepartment; }
            set { PrsnlDepartment = value; }
        }


    }
}
