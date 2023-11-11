using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProcessedFileService
{
    public class UploadFileModel : INotifyPropertyChanged
    {
        public virtual string SystemID
        {
            get => _systemID;
            set => SetProperty(ref _systemID, value);
        }

        public virtual string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        public virtual FileTypes Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public virtual DateTime FileDate
        {
            get => _fileDate;
            set => SetProperty(ref _fileDate, value);
        }

        public virtual DateTime LastModified
        {
            get => _lastModified;
            set => SetProperty(ref _lastModified, value);
        }

        public virtual long Size { get; set; }

        public long SizeK => Size / 1024;

        public string TypeText => _type.GetEnumDescription();

        public virtual FileStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string StatusText => _status.GetEnumDescription();

        public virtual int RowCount
        {
            get => _rowCount;
            set => SetProperty(ref _rowCount, value);
        }

        private string _systemID;
        private string _fileName;
        private FileTypes _type;
        private DateTime _fileDate;
        private DateTime _lastModified;
        private FileStatus _status;
        private int _rowCount;

        public UploadFileModel() => Status = FileStatus.NotProcessed;

        public override string ToString() => $"Filename {_fileName}, Type {_type}";

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
