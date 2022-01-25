using System.IO;

namespace ShowPathInTab.Models
{
    public class TabModel : ObservableModel
    {
        private string _fullPath;
        private string _displayPath;

        public string FullPath
        {
            get => _fullPath;
            set
            {
                if (value == _fullPath) return;
                _fullPath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FileName));
            }
        }

        public string DisplayPath
        {
            get => _displayPath;
            set
            {
                if (value == _displayPath) return;
                _displayPath = value;
                OnPropertyChanged();
            }
        }

        public string DirectoryName => Path.GetDirectoryName(FullPath);

        public string FileName => Path.GetFileName(FullPath);

        public TabModel()
        {
        }

        public TabModel(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}
