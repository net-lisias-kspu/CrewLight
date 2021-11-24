using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CrewLight
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class MorseCodeTranslator : MonoBehaviour
    {
        private static Dictionary<char, string> _morseAlphabetDictionary;

        void Start()
        {
            InitializeDictionary();
        }

        static void InitializeDictionary()
        {
            _morseAlphabetDictionary = new Dictionary<char, string>()
                                   {
                                       {'a', ".-"},
                                       {'b', "-..."},
                                       {'c', "-.-."},
                                       {'d', "-.."},
                                       {'e', "."},
                                       {'f', "..-."},
                                       {'g', "--."},
                                       {'h', "...."},
                                       {'i', ".."},
                                       {'j', ".---"},
                                       {'k', "-.-"},
                                       {'l', ".-.."},
                                       {'m', "--"},
                                       {'n', "-."},
                                       {'o', "---"},
                                       {'p', ".--."},
                                       {'q', "--.-"},
                                       {'r', ".-."},
                                       {'s', "..."},
                                       {'t', "-"},
                                       {'u', "..-"},
                                       {'v', "...-"},
                                       {'w', ".--"},
                                       {'x', "-..-"},
                                       {'y', "-.--"},
                                       {'z', "--.."},
                                       {'0', "-----"},
                                       {'1', ".----"},
                                       {'2', "..---"},
                                       {'3', "...--"},
                                       {'4', "....-"},
                                       {'5', "....."},
                                       {'6', "-...."},
                                       {'7', "--..."},
                                       {'8', "---.."},
                                       {'9', "----."},


                                        {',', "__..__"},
                                        {'?', "..__.."},
                                        {':', "	___..."},
                                        {'-', "_...._"},
                                        {'"', "._.._."},
                                        {'(', "_.__."},
                                        {'=', "_..._"},
                                        {'X', "_.._"},
                                        {'.', "._._._"},
                                        {';', "_._._."},
                                        {'/', "_.._."},

                                        {'\'', ".____."},
                                        {'_' ,"..__._"},
                                        {')' ,"	_.__._"},
                                        {'+' ,"._._."},
                                        {'@' ,"	.__._."}

                                   };
        }


        internal static string Translate(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (input == null || input == "")
                return "";
            Debug.Log("Translate, input: " + input);
            foreach (char c in input)
            {
                Debug.Log("Translate, c: " + c);
                var character = char.ToLower(c);
                if (_morseAlphabetDictionary.ContainsKey(character))
                {
                    stringBuilder.Append(_morseAlphabetDictionary[character] + " ");
                }
                else if (character == ' ')
                {
                    stringBuilder.Append("/ ");
                }
                else
                {
                    stringBuilder.Append(character + " ");
                }
            }

            return stringBuilder.ToString();
        }
    }
}