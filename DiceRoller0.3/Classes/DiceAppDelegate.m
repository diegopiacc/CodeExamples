#import "DiceAppDelegate.h"
#import "DiceView.h"

// CONSTANTS
#define kAccelerometerFrequency		100.0 // Hz
#define kFilteringFactor			0.1

@implementation DiceAppDelegate

@synthesize window;
@synthesize glView;

- (void) applicationDidFinishLaunching:(UIApplication *)application
{
	
	window = [[UIWindow alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
	[window setUserInteractionEnabled:YES];
	
	[glView startAnimation];
	
	//Configure and start accelerometer
	[[UIAccelerometer sharedAccelerometer] setUpdateInterval:(1.0 / kAccelerometerFrequency)];
	[[UIAccelerometer sharedAccelerometer] setDelegate:self];
	
	//
	[window addSubview:glView];
	[window makeKeyAndVisible];
	
	// [glView showSplash];
    // [glView performSelectorOnMainThread:@selector(mainGameLoop) withObject:nil waitUntilDone:NO]; 
}

- (void) applicationWillResignActive:(UIApplication *)application
{
	[glView stopAnimation];
}

- (void) applicationDidBecomeActive:(UIApplication *)application
{
	[glView startAnimation];
}

- (void)applicationWillTerminate:(UIApplication *)application
{
	[glView stopAnimation];
}

- (void) dealloc
{
	[window release];
	[glView release];
	
	[super dealloc];
}

- (void)accelerometer:(UIAccelerometer*)accelerometer didAccelerate:(UIAcceleration*)acceleration
{
	//Use a basic low-pass filter to only keep the gravity in the accelerometer values
	accel[0] = acceleration.x * kFilteringFactor + accel[0] * (1.0 - kFilteringFactor);
	accel[1] = acceleration.y * kFilteringFactor + accel[1] * (1.0 - kFilteringFactor);
	accel[2] = acceleration.z * kFilteringFactor + accel[2] * (1.0 - kFilteringFactor);
	
	//Update the accelerometer values for the view
	[glView setAccel:accel];
}

@end
