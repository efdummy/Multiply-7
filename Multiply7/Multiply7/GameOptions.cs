using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Multiply7
{
    public class GameOptions
    {
        public ResourceManager rm;
        string currentCultureString;
        public CultureInfo Culture;
        public int MaxOperationCount;
        public Vector2 Speed;
        public float SoundVolume;
        public TimeSpan MinimumTimeBetweenPress;
        
        public GameOptions()
        {
            MaxOperationCount = 10;
            Speed=new Vector2(100, 100);
            SoundVolume = 0.50f;
            MinimumTimeBetweenPress = new TimeSpan(0, 0, 0, 0, 650); // 750 ms

            // set this thread's current culture to the culture associated with the selected locale
            rm = new ResourceManager("Multiply7.AppResources", Assembly.GetExecutingAssembly());
            SetCulture("en-US");
        }

        public void SetCulture(string cultureString)
        {
            currentCultureString = cultureString;
            Culture = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = Culture;
            Thread.CurrentThread.CurrentUICulture = Culture;
        }

        public void SwitchCulture()
        {
            switch (currentCultureString)
            {
                case "en-US":
                    SetCulture("fr-FR");
                    break;
                case "fr-FR":
                    SetCulture("en-US");
                    break;
                default:
                    break;
            }

        }
    }
}
