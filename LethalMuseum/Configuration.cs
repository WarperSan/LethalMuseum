using BepInEx.Configuration;

namespace LethalMuseum;

public class Configuration
{
    public readonly ConfigEntry<string> Blacklist;
    
    public Configuration(ConfigFile cfg)
    {
        Blacklist = cfg.Bind(
            "General", 
            "itemBlacklist",
            "",
            "List of every item to disable by default"
        );
    }
}