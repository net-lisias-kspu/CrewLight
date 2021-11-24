using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSP.Localization;
using ClickThroughFix;

namespace CrewLight
{
    public class MoreMorseSettings : MonoBehaviour
    {
        public static MoreMorseSettings Instance;

        // window pos
        Vector2d windowPos;
        private Rect morseSettingsRect;
        private Rect morseAlphabetRect;

        //private bool showSettingsWindow = false;
        private bool showAlphabetWindow = false;

        private CL_GeneralSettings morseSettings;

        private Texture morseAlph;



        internal bool completed = false;
        internal bool active = false;

        internal double lastTimeTic = 0;
        void Start()
        {
            windowPos = new Vector2d(Screen.width / 2 + 120, Screen.height / 2 - 150);
            morseSettingsRect = new Rect((float)windowPos.x, (float)windowPos.y, 1, 1);
            morseAlphabetRect = new Rect((float)windowPos.x - 680, (float)windowPos.y, 450, 450);

            morseSettings = HighLogic.CurrentGame.Parameters.CustomParams<CL_GeneralSettings>();
            morseAlph = (Texture)GameDatabase.Instance.GetTexture("CrewLight/International_Morse_Code", false);

            Init();
            completed = false;
            Instance = this;
        }

        public void EnableWindow(bool b = true)
        {
            if (b)
            {
                if (!active)
                {
                    active = true;
                    completed = false;
                }
            }
            else
            {
                completed = false;
            }
        }

        public void OnGUI()
        {
            if (active)
            {
                if (Time.realtimeSinceStartup - lastTimeTic > 0.25)
                {
                    active = false;
                    return;
                }
                GUILayout.BeginArea(morseSettingsRect);
                morseSettingsRect = ClickThruBlocker.GUILayoutWindow(991237, morseSettingsRect, MorseSettingsWindow, Localizer.Format("#autoLOC_CL_0077"), GUILayout.ExpandWidth(true));
                GUILayout.EndArea();


                if (showAlphabetWindow)
                {
                    GUILayout.BeginArea(morseAlphabetRect);
                    morseAlphabetRect = ClickThruBlocker.GUILayoutWindow(596064, morseAlphabetRect, MorseAlphabetWindow, Localizer.Format("#autoLOC_CL_0078"));
                    GUILayout.EndArea();
                }
            }
        }
        public void MorseSettingsWindow(int id)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Text:");
            morseTextStr = GUILayout.TextField(morseTextStr, GUILayout.MinWidth(120), GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Morse:");
            morseCodeStr = GUILayout.TextField(morseCodeStr, GUILayout.MinWidth(120), GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (morseTextStr != "")
            {
                morseCodeStr = MorseCodeTranslator.Translate(morseTextStr);
            }
            else
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(/*Dit*/Localizer.Format("#autoLOC_CL_0029") + " (.)"))
                {
                    morseCodeStr += ".";
                }
                if (GUILayout.Button(/*"Dah*/Localizer.Format("#autoLOC_CL_0032") + " (_)"))
                {
                    morseCodeStr += "_";
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(/*"Letter Space */Localizer.Format("#autoLOC_CL_0036") + " ( )"))
                {
                    morseCodeStr += " ";
                }
                if (GUILayout.Button(/*"Word Space*/Localizer.Format("#autoLOC_CL_0038") + " (|)"))
                {
                    morseCodeStr += "|";
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label(/*"Dih duration :"*/Localizer.Format("#autoLOC_CL_0030"), GUILayout.Width(90));
            if (GUILayout.Button("--", GUILayout.Width(25)))
            {
                ditDuration -= .1f;
                UpdateTiming();
            }
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                ditDuration -= .01f;
                UpdateTiming();
            }
            GUILayout.Label(ditDuration.ToString(),GUILayout.Width(30));
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                ditDuration += .01f;
                UpdateTiming();
            }

            if (GUILayout.Button("++", GUILayout.Width(30)))
            {
                ditDuration += .1f;
                UpdateTiming();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            manualTiming = GUILayout.Toggle(manualTiming, /*"Manual Timing"*/Localizer.Format("#autoLOC_CL_0031"));

            GUILayout.BeginHorizontal();
            GUILayout.Label(/*"Dah duration :"*/Localizer.Format("#autoLOC_CL_0033"), GUILayout.Width(90));
            if (GUILayout.Button("--", GUILayout.Width(25)))
            {
                if (manualTiming)
                {
                    dahDuration -= .1f;
                }
            }
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                if (manualTiming)
                {
                    dahDuration -= .01f;
                }
            }
            GUILayout.Label(dahDuration.ToString(), GUILayout.Width(30));
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                if (manualTiming)
                {
                    dahDuration += .01f;
                }
            }
            if (GUILayout.Button("++", GUILayout.Width(30)))
            {
                if (manualTiming)
                {
                    dahDuration += .1f;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(/*"Symbol Space :"*/Localizer.Format("#autoLOC_CL_0034"), GUILayout.Width(90));
            if (GUILayout.Button("--", GUILayout.Width(25)))
            {
                if (manualTiming)
                {
                    symbolSpaceDuration -= .1f;
                }
            }
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                if (manualTiming)
                {
                    symbolSpaceDuration -= .01f;
                }
            }
            GUILayout.Label(symbolSpaceDuration.ToString(), GUILayout.Width(30));
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                if (manualTiming)
                {
                    symbolSpaceDuration += .01f;
                }
            }
            if (GUILayout.Button("++", GUILayout.Width(30)))
            {
                if (manualTiming)
                {
                    symbolSpaceDuration += .1f;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(/*"Letter Space :"*/Localizer.Format("#autoLOC_CL_0036"), GUILayout.Width(90));
            if (GUILayout.Button("--", GUILayout.Width(25)))
            {
                if (manualTiming)
                {
                    letterSpaceDuration -= .1f;
                }
            }
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                if (manualTiming)
                {
                    letterSpaceDuration -= .01f;
                }
            }
            GUILayout.Label(letterSpaceDuration.ToString(), GUILayout.Width(30));
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                if (manualTiming)
                {
                    letterSpaceDuration += .01f;
                }
            }
            if (GUILayout.Button("++", GUILayout.Width(30)))
            {
                if (manualTiming)
                {
                    letterSpaceDuration += .1f;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(/*"Word Space :"*/Localizer.Format("#autoLOC_CL_0038"), GUILayout.Width(90));
            if (GUILayout.Button("--", GUILayout.Width(25)))
            {
                if (manualTiming)
                {
                    wordSpaceDuration -= .1f;
                }
            }
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                if (manualTiming)
                {
                    wordSpaceDuration -= .01f;
                }
            }
            GUILayout.Label(wordSpaceDuration.ToString(), GUILayout.Width(30));
            if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                if (manualTiming)
                {
                    wordSpaceDuration += .01f;
                }
            }
            if (GUILayout.Button("++", GUILayout.Width(30)))
            {
                if (manualTiming)
                {
                    wordSpaceDuration += .1f;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(/*"Cancel"*/Localizer.Format("#autoLOC_CL_0079")))
            {
                CloseSettings();
            }
            if (GUILayout.Button(/*"Morse Alphabet"*/Localizer.Format("#autoLOC_CL_0078")))
            {
                showAlphabetWindow = !showAlphabetWindow;
            }
            if (GUILayout.Button(/*"Apply"*/Localizer.Format("#autoLOC_CL_0080")))
            {
                ApplySettings();
                CloseSettings();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        public void MorseAlphabetWindow(int id)
        {
            GUILayout.Label(morseAlph);

            GUI.DragWindow();
        }
        private void UpdateTiming()
        {
            if (!manualTiming)
            {
                dahDuration = ditDuration * 3;
                symbolSpaceDuration = ditDuration;
                letterSpaceDuration = dahDuration;
                wordSpaceDuration = dahDuration * 3;
            }
        }

        private void CloseSettings()
        {
            //showSettingsWindow = false;
            showAlphabetWindow = false;

            active = false;
            completed = true;
        }

        private void ParseSettings()
        {
            ParseMorseCode();
        }

        private void ParseMorseCode()
        {
            GameSettingsLive.morseCode = new List<MorseCode>();
            foreach (char c in morseCodeStr)
            {
                switch (c)
                {
                    case '.':
                        GameSettingsLive.morseCode.Add(MorseCode.dih);
                        break;
                    case '_':
                        GameSettingsLive.morseCode.Add(MorseCode.dah);
                        break;
                    case '-':
                        GameSettingsLive.morseCode.Add(MorseCode.dah);
                        break;
                    case ' ':
                        GameSettingsLive.morseCode.Add(MorseCode.letterspc);
                        break;
                    case '|':
                        GameSettingsLive.morseCode.Add(MorseCode.wordspc);
                        break;
                    default:
                        GameSettingsLive.morseCode.Add(MorseCode.dih);
                        break;
                }
                GameSettingsLive.morseCode.Add(MorseCode.symspc);
            }

        }

        private void ApplySettings()
        {
            ParseSettings();
            SaveData();

            //showSettingsWindow = false;
            showAlphabetWindow = false;
        }

        // Backup :
        private string morseCodeStr, morseTextStr;
        private float ditDuration, dahDuration, symbolSpaceDuration, letterSpaceDuration, wordSpaceDuration;
        private bool manualTiming;

        private void Init()
        {
            morseCodeStr = morseSettings.morseCodeStr;
            morseTextStr = morseSettings.morseTextStr;
            ditDuration = morseSettings.ditDuration;
            dahDuration = morseSettings.dahDuration;
            symbolSpaceDuration = morseSettings.symbolSpaceDuration;
            letterSpaceDuration = morseSettings.letterSpaceDuration;
            wordSpaceDuration = morseSettings.wordSpaceDuration;
            manualTiming = morseSettings.manualTiming;
        }


        private void SaveData()
        {
            morseSettings.morseCodeStr = morseCodeStr;
            morseSettings.morseTextStr = morseTextStr;
            morseSettings.ditDuration = ditDuration;
            morseSettings.dahDuration = dahDuration;
            morseSettings.symbolSpaceDuration = symbolSpaceDuration;
            morseSettings.letterSpaceDuration = letterSpaceDuration;
            morseSettings.wordSpaceDuration = wordSpaceDuration;
            morseSettings.manualTiming = manualTiming;
        }

    }
}
