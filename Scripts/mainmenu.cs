using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour
{
    private Button settingsButton, achievementButton, playButton;
    private Button closeSettingsButton, closeAchievementButton;

    private VisualElement settingsPanel, achievementPanel, buttonContainer;

    private Label headerLabel;

    private Slider volumeSlider, brightnessSlider;
    private TextField usernameField;
    private DropdownField resolutionDropdown;

    private Label firstPlayLabel, play5Label, score100Label;

    private Resolution[] resolutions;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ================= BUTTONS =================
        settingsButton = root.Q<Button>("Settingsbutton");
        achievementButton = root.Q<Button>("Achievementbutton");
        playButton = root.Q<Button>("PlayButton");

        closeSettingsButton = root.Q<Button>("Closesettingsbutton");
        closeAchievementButton = root.Q<Button>("CloseAchievementButton");

        // ================= PANELS =================
        settingsPanel = root.Q<VisualElement>("Settingspanel");
        achievementPanel = root.Q<VisualElement>("Achievementpanel");
        buttonContainer = root.Q<VisualElement>("buttoncontainer");

        // ================= TITLE =================
        headerLabel = root.Q<Label>("mainmenuheaderlabel");

        // ================= SETTINGS =================
        volumeSlider = root.Q<Slider>("Volumeslider");
        brightnessSlider = root.Q<Slider>("Brightnessslider");
        usernameField = root.Q<TextField>("Usernamefield");
        resolutionDropdown = root.Q<DropdownField>("Resolutiondropdown");

        // ================= ACHIEVEMENTS =================
        firstPlayLabel = root.Q<Label>("FirstPlayLabel");
        play5Label = root.Q<Label>("Play5Label");
        score100Label = root.Q<Label>("Score100Label");

        // ================= PLAY COUNT =================
        int count = PlayerPrefs.GetInt("PLAY_COUNT", 0);
        count++;
        PlayerPrefs.SetInt("PLAY_COUNT", count);

        if (count >= 5)
        {
            AchievementSystem.Unlock("PLAY_5_TIMES");
        }

        // ================= BUTTON LOGIC =================
        settingsButton.clicked += () =>
        {
            buttonContainer.style.display = DisplayStyle.None;
            settingsPanel.style.display = DisplayStyle.Flex;
            headerLabel.text = "Settings";
        };

        closeSettingsButton.clicked += () =>
        {
            settingsPanel.style.display = DisplayStyle.None;
            buttonContainer.style.display = DisplayStyle.Flex;
            headerLabel.text = "Main Menu";
        };

        achievementButton.clicked += () =>
        {
            buttonContainer.style.display = DisplayStyle.None;
            achievementPanel.style.display = DisplayStyle.Flex;
            headerLabel.text = "Achievements";

            UpdateAchievementsUI();
        };

        closeAchievementButton.clicked += () =>
        {
            achievementPanel.style.display = DisplayStyle.None;
            buttonContainer.style.display = DisplayStyle.Flex;
            headerLabel.text = "Main Menu";
        };

        playButton.clicked += () =>
        {
            AchievementSystem.Unlock("FIRST_PLAY");
            SceneManager.LoadScene("GameScene");
        };

        // ================= 🔊 VOLUME =================
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("VOLUME", 1f);
            AudioListener.volume = volumeSlider.value;

            volumeSlider.RegisterValueChangedCallback(evt =>
            {
                AudioListener.volume = evt.newValue;
                PlayerPrefs.SetFloat("VOLUME", evt.newValue);
            });
        }

        // ================= 👤 USERNAME (FIXED) =================
        if (usernameField != null)
        {
            usernameField.value = PlayerPrefs.GetString("USERNAME", "");

            usernameField.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetString("USERNAME", evt.newValue);
                Debug.Log("Username saved: " + evt.newValue);
            });
        }
        else
        {
            Debug.LogError("Username field not found!");
        }

        // ================= 🖥 RESOLUTION (FIXED) =================
        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;

            List<string> options = new List<string>();
            int currentIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string res = resolutions[i].width + "x" + resolutions[i].height;

                if (!options.Contains(res))
                    options.Add(res);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentIndex = i;
                }
            }

            resolutionDropdown.choices = options;
            resolutionDropdown.index = currentIndex;

            resolutionDropdown.RegisterValueChangedCallback(evt =>
            {
                string[] split = evt.newValue.Split('x');

                int width = int.Parse(split[0]);
                int height = int.Parse(split[1]);

                Screen.SetResolution(width, height, Screen.fullScreen);

                Debug.Log("Resolution changed to: " + evt.newValue);
            });
        }
        else
        {
            Debug.LogError("ResolutionDropdown not found!");
        }

        // ================= 🌗 BRIGHTNESS =================
        if (brightnessSlider != null)
        {
            brightnessSlider.value = PlayerPrefs.GetFloat("BRIGHTNESS", 1f);

            brightnessSlider.RegisterValueChangedCallback(evt =>
            {
                RenderSettings.ambientLight = Color.white * evt.newValue;
                PlayerPrefs.SetFloat("BRIGHTNESS", evt.newValue);
            });
        }
    }

    // ================= ACHIEVEMENT UPDATE =================
    void UpdateAchievementsUI()
    {
        if (firstPlayLabel == null || play5Label == null || score100Label == null)
        {
            Debug.LogError("Achievement UI not found!");
            return;
        }

        firstPlayLabel.text = AchievementSystem.IsUnlocked("FIRST_PLAY")
            ? "✔ First Play"
            : "🔒 First Play";

        play5Label.text = AchievementSystem.IsUnlocked("PLAY_5_TIMES")
            ? "✔ Play 5 Times"
            : "🔒 Play 5 Times";

        score100Label.text = AchievementSystem.IsUnlocked("SCORE_100")
            ? "✔ Score 100"
            : "🔒 Score 100";
    }
}