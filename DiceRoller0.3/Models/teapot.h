/*

File: teapot.h
Abstract: Contains data necessary for rendering the teapot model.

Version: 2.1

Disclaimer: IMPORTANT:  This Apple software is supplied to you by Apple Inc.
("Apple") in consideration of your agreement to the following terms, and your
use, installation, modification or redistribution of this Apple software
constitutes acceptance of these terms.  If you do not agree with these terms,
please do not use, install, modify or redistribute this Apple software.

In consideration of your agreement to abide by the following terms, and subject
to these terms, Apple grants you a personal, non-exclusive license, under
Apple's copyrights in this original Apple software (the "Apple Software"), to
use, reproduce, modify and redistribute the Apple Software, with or without
modifications, in source and/or binary forms; provided that if you redistribute
the Apple Software in its entirety and without modifications, you must retain
this notice and the following text and disclaimers in all such redistributions
of the Apple Software.
Neither the name, trademarks, service marks or logos of Apple Inc. may be used
to endorse or promote products derived from the Apple Software without specific
prior written permission from Apple.  Except as expressly stated in this notice,
no other rights or licenses, express or implied, are granted by Apple herein,
including but not limited to any patent rights that may be infringed by your
derivative works or by other works in which the Apple Software may be
incorporated.

The Apple Software is provided by Apple on an "AS IS" basis.  APPLE MAKES NO
WARRANTIES, EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION THE IMPLIED
WARRANTIES OF NON-INFRINGEMENT, MERCHANTABILITY AND FITNESS FOR A PARTICULAR
PURPOSE, REGARDING THE APPLE SOFTWARE OR ITS USE AND OPERATION ALONE OR IN
COMBINATION WITH YOUR PRODUCTS.

IN NO EVENT SHALL APPLE BE LIABLE FOR ANY SPECIAL, INDIRECT, INCIDENTAL OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE
GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
ARISING IN ANY WAY OUT OF THE USE, REPRODUCTION, MODIFICATION AND/OR
DISTRIBUTION OF THE APPLE SOFTWARE, HOWEVER CAUSED AND WHETHER UNDER THEORY OF
CONTRACT, TORT (INCLUDING NEGLIGENCE), STRICT LIABILITY OR OTHERWISE, EVEN IF
APPLE HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

Copyright (C) 2009 Apple Inc. All Rights Reserved.

*/


#ifndef TEAPOT_H
#define TEAPOT_H

unsigned short menu_indicies [] = {
0, 1, 2, 3
};

unsigned short room_indicies [] = {
4, 5, 6, 7,
8, 9, 10, 11,
12, 13, 14, 15,
16, 17, 18, 19
};



float all_texcoords [] = {
0, 0.25,
0.25, 0.25,
0, 0,
0.25, 0,

0, 0.25,
0.25, 0.25,
0, 0,
0.25, 0,

0, 0.25,
0.25, 0.25,
0, 0,
0.25, 0,

0, 0.25,
0.25, 0.25,
0, 0,
0.25, 0

};



float all_vertices [] = {


// menu

-1.5, -2, 2,
-1.5, 2, 2,
1.5, -2, 2,
1.5, 2, 2,




// room
-1.7, -2.6, -2,
-1.7, -2.6, 2,
1.7, -2.6, -2,
1.7, -2.6, 2,

-1.7, -2.6, -2,
-1.7, -2.6, 2,
-1.7, 2.6, -2,
-1.7, 2.6, 2,

-1.7, 2.6, -2,
-1.7, 2.6, 2,
1.7, 2.6, -2,
1.7, 2.6, 2,

1.7, -2.6, -2,
1.7, -2.6, 2,
1.7, 2.6, -2,
1.7, 2.6, 2,


// d4

0, 0.366f, 0,
0.5f, -0.5f, 0,
-0.5f, -0.5f, 0,

0, 0.366f, 0,
0.5f, -0.5f, 0,
0, 0, 1,

0, 0.366f, 0,
-0.5f, -0.5f, 0,
0, 0, 1,

0.5f, -0.5f, 0,
-0.5f, -0.5f, 0,
0, 0, 1,





// d6

// d8

// d10

// d12

// d20

-0.276400, -0.850600, -0.447200,
0.000000, 0.000000, -1.000000,
0.723600, -0.525700, -0.447200,
0.723600, -0.525700, -0.447200,
0.000000, 0.000000, -1.000000,
0.723600, 0.525700, -0.447200,
-0.894400, 0.000000, -0.447200,
0.000000, 0.000000, -1.000000,
-0.276400, -0.850600, -0.447200,
-0.276400, 0.850600, -0.447200,
0.000000, 0.000000, -1.000000,
-0.894400, 0.000000, -0.447200,
0.723600, 0.525700, -0.447200,
0.000000, 0.000000, -1.000000,
-0.276400, 0.850600, -0.447200,
0.723600, -0.525700, -0.447200,
0.723600, 0.525700, -0.447200,
0.894400, 0.000000, 0.447200,
-0.276400, -0.850600, -0.447200,
0.723600, -0.525700, -0.447200,
0.276400, -0.850600, 0.447200,
-0.894400, 0.000000, -0.447200,
-0.276400, -0.850600, -0.447200,
-0.723600, -0.525700, 0.447200,
-0.276400, 0.850600, -0.447200,
-0.894400, 0.000000, -0.447200,
-0.723600, 0.525700, 0.447200,
0.723600, 0.525700, -0.447200,
-0.276400, 0.850600, -0.447200,
0.276400, 0.850600, 0.447200,
0.894400, 0.000000, 0.447200,
0.276400, -0.850600, 0.447200,
0.723600, -0.525700, -0.447200,
0.276400, -0.850600, 0.447200,
-0.723600, -0.525700, 0.447200,
-0.276400, -0.850600, -0.447200,
-0.723600, -0.525700, 0.447200,
-0.723600, 0.525700, 0.447200,
-0.894400, 0.000000, -0.447200,
-0.723600, 0.525700, 0.447200,
0.276400, 0.850600, 0.447200,
-0.276400, 0.850600, -0.447200,
0.276400, 0.850600, 0.447200,
0.894400, 0.000000, 0.447200,
0.723600, 0.525700, -0.447200,
0.276400, -0.850600, 0.447200,
0.894400, 0.000000, 0.447200,
0.000000, 0.000000, 1.000000,
-0.723600, -0.525700, 0.447200,
0.276400, -0.850600, 0.447200,
0.000000, 0.000000, 1.000000,
-0.723600, 0.525700, 0.447200,
-0.723600, -0.525700, 0.447200,
0.000000, 0.000000, 1.000000,
0.276400, 0.850600, 0.447200,
-0.723600, 0.525700, 0.447200,
0.000000, 0.000000, 1.000000,
0.894400, 0.000000, 0.447200,
0.276400, 0.850600, 0.447200,
0.000000, 0.000000, 1.000000
};


float all_normals[] = {

// room
0, 1, 0,
0, 1, 0,
0, 1, 0,
0, 1, 0,

1, 0, 0,
1, 0, 0,
1, 0, 0,
1, 0, 0,

0, -1, 0,
0, -1, 0,
0, -1, 0,
0, -1, 0,

-1, 0, 0,
-1, 0, 0,
-1, 0, 0,
-1, 0, 0,

// d4


0, -1, 0,
0, -1, 0,
0, -1, 0,
0, 1, 0,
0, 1, 0,
0, 1, 0,
0, 1, 0,
0, 1, 0,
0, 1, 0,
0, 1, 0,
0, 1, 0,
0, 1, 0,



// d6

// d8

// d10

// d12

// d20

-0.276376, -0.850642, -0.447188,
0.000000, 0.000000, -1.000000,
0.723594, -0.525712, -0.447188,
0.723594, -0.525712, -0.447188,
0.000000, 0.000000, -1.000000,
0.723594, 0.525712, -0.447188,
-0.894406, 0.000000, -0.447188,
0.000000, 0.000000, -1.000000,
-0.276376, -0.850642, -0.447188,
-0.276376, 0.850642, -0.447188,
0.000000, 0.000000, -1.000000,
-0.894406, 0.000000, -0.447188,
0.723594, 0.525712, -0.447188,
0.000000, 0.000000, -1.000000,
-0.276376, 0.850642, -0.447188,
0.723594, -0.525712, -0.447188,
0.723594, 0.525712, -0.447188,
0.894406, 0.000000, 0.447188,
-0.276376, -0.850642, -0.447188,
0.723594, -0.525712, -0.447188,
0.276376, -0.850642, 0.447188,
-0.894406, 0.000000, -0.447188,
-0.276376, -0.850642, -0.447188,
-0.723594, -0.525712, 0.447188,
-0.276376, 0.850642, -0.447188,
-0.894406, 0.000000, -0.447188,
-0.723594, 0.525712, 0.447188,
0.723594, 0.525712, -0.447188,
-0.276376, 0.850642, -0.447188,
0.276376, 0.850642, 0.447188,
0.894406, 0.000000, 0.447188,
0.276376, -0.850642, 0.447188,
0.723594, -0.525712, -0.447188,
0.276376, -0.850642, 0.447188,
-0.723594, -0.525712, 0.447188,
-0.276376, -0.850642, -0.447188,
-0.723594, -0.525712, 0.447188,
-0.723594, 0.525712, 0.447188,
-0.894406, 0.000000, -0.447188,
-0.723594, 0.525712, 0.447188,
0.276376, 0.850642, 0.447188,
-0.276376, 0.850642, -0.447188,
0.276376, 0.850642, 0.447188,
0.894406, 0.000000, 0.447188,
0.723594, 0.525712, -0.447188,
0.276376, -0.850642, 0.447188,
0.894406, 0.000000, 0.447188,
0.000000, 0.000000, 1.000000,
-0.723594, -0.525712, 0.447188,
0.276376, -0.850642, 0.447188,
0.000000, 0.000000, 1.000000,
-0.723594, 0.525712, 0.447188,
-0.723594, -0.525712, 0.447188,
0.000000, 0.000000, 1.000000,
0.276376, 0.850642, 0.447188,
-0.723594, 0.525712, 0.447188,
0.000000, 0.000000, 1.000000,
0.894406, 0.000000, 0.447188,
0.276376, 0.850642, 0.447188,
0.000000, 0.000000, 1.000000
};


#endif // TEAPOT_H
