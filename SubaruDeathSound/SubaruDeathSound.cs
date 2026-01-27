using System.Media;
using System.Reflection;
using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using OWML.Utils;
using UnityEngine;

namespace SubaruDeathSound
{
    public class SubaruDeathSound : ModBehaviour
    {
        public static SubaruDeathSound Instance;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }


        private AudioClip deathClip;
        private bool firstWakeUp = true;

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(SubaruDeathSound)} is loaded!", MessageType.Success);
            ModHelper.Console.WriteLine("Hi Aubrey!", MessageType.Info);

            new Harmony("LostPython.SubaruDeathSound").PatchAll(Assembly.GetExecutingAssembly());

            // Example of accessing game code.
            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;

            GlobalMessenger.AddListener("WakeUp", OnWakeUp);
            
        }

        private void OnWakeUp()
        {
            // set clip to audio file
            deathClip = this.ModHelper.Assets.GetAudio("assets/subaru_death_sound.mp3");

            if (firstWakeUp != true)
            {
                ModHelper.Console.WriteLine("You just woke up (Queuing sound)", MessageType.Success);
                StartCoroutine(PlayDeathSoundDelayed());
            } else
            {
                firstWakeUp = false;
                ModHelper.Console.WriteLine("Noticed this is the first time waking up.", MessageType.Success);
            }
            ;

        }

        public void PlayDeathSound()
        {
            Locator.GetPlayerAudioController()._oneShotSource.PlayOneShot(deathClip);
        }

        private System.Collections.IEnumerator PlayDeathSoundDelayed()
        {
            yield return new WaitForSeconds(0.25f);
            ModHelper.Console.WriteLine("Done waiting, playing sound now.", MessageType.Success);
            PlayDeathSound();
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;
            ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);

            
            
        }


        
    }

}
