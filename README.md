# FasterScroll

## Description :

Increases SongList scrolling speed to your own taste, change its acceleration curve, its max speed
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

- Check the Release page https://github.com/Aryetis/FasterScroll/releases and donwload the latest release

- Use ModAssistant to install the bare minimal mods / requirements : BSIPA, BeatSaberMarkupLanguage

- Copy the FasterScroll.dll from the release's page .zip in the following folder [SteamFolder]\steamapps\common\Beat Saber\Plugins

**Or** 

2/ Wait for it to show up in ModAssistant and downnload it there

----------

## Known Issues

None. However this has only be tested on Rift S so far and it's still pretty new. So if you encounter any weird behavior/issues you can :
- Create a ticket describing the problem <a href="https://github.com/Aryetis/FasterScroll/issues">Here</a> 
- Join the <a href="https://discord.com/invite/beatsabermods">BSMG discord</a> and ask people over here for help. Might has well ping me while you're at it (@Aryetis#1461)
