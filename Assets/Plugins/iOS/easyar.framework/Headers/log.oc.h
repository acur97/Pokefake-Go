﻿//=============================================================================================================================
//
// EasyAR Sense 4.2.0.8700-7bcbc8b1c
// Copyright (c) 2015-2021 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
// EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
// and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//=============================================================================================================================

#import "easyar/types.oc.h"

/// <summary>
/// Log class.
/// It is used to setup a custom log output function.
/// </summary>
@interface easyar_Log : NSObject

+ (instancetype)new NS_UNAVAILABLE;
- (instancetype)init NS_UNAVAILABLE;

/// <summary>
/// Sets custom log output function. The log output function should be thread-safe.
/// </summary>
+ (void)setLogFunc:(void (^)(easyar_LogLevel level, NSString * message))func;
/// <summary>
/// Sets custom log output function and run it in scheduler.
/// </summary>
+ (void)setLogFuncWithScheduler:(easyar_CallbackScheduler *)scheduler func:(void (^)(easyar_LogLevel level, NSString * message))func;
/// <summary>
/// Clears custom log output function and reverts to default log output function.
/// </summary>
+ (void)resetLogFunc;

@end
