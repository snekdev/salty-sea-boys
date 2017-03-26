// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
namespace SoftRare.Div {
    public class ThirdPersonSimpleAnimation : MonoBehaviour {
        float runSpeedScale = 1.0f;
        float walkSpeedScale = 1.0f;
        Transform torso;
        bool punshing = false;
        bool beingHit = false;
        bool dying = false;

        void Awake() {
            // By default loop all animations
            GetComponent<Animation>().wrapMode = WrapMode.Loop;

            // We are in full control here - don't let any other animations play when we start
            GetComponent<Animation>().Stop();
            GetComponent<Animation>().Play("idle");
        }

        void Start() {

        }

        void Update() {

            ThirdPersonController marioController = GetComponent<ThirdPersonController>();
            float currentSpeed = marioController.GetSpeed();

            // Fade in run
            if (currentSpeed > marioController.walkSpeed) {
                GetComponent<Animation>().CrossFade("run");
                // We fade out jumpland quick otherwise we get sliding feet
                GetComponent<Animation>().Blend("jumpland", 0);
                //SendMessage("SyncAnimation", "run"); //uMMO: this is not neccessary anymore
            }
            // Fade in walk
            else if (currentSpeed > 0.1f) {
                GetComponent<Animation>().CrossFade("walk");
                // We fade out jumpland realy quick otherwise we get sliding feet
                GetComponent<Animation>().Blend("jumpland", 0);
                //SendMessage("SyncAnimation", "walk"); //uMMO: this is not neccessary anymore
            }
            // Fade out walk and run
            else if (!punshing && !beingHit && !dying) {
                GetComponent<Animation>().CrossFade("idle");
                //SendMessage("SyncAnimation", "idle"); //uMMO: this is not neccessary anymore
            }

            GetComponent<Animation>()["run"].normalizedSpeed = runSpeedScale;
            GetComponent<Animation>()["walk"].normalizedSpeed = walkSpeedScale;

            if (marioController.IsJumping()) {
                if (marioController.IsCapeFlying()) {
                    GetComponent<Animation>().CrossFade("jetpackjump", 0.2f);
                    //SendMessage("SyncAnimation", "jetpackjump"); //uMMO: this is not neccessary anymore
                } else if (marioController.HasJumpReachedApex()) {
                    GetComponent<Animation>().CrossFade("jumpfall", 0.2f);
                    //SendMessage("SyncAnimation", "jumpfall"); //uMMO: this is not neccessary anymore
                } else {
                    GetComponent<Animation>().CrossFade("jump", 0.2f);
                    //SendMessage("SyncAnimation", "jump"); //uMMO: this is not neccessary anymore
                }
            }
            // We fell down somewhere
            else if (!marioController.IsGroundedWithTimeout()) {
                GetComponent<Animation>().CrossFade("ledgefall", 0.2f);
                //SendMessage("SyncAnimation", "ledgefall"); //uMMO: this is not neccessary anymore
            }
            // We are not falling down anymore
            else {
                GetComponent<Animation>().Blend("ledgefall", 0.0f, 0.2f);
            }

        }

        public void DidLand() {
            GetComponent<Animation>().Play("jumpland");
            //SendMessage("SyncAnimation", "jumpland"); //uMMO: this is not neccessary anymore
        }

        public void DidButtStomp() {
            GetComponent<Animation>().CrossFade("buttstomp", 0.1f);
            //SendMessage("SyncAnimation", "buttstomp"); //uMMO: this is not neccessary anymore
            GetComponent<Animation>().CrossFadeQueued("jumpland", 0.2f);
        }

        public void DidPunch() {

            GetComponent<Animation>().CrossFadeQueued("punch", 0.3f, QueueMode.PlayNow);
            punshing = true;
            StartCoroutine("StopPunch");
        }

        private IEnumerator StopPunch() {

            yield return new WaitForSeconds(0.5f);
            punshing = false;
        }

        public void ApplyDamage() {
            GetComponent<Animation>().CrossFade("gotbit", 0.1f);
            beingHit = true;
            //SendMessage("SyncAnimation", "gothit"); //uMMO: this is not neccessary anymore
            StartCoroutine("StopBeingHit");
        }

        private IEnumerator StopBeingHit() {
            yield return new WaitForSeconds(0.5f);
            beingHit = false;
        }

        public void Die() {
            GetComponent<Animation>().CrossFade("idle");
            dying = true;

            StartCoroutine("StopDying");
        }

        private IEnumerator StopDying() {
            yield return new WaitForSeconds(0.5f);
            dying = false;
        }


        public void DidWallJump() {
            // Wall jump animation is played without fade.
            // We are turning the character controller 180 degrees around when doing a wall jump so the animation accounts for that.
            // But we really have to make sure that the animation is in full control so 
            // that we don't do weird blends between 180 degree apart rotations
            GetComponent<Animation>().Play("walljump");
            //SendMessage("SyncAnimation", "walljump"); //uMMO: this is not neccessary anymore
        }


    }
}