using System.IO;
using System.Windows.Input;

namespace Steamvoice
{
    public struct SavedData
    {
        public int InputMode { get; set; } = 0;
        public bool Callback { get; set; } = false;
        public Key KeyToTalk { get; set; } = Key.None;
        public Key KeyToMute { get; set; } = Key.None;

        public SavedData(int inputMode, bool callback, Key keyToTalk, Key keyToMute)
        {
            InputMode = inputMode;
            Callback = callback;
            KeyToTalk = keyToTalk;
            KeyToMute = keyToMute;
        }
    }

    public class FileHandler
    {
        public SavedData savedData = new SavedData();

        private string[] lines = { "Input mode: ", "Callback: ", "Key to talk: ", "Key to mute: " };
        private string filePath;

        public FileHandler(string filePath)
        {
            this.filePath = filePath;
        }

        public void WriteSave()
        {
            File.WriteAllText(filePath,
                    lines[0] + savedData.InputMode + '\n' +
                    lines[1] + savedData.Callback + '\n' +
                    lines[2] + savedData.KeyToTalk.ToString() + '\n' +
                    lines[3] + savedData.KeyToMute.ToString());
        }

        public void ReadSave()
        {
            if (File.Exists(filePath))
            {
                string[] data = File.ReadAllLines(filePath);

                foreach (string line in data)
                {
                    if (line.StartsWith(lines[0]))
                    {
                        if (int.TryParse(line.Substring(lines[0].Length), out int inputModeValue)) savedData.InputMode = inputModeValue;
                    }
                    else if (line.StartsWith(lines[1]))
                    {
                        if (bool.TryParse(line.Substring(lines[1].Length), out bool callbackValue)) savedData.Callback = callbackValue;
                    }
                    else if (line.StartsWith(lines[2]))
                    {
                        if (Key.TryParse(line.Substring(lines[2].Length), out Key keyToTalkValue)) savedData.KeyToTalk = keyToTalkValue;
                    }
                    else if (line.StartsWith(lines[3]))
                    {
                        if (Key.TryParse(line.Substring(lines[3].Length), out Key keyToMuteValue)) savedData.KeyToMute = keyToMuteValue;
                    }
                }
            }
            else WriteSave();
        }
    }
}
