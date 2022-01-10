using PZ1.Commands;
using PZ1.Models;
using PZ1.Xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ1.ViewModels
{
    public class AddImageViewModel : BindableBase
    {
        private CustomXmlRW _customXmlRW;
        private string _usersFilename;
        private string _projectDirectory;
        private string _imagesContainerDirectory;
        private string _imagesFileName;

        private string _currentUsername;
        private string _currentPassword;
        private string _initialImageUrl;
        private string _currentImageUrl;
        private string _currentImageRelativePath;
        private string _currentImageTitle;
        private string _currentImageDescription;
        private string _addImageErrorMessage;

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

        public string InitialImageUrl
        {
            get { return _initialImageUrl; }
            set { SetField<string>(ref _initialImageUrl, value); }
        }

        public string CurrentImageUrl
        {
            get { return _currentImageUrl; }
            set
            {
                SetField<string>(ref _currentImageUrl, value);
                AddImageCommand.RaiseCanExecuteChanged();
            }
        }

        public string CurrentImageTitle
        {
            get { return _currentImageTitle; }
            set
            {
                SetField<string>(ref _currentImageTitle, value);
                AddImageCommand.RaiseCanExecuteChanged();
            }
        }

        public string CurrentImageDescription
        {
            get { return _currentImageDescription; }
            set
            {
                SetField<string>(ref _currentImageDescription, value);
                AddImageCommand.RaiseCanExecuteChanged();
            }
        }

        public string CurrentImageRelativePath
        {
            get { return _currentImageRelativePath; }
            set { SetField<string>(ref _currentImageRelativePath, value); }
        }

        public string AddImageErrorMessage
        {
            get { return _addImageErrorMessage; }
            set
            {
                _addImageErrorMessage = value;
                OnPropertyChanged(nameof(AddImageErrorMessage));
            }
        }

        public MyICommand AddImageCommand { get; set; }
        public MyICommand BrowseImageCommand { get; set; }

        public delegate void AddedImageEventHandler(object sender, EventArgs args);
        public event AddedImageEventHandler ImageAdded;

        public AddImageViewModel() { }

        public AddImageViewModel(CustomXmlRW customXmlRW)
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

            var images = _customXmlRW.DeSerializeObject<List<Image>>(_imagesFileName);
            if (images == null)
            {
                images = new List<Image>();
                _customXmlRW.SerializeObject<List<Image>>(images, _imagesFileName);
            }

            _initialImageUrl = "../Resources/addImage.png";
            _currentImageUrl = _initialImageUrl;

            AddImageCommand = new MyICommand(OnAddImage, AddImageCanExecute);
            BrowseImageCommand = new MyICommand(OnBrowseImage);
        }

        public void SetUpInitialUsernameAndPassword(string username, string password)
        {
            CurrentUsername = username;
            CurrentPassword = password;
        }

        public void OnAddImage()
        {
            AddImageErrorMessage = "";

            if(!string.IsNullOrWhiteSpace(CurrentImageTitle))
            {
                if (CurrentImageTitle.Length > 30)
                {
                    AddImageErrorMessage = "Image title cannot be longer than 30 characters.";
                    return;
                }
            }
            else
            {
                AddImageErrorMessage = "Image title is required.";
                return;
            }

            if (!string.IsNullOrWhiteSpace(CurrentImageUrl))
            {
                if (CurrentImageUrl.Length > 150)
                {
                    AddImageErrorMessage = "Image URI cannot be longer than 150 characters.";
                    return;
                }
            }
            else
            {
                AddImageErrorMessage = "Image URI is required.";
            }
            
            if (!string.IsNullOrWhiteSpace(CurrentImageDescription))
            {
                if (CurrentImageDescription.Length > 150)
                {
                    AddImageErrorMessage = "Image description cannot be longer that 150 characters.";
                    return;
                }
            }

            var images = _customXmlRW.DeSerializeObject<List<Image>>(_imagesFileName);
            if(images.Count > 0)
            {
                if(images.FirstOrDefault(i => i.Path.Contains(CurrentImageRelativePath)) == null)
                {
                    string imageAbsolutePath = $@"{_projectDirectory}\{CurrentImageRelativePath}";
                    if (!File.Exists(imageAbsolutePath))
                    {
                        File.Copy(CurrentImageUrl, imageAbsolutePath);
                        Image newImage = new Image(CurrentImageRelativePath, 
                                                   CurrentImageTitle, 
                                                   CurrentImageDescription, 
                                                   CurrentUsername);
                        images.Add(newImage);
                        _customXmlRW.SerializeObject<List<Image>>(images, _imagesFileName);
                    }
                }
                else
                {
                    AddImageErrorMessage = "Image with this filename is already added.";
                    return;
                }
            }
            else
            {
                string imageAbsolutePath = $@"{_projectDirectory}\{CurrentImageRelativePath}";
                if (!File.Exists(imageAbsolutePath))
                {
                    File.Copy(CurrentImageUrl, imageAbsolutePath);
                    Image newImage = new Image(CurrentImageRelativePath,
                                               CurrentImageTitle,
                                               CurrentImageDescription,
                                               CurrentUsername);
                    images.Add(newImage);
                    _customXmlRW.SerializeObject<List<Image>>(images, _imagesFileName);
                }
            }
            ImageAdded?.Invoke(this, EventArgs.Empty);
            CurrentImageUrl = _initialImageUrl;
            CurrentImageTitle = "";
            CurrentImageDescription = "";
        }

        private bool AddImageCanExecute()
        {
            return !(string.IsNullOrWhiteSpace(CurrentImageTitle) || string.IsNullOrWhiteSpace(CurrentImageUrl) || CurrentImageUrl == _initialImageUrl);
        }

        public void OnBrowseImage()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Title = "Select an image";
            dialog.DefaultExt = ".png";
            dialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.gif|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "PNG (*.png)|*.png|" +
                        "GIF (*.gif)|*.gif";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                var absoluteFileName = dialog.FileName;
                int lastSlashIndex = absoluteFileName.LastIndexOf('\\');                  
                var relativeFileName = absoluteFileName.Substring(lastSlashIndex + 1);

                //var newAbsoluteCopyingPath = $@"{_projectDirectory}\{_imagesContainerDirectory}\{relativeFileName}";
                var newRelativePath = $@"{_imagesContainerDirectory}\{relativeFileName}";

                CurrentImageUrl = absoluteFileName;
                CurrentImageRelativePath = newRelativePath;
            }
        }
    }
}
