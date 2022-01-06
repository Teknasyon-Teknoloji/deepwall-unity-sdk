#import "UnityAppController.h"
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "DeepWall/DeepWall.h"
#import "DWRUserProperties.h"


extern UIViewController *UnityGetGLViewController();


@interface iOSPlugin : NSObject

@end

@interface iOSPlugin()<DeepWallNotifierDelegate>

@end

@implementation iOSPlugin


+(void)initDeepWall:(NSString*)apiKey environment:(long) environment
{
	
	DeepWallEnvironment env;
	if (environment == 1) {
		env = DeepWallEnvironmentSandbox;
	} else {
		env = DeepWallEnvironmentProduction;
	}
	
	[[DeepWallCore shared] observeEventsFor:[iOSPlugin self]];
	[DeepWallCore initializeWithApiKey:apiKey environment:env];
	}

+(void)setUserProperties:(NSString*)uuid country:(NSString *) country language:(NSString *) language phoneNumber:(NSString *) phoneNumber emailAdress:(NSString *) emailAdress firstName:(NSString *) firstname lastName:(NSString *) lastName envStyle:(NSNumber*) environmentStyle debugAdvertiseAttributions:(NSArray *)debugAdvertiseAttributions
{
	DWRUserProperties *dwProps = [[DWRUserProperties alloc] init];
	if (dwProps){
		dwProps.uuid = uuid;
		dwProps.country = country;
		dwProps.language = language;
		dwProps.environmentStyle = environmentStyle;
		dwProps.phoneNumber = phoneNumber;
		dwProps.emailAddress = emailAdress;
		dwProps.firstName = firstname;
		dwProps.lastName = lastName;
	}else{
		NSLog(@"[UnityDeepWall Failed to set user properties!");
		return;
	}
	
	[[DeepWallCore shared] setUserProperties:[dwProps toDWObject]];
}


+ (void)updateUserProperties:(nullable NSString*)uuid country:(nullable NSString *)country language:(nullable NSString *)language phoneNumber:(nullable NSString *)phoneNumber emailAddress:(nullable NSString *)emailAddress firstName:(nullable NSString *)firstName lastName:(nullable NSString *)lastName envStyle:(long) environmentStyle debugAdvertiseAttributions:(nullable NSArray *)debugAdvertiseAttributions
{
	NSString *dwCountry = nil;
	if (country != nil) {
		dwCountry = [DeepWallCountryManager getCountryByCode:country];
	}

	NSString *dwLanguage = nil;
	if (language != nil) {
		dwLanguage = [DeepWallLanguageManager getLanguageByCode:language];
	}

	DeepWallEnvironmentStyle dwEnvironmentStyle;
	if (environmentStyle != 0) {
		dwEnvironmentStyle = (DeepWallEnvironmentStyle)environmentStyle;
	} else {
		dwEnvironmentStyle = [[DeepWallCore shared] userProperties].environmentStyle;
	}


	[[DeepWallCore shared] updateUserPropertiesCountry:dwCountry language:dwLanguage environmentStyle:dwEnvironmentStyle debugAdvertiseAttributions:nil];

	
	DeepWallUserProperties *dwProps = [[DeepWallCore shared] userProperties];
	
	
	if (phoneNumber != nil){
		dwProps.phoneNumber = phoneNumber;
	}
	if (emailAddress != nil){
		dwProps.emailAddress = emailAddress;
	}
	if (firstName != nil){
		dwProps.firstName = firstName;
	}
	if (lastName != nil){
		dwProps.lastName = lastName;
	}
	 
}


+(void)requestPaywall:(NSString *)action extraData:(NSString *)extraData {
		
	NSDictionary *extraDataDict;
	if (extraData != nil){
		NSError *jsonError;
		NSData *objectData = [extraData dataUsingEncoding:NSUTF8StringEncoding];
		extraDataDict = [NSJSONSerialization JSONObjectWithData:objectData
														options:NSJSONReadingMutableContainers
														  error:&jsonError];
	}
	
	dispatch_async(dispatch_get_main_queue(), ^{
		[[DeepWallCore shared] requestPaywallWithAction:action inView:UnityGetGLViewController() extraData:extraDataDict];
	});
}

+(void)requestAppTracking:(NSString *)action extraData:(NSString *)extraData {
	NSDictionary *extraDataDict;
	if (extraData != nil){
		NSError *jsonError;
		NSData *objectData = [extraData dataUsingEncoding:NSUTF8StringEncoding];
		extraDataDict = [NSJSONSerialization JSONObjectWithData:objectData
														options:NSJSONReadingMutableContainers
														  error:&jsonError];
	}
	
	dispatch_async(dispatch_get_main_queue(), ^{
		if (@available(iOS 14, *)) {
			[[DeepWallCore shared] requestAppTrackingWithAction:action inView:UnityGetGLViewController() extraData:extraDataDict];
		} else {
			NSLog(@"UnityDeepWall Failed to call requestAppTracking. This feature requires ios14+");
		}
	});
}

+(void)sendExtraDataToPaywall:(NSString *)extraData {
	NSDictionary *extraDataDict;
	if (extraData != nil){
		NSError *jsonError;
		NSData *objectData = [extraData dataUsingEncoding:NSUTF8StringEncoding];
		extraDataDict = [NSJSONSerialization JSONObjectWithData:objectData
														options:NSJSONReadingMutableContainers
														  error:&jsonError];
	}
	
	dispatch_async(dispatch_get_main_queue(), ^{
		[[DeepWallCore shared] sendExtraDataToPaywall:extraDataDict];
	});
}

+(void)closePaywall{
	[[DeepWallCore shared] closePaywall];
}

+(const char*)dictionaryToChar:(NSDictionary *)dict
{
	NSData *data = [NSJSONSerialization dataWithJSONObject:dict options:0 error:nil];
	NSString  *sData = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
	const char *cData = [sData UTF8String];
	return cData;
}

+ (void) showAlert:(NSString *)Message {
	
	UIAlertView * alert = [[UIAlertView alloc]initWithTitle:@""
													message:Message
												   delegate:nil
										  cancelButtonTitle:nil
										  otherButtonTitles:nil, nil];
	
	alert.alertViewStyle = UIAlertViewStyleDefault;
	dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(3 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
		
		[alert show];
		
		dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(3.5 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
			[alert dismissWithClickedButtonIndex:0 animated:NO];
		});
		
	});
}


#pragma mark - DeepWallNotifierDelegate

+ (void)deepWallPaywallRequested {
	UnitySendMessage("DeepWall", "DeepWallPaywallRequested", "");
	UnitySendMessage("DeepWall", "OnDeepWallEvent", "");
}

+ (void)deepWallPaywallResponseReceived {
	UnitySendMessage("DeepWall", "DeepWallPaywallResponseReceived", "");
}

+ (void)deepWallPaywallResponseFailure:(DeepWallPaywallResponseFailedModel *)event {
	NSDictionary *data = @{
		@"errorCode": event.errorCode ?: @"",
		@"reason": event.reason ?: @""
	};
	
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallResponseFailure", cData);
}

+ (void)deepWallPaywallOpened:(DeepWallPaywallOpenedInfoModel *)event {
	NSDictionary *data = @{
		@"pageId": @(event.pageId)
	};
	
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallOpened", cData);
}

+ (void)deepWallPaywallNotOpened:(DeepWallPaywallNotOpenedInfoModel *)event {
	NSDictionary *data = @{
		@"pageId": @(event.pageId)
	};
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallNotOpened", cData);
}

+ (void)deepWallPaywallActionShowDisabled:(DeepWallPaywallActionShowDisabledInfoModel *)event {
	NSDictionary *data = @{
		@"pageId": @(event.pageId)
	};
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallActionShowDisabled", cData);
}

+ (void)deepWallPaywallClosed:(DeepWallPaywallClosedInfoModel)event {
	NSDictionary *data = @{
		@"pageId": @(event.pageId)
	};
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallClosed", cData);
}

+ (void)deepWallPaywallExtraDataReceived:(DeepWallExtraDataType)event {
	const char *cData = [iOSPlugin dictionaryToChar:event];
	UnitySendMessage("DeepWall", "DeepWallPaywallExtraDataReceived", cData);
	
}


+ (void)deepWallPaywallPurchasingProduct:(DeepWallPaywallPurchasingProduct *)event {
	NSDictionary *data = @{
		@"productCode": event.productCode
	};
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallPurchasingProduct", cData);
	
}

+ (void)deepWallPaywallPurchaseSuccess:(DeepWallValidateReceiptResult)event {
	NSDictionary *data = @{
		@"type": @((int)event.type),
		@"result": event.result != nil ? [event.result toDictionary] : @{}
	};
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallPurchaseSuccess", cData);
	
}


+ (void)deepWallPaywallPurchaseFailed:(DeepWallPurchaseFailedModel)event {
	NSDictionary *data = @{
		@"productCode": event.productCode ?: @"",
		@"reason": event.reason ?: @"",
		@"errorCode": event.errorCode ?: @"",
		@"isPaymentCancelled": @(event.isPaymentCancelled)
	};
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallPurchaseFailed", cData);
	
}

+ (void)deepWallPaywallRestoreSuccess {
	UnitySendMessage("DeepWall", "DeepWallPaywallRestoreSuccess", "");
}

+ (void)deepWallPaywallRestoreFailed:(DeepWallRestoreFailedModel)event {
	NSDictionary *data = @{
		@"reason": @((int)event.reason),
		@"errorCode": event.errorCode ?: @"",
		@"errorText": event.errorText ?: @"",
		@"isPaymentCancelled": @(event.isPaymentCancelled)
	};
	
	const char *cData = [iOSPlugin dictionaryToChar:data];
	UnitySendMessage("DeepWall", "DeepWallPaywallRestoreFailed", cData);
}

+ (void)deepWallATTStatusChanged {
	UnitySendMessage("DeepWall", "DeepWallATTStatusChanged", "");
}

@end

extern "C"
{
void _InitDeepWall(const char *apiKey, const int envirovement)
{
	[iOSPlugin initDeepWall:[NSString stringWithUTF8String:apiKey] environment:envirovement];
}

void _SetUserProperties(const char *uuid, const char *country, const char *language, const char *phoneNumber, const char *emailAdress, const char *firstName, const char *lastName, const int envStyle)
{
	[iOSPlugin setUserProperties:[NSString stringWithUTF8String:uuid]
					country:[NSString stringWithUTF8String:country]
					language: [NSString stringWithUTF8String:language]
					phoneNumber: [NSString stringWithUTF8String:phoneNumber]
					emailAdress: [NSString stringWithUTF8String:emailAdress]
					firstName: [NSString stringWithUTF8String:firstName]
					lastName: [NSString stringWithUTF8String:lastName]
					envStyle:[NSNumber numberWithInt:envStyle]
					debugAdvertiseAttributions:nil];
}

void _UpdateUserProperties(const char *uuid, const char *country, const char *language,  const char *phoneNumber, const char *emailAdress, const char *firstName, const char *lastName, const int envStyle)
{
	[iOSPlugin updateUserProperties:[NSString stringWithUTF8String:uuid]
					country:[NSString stringWithUTF8String:country]
					language:[NSString stringWithUTF8String:language]
					phoneNumber:[NSString stringWithUTF8String:phoneNumber]
					emailAddress:[NSString stringWithUTF8String:emailAdress]
					firstName:[NSString stringWithUTF8String:firstName]
					lastName:[NSString stringWithUTF8String:lastName]
					envStyle:envStyle debugAdvertiseAttributions:nil];
}

void _RequestPaywall(const char *action, const char *extraData)
{
	if (extraData != nil){
	[iOSPlugin requestPaywall:[NSString stringWithUTF8String:action] extraData:[NSString stringWithUTF8String:extraData]];
	}else {
		[iOSPlugin requestPaywall:[NSString stringWithUTF8String:action] extraData:nil];
	}
}
void _RequestAppTracking(const char *action, const char *extraData)
{
	if (extraData != nil){
	[iOSPlugin requestAppTracking:[NSString stringWithUTF8String:action] extraData:[NSString stringWithUTF8String:extraData]];
	}else {
		[iOSPlugin requestAppTracking:[NSString stringWithUTF8String:action] extraData:nil];
	}
}

void _SendExtraDataToPaywall(const char *extraData)
{
	if (extraData != nil){
	[iOSPlugin sendExtraDataToPaywall: [NSString stringWithUTF8String:extraData]];
	}else {
		[iOSPlugin sendExtraDataToPaywall: nil];
	}
}


void _ClosePaywall()
{
		[iOSPlugin closePaywall];
}

void _ShowAlertMessage(const char *message){
	[iOSPlugin showAlert:[NSString stringWithUTF8String:message]];
}
}
