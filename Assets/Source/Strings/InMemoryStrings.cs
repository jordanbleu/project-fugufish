using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Source.Strings
{
    /// <summary>
    /// This isn't how I wanted to do this but it was too difficult to maintain compatibility between webgl and standalone platforms.
    /// 
    /// This also looks weird because it used to be in XML files, but now it exists in memory but I wanted to maintain compatibility.
    /// </summary>
    public static class InMemoryStrings 
    {

        public static Dictionary<string, Dictionary<string, string>> Strings = new Dictionary<string, Dictionary<string, string>>()
        {

            // The Jordan Bleu easter egg
            { "EasterEgg/easterEgg.xml", new Dictionary<string, string>() {
                { "0", "What is up guys, it's ya boi j-bleu back at it again with another video!" },
                { "1", "...wait who the hell are you??" },
                { "2", "You weren't supposed to see this!" },
                { "3", "Did you seriously just jump that high?" },
                { "4", "Damn dude." },
                { "5", "Well, I guess you deserve something for being such a bad ass." },
                { "6", "Feel free to take these adrenaline shots." },
                { "7", "I don't need them of course because my body is made from pure adrenaline." },
                { "9", "Okay now please leave me alone I am trying to become a famous person." },
            } },

            { "EasterEgg/easterEgg_attack.xml", new Dictionary<string, string>() {
                { "0", "Ha!  You cannot kill me.  I created you!" },
            } },

            // Global strings 
            { "Global/tutorialMessagesKeyboard.xml", new Dictionary<string, string>() {
                { "attack", "Use Left Mouse Button to attack" },
                { "jump", "Press [SPACE] to jump" },
                { "ladder", "Walk up to ladders to climb them" },
                { "uppercut", "Hold [S] while attacking to swing upwards" },
                { "dodge", "Use [Q] and [E] keys to dodge" },
                { "does_not_open_from_side", "Door does not open from this side" },
                { "health_pickups", "Pick up adrenaline shots to heal" },
                { "bloodsamples", "Press [R] to show collected blood samples on your HUD" },
                { "double_dodge", "Press [Q] / [E] again while dodging to go farther" },
                { "bleeding", "You are fatally wounded and take damage over time.  Move quickly to stay alive." },
            } },

            { "Global/tutorialMessagesXbone.xml", new Dictionary<string, string>() {
                { "attack", "Use (X) Button to attack" },
                { "jump", "Press (A) to jump" },
                { "ladder", "Walk up to ladders to climb them" },
                { "uppercut", "Move left stick down while attacking to swing upwards" },
                { "dodge", "Use [LB] / [RB] buttons to dodge" },
                { "does_not_open_from_side", "Door does not open from this side" },
                { "health_pickups", "Pick up adrenaline shots to heal" },
                { "bloodsamples", "Press (Y) to show collected blood samples on your HUD" },
                { "double_dodge", "Press [LB] / [RB] again while dodging to go farther" },
                { "bleeding", "You are fatally wounded and take damage over time.  Move quickly to stay alive." },
            } },

            // The intro
            { "Intro/intro_01_coldOpen.xml", new Dictionary<string, string>() {
                { "cold_open0", "So they've sent a fool like you to kill me..." },
            } },

            { "Intro/intro_02.xml", new Dictionary<string, string>() {
                { "antag01", "To think, you came so close." },
                { "antag02", "yet you've made the biggest mistake a killer can make..." },
            } },

            { "Intro/intro_03.xml", new Dictionary<string, string>() {
                { "antag01", "...falling in love" },
            } },

            { "Intro/intro_04.xml", new Dictionary<string, string>() {
                { "antag01", "Now, tell me." },
                { "antag02", "Who sent you to assassinate me?" },
            } },

            { "Intro/intro_05_protagOptions.xml", new Dictionary<string, string>() {
                { "protag_scared", "Please don't hurt us!" },
                { "protag_smart", "They re-wired my neural network.  I couldn't tell you if I wanted to." },
                { "protag_brave", "Screw you, asshole." },
                { "protag_silent", "(Say nothing)" },
            } },

            { "Intro/intro_06_option1.xml", new Dictionary<string, string>() {
                { "antag01", "Trust me, it won't hurt a bit." },
            } },

            { "Intro/intro_06_option2.xml", new Dictionary<string, string>() {
                { "antag01", "Yes...Of course they did." },
                { "antag02", "Very well then.  No sense dragging this out any longer." },
            } },

            { "Intro/intro_06_option3.xml", new Dictionary<string, string>() {
                { "antag01", "Clever." },
                { "antag02", "This eruption of spontaneous vulgarity will be your last." },
            } },

            { "Intro/intro_06_option4.xml", new Dictionary<string, string>() {
                { "antag01", "The silent treatment, huh.  Very original." },
                { "antag02", "At least she will die knowing you were brave to the very end." },
            } },

            { "Intro/intro_07.xml", new Dictionary<string, string>() {
                { "antag01", "Her blood is on your hands." },
            } },

            { "Intro/intro_08.xml", new Dictionary<string, string>() {
                { "antag01", "...and thus, the cycle of violence continues" },
            } },

            { "Intro/intro_09.xml", new Dictionary<string, string>() {
                { "antag01", "As for you two, my children..." },
                { "antag02", "Do not let this happen again." },
            } },

            // Pre-Boss battle 
            { "PreBoss/preb_00.xml", new Dictionary<string, string>() {
                { "0", "You are right on time." },
                { "1", "Please, have a seat." },
            } },

            { "PreBoss/preb_01.xml", new Dictionary<string, string>() {
                { "0", "Not a fan of chairs, I see." },
                { "1", "Very well, this shouldn't take long." },
            } },


            { "PreBoss/preb_02.xml", new Dictionary<string, string>() {
                { "0", "What you have witnessed in this lab" },
                { "1", "Or rather, slaughtered..." },
                { "2", "was my attempt at creating the next evolution of mankind." },
                { "3", "...our last hope to survive on this dying planet" },
                { "4", "Creatures born from clones" },
                { "5", "and clones, born from human beings." },
                { "6", "Human beings that did not deserve to die." },
                { "7", ". . ." },
            } },

            { "PreBoss/preb_03.xml", new Dictionary<string, string>() {
                { "0", "The ones who sent you do not believe in me" },
                { "1", "They are the real enemy.  The enemy of progress." },
                { "2", "...but you don't care about any of that, do you?" },
                { "3", "Heh." },
                { "4", "How much did they pay you, anyway?" },
                { "5", "Was it worth the cost of your one true love?" },
                { "6", "She trusted you, and you let her die." },
                { "8", "You know..." },
                { "9", "You could kill every last living thing in this lab..." },
                { "10", "...and you still could never be forgiven for that" },
                { "11", "You'll be cursed until you draw your last bleeding breath." },
            } },

            { "PreBoss/preb_04.xml", new Dictionary<string, string>() {
                { "1", "You don't have much time left..." },
                { "2", "To be honest, I'm suprised you made it this far." },
                { "3", "It would be a shame for you to die without witnessing the completed formula..." },
                { "4", "Please, follow me." },
                { "5", "Allow me to show you what I have created." },
                { "6", "And after that...you will have your revenge" },
            } },

            // Boss Transformation scene
            { "Transform/t_00.xml", new Dictionary<string, string>() {
                { "0", "After countless failures, we have finally perfected the formula." },
                { "1", "And tonight, I will show you my life's work." },
                { "2", "And then..." },
                { "3", "I will kill you." },
            } },

            { "Transform/t_01.xml", new Dictionary<string, string>() {
                { "0", "Prepare yourself..." },
                { "1", "... you're about to... witness.... " },
                { "2", "... my.... final........ form..... " },
                { "3", "... my......... TRUE........ form..... " },
            } },

            { "Transform/t_02.xml", new Dictionary<string, string>() {
                { "0", "Gahhhh!" },
                { "1", "....." },
                { "2", "I.... will.... END..... you!!!" },
            } },

            { "Transform/t_03.xml", new Dictionary<string, string>() {
                { "0", "MY... POWER... IS... INCREASING!!!" },
                { "1", "MY... STRENGTH... CANNOT... BE MATCHED!!!" },
                { "2", "YOU WILL NOT... DEFEAT... MEEEEEEEE!!!!" },
            } },

            { "Transform/t_04.xml", new Dictionary<string, string>() {
                { "0", "Ahh, much better." },
                { "1", "It is a painful process, but well worth it in the end." },
                { "2", "My strength has grown exponentially, of course." },
                { "3", "In fact..." },
                { "4", "How about a little demonstration?" },
                { "5", "Ha ha ha!" },
                { "6", "Prepare to face me, you pitiful waste of flesh!" },
                { "7", "My final form will be the last thing you see!!!" },
            } },

        };


    }
}
