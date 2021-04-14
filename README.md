# FasterScroll

```diff
- Index users please read the "Known Issues" section below
```

## Description :

Increase SongList scrolling's speed to your own taste, change its acceleration curve, its max speed
Tweak SongList scrolling rumbles too (your wrists will thank you for this).
If a picture is worth a thousand words... How much is it for a gif ? Here's a quick demo of this mod <a href="https://www.youtube.com/watch?v=NgsHk499Rog">(Full demo YT link)</a> : 
<p align="center" href="https://www.youtube.com/watch?v=NgsHk499Rog">
  <img src="https://github.com/Aryetis/FasterScroll/blob/master/FasterScroll/Resources/Fasterscroll-Trimed480p30fps.gif">
</p>

----------

## Why ? 

Because if you're like me, you probably have over 9000 problems also known as maps. Therefore you know how much of a drag it can be to look for a song you don't know the exact title, look for a song that you added a couple weeks ago, scroll along your seemingly infinite "favorite" playlist etc...

"But why don't you use the scroll bar handle ?" : Because it's gone! It's too small to grab! I don't waste my time fixing non existing problem.

----------

## Settings :

How do they work ?

![InGameSettings](https://github.com/Aryetis/FasterScroll/blob/master/FasterScroll/Resources/SettingsMenuInGame.jpg)

**[Scrolling Mode]** Set the Song List scroll speed curve to one of the following settings :
- Constant : Scrolling's speed will reach [Scroll Max Speed] as soon as you push the joystick
- Linear : Scrolling's speed will accelerate according to the [Scroll Acceleration] factor until it reaches the [Scroll Max Speed]
- Exp : Scrolling's speed will accelerate **EXPONENTIALLY** according to the [Scroll Acceleration] factor until it reaches the [Scroll Max Speed]
- Stock (Default) : Default unmoded behavior 

**[Scroll Acceleration]** Factor describing the acceleration of SongList scrolling's speed (used only For Linear and Exp Scrolling Mode).

**[Scroll Max Speed]** Maximum SongList scrolling's speed, sky is the limit... Or more likely 3000 is the limit. It's already blinding fast.

**[Scrolling Rumble Mode]** Override the rumble settings of your controller while browsing/scrolling Songlist (should be compatible with nalulululuna's RumbleMod) :
- Override : Will replace the rumble's strength with [Override Rumble Strength] for the SongList UI section
- None : Will entirely disable the rumble 
- Stock : Default unmoded behavior (rip for your batteries)

**[Override Rumble Strength]** Describe how strong you want your controllers to rumble each time you over a song / scroll

----------

## Where do I download it ? How do I install it ?

**2 SOLUTIONS :**

1/ Manual installation :

- Check the <a href="https://github.com/Aryetis/FasterScroll/releases">Release page</a> and donwload the latest FasterScroll-\*.\*.\*-gibberish.zip

- Use <a href="https://github.com/Assistant/ModAssistant">ModAssistant</a> to install the bare minimal mods / requirements : BSIPA, BeatSaberMarkupLanguage

- Copy the FasterScroll.dll from the release's page .zip in the following folder [SteamFolder]\steamapps\common\Beat Saber\Plugins

**Or** 

2/ Wait for it to show up in ModAssistant and downnload it there

----------

## Known Issues

- "I can't use my joystick to scroll with my Vive Index" : Yes scrolling with index's joystick input isn't officialy supported by the game. As I don't own an index I assumed it was and didn't plan to add extra code for that. I'll see what I can do about that, but without regular access to a vive it will be difficult to fix this issue. In the meantime <a href="https://www.reddit.com/r/beatsaber/comments/jakcpw/beat_saber_scroll_issue/gdj598y/?utm_source=share&utm_medium=web2x&context=3">someone on reddit pointed out a custom binding</a> named "1.12.2 Playlist Scrolling Fix", I got multiple sources confirming me that it did fix the issues for them. Or you can <a href="https://www.youtube.com/watch?v=Bb4YKwmYvWk">create your own CustomBinding</a> for it.
![InGameSettings](https://github.com/Aryetis/FasterScroll/blob/master/FasterScroll/Resources/CustomBindings.jpg)


- "Scrolling's speed is way faster than it should be. Event at stock speed" : Have you installed Kinsi55's Tweaks55 mod ? If so check its settings, there should be a Scroll speed multiplier (soon :tm:). This multiplier (x1 by default) should stack with whatever settings you have set in FasterScroll. Leave it at 1.00 if you don't know what you're doing :3.

- "That's it?" : Yes that's it so far however this mod is still pretty new, so if you encounter any weird behavior/issues you can :
    - Create a ticket describing the problem <a href="https://github.com/Aryetis/FasterScroll/issues">Here</a> 
    - Join the <a href="https://discord.com/invite/beatsabermods">BSMG discord</a> and ask people over here for help. Might has well ping me while you're at it (@Aryetis#1461)


