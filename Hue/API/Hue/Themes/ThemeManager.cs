using Hue.API.Hue.Factories;
using Hue.API.Hue.Themes.SystemThemes;
using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;

namespace Hue.API.Hue.Themes
{
    public class ThemeManager
    {
        // Events
        public EventHandler ThemeListChanged;
        public EventHandler ThemeChanged;

        private static volatile ThemeManager instance;
        private static object syncRoot = new Object();

        public static ThemeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ThemeManager();
                    }
                }

                return instance;
            }
        }

        public List<HueTheme> Themes { get; set; }
        public List<HueTheme> DefaultThemeTemplates { get; set; }

        private ThemeManager()
        {
            Themes = new List<HueTheme>();
            
            //Themes.Add(new DeepBlueThemeTemplate());
            
            /*
            Themes.Add(new HueTheme { Name = "Dreaming", IsSystemTheme = true, BannerImage = "/Assets/Themes/Dream.png" });
            Themes.Add(new HueTheme { Name = "Enchanted", IsSystemTheme = true, BannerImage = "/Assets/Themes/Enchanted.png" });
            Themes.Add(new HueTheme { Name = "Pure Essense", IsSystemTheme = true, BannerImage = "/Assets/Themes/Pure.png" });
            Themes.Add(new HueTheme { Name = "Serenity", IsSystemTheme = true, BannerImage = "/Assets/Themes/Serenity.png" });
            Themes.Add(new HueTheme { Name = "Starry Night", IsSystemTheme = true, BannerImage = "/Assets/Themes/Starry.png" });
             */

            DefaultThemeTemplates = new List<HueTheme>();
            DefaultThemeTemplates.Add(new DeepBlueThemeTemplate());
        }

        public async Task LoadThemesFromCacheAsync()
        {
            Themes.Clear();

            StorageFolder themeFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Themes", CreationCollisionOption.OpenIfExists);
            IReadOnlyList<StorageFile> themeFiles = await themeFolder.GetFilesAsync();

            if (themeFiles.Count == 0)
            {
                await CreateDefaultThemesAsync(themeFolder);
            }
            else
            {
                foreach (var file in themeFiles)
                {
                    Debug.WriteLine(file.Name);
                    HueTheme theme = await LoadThemeFromFile(file);
                    if (theme != null)
                    {
                        Themes.Add(theme);
                    }
                }
            }


            return;
        }

        private async Task CreateDefaultThemesAsync(StorageFolder folder)
        {
            try{
                foreach (var theme in DefaultThemeTemplates)
                {
                    var file = await folder.CreateFileAsync(theme.Name, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, theme.ToJsonString());
                    Themes.Add(theme);
                }
            }
            catch(Exception ex){
                Debug.WriteLine(ex);
            }
        }

        private async Task<HueTheme> LoadThemeFromFile(StorageFile file)
        {
            try
            {
                string content = await FileIO.ReadTextAsync(file);
                HueTheme theme = ThemeFactory.CreateThemeFromJson(content);
                theme.FileName = file.Name;

                return theme;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task SaveThemeToFileAsync(HueTheme theme)
        {
            try
            {
                StorageFolder themeFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Themes", CreationCollisionOption.OpenIfExists);
                var name = (theme.FileName !=  null) ? theme.FileName : theme.Name;
                CreationCollisionOption options;
                if (theme.IsSystemTheme)
                {
                    options = CreationCollisionOption.OpenIfExists;
                }
                else
                {
                    options = CreationCollisionOption.GenerateUniqueName;
                }

                var file = await themeFolder.CreateFileAsync(name, options);

                // Update theme filename
                theme.FileName = file.Name;

                string content = theme.ToJsonString();
                await FileIO.WriteTextAsync(file, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task UpdateThemeAsync(HueTheme theme)
        {
            try
            {
                StorageFolder themeFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Themes", CreationCollisionOption.OpenIfExists);
                var name = (theme.FileName != null) ? theme.FileName : theme.Name;
                var file = await themeFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);

                string content = theme.ToJsonString();
                await FileIO.WriteTextAsync(file, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task<bool> ApplyThemeAsync(HueTheme theme)
        {
            if (theme.ColorList.Count == 0)
            {
                return true;
            }

            int colorIndex = 0;
            foreach (var light in BridgeManager.Instance.CurrentBridge.LightList)
            {
                HSBColor color = theme.ColorList[colorIndex];
                var attrs = new { hue = (int)color.H, sat = (int)color.S, bri = (int)color.B };
                await HueAPI.Instance.SetLightStateAsync(light.LightId, attrs);

                // Update light attrs
                light.Hue = (int)color.H;
                light.Saturation = (int)color.S;
                light.Brightness = (int)color.B;
                BridgeManager.Instance.InvalidateLightProperties(light);

                if (colorIndex == theme.ColorList.Count - 1)
                {
                    colorIndex = 0;
                }
            }

            return true;
        }

        public async Task<HueTheme> CreateThemeAsync()
        {
            var newTheme = new HueTheme();
            var newIndex = Themes.Count + 1;
            newTheme.Name = "theme " + newIndex.ToString();
            newTheme.FileName = "theme_" + newIndex.ToString();
            newTheme.IsSystemTheme = false;

            // Add a default color
            newTheme.ColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xec, 0xcd, 0x67)));
            newTheme.ColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xec, 0xcd, 0x67)));
            newTheme.ColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xec, 0xcd, 0x67)));

            // Add to themes
            Themes.Add(newTheme);
            await SaveThemeToFileAsync(newTheme);

            // Notify theme list changes
            if (ThemeListChanged != null)
            {
                ThemeListChanged(this, null);
            }

            return newTheme;
        }

        public void InvalidateTheme(HueTheme theme)
        {
            if (ThemeChanged != null)
            {
                ThemeChanged(theme, null);
            }
        }

    }
}
