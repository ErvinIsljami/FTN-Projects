using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentServiceData
{
    public class Student : TableEntity
    {
        public String Name { get; set; }

        public String LastName { get; set; }

        public Student(String indexNo)
        {
            PartitionKey = "Student";
            RowKey = indexNo;
        }
        public Student() { }
    }
}
