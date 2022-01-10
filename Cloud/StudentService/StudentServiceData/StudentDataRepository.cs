using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

namespace StudentServiceData
{
    public class StudentDataRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;
        public StudentDataRepository()
        {
            _storageAccount =
           CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new
           Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("StudentTable");
            _table.CreateIfNotExists();
        }
        public IQueryable<Student> RetrieveAllStudents()
        {
            var results = from g in _table.CreateQuery<Student>()
                          where g.PartitionKey == "Student"
                          select g;
            return results;
        }
        public void AddStudent(Student newStudent)
        {
            // Samostalni rad: izmestiti tableName u konfiguraciju servisa.
            TableOperation insertOperation = TableOperation.Insert(newStudent);
            _table.Execute(insertOperation);
        }
    }

}
