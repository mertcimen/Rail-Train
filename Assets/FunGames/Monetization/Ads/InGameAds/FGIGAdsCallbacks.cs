using System;

namespace FunGames.Monetization.InGameAds
{
    public class FGIGAdsCallbacks : FGModuleCallbacks
    {
        internal Action _adDisplayed;
        internal Action _adLoaded;
        internal Action _adRequestFailed;
        internal Action _adImpressionValidated;
        internal Action _adClicked;
        internal Action _adCompleted;

        public event Action OnAdDisplayed
        {
            add => _adDisplayed += value;
            remove => _adDisplayed -= value;
        }

        public event Action OnAdLoaded
        {
            add => _adLoaded += value;
            remove => _adLoaded -= value;
        }

        public event Action OnAdRequestFailed
        {
            add => _adRequestFailed += value;
            remove => _adRequestFailed -= value;
        }

        public event Action OnAdImpressionValidated
        {
            add => _adImpressionValidated += value;
            remove => _adImpressionValidated -= value;
        }

        public event Action OnAdClicked
        {
            add => _adClicked += value;
            remove => _adClicked -= value;
        }

        public event Action OnAdCompleted
        {
            add => _adCompleted += value;
            remove => _adCompleted -= value;
        }

        public override void Clear()
        {
            base.Clear();
            _adDisplayed = null;
            _adLoaded = null;
            _adRequestFailed = null;
            _adImpressionValidated = null;
            _adClicked = null;
            _adCompleted = null;
        }
    }
}