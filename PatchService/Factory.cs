using ObjectLibrary;

namespace PatchService
{
    public static class Factory
    {
        public static PatchViewModel CreatePatchModel(SystemInfo systemInfo, Patch patch) =>
            new PatchViewModel(systemInfo.SystemID, systemInfo.Description, patch.FromTalkgroupID, patch.FromTalkgroupName, patch.ToTalkgroupID,
                patch.ToTalkgroupName, patch.HitCount);
    }
}
