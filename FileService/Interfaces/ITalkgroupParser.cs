using ObjectLibrary;

namespace FileService.Interfaces
{
    public interface ITalkgroupParser
    {
        Talkgroup ParseTalkgroup(string[] row);
    }
}
