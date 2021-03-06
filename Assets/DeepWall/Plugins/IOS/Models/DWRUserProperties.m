//
//  DWRUserProperties.m
//  RNDeepWall
//
//  Created by Burak Yalcin on 10.11.2020.
//  Copyright © 2020 Facebook. All rights reserved.
//

#import "DWRUserProperties.h"

@implementation DWRUserProperties

- (DeepWallUserProperties *)toDWObject {
	NSString *dwCountry = [DeepWallCountryManager getCountryByCode:self.country];
	NSString *dwLanguage = [DeepWallLanguageManager getLanguageByCode:self.language];
	
	DeepWallEnvironmentStyle dwEnvironmentStyle;
	if (self.environmentStyle != nil) {
		dwEnvironmentStyle = (DeepWallEnvironmentStyle)self.environmentStyle.integerValue;
	} else {
		dwEnvironmentStyle = DeepWallEnvironmentStyleAutomatic;
	}
	
	DeepWallUserProperties *dwProps = [[DeepWallUserProperties alloc] initWithUuid:self.uuid country:dwCountry language:dwLanguage environmentStyle:dwEnvironmentStyle debugAdvertiseAttributions:self.debugAdvertiseAttributions];
	
	dwProps.phoneNumber = self.phoneNumber;
	dwProps.emailAddress = self.emailAddress;
	dwProps.firstName = self.firstName;
	dwProps.lastName = self.lastName;
	return dwProps;
}

@end
