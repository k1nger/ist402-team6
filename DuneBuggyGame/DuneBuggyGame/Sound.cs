// Project: DuneBuggyGame, File: Sound.cs
// Namespace: DuneBuggyGame.Sounds, Class: Sound
// Path: \DuneBuggyGame\Sounds
// Author: Team 6 - Ryan King
// Code lines: 232
// Creation date: 10.29.2008 18:22
// Last modified: 10.29.2008 18:22


#region Using Directives
using Microsoft.Xna.Framework.Audio;
using System;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework;
using System.IO;
#endregion


namespace DuneBuggyGame
{
    class Sound
    {
        #region Variables
        /// <summary>
        /// Sound stuff for XAct
        /// </summary>
        static AudioEngine audioEngine;
        /// <summary>
        /// Wave bank
        /// </summary>
        static WaveBank waveBank;
        /// <summary>
        /// Sound bank
        /// </summary>
        static SoundBank soundBank;
        static AudioCategory musicCategory;
        #endregion

        #region Enums
        /// <summary>
        /// Sounds we use in this game.
        /// </summary>
        /// <returns>Enum</returns>
        public enum Sounds
        {
            Booing,
            Cheering,
            Congratulations,
            MenuSong,
            MenuMove,
            MenuSelect
        } // enum Sounds
        #endregion

        #region Constructor
        /// <summary>
        /// Create sound
        /// </summary>
        static Sound()
        {
            try
            {
                audioEngine = new AudioEngine(@"Content\Sounds\GameSounds.xgs");
                
                waveBank = new WaveBank(audioEngine,
                    @"Content\Sounds\Wave Bank.xwb");
                
                if (waveBank != null)
                  soundBank = new SoundBank(audioEngine,
                      @"Content\Sounds\Sound Bank.xsb");

                // Get the music category to change the music volume and stop music
              musicCategory = audioEngine.GetCategory("Music");
            } // try
            catch (Exception ex)
            {
                Log.Instance.Write("Failed to create sound class: " + ex.ToString());
            } // catch
        } // Sound()
        #endregion

        #region Play
        /// <summary>
        /// Play
        /// </summary>
        /// <param name="soundName">Sound name</param>
        public static void Play(string soundName)
        {
            if (soundBank == null)
                return;
            try
            {
                soundBank.PlayCue(soundName);
            } // try
            catch (Exception ex)
            {
                Log.Instance.Write("Playing sound " + soundName + " failed: " + ex.ToString());
            } // catch
        } // Play(soundName)


        /// <summary>
        /// Play
        /// </summary>
        /// <param name="sound">Sound</param>
        public static void Play(Sounds sound)
        {
            Play(sound.ToString());
        } // Play(sound)
        #endregion
        
        #region Music
        /// <summary>
        /// Start music
        /// </summary>
        public static void StartMusic()
        {
            if (soundBank.IsInUse == false)
                Play(Sounds.MenuSong);
        } // StartMusic()

        /// <summary>
        /// Stop music
        /// </summary>
        public static void StopMusic()
        {
            musicCategory.Stop(AudioStopOptions.Immediate);
        } // StopMusic()
        #endregion

        #region Update
        /// <summary>
        /// Update, calls audioEngine.Update
        /// </summary>
        public static void Update()
        {
            if (audioEngine != null)
                audioEngine.Update();
        } // Update()
        #endregion
    }
}
