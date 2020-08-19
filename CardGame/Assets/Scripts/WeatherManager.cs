using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeatherManager : MonoBehaviour
{
    public enum Weather { Clear, Sunny, Rainy, Snowy, Sandstorm, Stormy, Windy, Cloudy, MeteorShower }
    public Weather weather;
    public Image weatherDisplay;
    public TextMeshProUGUI weatherDisplayName;
    public List<Sprite> weatherSprites;
    public List<GamePiece> weatherPokemon = new List<GamePiece>();
    public Dictionary<string, Weather> weatherByAbility = new Dictionary<string, Weather>()
    {
        { "Air Lock", Weather.Clear },
        { "Drought", Weather.Sunny },
        { "Flood", Weather.Rainy },
        { "Blizzard", Weather.Snowy },
        { "Sand Stream", Weather.Sandstorm },
        { "Stormy", Weather.Stormy },
        { "Whirlwind", Weather.Windy },
        { "Overcast", Weather.Cloudy },
        { "Meteor Shower", Weather.MeteorShower }
    };

    public Dictionary<Weather, int> spriteIndexByWeather = new Dictionary<Weather, int>()
    {
        { Weather.Clear, 0 },
        { Weather.Sunny, 1 },
        { Weather.Rainy, 2 },
        { Weather.Snowy, 3 },
        { Weather.Sandstorm, 4 },
        { Weather.Stormy, 5 },
        { Weather.Windy, 6 },
        { Weather.Cloudy, 7 },
        { Weather.MeteorShower, 8 }
    };

    void Update()
    {
        // Check which Pokemon is affecting the weather
        for (int i = 0; i < weatherPokemon.Count; i++)
        {
            if (weatherPokemon[weatherPokemon.Count - 1] == null || weatherPokemon[weatherPokemon.Count - 1].disabled)
            {
                weatherPokemon.RemoveAt(weatherPokemon.Count - 1);
            }
            else
            {
                break;
            }
        }

        if (weatherPokemon.Count > 0)
        {
            string weatherPokemonAbility = weatherPokemon[weatherPokemon.Count - 1].ability;
            weather = weatherByAbility[weatherPokemonAbility];
        }
        else
        {
            weather = Weather.Clear;
        }

        weatherDisplay.sprite = weatherSprites[spriteIndexByWeather[weather]];
        weatherDisplayName.text = weather.ToString();
    }
}
