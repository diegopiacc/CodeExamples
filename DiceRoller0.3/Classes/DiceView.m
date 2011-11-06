#import <QuartzCore/QuartzCore.h>
#import <OpenGLES/EAGLDrawable.h>

#import "DiceView.h"
#import "teapot.h"

// MACROS
#define DEGREES_TO_RADIANS(__ANGLE__) ((__ANGLE__) / 180.0 * M_PI)

// A class extension to declare private methods
@interface DiceView (private)

- (BOOL)createFramebuffer;
- (void)destroyFramebuffer;
- (void)setupView;

@end

@implementation DiceView

@synthesize animating;
@dynamic animationFrameInterval;
@synthesize accel;

// Implement this to override the default layer class (which is [CALayer class]).
// We do this so that our view will be backed by a layer that is capable of OpenGL ES rendering.
+ (Class) layerClass
{
	return [CAEAGLLayer class];
}

// The GL view is stored in the nib file. When it's unarchived it's sent -initWithCoder:
- (id)initWithCoder:(NSCoder*)coder {
    
    if ((self = [super initWithCoder:coder])) {
		CAEAGLLayer *eaglLayer = (CAEAGLLayer *)self.layer;
	
		eaglLayer.opaque = YES;
		eaglLayer.drawableProperties = [NSDictionary dictionaryWithObjectsAndKeys:
									[NSNumber numberWithBool:NO], kEAGLDrawablePropertyRetainedBacking, kEAGLColorFormatRGBA8, kEAGLDrawablePropertyColorFormat, nil];
	
		context = [[EAGLContext alloc] initWithAPI:kEAGLRenderingAPIOpenGLES1];
	
		if (!context || ![EAGLContext setCurrentContext:context]) {
			[self release];
			return nil;
		}
	
		showMenu = TRUE;
		animating = FALSE;
		displayLinkSupported = FALSE;
		animationFrameInterval = 1;
		displayLink = nil;
		animationTimer = nil;
	
		// A system version of 3.1 or greater is required to use CADisplayLink. The NSTimer
		// class is used as fallback when it isn't available.
		NSString *reqSysVer = @"3.1";
		NSString *currSysVer = [[UIDevice currentDevice] systemVersion];
		if ([currSysVer compare:reqSysVer options:NSNumericSearch] != NSOrderedAscending)
			displayLinkSupported = TRUE;
		
		//
		diceManager = [[ DiceManager alloc ] init ];
		//collider = [[NewCollider alloc ] init ];
		
		[diceManager addDx: 4];
		[diceManager addDx: 20];
		
		[[diceManager getDie: 0] setPosition: 1: 1: 0];
		
		//
		accel = calloc(3, sizeof(UIAccelerationValue));
		
		[self setupView];
	}
	
	return self;
}
	
-(void)setupView
{
	
	// NSLog(@"setupView\n");
	
	// passa da qui una volta sola
	
	const GLfloat			lightAmbient[] = {0.5, 0.5, 0.5, 1.0};
	const GLfloat			lightDiffuse[] = {1.0, 0.6, 0.0, 1.0};
	const GLfloat			matAmbient[] = {0.6, 0.6, 0.6, 1.0};
	const GLfloat			matDiffuse[] = {1.0, 1.0, 1.0, 1.0};	
	const GLfloat			matSpecular[] = {1.0, 1.0, 1.0, 1.0};
	const GLfloat			lightPosition[] = {1.0, 2.0, 1.0, 0.0}; 
	const GLfloat			lightShininess = 100.0;
	 
	 
	const GLfloat			zNear = 0.1,
							zFar = 1000.0,
							fieldOfView = 60.0;
	GLfloat					size;
	
	// Configure Textures
	glEnable(GL_TEXTURE_2D);
    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_COLOR, GL_DST_COLOR);
	
	glGenTextures(1, &texture[0]);
	
	glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR); 
    glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
	
	[self createTexture];
	
	
	//Configure OpenGL lighting
	glEnable(GL_LIGHTING);
	glEnable(GL_LIGHT0);
	glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, matAmbient);
	glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, matDiffuse);
	glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, matSpecular);
	glMaterialf(GL_FRONT_AND_BACK, GL_SHININESS, lightShininess);
	glLightfv(GL_LIGHT0, GL_AMBIENT, lightAmbient);
	glLightfv(GL_LIGHT0, GL_DIFFUSE, lightDiffuse);
	glLightfv(GL_LIGHT0, GL_POSITION, lightPosition); 			
	glShadeModel(GL_FLAT);
	glEnable(GL_DEPTH_TEST);
	
	
	//Configure OpenGL arrays
	glEnableClientState(GL_VERTEX_ARRAY);
	glEnableClientState(GL_NORMAL_ARRAY);
	glEnableClientState(GL_TEXTURE_COORD_ARRAY);
	glVertexPointer(3 ,GL_FLOAT, 0, all_vertices);
	glNormalPointer(GL_FLOAT, 0, all_normals);
	glTexCoordPointer(2, GL_FLOAT, 0, all_texcoords);
	glEnable(GL_NORMALIZE);
	
	 
	//Set the OpenGL projection matrix
	glMatrixMode(GL_PROJECTION);
	size = zNear * tanf(DEGREES_TO_RADIANS(fieldOfView) / 2.0);
	CGRect rect = self.bounds;
	
	NSLog(@"rect.size.width = %d", rect.size.width);
	
	glFrustumf(-size, size, -size / (rect.size.width / rect.size.height), size / (rect.size.width / rect.size.height), zNear, zFar);
	glViewport(0, 0, rect.size.width, rect.size.height);
	
	//Make the OpenGL modelview matrix the default
	glMatrixMode(GL_MODELVIEW);
}

// Updates the OpenGL view
- (void)drawView
{
	
	// Make sure that you are drawing to the current context
	[EAGLContext setCurrentContext:context];
		
	glBindFramebufferOES(GL_FRAMEBUFFER_OES, viewFramebuffer);
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		
	//GLfloat matrix[4][4];
	// GLfloat length;
		
	//Make sure we have a big enough acceleration vector
	// length = sqrtf(accel[0] * accel[0] + accel[1] * accel[1] + accel[2] * accel[2]);
		
	//Setup model view matrix
	glLoadIdentity();
	glTranslatef(0.0, 0.0, -5.0);
		
#if TARGET_IPHONE_SIMULATOR
	accel[0] = 0.0f;//-0.001f;
	accel[1] = -9.8f;
	accel[2] = 0.0f;
#endif
	
	
	int diceCount = [diceManager getDiceCount];
	
	// printf("Accel[1]=%f", accel[1]);
	
	// Update dice (better not here, should separate from rendering)
	[diceManager setAccelerationValue: accel[0]: accel[1]: accel[2]];
	[diceManager step];

	//[collider setAcceleration: accel[0]: accel[1]: accel[2]];
	//[collider step];
	
	// Draw Room	
	glDrawElements(GL_TRIANGLE_STRIP, 4, GL_UNSIGNED_SHORT, &room_indicies[0]);
	glDrawElements(GL_TRIANGLE_STRIP, 4, GL_UNSIGNED_SHORT, &room_indicies[4]);
	glDrawElements(GL_TRIANGLE_STRIP, 4, GL_UNSIGNED_SHORT, &room_indicies[8]);
	glDrawElements(GL_TRIANGLE_STRIP, 4, GL_UNSIGNED_SHORT, &room_indicies[12]);
	
	
	// Draw Dice
	for(int di=0; di<diceCount; di++)
	{
		Die *d = [diceManager getDie:di];
		[d render];
		
	}
	
	// Draw Menu
	//if(showMenu)
	//{
	//	glDrawElements(GL_TRIANGLE_STRIP, 4, GL_UNSIGNED_SHORT, &menu_indicies[0]);
	//}
	
	glBindRenderbufferOES(GL_RENDERBUFFER_OES, viewRenderbuffer);
	[context presentRenderbuffer:GL_RENDERBUFFER_OES];
}

-(void)createTexture
{
	NSString *path = [[NSBundle mainBundle] pathForResource:@"wood" ofType:@"png"];
    NSData *texData = [[NSData alloc] initWithContentsOfFile:path];
    UIImage *image = [[UIImage alloc] initWithData:texData];
    if (image == nil)
        NSLog(@"Do real error checking here");
	
    GLuint width = CGImageGetWidth(image.CGImage);
    GLuint height = CGImageGetHeight(image.CGImage);
    CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
    void *imageData = malloc( height * width * 4 );
    CGContextRef textureContext = CGBitmapContextCreate( imageData, width, height, 8, 4 * width, colorSpace, kCGImageAlphaPremultipliedLast | kCGBitmapByteOrder32Big );
    CGColorSpaceRelease( colorSpace );
    CGContextClearRect( textureContext, CGRectMake( 0, 0, width, height ) );
    // CGContextTranslateCTM( textureContext, 0, height - height );
    CGContextDrawImage( textureContext, CGRectMake( 0, 0, width, height ), image.CGImage );
	
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, imageData);
	
    CGContextRelease(textureContext);
	
    free(imageData);
    [image release];
    [texData release];
	
}





// If our view is resized, we'll be asked to layout subviews.
// This is the perfect opportunity to also update the framebuffer so that it is
// the same size as our display area.
-(void)layoutSubviews
{
	[EAGLContext setCurrentContext:context];
	[self destroyFramebuffer];
	[self createFramebuffer];
	[self drawView];
}

- (BOOL)createFramebuffer
{
	// Generate IDs for a framebuffer object and a color renderbuffer
	glGenFramebuffersOES(1, &viewFramebuffer);
	glGenRenderbuffersOES(1, &viewRenderbuffer);
	
	glBindFramebufferOES(GL_FRAMEBUFFER_OES, viewFramebuffer);
	glBindRenderbufferOES(GL_RENDERBUFFER_OES, viewRenderbuffer);
	// This call associates the storage for the current render buffer with the EAGLDrawable (our CAEAGLLayer)
	// allowing us to draw into a buffer that will later be rendered to screen wherever the layer is (which corresponds with our view).
	[context renderbufferStorage:GL_RENDERBUFFER_OES fromDrawable:(id<EAGLDrawable>)self.layer];
	glFramebufferRenderbufferOES(GL_FRAMEBUFFER_OES, GL_COLOR_ATTACHMENT0_OES, GL_RENDERBUFFER_OES, viewRenderbuffer);
	
	glGetRenderbufferParameterivOES(GL_RENDERBUFFER_OES, GL_RENDERBUFFER_WIDTH_OES, &backingWidth);
	glGetRenderbufferParameterivOES(GL_RENDERBUFFER_OES, GL_RENDERBUFFER_HEIGHT_OES, &backingHeight);
	
	// For this sample, we also need a depth buffer, so we'll create and attach one via another renderbuffer.
	glGenRenderbuffersOES(1, &depthRenderbuffer);
	glBindRenderbufferOES(GL_RENDERBUFFER_OES, depthRenderbuffer);
	glRenderbufferStorageOES(GL_RENDERBUFFER_OES, GL_DEPTH_COMPONENT16_OES, backingWidth, backingHeight);
	glFramebufferRenderbufferOES(GL_FRAMEBUFFER_OES, GL_DEPTH_ATTACHMENT_OES, GL_RENDERBUFFER_OES, depthRenderbuffer);

	if(glCheckFramebufferStatusOES(GL_FRAMEBUFFER_OES) != GL_FRAMEBUFFER_COMPLETE_OES)
	{
		NSLog(@"failed to make complete framebuffer object %x", glCheckFramebufferStatusOES(GL_FRAMEBUFFER_OES));
		return NO;
	}
	
	return YES;
}

// Clean up any buffers we have allocated.
- (void)destroyFramebuffer
{
	glDeleteFramebuffersOES(1, &viewFramebuffer);
	viewFramebuffer = 0;
	glDeleteRenderbuffersOES(1, &viewRenderbuffer);
	viewRenderbuffer = 0;
	
	if(depthRenderbuffer)
	{
		glDeleteRenderbuffersOES(1, &depthRenderbuffer);
		depthRenderbuffer = 0;
	}
}

- (NSInteger) animationFrameInterval
{
	return animationFrameInterval;
}

- (void) setAnimationFrameInterval:(NSInteger)frameInterval
{
	// Frame interval defines how many display frames must pass between each time the
	// display link fires. The display link will only fire 30 times a second when the
	// frame internal is two on a display that refreshes 60 times a second. The default
	// frame interval setting of one will fire 60 times a second when the display refreshes
	// at 60 times a second. A frame interval setting of less than one results in undefined
	// behavior.
	if (frameInterval >= 1)
	{
		animationFrameInterval = frameInterval;
		
		if (animating)
		{
			[self stopAnimation];
			[self startAnimation];
		}
	}
}

- (void) startAnimation
{
	if (!animating)
	{
		if (displayLinkSupported)
		{
			// CADisplayLink is API new to iPhone SDK 3.1. Compiling against earlier versions will result in a warning, but can be dismissed
			// if the system version runtime check for CADisplayLink exists in -initWithCoder:. The runtime check ensures this code will
			// not be called in system versions earlier than 3.1.
			
			displayLink = [NSClassFromString(@"CADisplayLink") displayLinkWithTarget:self selector:@selector(drawView)];
			[displayLink setFrameInterval:animationFrameInterval];
			[displayLink addToRunLoop:[NSRunLoop currentRunLoop] forMode:NSDefaultRunLoopMode];
		}
		else
			animationTimer = [NSTimer scheduledTimerWithTimeInterval:(NSTimeInterval)((1.0 / 60.0) * animationFrameInterval) target:self selector:@selector(drawView) userInfo:nil repeats:TRUE];
		
		animating = TRUE;
	}
}

- (void)stopAnimation
{
	if (animating)
	{
		if (displayLinkSupported)
		{
			[displayLink invalidate];
			displayLink = nil;
		}
		else
		{
			[animationTimer invalidate];
			animationTimer = nil;
		}
		
		animating = FALSE;
	}
}

- (void)touchesBegan:(NSSet*)touches withEvent:(UIEvent*)event view:(UIView*)aView {
	NSLog(@"touches began");
}

- (void)touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event view:(UIView*)aView {
	NSLog(@"touches moved");
	
}

- (void)touchesEnded:(NSSet*)touches withEvent:(UIEvent*)event view:(UIView*)aView {
	NSLog(@"touches ended");
}


- (void)dealloc
{
	free(accel);
	
	if([EAGLContext currentContext] == context)
	{
		[EAGLContext setCurrentContext:nil];
	}
	
	[context release];
	[super dealloc];
}



@end
