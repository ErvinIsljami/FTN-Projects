using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using FileReader.Interfaces;

namespace FileReader
{
    public class Watcher : IWatcher
    {
        private IReader _reader;
        private ILogger _logger;
        private IWriter _writer;
        private FileSystemWatcher _fileSystemWatcher;
        private Task _watcherTask;
        private string _directoryToWatch;
        private string _logDirectory;

        public IReader Reader { get => _reader; set => _reader = value; }
        public ILogger Logger { get => _logger; set => _logger = value; }
        public IWriter Writer { get => _writer; set => _writer = value; }
        public FileSystemWatcher FileSystemWatcher { get => _fileSystemWatcher; set => _fileSystemWatcher = value; }
        public Task WatcherTask { get => _watcherTask; set => _watcherTask = value; }
        public string DirectoryToWatch { get => _directoryToWatch; set => _directoryToWatch = value; }
        public string LogDirectory { get => _logDirectory; set => _logDirectory = value; }

        public Watcher(IReader reader, ILogger logger, IWriter writer, string directoryToWatch, string logDirectory)
        {
            _reader = reader;
            _logger = logger;
            _writer = writer;
            _directoryToWatch = directoryToWatch;
            _logDirectory = logDirectory;
        }

        public void Watch()
        {
            _watcherTask = new Task(() =>
            {
                InitializeWatcher(_directoryToWatch);
            });
            _watcherTask.Start();
        }

        private void InitializeWatcher(string directoryToWatch)
        {
            _fileSystemWatcher = new FileSystemWatcher(directoryToWatch)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                IncludeSubdirectories = false,
                Filter = "*.*"
            };
            _fileSystemWatcher.Created += new FileSystemEventHandler(OnNewFileAdded);
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void OnNewFileAdded(object sender, FileSystemEventArgs eventArgs)
        {
            string readErrorMessage;
            string writeErrorMessage;

            List<PowerConsumptionData> data = _reader.Read(eventArgs.FullPath, out readErrorMessage);

            if (readErrorMessage == "")
            {
                _writer.Write(data, out writeErrorMessage);

                if (writeErrorMessage != "")
                {
                    _logger.Log($"File: '{eventArgs.Name}' Error(s): {writeErrorMessage}", _logDirectory);
                }
            }
            else
            {
                _logger.Log($"File: '{eventArgs.Name}' Error: {readErrorMessage}", _logDirectory);
            }
        }
    }
}
