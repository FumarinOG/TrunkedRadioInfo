using ObjectLibrary;

namespace FileService.Interfaces
{
    public interface IRadioParser
    {
        Radio ParseRadio(string[] row);
    }
}
