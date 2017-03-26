using UnityEngine;
using System.Collections;
using System;

namespace SoftRare.Net.Plugin {
    public class PL_MecanimSyncer : PL_AuthoritySerializer {
        //Done by Michael Schumann in order to help SoftRare, and this amazing asset. 
        //In order to sync mecanim animations, one will send bools / floats / ints / strings back and forth. 
        //In order to make those work with the animations however, one has to do this a certain way. 
        //The way to do that, is actually by first grabbing reference to the Animator, then using it like so: 
        //Animator = anim. 
        //anim.GetBool("someBool");
        //anim.GetFloat("someFloat");
        //That is how one would sync mecanim animations. 
        //Now, to do this with a list of strings, one needs a method to check what kind of variable they are. 
        //As in, if(anim.GetBool("ListItem1") == null){ return; }else{ SyncThisAsBool(); } 
        //if(anim.GetFloat("ListItem1") == null){ return; }else{ SyncThisAsFloat(); }
        //Frankly, I would do this once, so call it in the start method maybe, or whatever, so its done once, then done forever. 
        //This is how you will do it. 
        //changed by SoftRare to fit uMMO 2.0 "UNET Edition"

        protected Animator anim;

        public override void OnAuthoritySerialize() {
            if (myNetObject.pluginShouldSyncAnimations) {

                for (int i = 0; i < anim.layerCount; i++) {
                    float layerweight = anim.GetLayerWeight(i);
                    Sync("ml" + i, ref layerweight);
                }

                for (int i = 0; i < myNetObject.mecanimVarsToSync.Count; i++) {
                    AnimatorControllerParameter acp = anim.GetParameter(myNetObject.mecanimVarsToSync[i]);

                    if (acp.type == AnimatorControllerParameterType.Bool) {
                        bool v = anim.GetBool(acp.name);
                        Sync("m" + i, ref v);
                    } else if (acp.type == AnimatorControllerParameterType.Float) {
                        float v = anim.GetFloat(acp.name);
                        Sync("m" + i, ref v);
                    } else if (acp.type == AnimatorControllerParameterType.Int) {
                        int v = anim.GetInteger(acp.name);
                        Sync("m" + i, ref v);
                    }
                }
            }
        }

        public override void OnNonAuthorityDeserialize(double latestRemoteTimestampOfChange) {
            if (myNetObject.pluginShouldSyncAnimations) {

                for (int i = 0; i < anim.layerCount; i++) {
                    float layerweight = 0f;
                    Sync("ml" + i, ref layerweight);
                    anim.SetLayerWeight(i, layerweight);
                }

                for (int i = 0; i < myNetObject.mecanimVarsToSync.Count; i++) {
                    AnimatorControllerParameter acp = anim.GetParameter(myNetObject.mecanimVarsToSync[i]);

                    if (acp.type == AnimatorControllerParameterType.Bool) {
                        bool v = false;
                        Sync("m" + i, ref v);
                        anim.SetBool(acp.name , v);
                    } else if (acp.type == AnimatorControllerParameterType.Float) {
                        float v = 0f;
                        Sync("m" + i, ref v);
                        anim.SetFloat(acp.name, v);
                    } else if (acp.type == AnimatorControllerParameterType.Int) {
                        int v = 0;
                        Sync("m" + i, ref v);
                        anim.SetInteger(acp.name, v);
                    }
                }
            }
        }

        public override void __awake(GameObject pluginOwner) {
            base.__awake(pluginOwner);

            //Get our animator 
            if (myNetObject.pluginShouldSyncAnimations) {
                anim = myNetObject.animatorComp;
            }
        }
    }
}
