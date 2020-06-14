using Assets.Source.Configuration;
using System;
using UnityEngine;


namespace Assets.Source
{
    public static class UnityApplication
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Startup()
        {
            ConfigurationRepository.RefreshAll();
        }

    }
}
