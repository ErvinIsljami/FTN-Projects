using PZ1.Models;
using PZ1.Xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ1.ViewModels
{
    public class ImageCollectionViewModel : BindableBase
    {
        private CustomXmlRW _customXmlRW;
        private string _usersFilename;
        private string _projectDirectory;
        private string _imagesContainerDirectory;
        private string _imagesFileName;

        private string _currentUsername;
        private string _currentPassword;
        private bool _isImagesEmpty;

        public ObservableCollection<Image> Images { get; set; }

        public string CurrentUsername
        {
            get { return _currentUsername; }
            set { SetField<string>(ref _currentUsername, value); }
        }

        public string CurrentPassword
        {
            get { return _currentPassword; }
            set { SetField<string>(ref _currentPassword, value); }
        }

        public bool IsImagesEmpty
        {
            get { return _isImagesEmpty; }
            set { SetField<bool>(ref _isImagesEmpty, value); }
        }

        public ImageCollectionViewModel() { }

        public ImageCollectionViewModel(CustomXmlRW customXmlRW)
        {
            _customXmlRW = customXmlRW;

            string workingDirectory = Environment.CurrentDirectory;
            _projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            _usersFilename = ConfigurationManager.AppSettings["usersFile"];
            _usersFilename = _usersFilename.Replace("{AppDir}", _projectDirectory);
            _imagesContainerDirectory = ConfigurationManager.AppSettings["imagesContainerDirectory"];
            _imagesContainerDirectory = _imagesContainerDirectory.Replace("{AppDir}", _projectDirectory);
            _imagesFileName = ConfigurationManager.AppSettings["imagesFile"];
            _imagesFileName = _imagesFileName.Replace("{AppDir}", _projectDirectory);

        }

        public void SetUpInitialUsernameAndPassword(string username, string password)
        {
            CurrentUsername = username;
            CurrentPassword = password;
            GetUserImages();
        }

        public void GetUserImages()
        {
            var allImages = _customXmlRW.DeSerializeObject<List<Image>>(_imagesFileName);
            if(allImages.Any(i => i.Owner == CurrentUsername))
            {
                Images = new ObservableCollection<Image>(allImages.Where(i => i.Owner == CurrentUsername));
                IsImagesEmpty = false;
            }
            else
            {
                Images = new ObservableCollection<Image>();
                IsImagesEmpty = true;
            }
            if(allImages.Count > 0)
            {
                ConcatenateProjectDirectoryToImagePaths();
            }
        }

        private void ConcatenateProjectDirectoryToImagePaths()
        {
            foreach(var image in Images)
            {
                image.Path = $@"{_projectDirectory}\{image.Path}";
            }
        }
    }
}
