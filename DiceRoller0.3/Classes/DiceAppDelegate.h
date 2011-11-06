#import <UIKit/UIKit.h>

@class DiceView;

@interface DiceAppDelegate : NSObject <UIApplicationDelegate, UIAccelerometerDelegate> {
    UIWindow *window;
    DiceView *glView;
	UIAccelerationValue accel[3];
}

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet DiceView *glView;

@end

