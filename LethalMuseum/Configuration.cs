using BepInEx.Configuration;

namespace LethalMuseum;

public class Configuration
{
    public readonly ConfigEntry<string> Blacklist;

    public readonly ConfigEntry<bool> AllowBaby;
    public readonly ConfigEntry<bool> AllowBody;
    
    public Configuration(ConfigFile cfg)
    {
        Blacklist = cfg.Bind(
            "General", 
            "itemBlacklist",
            "",
            "List of every item to disable by default"
        );

        AllowBaby = cfg.Bind(
            "Items",
            "allowBabyItem",
            false,
            "Defines if the Maneater should count as an item to collect.\n\nThis can lead to problem from the tracker, but it is a fun challenge."
        );
        
        AllowBody = cfg.Bind(
            "Items",
            "allowBodyItem",
            false,
            "Defines if the body of a dead player should count as an item to collect."
        );
    }
}