using UnityEngine;
using System.Collections;
using System;

namespace SoftRare.Net.Plugin {
    public class PL_LegacyAnimsSyncer : PL_AuthoritySerializer {

        // contains the current animation number 
        protected int currentAnimation = -1;
        // contains the current normalized speed of the animation 
        protected float currentNormalizedSpeed;
        // the Animation component to use (legacy animations) 
        public Animation animationComp;

        // resets animation weights (weights determine which animation is currently playing) 
        public void resetAnimationWeights() {
            foreach (AnimationState aS in animationComp) {
                animationComp[aS.name].weight = 0f;
                animationComp[aS.name].enabled = false;
            }
        }

        // sets animation 
        public void setAnimationValues(int currentAnimation, float normalizedSpeed) {
            int c = 0;
            foreach (AnimationState aS in animationComp) {
                if (currentAnimation == c) {

                    animationComp[aS.name].normalizedSpeed = normalizedSpeed;
                    animationComp.Play(aS.name);

                    break;
                }
                c++;
            }
        }

        public override void OnAuthoritySerialize() {
            if (myNetObject.pluginShouldSyncAnimations) {
                int c = 0;

                // contains the highest weight of an animation 
                float HighestWeight = 0f;
                //contains the number of the animation with the heighest weight 
                int HighestAnim = -1;
                //contains the highest normalized speed of an animation
                float HighestNormalizedSpeed = 0f;

                foreach (AnimationState aS in animationComp) {

                    if (animationComp.IsPlaying(aS.name)) {

                        currentAnimation = c;
                        //contains the current weight of the animation
                        float currentWeight = aS.weight;
                        currentNormalizedSpeed = aS.normalizedSpeed;
                        if (currentWeight > 0f && currentWeight > HighestWeight) {
                            HighestWeight = currentWeight;
                            HighestAnim = currentAnimation;

                            HighestNormalizedSpeed = currentNormalizedSpeed;
                        }
                    }
                    c++;
                }

                //Legacy networking:
                //stream.Serialize(ref HighestAnim);
                //stream.Serialize(ref HighestNormalizedSpeed);

                //uMMO "UNET Edition":
                Sync("s", ref HighestNormalizedSpeed);
                Sync("a", ref HighestAnim);
            }

        }

        public override void OnNonAuthorityDeserialize(double timestamp) {
            if (myNetObject.pluginShouldSyncAnimations) {
                //Legacy networking:
                //stream.Serialize(ref currentAnimation);
                //stream.Serialize(ref currentNormalizedSpeed);

                //uMMO "UNET Edition":
                Sync("a", ref currentAnimation);
                Sync("s", ref currentNormalizedSpeed);

                if (currentAnimation > -1) {
                    //resetAnimationWeights();
                    setAnimationValues(currentAnimation, currentNormalizedSpeed);
                }
            }
        }

        public override void __awake(GameObject pluginOwner) {
            base.__awake(pluginOwner);

            if (myNetObject.pluginShouldSyncAnimations) {
                animationComp = myNetObject.animationComp;
                resetAnimationWeights();
                //play standard animation
                animationComp.Play(animationComp.clip.name);
            }
        }
    }
}
