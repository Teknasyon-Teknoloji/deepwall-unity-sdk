//
//  DWRUserProperties.h
//  RNDeepWall
//
//  Created by Burak Yalcin on 10.11.2020.
//  Copyright © 2020 Facebook. All rights reserved.
//

#import <JSONModel/JSONModel.h>
#import <DeepWall/DeepWall.h>

NS_ASSUME_NONNULL_BEGIN

@interface DWRUserProperties : JSONModel

@property (nonatomic) NSString *uuid;
@property (nonatomic) NSString *country;
@property (nonatomic) NSString *language;
@property (nonatomic) NSNumber<Optional> *environmentStyle;
@property (nonatomic) NSArray<NSString *><Optional> *debugAdvertiseAttributions;
@property (nonatomic, nullable) NSString *phoneNumber;
@property (nonatomic, nullable) NSString *firstName;
@property (nonatomic, nullable) NSString *lastName;
@property (nonatomic, nullable) NSString *emailAddress;
- (DeepWallUserProperties *)toDWObject;

@end

NS_ASSUME_NONNULL_END
