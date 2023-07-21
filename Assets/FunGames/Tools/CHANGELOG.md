# Version 3.2.12 (11/04/2023)

## Updated

* Update FGDebugConsole (add space for banner at bottom of screen)
* FG IAP : Add priceString attribute to FGProductInfo

## Fixed

* Fix GDPR initialization with "GdprDisplay" remote config

# Version 3.2.11 (04/04/2023)

## Added

* FG IAP : Add the FGIAP.GetProductInfo() function to get the ProductInfo of a specific Product Id.

## Updated

* Set Default value for "AudioAds" RemoteConfig to 0 (= no ads).

# Version 3.2.10 (30/03/2023)

## Added

* Add FGMainSettings to handle API Key and Game Name displayed in custom UIs.
* Integrate all mandatory softlaunch packages in the main FunGamesSDK unitypackage.

## Fixed

* Fix DebugConsole : display logs that are not processed in the main thread.

# Version 3.2.9 (28/03/2023)

## Added

* Add Restore function to be accessible in IAP module.
* Send Analytics Events when Audio Ads are Requested, Started or Completed.
* Send Analytics Events when In-Game Ads Impressions are validated and when the ad is Clicked.
* Automatically call the GetNotifUserOpenAppWith() function once FGNotification is initialized

# Version 3.2.8 (20/03/2023)

## Added

* Add DateUtils functions to read/parse DateTime format
* Add CurrentABTest object to FGRemoteConfig module

* ## Fixed

* Add safety to DateTime format checking, and handle Exception in case of error.

# Version 3.2.7 (15/03/2023)

## Added

* Add optional ATT PrePopup module to the User Consent flow.

## Fixed

* Module versions properly saved on Asset Database.

# Version 3.2.6 (06/03/2023)

## Updated

* Localisation link changed to "Ip-Api Pro" (https + unlimited requests).

# Version 3.2.5 (03/03/2023)

## Fixed

* Module versions saved on Asset Database.

# Version 3.2.4 (21/02/2023)

## Added

* Add RemoteConfigs to handle module initialization
* Add RemoteConfig value to activate Debug Console
* Add possibility to remove Ads in game, directly through FunGameSDK interface (this will deactivate AudioAds, InGame
  Ads and Crosspromo modules, and prevent Mediation Ads to be displayed)

# Version 3.2.3 (08/02/2023)

## Updated

* FGIAP : Integrate Receipt Validation event

# Version 3.2.2 (03/02/2023)

## Updated

* FGMediation : Update module to allow multiple Ad Units
* Update GDPR flow

# Version 3.2.1 (02/02/2023)

## Updated

* Update architecture
* Update LoadingScreen
* Update DebugConsole

# Version 3.2 (13/01/2023)

## Updated

* Update architecture for 3.2

## Added

* FunGames Integration Manager
* FGDebugConsole can be added as an overlay to the MainScene in order to display FG logs directly on device.
* FGLoadingScreen can be added as an overlay to the MainScene in order to display a Loading Screen matching FG SDK
  initialization.