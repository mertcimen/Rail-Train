using System;
using System.Collections.Generic;
using System.Linq;
using Proyecto26;
using SimpleJSON;
using UnityEngine;

namespace FunGames.Tools.Utils
{
    public static class LocalisationUtils
    {
        static LocalisationUtils()
        {
            GetCountryCodesGDPR();
        }

        private static string[] _countryCodesGdpr;

        public static void GetLocalisationCode(Action<Location> callback)
        {
            RestClient.Get("https://pro.ip-api.com/json/?key=ldlId2QWc7P1nDk")
                .Then(response =>
                    {
                        JSONNode node = JSON.Parse(response.Text);
                        Location location = new Location();
                        location.countryCode = node["countryCode"].Value;
                        location.regionCode = node["region"].Value;
                        callback?.Invoke(location);
                    }
                )
                .Catch(err =>
                {
                    Debug.LogError("An error happened when trying to get country code : " + err);
                    callback?.Invoke(new Location());
                });
        }

        public static bool isGDPRApplied(Location location)
        {
            bool isApplied = false;
            isApplied |= _countryCodesGdpr.Contains(location.countryCode);
            isApplied |= CountryCode.USA.Equals(location.countryCode) &&
                         RegionCode.California.Equals(location.regionCode);
            return isApplied;
        }

        private static string[] GetCountryCodesGDPR()
        {
            if (_countryCodesGdpr != null) return _countryCodesGdpr;
            List<string> countryCodes = new List<string>();
            countryCodes.Add(CountryCode.Austria);
            countryCodes.Add(CountryCode.Belgium);
            countryCodes.Add(CountryCode.Bulgaria);
            countryCodes.Add(CountryCode.Croatia);
            countryCodes.Add(CountryCode.Cyprus);
            countryCodes.Add(CountryCode.CzechRepublic);
            countryCodes.Add(CountryCode.Denmark);
            countryCodes.Add(CountryCode.Estonia);
            countryCodes.Add(CountryCode.Finland);
            countryCodes.Add(CountryCode.France);
            countryCodes.Add(CountryCode.Germany);
            countryCodes.Add(CountryCode.Greece);
            countryCodes.Add(CountryCode.Hungary);
            countryCodes.Add(CountryCode.Ireland);
            countryCodes.Add(CountryCode.Italy);
            countryCodes.Add(CountryCode.Latvia);
            countryCodes.Add(CountryCode.Lithuania);
            countryCodes.Add(CountryCode.Luxembourg);
            countryCodes.Add(CountryCode.Malta);
            countryCodes.Add(CountryCode.Netherlands);
            countryCodes.Add(CountryCode.Poland);
            countryCodes.Add(CountryCode.Portugal);
            countryCodes.Add(CountryCode.Romania);
            countryCodes.Add(CountryCode.Slovakia);
            countryCodes.Add(CountryCode.Slovenia);
            countryCodes.Add(CountryCode.Spain);
            countryCodes.Add(CountryCode.Sweden);
            countryCodes.Add(CountryCode.UnitedKingdom);
            _countryCodesGdpr = countryCodes.ToArray();
            return countryCodes.ToArray();
        }
    }
}