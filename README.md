# DeepWall (deepwall-unity-sdk)

- This plugin gives wrapper methods for deepwall sdks. [iOS](https://github.com/Teknasyon-Teknoloji/deepwall-ios-sdk) - [Android](https://github.com/Teknasyon-Teknoloji/deepwall-android-sdk)
- This respository also contains a sample project.
- Before implementing this plugin, you need to have **api_key** and list of actions.
- You can get api_key and actions from [DeepWall Dashboard](https://console.deepwall.com/)

## Getting Started

### Notes

- Make sure you have **Unity 2017.x or higher**.
- If you are using Unity Jar Resolver please see [FinalNotes](#final-notes).
- The name of the GameObject that contains the Deepwall script should be "DeepWall" (Which is set by default).

### IOS
  - Set Target minimum iOS Version to 10.x+ in `PlayerSettings`
  - Set minimum ios version to 10.0 in `ios/Podfile` like: `platform :ios, '10.0'`
  - Add `use_frameworks!` into `ios/Podfile` if not exists.
  - Add `pod 'DeepWall'` into `ios/Podfile`.
  - Remove flipper from `ios/Podfile` if exists.
  - Run `$ cd ios && pod install`
	
### ANDROID
- Set minSdkVersion to 21 in `PlayerSettings`

## Installation
1. Download & Import `deepwall-unity_x_x.unitypackage` to your project. If you do not want to use the prepared sample scene, you can uncheck the `Sample` folder when importing the plugin.

That's all! You're ready to use the DeepWall.

## Usage

### 1. Initialize DeepWall

Before doing anything, you should call the InitDeepWall method.

Pass your Android & IOS api keys, environment type  and opsionally eventlistener object like below. Once you call this method, it will automatically instantiate a DeepWall `GameObject` on your scene.
```cs 
using DeepwallModule;

DeepWall.InitDeepWall("ADD_YOUR_ANDROID_API_KEY_HERE","ADD_YOUR_IOS_API_KEY_HERE", DeepWallEnvironment.Sandbox, eventListener: this);
```
**Important:** Once you successfully initialize the DeepWall, you don't need to instantiate it over and over again on your scene changes. All the process of DeepWall lifecycle is handling automatically. So SplashScene or MainMenuScene would be best place to initialize it.

  However if you pass optional `dontDestroyOnLoad` paramater `false`, like this;
```cs 
DeepWall.InitDeepWall("ADD_YOUR_ANDROID_API_KEY_HERE","ADD_YOUR_IOS_API_KEY_HERE", DeepWallEnvironment.Sandbox, eventListener: this, dontDestroyOnLoad: false);
```
  Then you have to be responsible to handling DeepWall lifecycle states.
  
**Warning:** Do not forget to change the `environment type` to `production` before releasing the app.

> Optionally: After writing the initialize DeepWall method, press play and see the DeepWall gameobject instantiated properly.


### 2. Set User Properties

For requesting any paywall you need to set UserProperties
```cs 
myUserProperty = new DeepWallUserProperty("myUUID", "us", "en-US");
//or
myUserProperty = new DeepWallUserProperty("myUUID", "us", "en-US", DeepWallEnvironmentStyle.DARK);

DeepWall.SetUserProperties(myUserProperty);
```

Sure you can change & update properties anytime you want like this
```cs 
myUserProperty.Country = "tr";
myUserProperty.Language = "tr-Tr";
myUserProperty.EnvironmentStyle = DeepWallEnvironmentStyle.DARK;

DeepWall.UpdateUserProperties(myUserProperty);
``` 

### 3. Request a paywall

For requesting a paywall simply call RequestPaywall. It will display immediately whenever it loads.
```cs 
string actionKey = "AppLaunch"; //Your ActionKey is here 
DeepWall.RequestPaywall(actionKey, null);

/* You can send extra parameter if needed as below: */
DeepWall.RequestPaywall(actionKey, "{\"sliderIndex\": 2, \"title\": \"Deepwall\"}"); 

``` 

### 4. Event Listening

You can listen your paywall native event callbacks. Just implement the `IDeepWallEventListener` interface to your class. Here is an example of a callback method.
```cs 
public void DeepWallPaywallRestoreFailed(int productCode, string reason, string errorCode, bool isPaymentCancelled)
  {
    Debug.Log($"DeepWallPaywallRestoreFailed with productCode:{productCode}, reason: {reason}, errorCode:{errorCode}, isPaymentCancelled: {isPaymentCancelled}");
  }
```

## Testing

You can always test DeepWall with opening & loading `DeepWallSampleScene` in the `Sample` folder. This scene includes all of your need for testing DeepWall Unity Plugin.
Don't forget to change `apikey` values in `DeepWallTester` script.

To understand what is going on, it will be enough to touch the buttons in the order you see above on your mobile devices.

## Final Notes
- Do not forget to change the environment type to `production` before releasing the app.
- You can check your DeepWall version, access the updated documentation file and the DeepWall dashboard through the editor window. -> `Window/DeepWall`
- If you are using Unity Jar Resolver or something similar you can remove all the files under `DeepWall/Plugins/Android/` directory except the `deepwall-unity_x_x.aar`.
Then add the following lines to implement dependencies (versions may differ).
```xml
        <repositories>
            <repository>https://repo.maven.apache.org/maven2</repository>
            <repository>https://raw.githubusercontent.com/Teknasyon-Teknoloji/deepwall-android-sdk/master/</repository>
        </repositories>
        
        <androidPackage spec="org.jetbrains.kotlin:kotlin-stdlib:1.4.20"/>
        <androidPackage spec="io.reactivex.rxjava2:rxjava:2.1.8"/>
        <androidPackage spec="android.arch.lifecycle:extensions:1.1.0"/>
        <androidPackage spec="androidx.lifecycle:lifecycle-viewmodel-ktx:2.2.0"/>
        <androidPackage spec="androidx.lifecycle:lifecycle-livedata-ktx:2.2.0"/>
        <androidPackage spec="deepwall:deepwall-core:2.2.1"/>
```

