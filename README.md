# AiR-Mouse
![Air Mouse Logo](https://github.com/roeticvampire/AiR-Mouse/blob/master/AirMousePhone-compressed.jpg)
## Summary
 An Android App coupled with a Windows PC Client, which uses the phone's Gyro sensors to control the mouse cursor on the PC

## Random Technical Mentions
The App was made using Unity, which I feel is not as efficient of a solution as Android Studio might be, considering the app's usage of phone's resources. I might port the app to a better version in Android Studio some time later, but for this project I feel Unity suffices the task well.
There are quite a few things I'm thinking about taking this project towards, but for now there's one major question I'll be immensely grateful if someone can answer.

## The Question
Essentially the original idea for this app was to measure the linear displacement of the phone in vertical and horizontal space and convert that to a reasonable input for the Mouse movement system. I had presumed that with the accelerometer this would've been an easy task, but I guess I've been so wrong a few times about my life as well. Apparently all my tweaks and adjustments to work with the acceleration failed to achieve an accurate result. Although I could make that work, but that project would've been totally unusable from a UX perspective. Also, I struggled with the abrupt retarding forces on the accelerometer in cases of the phone's sudden stopping in mid air. This resulted in abrupt motions on the mouse cursor, which eventually made me conclude that either me or the accelerometer wasn't advanced enough to get this working correctly. Despite that, if anyone has a way to help me with that, it'd be amazing.

## The Working
* The phone's Gyroscope records the orientation of the phone at each frame, and this is normalised to a unit vector in the direction of gravity. The x and y components of this vector are therefore limited to float values ranging from 0f to 1f.
* The phone creates a data buffer including processed values for the gyroscope input and the values for flags provided for each button pressed.
* The Windows Client recieves the data and the mouse cursor is affected by the Client as per the data recieved.
* The Windows Client hosts a TCP Listener, while the Phone app only hosts a TCP Client which connects to the PC's TCP Listener *(At this moment I don't really know why I'm writing this since everyone on GitHub knows about this stuff more than me, but let's just assume I'm documenting my own journey for a better understanding.)*

## The most unawaited part: How To Try it!
**There are a few minor things I'd like to state before you try it out, so we have less issues trying this out, and if this still doesn't work, just remember,
"It works on my PC"
(Just Kidding... Not actually)**

* Firstly install and run the PC Client
* Ensure that your PC and Phone are on the same network, and the network isn't marked Public on your PC
* Click Start to initialise the Server/TCP Listener
* Now let's open the Android App
* Copy the IP Address Displayed on the PC Client and click Connect.
* The app should successfully connect and you can turn the Air Mouse Mode ON and use your phone as a Gyro Mouse.
* In case the Android App creates any issues, hit the restart button on the right top of the screen (Below connect button) and thing's should be fine.

#### Download links
Pc Client: [download](https://github.com/roeticvampire/AiR-Mouse/blob/master/AiRMouse%20PC%20Client/MouseMotion/bin/Debug/MouseMotion.exe)

Phone app: [download](https://github.com/roeticvampire/AiR-Mouse/blob/master/AiRMouse%20Unity%20App/exports/AiRmousev0.2.apk)

#### Disclaimer
Huge shoutout to a senior whom I'll not name explicitly without his permission, for being a constant source of multi-talented inspiration for me. And a warm thanks to my friend @yogesh-01 for the **App Logo**, which although did delay the project due to a complete redesign that I had to work on to suit the logo theme, was amazing and I really like how much that improved the visual aesthetics of the App. I'm still not done with the PC Client's UI Work.. and the App might be buggy right now, but those are updates to push for another day. 
If you've really read through all that, You sir deserve a coffee. Cheers to that.
