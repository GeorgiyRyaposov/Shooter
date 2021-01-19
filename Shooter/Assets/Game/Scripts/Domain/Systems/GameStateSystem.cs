using Assets.Game.Scripts.Domain.Components;
using Assets.Game.Scripts.Domain.Contexts;
using Assets.Game.Scripts.Domain.Signals;
using Assets.Game.Scripts.Domain.Views;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.Domain.Systems
{
    public class GameStateSystem : IInitializable, IDisposable
    {
        private Protagonist _protagonist;
        private SettingsSystem _settingsSystem;
        private WeaponSystem _weaponSystem;
        private GameInputSystem _gameInputSystem;
        private SignalBus _signalBus;

        private StartScreenView _startScreenView;
        private SettingsView _settingsView;

        private bool _gameStarted;

        public GameStateSystem(Protagonist protagonist, WeaponSystem weaponSystem, GameInputSystem gameInputSystem,
            SignalBus signalBus, SettingsSystem settingsSystem, StartScreenView startScreenView, SettingsView settingsView)
        {
            _protagonist = protagonist;
            _weaponSystem = weaponSystem;
            _settingsSystem = settingsSystem;
            _gameInputSystem = gameInputSystem;
            _signalBus = signalBus;

            _startScreenView = startScreenView;
            _settingsView = settingsView;
        }

        public void Initialize()
        {
            _gameInputSystem.OpenMenuEvent += OnShowSettingsClick;

            SetupViews();
        }
        public void Dispose()
        {
            _gameInputSystem.OpenMenuEvent -= OnShowSettingsClick;
        }

        private void SetupViews()
        {
            _startScreenView.StartGame.onClick.AddListener(OnStartNewGameClick);
            _startScreenView.Settings.onClick.AddListener(OnShowSettingsClick);
            _startScreenView.Quit.onClick.AddListener(Application.Quit);

            _settingsView.ApplyButton.onClick.AddListener(OnApplySettingsClick);
            _settingsView.RestoreDefaultsButton.onClick.AddListener(_settingsSystem.RestoreDefaultSettings);
            _settingsView.QuitButton.onClick.AddListener(Application.Quit);

            _startScreenView.Show();
            _settingsView.Hide();
        }

        private void OnStartNewGameClick()
        {
            _startScreenView.Hide();
            StartNewGame();
        }
        private void OnShowSettingsClick()
        {
            HideCursor(false);

            _startScreenView.Hide();
            _settingsView.Show();

            _settingsView.QuitButton.gameObject.SetActive(_gameStarted);

            _gameInputSystem.EnableUiInput();
        }
        private void OnApplySettingsClick()
        {
            HideSettings();
            _settingsSystem.SaveSettings();
        }
        public void HideSettings()
        {
            _settingsView.Hide();

            if (_gameStarted)
            {
                HideCursor(true);
                _gameInputSystem.EnableGameplayInput();
            }
            else
            {
                _startScreenView.Show();
            }
        }

        public void StartNewGame()
        {
            HideCursor(true);

            Camera.main.enabled = false; //hide camera for start screen menu

            GameContext.Current = new GameContext();
            _weaponSystem.CreateContexts();

            //activate player
            _protagonist.gameObject.SetActive(true);
            _protagonist.Attach(GameContext.Current);

            _weaponSystem.SelectDefaultWeapon();

            _gameInputSystem.EnableGameplayInput();

            _signalBus.Fire<NewGameStarted>();
            _gameStarted = true;
        }

        private void HideCursor(bool hide)
        {
            Cursor.lockState = hide ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !hide;
        }
    }
}
