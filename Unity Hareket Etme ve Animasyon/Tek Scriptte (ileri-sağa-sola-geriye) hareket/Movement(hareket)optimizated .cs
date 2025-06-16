//?? bu kodda optimizasyon var

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class twoDimensionalAnimationStateController : MonoBehaviour
{
    Animator animator;
    float velocityX = 0.0f;
    float velocityZ = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;

    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;

    //* increase performance
    int VelocityZHash;
    int VelocityXHash;

    void Start()
    {
        //* search the gameObject this script is attached to and get the animator component
        animator = GetComponent<Animator>();

        //* increase performance
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");
        
    }

    //* handles acceleration and deceleration
    void ChangeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, bool backwardPressed, float currentMaxVelocity)
    {
        //* if player presses forward, increase velocity in z direction
        // if(forwardPressed && velocityZ < 0.05f && !runPressed) 
        if(forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        //* if player presses backward, decrease velocity in z direction
        if(backwardPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        //* increase velocity in left direction
        // if(leftPressed && velocityX > -0.05f && !runPressed)
        if(leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //* increase velocity in right direction
        // if(rightPressed && velocityX < 0.05f && !runPressed)
        if(rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //* decrase velocityZ for forward
        if(!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //*increase velocityZ for backward
        if(!backwardPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        //* increase velocityX if left is not pressed and velocityX < 0
        if(!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //* decrease velocityX if right is not pressed and velocityX > 0
        if(!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }

    //* handling reset or lock velocity
    void LockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, bool backwardPressed, float currentMaxVelocity)
    {
        //* reset velocityZ 
        if(!forwardPressed && !backwardPressed && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }

        //* reset velocityX
        if(!leftPressed && !rightPressed && velocityX != 0.0f  && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        //* lock forward (bu kısımda tam 2'nin üstünde değer ise tam 2'ye sabitliyoruz)
        if(forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        //* decelerate to the maximum walk velocity
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            //* round to the currentMaxVelocity if within offset
            if(velocityZ > currentMaxVelocity && velocityZ <(currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        //* round to the currentMaxVelocity if within offset ( bu kısımda küçük sapmalar bile olsa direk 0.5f'e sabit olmasını sağlıyoruz)
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        //* lock backward
        if(backwardPressed && runPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ = -currentMaxVelocity;
        }
        //* decelerate to the maximum walk velocity
        else if(backwardPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;
            //* round to the currentMaxVelocity if within offset
            if(velocityZ < -currentMaxVelocity && velocityZ >(currentMaxVelocity -0.05f))
            {
                velocityZ = -currentMaxVelocity;
            }
        }
        //* round to the currentMaxVelocity if within offset ( bu kısımda küçük sapmalar bile olsa direk 0.5f'e sabit olmasını sağlıyoruz)
        else if(backwardPressed && velocityZ > -currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
        {
            velocityZ = -currentMaxVelocity;
        }

        //* locking left
        if(leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        //* decelerate to the maximum left velocity
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            //* round to the currentMaxVelocity if within offset
            if(velocityX < -currentMaxVelocity && velocityX >(-currentMaxVelocity -0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        //* round to the currentMaxVelocity if within offset
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

        //* locking right
        if(rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        //* decelerate to the maximum right velocity
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            //* round to the currentMaxVelocity if within offset
            if(velocityX > currentMaxVelocity && velocityX <(currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        //* round to the currentMaxVelocity if within offset
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity -0.05f))
        {
            velocityX = currentMaxVelocity;
        }

    }

    void Update()
    {
        //* Input will be true if the player is pressing on the passed in key parameters
        //* Get key input from player
        // bool forwardPressed = Input.GetKey("w");
        // bool leftPressed = Input.GetKey("a");
        // bool rightPressed = Input.GetKey("d");
        // bool runPressed = Input.GetKey("left shift");

        //* increase performance
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool backwardPressed = Input.GetKey(KeyCode.S);


        //* set current MaxVelocity
        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        //* handle changes in velocity
        ChangeVelocity(forwardPressed, leftPressed, rightPressed, runPressed, backwardPressed, currentMaxVelocity);
        LockOrResetVelocity(forwardPressed, leftPressed, rightPressed, runPressed, backwardPressed,currentMaxVelocity);    

        //* set the parameter to our local variables values
        animator.SetFloat(VelocityZHash,velocityZ);
        animator.SetFloat(VelocityXHash,velocityX);
    }
}
