using Assets.Game.Scripts.Domain.Contexts;
using Assets.Game.Scripts.Domain.Models;
using Assets.Game.Scripts.Domain.Views;
using System.IO;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Systems
{
    public class SettingsSystem : IInitializable
    {
        public float MouseSensitivity => _settings.MouseSensitivity.Value;
        public float MovementSpeed => _settings.MovementSpeed.Value;
        public float JumpHeight => _settings.JumpHeight.Value;
        public float Gravity => Physics.gravity.y;

        private const string SettingsName = "Settings";

        private SettingsContext _settings = default;
        private Settings _defaultSettings = default;

        private SettingsView _settingsView;

        [Inject]
        public void Construct(SettingsView settingsView)
        {
            _settingsView = settingsView;
        }

        public void Initialize()
        {
            _defaultSettings = LoadFileDataFromAssets<Settings>($"Data/{SettingsName}");

            if (TryLoadPersistentFileData<SettingsContext>(SettingsName, out var savedSettings))
            {
                _settings = savedSettings;
            }
            else
            {
                RestoreDefaultSettings();
            }

            _settingsView.Attach(_settings);
        }

        public void SaveSettings()
        {
            SavePersistentFileData(SettingsName, _settings);
        }
        public void RestoreDefaultSettings()
        {
            _settings = new SettingsContext(_defaultSettings);
            _settingsView.Attach(_settings);
        }

        private T LoadFileDataFromAssets<T>(string name)
        {
            var fileName = $"{name}.json";
            var filePath = Path.Combine(Application.streamingAssetsPath, fileName);

            if (File.Exists(filePath))
            {
                var dataAsJson = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(dataAsJson);
            }
            else
            {
                Debug.LogError($"Missing {fileName}!");
                return default;
            }
        }
        private bool TryLoadPersistentFileData<T>(string name, out T data)
        {
            var fileName = $"{name}.json";
            var filePath = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(filePath))
            {
                var dataAsJson = File.ReadAllText(filePath);
                data = JsonUtility.FromJson<T>(dataAsJson);
                return true;
            }
            else
            {
                data = default;
                return false;
            }
        }
        private void SavePersistentFileData<T>(string name, T data)
        {
            var fileName = $"{name}.json";
            var filePath = Path.Combine(Application.persistentDataPath, fileName);

            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);

            UnityEngine.Debug.Log($"Saved settings to {filePath}");
        }
    }
}
