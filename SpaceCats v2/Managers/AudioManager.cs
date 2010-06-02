using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpaceCats_v2
{
    public enum SongList
    {
        None,
        Theme,
        Mission1,
        Mission2,
        Mission3,
        Mission4,
        Mission5,
        Boss,
        GameOver,
        MissionComplete,
        Dialog,
        Credits
    }

    public class AudioManager
    {
        //********************************************
        // Fields
        //********************************************
        private Main z_game;
        private Song z_theme, z_boss, z_gameOver, z_missionComplete, z_dialog, z_credits;
        private Song z_mission1, z_mission2, z_mission3, z_mission4, z_mission5;
        private SongList z_currentSong;
        private bool z_musicOn, z_soundFXOn, z_soundOn;
        private float z_volMusic, z_volSoundFX, z_volMaster;

        //********************************************
        // Public Properties
        //********************************************
        public SongList CurrentSong
        {
            get { return z_currentSong; }
        }
        public bool SoundOn
        {
            get { return z_soundOn; }
            set
            {
                z_soundOn = value;
                if (value)
                {
                    if (z_musicOn)
                        MediaPlayer.Volume = z_volMusic * z_volMaster;
                    if (z_soundFXOn)
                        SoundEffect.MasterVolume = z_volSoundFX * z_volMaster;
                }
                else
                {
                    MediaPlayer.Volume = 0.0f;
                    SoundEffect.MasterVolume = 0.0f;
                }
            }
        }
        public bool MusicOn
        {
            get { return z_musicOn; }
            set 
            { 
                z_musicOn = value;
                if (value && z_soundOn)
                    MediaPlayer.Volume = z_volMusic * z_volMaster;
                else
                    MediaPlayer.Volume = 0.0f;
            }
        }
        public bool SoundFXOn
        {
            get { return z_soundFXOn; }
            set 
            { 
                z_soundFXOn = value;
                if (value && z_soundOn)
                    SoundEffect.MasterVolume = z_volSoundFX * z_volMaster;
                else
                    SoundEffect.MasterVolume = 0.0f;
            }
        }
        public float MusicVolume
        {
            get { return z_volMusic; }
            set 
            {
                if (value < 0.0f)
                    z_volMusic = 0.0f;
                else if (value > 1.0f)
                    z_volMusic = 1.0f;
                else
                    z_volMusic = value;
                if (z_musicOn && z_soundOn)
                    MediaPlayer.Volume = value * z_volMaster;
            }
        }
        public float SoundFXVolume
        {
            get { return z_volSoundFX; }
            set 
            {
                if (value < 0.0f)
                    z_volSoundFX = 0.0f;
                else if (value > 1.0f)
                    z_volSoundFX = 1.0f;
                else
                    z_volSoundFX = value;
                if (z_soundFXOn && z_soundOn)
                    SoundEffect.MasterVolume = value * z_volMaster;
            }
        }
        public float MasterVolume
        {
            get { return z_volMaster; }
            set 
            {
                if (value < 0.0f)
                    z_volMaster = 0.0f;
                else if (value > 1.0f)
                    z_volMaster = 1.0f;
                else
                    z_volMaster = value;
                if (z_soundFXOn && z_soundOn)
                    SoundEffect.MasterVolume = z_volSoundFX * z_volMaster;
                if (z_musicOn && z_soundOn)
                    MediaPlayer.Volume = z_volMusic * z_volMaster;
            }
        }

        //********************************************
        // Constructors
        //********************************************
        public AudioManager(Main game)
        {
            z_game = game;
            MediaPlayer.IsRepeating = true;
            z_currentSong = SongList.None;
            
            z_volMusic = 1.0f;
            z_volSoundFX = 1.0f;
            MasterVolume = 1.0f;
            MusicOn = true;
            SoundFXOn = true;
            SoundOn = true;
        }

        //********************************************
        // Methods
        //********************************************
        public void Update(GameTime gametime)
        {
        }

        public void Reset()
        {
        }

        public void Play(SongList song)
        {
            Song newSong = null;
            z_currentSong = song;
            switch (z_currentSong)
            {
                case SongList.Theme:
                    newSong = z_theme;
                    break;
                case SongList.Boss:
                    newSong = z_boss;
                    break;
                case SongList.Credits:
                    newSong = z_credits;
                    break;
                case SongList.Dialog:
                    newSong = z_dialog;
                    break;
                case SongList.GameOver:
                    newSong = z_gameOver;
                    break;
                case SongList.MissionComplete:
                    newSong = z_missionComplete;
                    break;
                case SongList.Mission1:
                    newSong = z_mission1;
                    break;
                case SongList.Mission2:
                    newSong = z_mission2;
                    break;
                case SongList.Mission3:
                    newSong = z_mission3;
                    break;
                case SongList.Mission4:
                    newSong = z_mission4;
                    break;
                case SongList.Mission5:
                    newSong = z_mission5;
                    break;
                case SongList.None:
                    if(MediaPlayer.State!= MediaState.Stopped)
                        MediaPlayer.Stop();
                    return;
                default:
                    break;
            }
            if (MediaPlayer.Queue.ActiveSong != newSong)
            {
                MediaPlayer.Play(newSong);
            }
        }

        public void LoadContent()
        {
            z_theme = z_game.Content.Load<Song>("Audio\\Music\\AdventuresofCaptainSquiggles");
            z_mission1 = z_game.Content.Load<Song>("Audio\\Music\\ATreeFalls");
            z_mission2 = z_game.Content.Load<Song>("Audio\\Music\\OutsideMyComfortZone");
            z_boss = z_game.Content.Load<Song>("Audio\\Music\\Mark Oleson - Peril Probability Prime WIP 1");
        }

    }
}
