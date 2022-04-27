using Assets.Source.Configuration;
using UnityEngine;


namespace Assets.Source
{
    public static class UnityApplication
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Startup()
        {
            var hello =$@"HELLO!
            I see you've opened the console.  I hope you have an awesome day!  Please don't do anything illegal.

            ROTP 
            version: {VERSION}
            (C) 2022 by Jordan Bleu
            ";

            Debug.Log(hello);

            ConfigurationRepository.RefreshAll();
        }

        public const string VERSION = "1.2.0";

    }
}
